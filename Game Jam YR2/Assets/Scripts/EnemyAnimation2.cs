/* Made by Blake Rubadue */

using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyController))]
public class EnemyAnimation2 : MonoBehaviour
{
    public AnimationClip Fall;
    public AnimationClip Run;
    public AnimationClip Jump;
    public AnimationClip Idle;


    private EnemyController controller;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<EnemyController>();
        controller.JumpEvent.AddListener(PlayJump);
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.Grounded)
        {
            if (rb.velocity.x > 0.01f)
            {
                animator.Play("Run");
                renderer.flipX = true;
            }
            else if (rb.velocity.x < 0.01f)
            {
                animator.Play("Run");
                renderer.flipX = false;
            }
            else
            {
                animator.Play("Idle");
            }
        }
        if(rb.velocity.y < 0)
        {
            animator.Play("Fall");
        }
    }

    public void PlayJump()
    {
        animator.Play("Jump");
    }
}
