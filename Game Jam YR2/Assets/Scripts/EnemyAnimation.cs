/*Ryan Chen*/
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorScript : MonoBehaviour
{
    [ExecuteInEditMode]
    void OnEnable()
    {
        EnemyAnimation.instance.animations = new AnimationInformation[5];
        AnimationInformation[] plagarism = new AnimationInformation[] { new AnimationInformation("Idle"), new AnimationInformation("Running"), new AnimationInformation("Blinded"), new AnimationInformation("Jumping"), new AnimationInformation("Falling") };
        EnemyAnimation.instance.animations = plagarism;
        EnemyAnimation.instance.animations[0].type = "Idle";
        EnemyAnimation.instance.animations[1].type = "Running";
        EnemyAnimation.instance.animations[2].type = "Blinded";
        EnemyAnimation.instance.animations[3].type = "Jumping";
        EnemyAnimation.instance.animations[4].type = "Falling";
    }
}
[System.Serializable]
public class AnimationInformation
{
    public AnimationInformation(string typee, AnimationClip clipe = null)
    {
        type = typee;
        clip = clipe;
    }
    public string type; 
    public AnimationClip clip;
}

public class EnemyAnimation : MonoBehaviour
{
    static public EnemyAnimation instance;
    public AnimationInformation[] animations = new AnimationInformation[]{new AnimationInformation("Idle"),new AnimationInformation("Running"),new AnimationInformation("Blinded"),new AnimationInformation("Jumping"),new AnimationInformation("Falling")};


    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(rb.velocity.x < -0.01f)
        {
            transform.localScale = new Vector2(1,1);
        }
        else if (rb.velocity.x > 0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            animator.Play(CheckStates().clip.name);
        }
    }

    private AnimationInformation CheckStates()
    {
        if (GameManager.Instance.Blinded)
        {
            return animations[2];
        }
        //Running
        else if (Mathf.Abs(rb.velocity.x) > 0.01f && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            return animations[1];
        }
        //Jumping
        else if (rb.velocity.y > 0.01f)
        {
            return animations[3];
        }
        //Falling
        else if (rb.velocity.y < 0.01f)
        {
            return animations[4];
        }
        //Idle
        else
        {
            return animations[0];
        }
    }
}
