using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IluminatorSwitching : MonoBehaviour
{
    
    public GameObject iluminator;
    public Rig rightHandRigLayer;

    private bool iluminatorEquiped = false;

    void Update()
    {
        if ( Input.GetKeyDown("e") && !iluminatorEquiped )
        {
            iluminatorEquiped = true;
            iluminator.SetActive(true);
            rightHandRigLayer.weight = 1;
        }
        else
        if ( Input.GetKeyDown("e") && iluminatorEquiped )
        {
            iluminatorEquiped = false;
            iluminator.SetActive(false);
            rightHandRigLayer.weight = 0;
        }
        

    }
}
