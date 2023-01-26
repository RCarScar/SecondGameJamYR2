using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum enemyState
{
    idle,
    running,
    jumping,
    falling,
    blind
}
[System.Serializable]
public class animationInfo
{
    public AnimationClip anim;
    public enemyState state;
    public float length;
}


public class EnemyAnimation : MonoBehaviour
{
    static public EnemyAnimation Instance;
    enemyState state = enemyState.idle;
    [SerializeField] private EnemyController ec;
    [SerializeField] private int playerDirection = 1;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public animationInfo[] animations;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].length = animations[i].anim.length;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerDirection = PlayerController.Instance.gameObject.transform.position.x > transform.position.x ? -1 : 1;

        gameObject.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * playerDirection, transform.localScale.y, 0);

        SetStates();

        CheckStates();
    }

    private AnimationClip CheckStates()
    {
        for(int i = 0; i < animations.Length; i++)
        {
            if (animations[i].state == state)
            {
                return animations[i].anim;
            }
        }

        return animations[0].anim;
    }

    private void SetStates()
    {
        //If the velocity of Y is greater than 0, enemy is jumping.
        if (rb.velocity.y > 0)
        {
            state = enemyState.jumping;
        }
        //Blind if blind.
        if (GameManager.Instance.Blinded == true)
        {
            state = enemyState.blind;
        }
        //If the player has 0 velocity, idle.
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            state = enemyState.idle;
        }
        //If not in the air and moving in the x, running.
        else if (rb.velocity.y == 0 && rb.velocity.x > 0)
        {
            state = enemyState.running;
        }
        //If the enemy is going down, falling.
        if (rb.velocity.y < 0)
        {
            state = enemyState.falling;
        }

    }
}
