using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShadowAI : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Transform target;
    PlayerHealth playerHealthScript;

    private Volume volume;
    private Vignette vignette;

    public float attackPower = 5f;
    public float distanceTreshold = 10f;

    public enum AIState { idle, chasing, attack };
   
    public AIState aiState = AIState.idle;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        StartCoroutine("Think");
    }


    IEnumerator Think()
    {
        while(true)
        {
            switch (aiState)
            {
                case AIState.idle:
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance < distanceTreshold)
                {
                    aiState = AIState.chasing;
                }
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
                    navMeshAgent.SetDestination(target.position);
                break;

                case AIState.attack:
                distance = Vector3.Distance(target.position, transform.position);
                if (distance > 3)
                {
                    aiState = AIState.chasing;
                }
                vignette.intensity.value += attackPower; // Damaging player
                playerHealthScript.basicRegenTrigger = false; // Deactivating regen option for player
                playerHealthScript.basicHealthRegenCoroutineFlag = true; // Triggering Regen Courutione RE-execution
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}