using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed = 0.1f, xOffset = 0;

    private void FixedUpdate()
    {
        if (PlayerController.Instance.transform.position.x != transform.position.x + xOffset)
        {
            float targetXPos = Mathf.Lerp(transform.position.x, PlayerController.Instance.transform.position.x + xOffset, lerpSpeed);
            transform.position = new Vector3(targetXPos, transform.position.y, transform.position.z);
        }
    }
}