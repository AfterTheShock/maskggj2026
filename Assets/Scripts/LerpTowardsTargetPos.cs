using UnityEngine;

public class LerpTowardsTargetPos : MonoBehaviour
{
    [SerializeField] Transform targetPos;
    [SerializeField] float lerpSpeed = 5f;

    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos.position, lerpSpeed * Time.deltaTime);
    }
}
