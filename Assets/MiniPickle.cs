using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPickle : MonoBehaviour
{
    public float force;
    public static List<GameObject> miniPickles = new();

    void Start()
    {
        miniPickles.Add(gameObject);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * force);

    }
}
