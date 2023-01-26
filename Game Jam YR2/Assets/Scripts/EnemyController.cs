/*Made by Ryan C.*/
using GGUtil; /* GGUtil is a helper library made by Blake Rubadue */
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Debugging
{
    public float velocityX, velocityY, playerDirection;
    public bool left, right, peakGravity;
}

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    #region Variables
    //Editor Assigned Variables
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1, maxSpeed = 1, acceleration = 1, jumpPower, airAcceleration = 0.2f, peakGravity, targetDistance = 5;
    [SerializeField] private int playerDirection = 1;

    //Ground Check Stuff
    [Header("Ground Check")]
    [Min(0)] public float GCRadius = 0.5f;
    public Vector2 GCPosition;
    public LayerMask GCMask;

    //Variables Not Really Accessed in Editor
    private float pastVelocity, playerDist, originalTargetDistance;
    private bool jumpPeaked = false;
    private Rigidbody2D rb;
    private float groundAcceleration = 1;

    [Header("Debugging Strictly")]
    public Debugging db = new();
    #endregion

    [Header("Wall Check")]
    public float WallCheckDistance = 2f;
    public LayerMask WallMask;

    private Vector3 InitialPosition;

    void Start()
    {
        originalTargetDistance = targetDistance;
        groundAcceleration = acceleration;
        rb = GetComponent<Rigidbody2D>();

        GameManager.CloseEyesVisualEvent.AddListener(EyesClosed);
        GameManager.OpenEyesStartEvent.AddListener(EyesOpen);
        GameManager.PlayerRespawnEvent.AddListener(Respawn);

        InitialPosition = transform.position;
    }

    void FixedUpdate()
    {
        #region Debugging Stuff
        db.velocityX = rb.velocity.x;
        db.velocityY = rb.velocity.y;
        db.playerDirection = playerDirection;
        //Wall Right
        db.right = (Physics2D.OverlapCircle((Vector2)transform.position + Vector2.right / 2, GCRadius, GCMask)) ? true : false;
        //Wall Left
        db.left = (Physics2D.OverlapCircle((Vector2)transform.position + Vector2.left / 2, GCRadius, GCMask)) ? true : false;
        db.peakGravity = jumpPeaked;
        #endregion

        if (GameManager.Instance.Respawning) return;

        //The player direction is -1 if left, 1 if right.
        playerDirection = (int) Mathf.Sign(player.transform.position.x - transform.position.x);

        //Target Player
        playerDist = Mathf.Abs(transform.position.x - player.transform.position.x);
        if (playerDist < targetDistance)
        {
            //Move towards Player
            PushPlayer(playerDirection);

            Navigate();
        }

        //If at the peak of jump, make gravity faster.
        //GravityChange(); //Incompatible with jump height calculation

        //If in the air, set the acceleration to the air acceleration.
        acceleration = Grounded ? groundAcceleration : airAcceleration;
    }

    private void EyesClosed()
    {
        rb.velocity = GameManager.Instance.Blinded ? Vector3.zero : rb.velocity;

        targetDistance = 0;
    }

    private void EyesOpen()
    {

        targetDistance = originalTargetDistance;
    }

    //Incompatible with jump height calculation
    private void GravityChange()
    {
        if (Grounded == true)
        {
            rb.gravityScale = 9.8f;
            jumpPeaked = false;
            return;
        }

        if (pastVelocity < rb.velocity.x)
        {
            jumpPeaked = true;
            rb.gravityScale = peakGravity;
        }
        else
        {
            pastVelocity = rb.velocity.y;
        }
    }

    private bool Jumping = false;
    private void Navigate()
    {

        /*//Left & Right Wall Collision Detection and Velocity Set
        rb.velocity = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.right / 2, GCRadius, GCMask)
            || Physics2D.OverlapCircle((Vector2)transform.position + Vector2.left / 2, GCRadius, GCMask)
            ? new Vector2(rb.velocity.x, rb.velocity.y + jumpPower)
            : rb.velocity;*/

        /* Made by Blake Rubadue */
        if (!Grounded || Jumping) return; //can't jump if not grounded so don't do anything

        var pDir = new Vector2(playerDirection, 0);

        var hit = Physics2D.Raycast(transform.position, pDir, WallCheckDistance, WallMask);

        if (hit.collider == null) return;
        var Tilemap = hit.collider.GetComponent<Tilemap>();
        if (!Tilemap) return; //GetSurfaceHeight requires a tilemap

        var height = GGMath.GetSurfaceHeight(hit.point + pDir * 0.1f, Tilemap) + 1.5f;

        var gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        float force = Mathf.Sqrt(2 * height * gravity); //normally this would be (jumpDuration^2), but jump duration already had a sqrt so they cancel out and don't need to be performed
        //float jumpDuration = 2 * force / gravity;

        Debug.Log($"Height: {height}, Force: {force}");

        rb.velocity = (new Vector2(rb.velocity.x, (force)));

    }

    private void PushPlayer(int playerDirection = 1)
    {
        //From the current velocity, increase by Acceleration every Fixed Frame until you hit the Max Speed
        //If on the right side, go to maxSpeed. If on the left side, go to -maxSpeed.
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, playerDirection * maxSpeed, Time.fixedDeltaTime * acceleration), rb.velocity.y);
    }

    private void Respawn()
    {
        transform.position = InitialPosition;
    }

    private bool Grounded => Physics2D.OverlapCircle((Vector2)transform.position + GCPosition, GCRadius, GCMask);

    private bool GroundAhead => Physics2D.Raycast((Vector2)transform.position + (playerDirection * Vector2.right), Vector2.down * Mathf.Infinity);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.left / 2, GCRadius);
        Gizmos.DrawSphere((Vector2)transform.position + GCPosition, GCRadius);
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.right / 2, GCRadius);
        Gizmos.DrawRay(transform.position + (playerDirection * Vector3.right), Vector2.down * Mathf.Infinity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == player && GameManager.Instance.Blinded == false)
        {
            GameManager.PlayerDeathEvent.Invoke();
        }
    }
}