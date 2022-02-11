using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextReveal : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float textRevealSpeed;
    
    private bool hasTextChanged;
    private TMP_Text textField;

    public delegate void RevealDone();

    public static event RevealDone OnRevealDone;
    

    void Awake()
    {
        textField = gameObject.GetComponent<TMP_Text>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    public void StartTextReveal()
    {
        StopCoroutine(RevealCharacters(textField));
        StartCoroutine(RevealCharacters(textField));
    }
    
    IEnumerator RevealCharacters(TMP_Text textComponent)
    {
        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (true)
        {
            if (hasTextChanged)
            {
                totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                hasTextChanged = false; 
            }

            if (visibleCount > totalVisibleCharacters)
            {
                yield return new WaitForSeconds(2.0f);
                OnRevealDone?.Invoke();
                yield break;
            }

            textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return new WaitForSeconds(textRevealSpeed);
        }
    }
}
