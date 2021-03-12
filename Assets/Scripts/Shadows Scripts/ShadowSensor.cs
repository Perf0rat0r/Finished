using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSensor : MonoBehaviour
{
    public float fov = 120f;
    public float viewDistance = 10f;

    [HideInInspector] public bool targetFound;
    Transform raycastOrigin;
    Transform raycastPlayerDetector;
    ShadowAI shadowAIScript;
    Transform target;

    void Start()
    {
        raycastOrigin = transform.Find("RaycastOrigin").transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        raycastPlayerDetector = GameObject.FindGameObjectWithTag("RaycastPlayerDetector").transform;
        shadowAIScript = GetComponent<ShadowAI>();
    }

    void Update()
    {
        CheckForTarget();
    }


    public void CheckForTarget()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.position)) < fov / 2f)    //if target is within field of view angle
        {
            if (Vector3.Distance(target.position, transform.position) < viewDistance)   //if the target is within view of distance
            {
                RaycastHit hit; 
                if(Physics.Linecast(raycastOrigin.position, raycastPlayerDetector.position, out hit, -1))  //Create a raycast from object position to target position)
                {
                    Debug.DrawLine(raycastOrigin.position, raycastPlayerDetector.position, Color.red);
                    if (hit.transform.CompareTag("Player"))
                    {
        
                        TargetFound();
                    }
                }
            }
        }
    }

    public void TargetFound()
    {
        targetFound = true;
    }

    public void TargetLost()
    {
        targetFound = false;
    }
}
