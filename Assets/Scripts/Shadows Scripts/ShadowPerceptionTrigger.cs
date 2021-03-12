using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPerceptionTrigger : MonoBehaviour
{

    ShadowSensor shadowSensorScript;

    void Start()
    {
        shadowSensorScript = gameObject.GetComponentInParent<ShadowSensor>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
        shadowSensorScript.TargetFound();
        }
    }
}
