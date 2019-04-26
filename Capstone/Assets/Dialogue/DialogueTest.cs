using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTest : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject dialogueC;
    public GameObject gameManager;
    public GameObject continueButton;
    public Animator animator;
    public GameObject trigger;

    public IEnumerator Type()
    {
        //dialogueC.SetActive(true);
        animator.SetBool("IsOpen", true);
        foreach (char letter in sentences[index].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);

        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            gameManager.GetComponent<PauseGame>().combatActive = true;
            textDisplay.text = "";
            animator.SetBool("IsOpen", false);
            trigger.SetActive(false);
            dialogueC.SetActive(false);
        }
    }
	
	void Start () {
        //StartCoroutine(Type());
	}

    void Update() {

        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
	}

    public void EndConvo()
    {
        StopCoroutine(Type());
        GetComponent<EnemyMarauder>().moveSpeed = 2.5f;
    }
}
