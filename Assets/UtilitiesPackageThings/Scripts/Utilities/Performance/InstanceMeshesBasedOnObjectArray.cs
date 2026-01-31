using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class InstanceMeshesBasedOnObjectArray : MonoBehaviour
{
    [SerializeField] Mesh meshToInstance;
    [SerializeField] Material materialToInstance;

    [SerializeField] List<Transform> listOfGoForPosRotAndScale;

    [SerializeField] bool updateMatrixesButton = false;

    private List<Matrix4x4> matrix = new List<Matrix4x4>();

    private void Update()
    {
        if (updateMatrixesButton || Input.GetKeyDown(KeyCode.P))
        {
            updateMatrixesButton = false;
            UpdateMatrixes();
        }

        if (meshToInstance == null || materialToInstance == null || matrix.Count == 0) return;

        Graphics.DrawMeshInstanced(meshToInstance, 0, materialToInstance, matrix);

    }


    private void UpdateMatrixes()
    {
        matrix.Clear();


        foreach (Transform go in listOfGoForPosRotAndScale) 
        {
            matrix.Add(Matrix4x4.TRS(go.position, go.rotation, go.localScale));
        }
    }
}
