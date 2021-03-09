using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShadowAI : MonoBehaviour
{

    private Volume volume;
    private Vignette vignette;

    public float attackPower = 5f;
    public float distanceTreshold = 10f;

    public enum AIState { idle, chasing, attack };
   
    public AIState aiState = AIState.idle;

    NavMeshAgent navMeshAgent;
    Transform target;
    PlayerHealth playerHealthScript;
    Animator shadowAnimator;
    Animator playerAnimator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        shadowAnimator = GetComponent<Animator>();
        StartCoroutine(Think());
        
    }


    IEnumerator Think()
    {
        while(true)
        {
            switch (aiState)
            {
                case AIState.idle:
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance < distanceTreshold && !playerAnimator.GetBool("isCrouching"))
                {
                    aiState = AIState.chasing;
                }
                //Idle code
                    shadowAnimator.SetBool("isChasing", false);
                    shadowAnimator.SetBool("isAttacking", false);
                    navMeshAgent.SetDestination(transform.position);
                break;

                case AIState.chasing:
                distance = Vector3.Distance(target.position, transform.position);
                if (distance > distanceTreshold)
                {
                    aiState = AIState.idle;
                }
                if (distance < 3)
                {
                    aiState = AIState.attack;
                }
                //Chasing code
                    navMeshAgent.SetDestination(target.position);
                    shadowAnimator.SetBool("isChasing", true);
                    shadowAnimator.SetBool("isAttacking", false);
                break;

                case AIState.attack:
                distance = Vector3.Distance(target.position, transform.position);
                if (distance > 3)
                {
                    aiState = AIState.chasing;
                }
                //Atacking code
                shadowAnimator.SetBool("isAttacking", true);
                shadowAnimator.SetBool("isChasing", false);
                
                break;
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    void Attack()
    {
        vignette.intensity.value += attackPower/10; // Damaging player
        playerHealthScript.basicRegenTrigger = false; // Deactivating regen option for player
        playerHealthScript.basicHealthRegenCoroutineFlag = true; // Triggering Regen Courutione RE-execution
    }
}