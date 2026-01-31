using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class Options_SaveSystem
{
    static string path = Application.persistentDataPath + "/Options_Data.Zagon";
    public static void SaveData(Options_DataToSave dataToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        Options_PlayerData data = new Options_PlayerData(dataToSave);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Options_PlayerData LoadPlayer()
    {

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Options_PlayerData data = formatter.Deserialize(stream) as Options_PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeletePlayerData()
    {
        System.IO.File.Delete(path);
    }
}
