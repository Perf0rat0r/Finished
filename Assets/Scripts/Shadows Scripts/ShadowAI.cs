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
    bool targetCatched;

    public bool patrol;
    public Transform[] patrolPoints;

    public bool guardPos;
    public Transform guardPosition;

    public float attackPower = 5f;
    public float waitingTimeAfterLoosingPlayer = 6f;
    public float waitingOnPatrolPoint = 8f;

    public enum AIState { idle, chasing, attack, chasingOnLastSeen, guardPosReturn, patrol, receiveAttack };
   
    AIState aiState = AIState.idle;

    NavMeshAgent navMeshAgent;
    Transform target;

    PlayerHealth playerHealthScript;
    ShadowSensor shadowSensorScript;

    Animator shadowAnimator;
    Animator playerAnimator;
    Animator handsAnimator;

    Volume volume;
    Vignette vignette;

    float maxTimeSet;
    int currentControlPointIndex = 0;
    float timePassed;
    float distance;
    float guardPosDistance;
    float patrolPointDistance;
    int switchCaseDistance = 2;

    //Declarative numbers
    int one = 1;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        shadowSensorScript = GetComponent<ShadowSensor>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        shadowAnimator = GetComponent<Animator>();
        StartCoroutine(Think());
        CalculateTime();
    }

    void Update()
    {
        if (targetCatched)
        {
            Attack();
        }
    }

    IEnumerator Think()
    {
        while(true)
        {
            switch (aiState)
            {
                case AIState.idle:
                distance = Vector3.Distance(target.position, transform.position);
                guardPosDistance = Vector3.Distance(guardPosition.position, transform.position);
                if (shadowSensorScript.targetFound)
                {
                    aiState = AIState.chasing;
                }
                //Idle code
                    shadowSensorScript.TargetLost();
                    shadowAnimator.SetBool("isChasing", false);
                    shadowAnimator.SetBool("isAttacking", false);
                    navMeshAgent.SetDestination(transform.position);
                //Calculating Time
                yield return new WaitForSeconds(1);
                if (timePassed < maxTimeSet)
                {
                    timePassed += one;
                    yield return null;
                }
                //Debug.Log(timePassed);
                if (guardPosDistance > one && timePassed >= waitingTimeAfterLoosingPlayer && guardPos)
                {
                    aiState = AIState.guardPosReturn;
                }
                if (timePassed >= waitingOnPatrolPoint && patrol)
                {
                    if (patrolPoints.Length > 0)
                    {
                        navMeshAgent.SetDestination(patrolPoints[currentControlPointIndex].position);
                        aiState = AIState.patrol;
                    }
                }
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
                if (distance < switchCaseDistance)
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
                if (distance > switchCaseDistance)
                {
                    aiState = AIState.chasing;
                }
                //Atacking code
                shadowAnimator.SetBool("isAttacking", true);
                shadowAnimator.SetBool("isChasing", false);
                
                break;

                case AIState.chasingOnLastSeen:
                distance = Vector3.Distance(transform.position, targetLastPosition);
                if (distance <= one)
                {
                    shadowHeadsToLastSeenPosition = false;
                    shadowSensorScript.TargetLost();
                    timePassed = 0;
                    aiState = AIState.idle;
                }
                //Debug.Log(distance);
                break;

                case AIState.guardPosReturn:
                distance = Vector3.Distance(target.position, transform.position);
                guardPosDistance = Vector3.Distance(guardPosition.position, transform.position);
                if ( guardPosDistance <= one)
                {
                aiState = AIState.idle;
                }
                if (shadowSensorScript.targetFound)
                {
                    aiState = AIState.chasing;
                }
                if (distance < switchCaseDistance)
                {
                    aiState = AIState.attack;
                }
                shadowAnimator.SetBool("isChasing", true);
                navMeshAgent.SetDestination(guardPosition.position);
                break;

                case AIState.patrol:
                patrolPointDistance = Vector3.Distance(transform.position, patrolPoints[currentControlPointIndex].position);
                if (shadowSensorScript.targetFound)
                {
                    aiState = AIState.chasing;
                }
                if (distance < switchCaseDistance)
                {
                    aiState = AIState.attack;
                }
                if (patrolPointDistance <= one)
                {
                    currentControlPointIndex++;
                    currentControlPointIndex %= patrolPoints.Length;
                    timePassed = 0;
                    aiState = AIState.idle;
                }
                shadowAnimator.SetBool("isChasing", true);
                break;
                
                case AIState.receiveAttack:

                break;
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    void AttackTrigger()
    {
        targetCatched = true;
        playerAnimator.SetBool("isDead", true);
    }

    void Attack()
    {
        vignette.intensity.value += attackPower/50; // Damaging player

        // Script for Regeneration System
        /*  playerHealthScript.basicRegenTrigger = false; // Deactivating regen option for player
            playerHealthScript.basicHealthRegenCoroutineFlag = true; // Triggering Regen Courutione RE-execution */
    }

    void CalculateTime()
    {
        if (waitingTimeAfterLoosingPlayer > waitingOnPatrolPoint)
        {
            maxTimeSet = waitingTimeAfterLoosingPlayer;
        }
        else
        {
            maxTimeSet = waitingOnPatrolPoint;
        }
    }
}