using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterLocomotion : MonoBehaviour
{
    Animator animator;

    float clampMax = .5f;
    float clampMin = -.5f;
    float runClampMax = 1f;
    float runClampMin = -1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RunningBoolTrigger();
        Jumping();
        CrouchingBoolTrigger();
        Crouching();
    }

    void Move()
    {
        if (animator.GetBool("isRunning"))
        {
            
            animator.SetFloat("VerticalRunning", Mathf.Clamp(Input.GetAxis("Vertical"), 0, runClampMax));
            animator.SetFloat("HorizontalRunning", Mathf.Clamp(Input.GetAxis("Horizontal"), runClampMin, runClampMax));
            animator.SetFloat("Vertical", Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1f));
            animator.SetFloat("Horizontal", Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f));
        }
        else
        {
            animator.SetFloat("Vertical", Mathf.Clamp(Input.GetAxis("Vertical"), clampMin, clampMax));
            animator.SetFloat("Horizontal", Mathf.Clamp(Input.GetAxis("Horizontal"), clampMin, clampMax));
            
        }
    }

    void RunningBoolTrigger()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && animator.GetFloat("Vertical") >= .3f) 
        {
            animator.SetBool("isRunning", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            animator.SetBool("isRunning", false);
        }
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && animator.GetFloat("Vertical") == 0)
        {
            animator.SetTrigger("isIdleJumping");
        }

        if (Input.GetKeyDown(KeyCode.Space) && animator.GetFloat("Vertical") > .5f)
        {
            animator.SetTrigger("isJumping");
        }
    }

    void CrouchingBoolTrigger()
    {
        if (Input.GetKeyDown("c") && !animator.GetBool("isCrouching"))
        {
            animator.SetBool("isCrouching", true);
        }
        else
            if (Input.GetKeyDown("c") && animator.GetBool("isCrouching"))
            {
                animator.SetBool("isCrouching", false);
            }
    }

    void Crouching()
    {
        if (animator.GetBool("isCrouching"))
        {
            animator.SetFloat("VerticalCrouching", Mathf.Clamp(Input.GetAxis("Vertical"), clampMin, clampMax));
            animator.SetFloat("HorizontalCrouching", Mathf.Clamp(Input.GetAxis("Horizontal"), clampMin, clampMax));
        }
    }
}
