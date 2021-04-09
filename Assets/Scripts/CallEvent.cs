using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEvent : MonoBehaviour
{
  public void CallEventTrigger(string s)
    {
        AkSoundEngine.PostEvent(s, gameObject);
        Debug.Log("Print Event: " + s);
    }
}
