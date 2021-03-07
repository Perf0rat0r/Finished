using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowAI : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Transform target;

   public float distanceTreshold = 10f;

   public enum AIState { idle, chasing };
   
   public AIState aiState = AIState.idle;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
                    navMeshAgent.SetDestination(target.position);
                break;
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
}