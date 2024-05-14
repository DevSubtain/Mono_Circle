using UnityEngine;

public class Dash : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        speed = transform.parent.GetComponent<DashedLine>().dashSpeed;
    }
    private void FixedUpdate()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DashDeath"))
        {
            Destroy(gameObject);
        }
    }
}