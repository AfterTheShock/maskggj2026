using UnityEngine;

public class MuzzelFlashRand : MonoBehaviour
{
    [SerializeField] float randomMuzzelScaleRange = 0.15f;
    private float startSize;
    private void Awake()
    {
        startSize = this.transform.localScale.x;
    }

    private void OnEnable()
    {
        this.transform.localScale = Vector3.one * Random.Range(startSize - randomMuzzelScaleRange, startSize + randomMuzzelScaleRange);
    }
}
