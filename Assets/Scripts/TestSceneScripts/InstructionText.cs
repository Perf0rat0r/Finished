using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionText : MonoBehaviour
{
    public GameObject instructionText;

    public bool instructionTextFlag = true;

    void Update()
    {
        if (Input.GetKeyDown("h") && instructionTextFlag)
        {
            DeActivateInstructionText();
            instructionTextFlag = false;
        }
        else
        if (Input.GetKeyDown("h") && !instructionTextFlag)
        {
            ActivateInstructionText();
            instructionTextFlag = true;
        }
        
    }
        void ActivateInstructionText()
        {
            instructionText.gameObject.SetActive(true);
        }
        void DeActivateInstructionText()
        {
            instructionText.gameObject.SetActive(false);
        }
}
