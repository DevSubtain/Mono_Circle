using UnityEngine;

public class Pipe : MonoBehaviour
{
    Transform[] pipes;

    private void Start()
    {
        transform.position += Random.Range(-2.5f, 2.5f) * Vector3.up;
        transform.GetChild(0).position += Vector3.up * PipesManager.Instance.singlePipeDistanceModifier * Mathf.Clamp(PlayerController.Instance.difficultyLevel, 0, 5);
        transform.GetChild(1).position += Vector3.down * PipesManager.Instance.singlePipeDistanceModifier * Mathf.Clamp(PlayerController.Instance.difficultyLevel, 0, 5);
    }
}