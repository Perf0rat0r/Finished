using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutScene : MonoBehaviour
{ 
    public GameObject mainCharacter;

    public int textAreas;
    public TextMeshProUGUI[] text = new TextMeshProUGUI[1];
    [TextArea]
    public string[] sentences;
    public float typingSpeed;

    private int index;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("SkipHint", false);
            animator.SetTrigger("EndCutScene");
            mainCharacter.SetActive(true);
            Destroy(gameObject, 3);
        }
    }

   IEnumerator TypeEffect()
   {
      
       foreach(char letter in sentences[index].ToCharArray())
       {
           text[index].text += letter;
           yield return new WaitForSeconds(typingSpeed);
       }
           index++;
           animator.SetTrigger("NextImage");
           if (index >= textAreas)
           {
               animator.SetBool("SkipHint", true);
           }
   }

    void endCutScene()
    {
        animator.SetTrigger("EndCutScene");
    }

    void StopTime()
    {
        Time.timeScale = 0;
    }

    void DeactivatePlayer()
    {
        mainCharacter.SetActive(false);
    }
}
