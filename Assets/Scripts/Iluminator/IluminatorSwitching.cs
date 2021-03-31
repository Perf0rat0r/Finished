using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IluminatorSwitching : MonoBehaviour
{
    
    public GameObject iluminator;
    public GameObject iluminatorBack;
    public Rig rightHandRigLayer;
    public IluminatorUI iluminatorUIScript;
    

    private bool iluminatorEquiped = false;

    Animator playerAnimatorController;
    CharacterController characterController;

    void Start()
    {
        playerAnimatorController = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if ( Input.GetKeyDown("e") && !iluminatorEquiped )
        {
            StaffOn();
            
        }
        else
        if ( Input.GetKeyDown("e") && iluminatorEquiped )
        {
            StaffOff();
        }
        if (iluminatorEquiped)
        {
            rightHandRigLayer.weight = 1;
        }
        else 
        {
            rightHandRigLayer.weight = 0;
        }

    }

    public void StaffOn()
    {
        iluminatorEquiped = true;
        iluminator.SetActive(true);
        iluminatorBack.SetActive(false);
        rightHandRigLayer.weight = 1;
        iluminatorUIScript.DisableUIObject();

        characterController.radius = 0.65f;
        characterController.center = new Vector3(0, 0.925f, 0.4f);
    }

    public void StaffOff()
    {
        iluminatorEquiped = false;
        iluminator.SetActive(false);
        iluminatorBack.SetActive(true);
        rightHandRigLayer.weight = 0;
        iluminatorUIScript.EnableUIObject();

        characterController.radius = 0.3f;
        characterController.center = new Vector3(0, 0.925f, 0);
    }
}
