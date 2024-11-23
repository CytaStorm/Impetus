using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        //Clear all variables first
        animator.SetFloat("xVelocity", 0f);
        animator.SetFloat("yVelocity", 0f);
        direction = rb.velocity.normalized;
        if(direction.x >= 0.5f)
        {
            animator.SetFloat("xVelocity", 1f);
        }
        else if (direction.x <= -0.5f)
        {
            animator.SetFloat("xVelocity", -1f);
        }
        else if (direction.y > 0.5f)
        {
            animator.SetFloat("yVelocity", 1f);
        }
        else if (direction.y < -0.5f)
        {
            animator.SetFloat("yVelocity", -1f);
        }

        if(animator.GetFloat("xVelocity") == 0f
            && animator.GetFloat("yVelocity") == 0f)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
        }
    }
}
