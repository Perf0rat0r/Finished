using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;

    Volume volume;
    Vignette vignette;

    void Start()
    {
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
    }

    void Update()
    {
        if (vignette.intensity.value >= 1)
        {
            gameOverUI.SetActive(true);
        }
    }
}
