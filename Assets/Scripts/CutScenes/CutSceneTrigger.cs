using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CutSceneTrigger : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("StartCutScene");  
    }

}
