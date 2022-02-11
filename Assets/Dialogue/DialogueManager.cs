using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;

    public string[] sentenceArray;

    public static DialogueManager instance;

    
    
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public TextReveal boxreveal;
    private Dialogue dialogue;

    [SerializeField]
    private Dialogue hoboDialogue;
    


    IEnumerator<string> TypeText(string textToType)
    {
        dialogueText.text = "";
        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void Awake()
    {
        sentences = new Queue<string>();
        //StartDialogue(dialogue);

        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void ClearDialogue()
    {
        sentences.Clear();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        
        sentences.Clear();
        
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    public void RestartDialogue(Dialogue dialogue)
    {
        if(sentences.Count == 0)
        {
            StartDialogue(dialogue);
        }
    }

    public void DisplayRandomSentence(Dialogue dialogue)
    {
        var rand = Random.Range(0, dialogue.sentences.Count);
        string sentence = (dialogue.sentences[rand]);
        //string sentence = (dialogue.sentences[rand]);
        //string sentence = sentences.Dequeue();
       // StopCoroutine(TypeText(dialogueText.text));
       // StartCoroutine(TypeText(dialogueText.text));
       StopCoroutine(TypeText(sentence));
       StartCoroutine(TypeText(sentence));
    }
    
    public void DisplayRandomSentence()
    {
        var rand = Random.Range(0, hoboDialogue.sentences.Count);
        string sentence = (hoboDialogue.sentences[rand]);
        //string sentence = (dialogue.sentences[rand]);
        //string sentence = sentences.Dequeue();
        // StopCoroutine(TypeText(dialogueText.text));
        // StartCoroutine(TypeText(dialogueText.text));
        // StopCoroutine(TypeText(sentence));
        // StartCoroutine(TypeText(sentence));
        dialogueText.text = sentence;
        boxreveal.StartTextReveal();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopCoroutine(TypeText(sentence));
        StartCoroutine(TypeText(sentence));
    }

    void EndDialogue()
    {
        Debug.Log($"End of conversation");
    }
}
