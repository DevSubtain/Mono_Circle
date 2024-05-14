using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashedLine : MonoBehaviour
{
    public GameObject dashPrefab;
    public float spawnDelay = 0.15f;
    public float dashSpeed = 1f;
    public Transform spawnPoint;
    public float offset = 0;
    public float fadeSpeed = 1;

    public static DashedLine Instance;

    private Transform player = null;
    private Color initColor = Color.black;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(GenerateDashes());
    }

    private void Update()
    {
        if (player)
        {
            initColor = Color.Lerp(initColor, new Color(initColor.r, initColor.g, initColor.b, 0), fadeSpeed * Time.deltaTime);
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                if (item && item.transform)
                {
                    item.color = initColor;
                }
            }

            if (player.transform.position.x > transform.position.x)
                transform.position = new Vector3(player.position.x + offset, transform.position.y, 0);
        }
    }

    private IEnumerator GenerateDashes()
    {
        var go = Instantiate(dashPrefab, spawnPoint.position, Quaternion.identity, transform);
        if (initColor == Color.black)
        {
            initColor = go.GetComponent<SpriteRenderer>().color;
            initColor.a = 1;
        }
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(GenerateDashes());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            Instance = null;
            Destroy(gameObject, 10);
        }
    }
}
