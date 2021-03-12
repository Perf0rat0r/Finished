using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShadowAI : MonoBehaviour
{

    Vector3 targetLastPosition;
    bool shadowHeadsToLastSeenPosition;
    bool targetFound;

    public float attackPower = 5f;

    public enum AIState { idle, chasing, attack, chasingOnLastSeen };
   
    public AIState aiState = AIState.idle;

    NavMeshAgent navMeshAgent;
    Transform target;

    PlayerHealth playerHealthScript;
    ShadowSensor shadowSensorScript;

    Animator shadowAnimator;
    Animator playerAnimator;

    Volume volume;
    Vignette vignette;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        shadowSensorScript = GetComponent<ShadowSensor>();
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
                if (shadowSensorScript.targetFound)
                {
                    aiState = AIState.chasing;
                }
                //Idle code
                    shadowSensorScript.TargetLost();
                    shadowAnimator.SetBool("isChasing", false);
                    shadowAnimator.SetBool("isAttacking", false);
                    navMeshAgent.SetDestination(transform.position);
                break;

                case AIState.chasing:

                distance = Vector3.Distance(target.position, transform.position);
                if (distance > shadowSensorScript.viewDistance && !shadowHeadsToLastSeenPosition) // if distance is higher than view distance and memorizedPosition flag is turned off
                {
                    shadowHeadsToLastSeenPosition = true;
                    targetLastPosition = target.position;
                    navMeshAgent.SetDestination(targetLastPosition);
                    aiState = AIState.chasingOnLastSeen;
                }
                if (distance < 3)
                {
                    aiState = AIState.attack;
                }
                //Chasing code
                if (!shadowHeadsToLastSeenPosition)
                {
                    navMeshAgent.SetDestination(target.position);
                    shadowAnimator.SetBool("isChasing", true);
                    shadowAnimator.SetBool("isAttacking", false);
                }
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

                case AIState.chasingOnLastSeen:
                distance = Vector3.Distance(transform.position, targetLastPosition);
                if (distance <= 1)
                {
                    shadowHeadsToLastSeenPosition = false;
                    shadowSensorScript.TargetLost();
                    aiState = AIState.idle;
                }
                //Debug.Log(distance);
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