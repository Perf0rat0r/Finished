using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    public Animator animator;
    public GameObject mainCharacter;
    
    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("StartCutScene");
    }

    IEnumerator DeactivatePlayer(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
