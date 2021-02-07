using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    public Animator animator;
    public GameObject mainCharacter;
    private MainCharacterLocomotion mainChrLoc;

    void Start()
    {
        mainChrLoc = mainCharacter.GetComponent<MainCharacterLocomotion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("StartCutScene");
    }
}
