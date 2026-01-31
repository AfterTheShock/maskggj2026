using UnityEngine;

public class UnparentAnThenDontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        this.transform.SetParent(null);

        DontDestroyOnLoad(this.transform);
    }
}
