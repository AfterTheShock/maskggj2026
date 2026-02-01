
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Options_DataToSave : MonoBehaviour
{
    [Header("PlayerSettings")]
    public bool vsync = true;
    public int resolutionIndex = 0;
    public bool fullscreen = true;
    public int qualityIndex = 1;

    [Header("PlayerSoundSetting")]
    public float masterVolume = 0.35f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambianceVolume = 1f;

    [SerializeField] bool saveDataPlayerDataButton = false;
    [SerializeField] bool deletePlayerDataButton = false;

    [Header("OtherNecessities")]
    private bool dataLoaded = false;
    [SerializeField] AudioMixer masterMixer;

    #region singletonPatern
    private static Options_DataToSave _instance;
    public static Options_DataToSave Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindFirstObjectByType<Options_DataToSave>();
            }
            return _instance;
        }
    }
    #endregion


    public void LoadData()
    {
        Options_PlayerData data = Options_SaveSystem.LoadPlayer();

        if (data == null) return;

        //PlayerSettings
        vsync = data.vsync;
        resolutionIndex = data.resolutionIndex;
        fullscreen = data.fullscreen;
        qualityIndex = data.qualityIndex;

        //PlayerSoundSetting
        masterVolume = data.masterVolume;
        musicVolume = data.musicVolume;
        sfxVolume = data.sfxVolume;
        ambianceVolume = data.ambianceVolume;

        dataLoaded = true;
    }

    private void Update()
    {
        if (deletePlayerDataButton)
        {
            deletePlayerDataButton = false;
            DeletePlayerData();
        }
        if (saveDataPlayerDataButton)
        {
            saveDataPlayerDataButton = false;
            SaveData();
        }
    }

    private void Awake()
    {
        LoadData();

    }
    private void Start()
    {
        if (!dataLoaded) LoadData();

        SetVolumesWithCurrentData();

        InicializePlayerSettings();

        //Screen.SetResolution(1280, 720,true);
    }

    private void InicializePlayerSettings()
    {
        OptionsMenuManager.Instance.ToggleVsync(vsync);
        OptionsMenuManager.Instance.SetResolution(resolutionIndex);
        OptionsMenuManager.Instance.ToggleFullScreen(fullscreen);
        OptionsMenuManager.Instance.SetQualityLevel(qualityIndex);
    }

    public void SaveData()
    {
        Options_SaveSystem.SaveData(this);
        SetVolumesWithCurrentData();
    }

    private void SetVolumesWithCurrentData()
    {
        masterMixer.SetFloat("MasterVolume", LinearToDecibel(masterVolume));
        masterMixer.SetFloat("MusicVolume", LinearToDecibel(musicVolume));
        masterMixer.SetFloat("SFXVolume", LinearToDecibel(sfxVolume));
        masterMixer.SetFloat("AmbianceVolume", LinearToDecibel(ambianceVolume));
    }

    public float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f) return -80f; // Clamp silence

        return Mathf.Log10(linear) * 20.0f;
    }

    public void DeletePlayerData()
    {
        Options_SaveSystem.DeletePlayerData();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
