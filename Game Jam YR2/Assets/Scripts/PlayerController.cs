using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public float _Speed = 10f;
    public float _Acceleration = 100f;

    // -------------------- Components ------------------- //
    private Rigidbody2D rb;

    private float MoveInput;

    private float vel = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();

        HandleMovement(Time.deltaTime);
    }

    void GatherInput()
    {
        MoveInput = GameManager.actions.Game.Move.ReadValue<float>();
    }

    void HandleMovement(float dt)
    {
        vel = Mathf.MoveTowards(vel, _Speed * MoveInput, _Acceleration * dt);
        rb.velocity = new Vector2(vel, rb.velocity.y);
    }
}
