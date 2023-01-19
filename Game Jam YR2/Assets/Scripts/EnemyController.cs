using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class Debugging
{
    public float velocityX, velocityY, playerDirection;
    public bool left, right;
}

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1, maxSpeed = 1, acceleration = 1, jumpPower, airAcceleration = 0.2f; 
    [SerializeField] private int playerDirection = 1;

    public Debugging db = new Debugging();
    private Rigidbody2D rb;
    private float groundAcceleration = 1;

    [Header("Ground Check")]
    [Min(0)] public float GCRadius = 0.5f;
    public Vector2 GCPosition;
    public LayerMask GCMask;

    void Start()
    {
        groundAcceleration = acceleration;
        pc = player.GetComponent<PlayerController>();
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

        #endregion

        acceleration = Grounded ? groundAcceleration : airAcceleration;

        playerDirection = Mathf.RoundToInt(Mathf.Sign(transform.position.x - player.transform.position.x));

        EnemyMove(playerDirection);

        Navigate();
    }
    private void Navigate()
    {
        //Wall Left
        rb.velocity = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.right / 2, GCRadius, GCMask)
            ? rb.velocity + new Vector2(0, jumpPower)
            : rb.velocity;
        //Wall Left
        rb.velocity = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.left/2, GCRadius, GCMask)
            ? rb.velocity + new Vector2(0, jumpPower)
            : rb.velocity;
    }

    private bool Grounded => Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down, GCRadius, GCMask);

    private void EnemyMove(int playerDirection = 1)
    {
        //When on the left, do max speed positive.
        rb.velocity = playerDirection < 0
            ? new Vector2(Mathf.MoveTowards(rb.velocity.x, maxSpeed, Time.fixedDeltaTime * acceleration), rb.velocity.y)
            : new Vector2(Mathf.MoveTowards(rb.velocity.x, -maxSpeed, Time.fixedDeltaTime * acceleration), rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.left/2, GCRadius);
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.down, GCRadius);
        Gizmos.DrawSphere((Vector2)transform.position + Vector2.right/2, GCRadius);
    }
}