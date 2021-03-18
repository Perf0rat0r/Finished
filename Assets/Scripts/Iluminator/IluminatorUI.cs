using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IluminatorUI : MonoBehaviour
{
    public GameObject iluminatorUI;

    public void EnableUIObject()
    {
        iluminatorUI.SetActive(true);
    }

    public void DisableUIObject()
    {
        iluminatorUI.SetActive(false);
    }
}
