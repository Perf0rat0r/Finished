using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAuditoryTrigger : MonoBehaviour
{
    Animator playerAnimator;
    ShadowSensor shadowSensorScript;

    void Start()
    {
        shadowSensorScript = gameObject.GetComponentInParent<ShadowSensor>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!playerAnimator.GetBool("isCrouching"))
            {
                shadowSensorScript.TargetFound();
            }
        }
    }
}
