using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesManager : MonoBehaviour
{
    public GameObject pipePrefab;
    public float xStep;
    public float xSpawnRange = 20;
    public float singlePipeDistanceModifier = 0.1f;
    public GameObject bottomCollider;

    public static PipesManager Instance;

    private float lastGeneratedXPosition;

    private void Awake()
    {
        Instance = this;
        lastGeneratedXPosition = 0;
    }

    void Start()
    {
        GeneratePipes();
    }

    public void GeneratePipes()
    {
        float i;
        for (i = lastGeneratedXPosition; i < lastGeneratedXPosition + xSpawnRange; i += xStep)
        {
            Instantiate(pipePrefab, new Vector3(i, 0, 0), Quaternion.identity, transform);
        }

        var go = Instantiate(bottomCollider);
        go.transform.position = new Vector3(lastGeneratedXPosition + ((i - lastGeneratedXPosition) / 2f), -4.35f, go.transform.position.z);
        go.transform.localScale = new Vector3(xSpawnRange * 1.2f, go.transform.localScale.y, go.transform.localScale.z);

        lastGeneratedXPosition = i;
    }
}
