using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    public GameObject gameOver;
    public bool basicRegenTrigger = true;
    public float healthPower = 2f;

    private Volume volume;
    private Vignette vignette;

    private bool basicHealthRegenFlag = false;
    [HideInInspector] public bool basicHealthRegenCoroutineFlag = false;

    void Start()
    {
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        StartCoroutine(basicHealthRegen());
    }
    void Update()
    {
        if (!basicHealthRegenFlag && !basicRegenTrigger)    // if coroutine flag allows couroutine execution & regen option for player is disabled
        {
            StartCoroutine(basicHealthRegen());
            basicHealthRegenFlag = true;    // Don't allow coroutine to be RE-executed
        }
        if (vignette.intensity.value > 0.3f && basicRegenTrigger)
        {
        vignette.intensity.value -= ( healthPower * Time.deltaTime ) / 10;
        }

        if (vignette.intensity.value >= 1f)
        {
            gameOver.SetActive(true);
        }
        
    }

    IEnumerator basicHealthRegen()
    {

        yield return new WaitForSeconds(10f);
        if(basicHealthRegenCoroutineFlag)   // if Coroutine is RE-executed 
        {
            basicHealthRegenCoroutineFlag = false;  // Reset RE-execution bool
            basicHealthRegenFlag = false;   // Reset coroutine-execution permission
            Debug.Log("Coroutine Breaked");
            yield break;
        }
        Debug.Log("Coroutine Executed");
        basicRegenTrigger = true;
        basicHealthRegenFlag = false;
    }
}
