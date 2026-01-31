using UnityEngine;

public class UnparentAtStart : MonoBehaviour
{

    void Start()
    {
        this.transform.SetParent(null);
    }


}
