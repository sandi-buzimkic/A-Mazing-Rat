using UnityEngine;

public class SkinMover : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetScale;
    public float moveSpeed = 10f;
    public float scaleSpeed = 10f;

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }
}
