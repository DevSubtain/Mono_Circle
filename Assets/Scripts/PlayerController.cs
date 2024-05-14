using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 1;
    public float horSpeed = 1;
    public float maxVelocity = 2;
    public float difficultyOffset = 20;
    [Space]
    public float rotationModifier = 1.5f;

    private bool buttonDown = false;
    private Rigidbody2D rb;
    private Vector2 initPos;
    private float difficultyChangePoint;
    private float diffDistanceTracker = 0;
    private float initHorSpeed;
    private float maxPlayerPos = 0;

    public static PlayerController Instance;

    [HideInInspector] public float traveledDistance = 0;
    [HideInInspector] public int passedPipes = 0;
    public float difficultyLevel = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        initPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        difficultyChangePoint = difficultyOffset;
        diffDistanceTracker = transform.position.x;
        initHorSpeed = horSpeed;

        // callbacks
        GameManager.Instance.OnResetGame += () =>
        {
            Reset();
        };
        GameManager.Instance.OnGameOver += () =>
        {
            Lose();
        };
        GameManager.Instance.OnGameStarted += () =>
        {
            rb.gravityScale = 1;
            rb.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<BoxCollider2D>().enabled = true;
        };

        GameManager.Instance.SetStartingKeys(new System.Collections.Generic.List<KeyCode>() { KeyCode.Space, KeyCode.UpArrow, KeyCode.W, KeyCode.Mouse0, KeyCode.Mouse1 });
    }

    private void Update()
    {
        HandleRotation();

        if (horSpeed != 0 && !buttonDown && (Input.GetKey(KeyCode.Space) || Input.GetAxis("Vertical") > 0 || Input.GetMouseButton(0) || Input.GetMouseButton(1)))
        {
            if (GameManager.Instance.gameOver) return;

            rb.AddForce(Vector2.up * jumpForce * 10);
            buttonDown = true;
            SoundManager.Instance.PlayClip(AUDIO_CLIP.FLY, 0);
        }

        if (!GameManager.Instance.gameStarted) return;

        if (!Input.GetKey(KeyCode.Space) && Input.GetAxis("Vertical") == 0 && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            buttonDown = false;

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxVelocity, maxVelocity));

        if (traveledDistance > diffDistanceTracker + PipesManager.Instance.xSpawnRange / 2f)
        {
            PipesManager.Instance.GeneratePipes();
            diffDistanceTracker = transform.position.x;
        }

        traveledDistance = transform.position.x - initPos.x;
        if (transform.position.x > maxPlayerPos)
            maxPlayerPos = transform.position.x;

        HandleDifficulty();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.gameStarted) return;
        transform.position += Vector3.right * horSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        float zRot = rb.velocity.y;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, zRot * rotationModifier), 0.1f);
    }

    private void HandleDifficulty()
    {
        if (traveledDistance > difficultyChangePoint)
        {
            difficultyChangePoint += difficultyOffset;
            PipesManager.Instance.xStep = Mathf.Clamp(PipesManager.Instance.xStep * 0.95f, 4.5f, 10);
            difficultyLevel++;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Score"))
        {
            SoundManager.Instance.PlayClip(AUDIO_CLIP.SCORE, 1);
            passedPipes++;
        }
    }

    [Space]
    public GameObject miniPicklePrefab;
    public float miniPickleCount;
    private void Lose()
    {
        SoundManager.Instance.PlayClip(AUDIO_CLIP.DIE, 1);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        horSpeed = 0;

        // Pickle death animation
        for (int i = 0; i < miniPickleCount; i++)
        {
            Instantiate(miniPicklePrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 180f)));
            if (MiniPickle.miniPickles.Count > miniPickleCount * 5)
            {
                Destroy(MiniPickle.miniPickles[i]);
                MiniPickle.miniPickles.RemoveAt(i);
            }
        }
    }

    public GameObject dashedLinePrefab;
    public void Reset()
    {
        traveledDistance = 0;

        diffDistanceTracker = initPos.x;
        difficultyChangePoint = difficultyOffset;
        difficultyLevel = 0;
        buttonDown = false;

        GetComponent<SpriteRenderer>().enabled = true;

        horSpeed = initHorSpeed;
        passedPipes = 0;
        transform.position = initPos;
    }
}