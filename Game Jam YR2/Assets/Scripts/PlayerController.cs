using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    // -------------------- Fields ------------------- //
    public float _Speed = 10f;
    public float GroundAcceleration = 100f;
    public float GroundDeceleration = 50f;
    public float AirAcceleration = 50f;
    public float AirDeceleration = 25f;

    [Header("Jump")]
    public float JumpForce = 10f;
    [Min(0)] public float RisingGScale = 1f;
    [Min(0)] public float FallingGScale = 2f;
    [Min(0)] public float PeakGScale = 4f;

    [Min(0)] public float PeakVelAmount = 1f;

    [Min(0)] public float _CoyoteTime = 0.1f;
    [Min(0)] public float _JumpBuffer = 0.25f;

    [Min(0)] public float JumpDisableTime = 0.1f;

    [Header("Ground Check")]
    [Min(0)] public float GCRadius = 0.5f;
    public Vector2 GCPosition;
    public LayerMask GCMask;

    // -------------------- Components ------------------- //
    private Rigidbody2D rb;

    // -------------------- Events ----------------------- //
    [HideInInspector] public UnityEvent LandEvent = new UnityEvent();

    // -------------------- Input ------------------- //
    private float MoveInput;
    private InputAction JumpAction;

    // -------------------- Variables -------------------- //
    private float vel = 0;
    public bool Grounded { get; private set; } = false;

    private bool canJump;
    private bool jumping;

    private float jumpBuffer = 0;
    private float coyoteTime = 0;

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

        UpdateGrounded();

        HandleMovement(Time.deltaTime);

        HandleJump(Time.deltaTime);
    }

    void GatherInput() //gather required inputs
    {
        MoveInput = GameManager.Actions.Game.Move.ReadValue<float>();
        JumpAction = GameManager.Actions.Game.Jump;
    }

    void UpdateGrounded()
    {
        var lastGrounded = Grounded;
        Grounded = Physics2D.OverlapCircle(transform.position + (Vector3) GCPosition, GCRadius, GCMask);
        if (Grounded && !lastGrounded) LandEvent.Invoke(); //invoke event when the player lands on the ground
    }

    float CurrentAcceleration => Grounded ? GroundAcceleration : AirAcceleration;
    float CurrentDeceleration => Grounded ? GroundDeceleration : AirDeceleration;

    void HandleMovement(float dt) //move the player with input
    {
        vel = Mathf.MoveTowards(rb.velocity.x, _Speed * MoveInput, (MoveInput == 0 ? CurrentDeceleration : CurrentAcceleration) * dt);
        rb.velocity = new Vector2(vel, rb.velocity.y);
    }

    private float _jumpDisableTime = 0;
    void HandleJump(float dt)
    {
        if (JumpAction.triggered) jumpBuffer = _JumpBuffer;

        jumpBuffer -= dt;
        if (Grounded)
        {
            coyoteTime = _CoyoteTime;
            rb.gravityScale = RisingGScale;
            if (_jumpDisableTime <= 0) jumping = false;
        }
        else coyoteTime -= dt;

        //when falling
        if (rb.velocity.y <= 0 && !jumping) rb.gravityScale = FallingGScale; //falling gravity

        //if grounded and not jumping and delay ran out.
        canJump = coyoteTime > 0 && _jumpDisableTime <= 0 && !jumping; //update "jumpability"

        if (canJump && jumpBuffer > 0) //if able to jump & told to jump, then jump
        {
            Jump();
        }

        //when at the peak of the jump change gravity
        if (jumping && (!JumpAction.IsPressed() || rb.velocity.y < PeakVelAmount))
        {
            //cancel the jump when input is released
            rb.gravityScale = PeakGScale; //quickly slow the player down to reduce floaty-ness
        }

        if (_jumpDisableTime > 0) _jumpDisableTime -= dt;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        jumpBuffer = 0;
        coyoteTime = 0;
        rb.gravityScale = RisingGScale;

        jumping = true;

        _jumpDisableTime = JumpDisableTime;
    }

#if DEBUG //if unity editor or debugging enabled

    private void OnDrawGizmos()
    {
        UpdateGrounded();
        Gizmos.color = Grounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3)GCPosition, GCRadius); //Debug ground check
    }

#endif
}
