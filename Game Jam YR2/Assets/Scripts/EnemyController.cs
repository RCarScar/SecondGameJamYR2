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
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1, maxSpeed = 1, acceleration = 1, jumpPower, airAcceleration = 0.2f, peakGravity; 
    [SerializeField] private int playerDirection = 1;

    private float timeKeeper, pastVelocity;
    private bool jumpPeaked = false;
    private Rigidbody2D rb;
    private float groundAcceleration = 1;

    [Header("Ground Check")]
    [Min(0)] public float GCRadius = 0.5f;
    public Vector2 GCPosition;
    public LayerMask GCMask;

    [Header("Debugging Strictly")]
    public Debugging db = new Debugging();
    #endregion

    void Start()
    {
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
        
        //If at the peak of jump, make gravity faster.
        GravityChange();

        //If in the air, set the acceleration to the air acceleration.
        acceleration = Grounded ? groundAcceleration : airAcceleration;

        //The player direction is -1 if left, 1 if right.
        playerDirection = Mathf.RoundToInt(Mathf.Sign(player.transform.position.x - transform.position.x));

        //Move towards Player
        EnemyMove(playerDirection);

        //Jump if stuck on a block.
        Navigate();
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
            ? rb.velocity + new Vector2(playerDirection, jumpPower)
            : rb.velocity;
    }

    private void EnemyMove(int playerDirection = 1)
    {
        //From the current velocity, increase by Acceleration every Fixed Frame until you hit the Max Speed
        //If on the right side, go to maxSpeed. If on the left side, go to -maxSpeed.
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, Mathf.Sign(playerDirection) * maxSpeed, Time.fixedDeltaTime * acceleration), rb.velocity.y);
    }

    private bool Grounded => Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down, GCRadius, GCMask);

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere((Vector2)transform.position + Vector2.left/2, GCRadius);
    //    Gizmos.DrawSphere((Vector2)transform.position + Vector2.down, GCRadius);
    //    Gizmos.DrawSphere((Vector2)transform.position + Vector2.right/2, GCRadius);
    //}
}