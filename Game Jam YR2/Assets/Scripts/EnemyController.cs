/*Made by Ryan C.*/
using UnityEngine;

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
    private Rigidbody2D playerRB;
    private float timeKeeper, pastVelocity, playerDist;
    private bool jumpPeaked = false;
    private Rigidbody2D rb;
    private float groundAcceleration = 1;

    [Header("Debugging Strictly")]
    public Debugging db = new Debugging();
    #endregion

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
        groundAcceleration = acceleration;
        rb = GetComponent<Rigidbody2D>();
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

        //The player direction is -1 if left, 1 if right.
        playerDirection = (int)Mathf.Sign(player.transform.position.x - transform.position.x);

        //Target Player
        playerDist = Mathf.Abs(transform.position.x - player.transform.position.x);
        if(playerDist < targetDistance)
        {
            //Move towards Player
            PushPlayer(playerDirection);

            //Jump if stuck on a block.
            Navigate();
        }

        //If at the peak of jump, make gravity faster.
        GravityChange();

        //If in the air, set the acceleration to the air acceleration.
        acceleration = Grounded ? groundAcceleration : airAcceleration;
    }

    private void GravityChange()
    {
        if(Grounded == true)
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
            timeKeeper = Time.time;
            pastVelocity = rb.velocity.y;
        }
    }

    private void Navigate()
    {
        //Left & Right Wall Collision Detection and Velocity Set
        rb.velocity = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.right / 2, GCRadius, GCMask)
            || Physics2D.OverlapCircle((Vector2)transform.position + Vector2.left / 2, GCRadius, GCMask)
            ? new Vector2(rb.velocity.x, rb.velocity.y + jumpPower)
            : rb.velocity;
    }

    private void PushPlayer(int playerDirection = 1)
    {
        //From the current velocity, increase by Acceleration every Fixed Frame until you hit the Max Speed
        //If on the right side, go to maxSpeed. If on the left side, go to -maxSpeed.
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, playerDirection * maxSpeed, Time.fixedDeltaTime * acceleration), rb.velocity.y);
    }

    private bool Grounded => Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down, GCRadius, GCMask);

    private bool GroundAhead => Physics2D.Raycast((Vector2)transform.position + (playerDirection * Vector2.right), Vector2.down * Mathf.Infinity);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.left / 2, GCRadius);
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.down, GCRadius);
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.right / 2, GCRadius);
        Gizmos.DrawRay(transform.position + (playerDirection * Vector3.right), Vector2.down * Mathf.Infinity);
    }
}