using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneMachine : MonoBehaviour
{
    [SerializeField] private GameEvent endCutsceneEvent;
    
    [SerializeField]
    private TMP_Text textField;

    [SerializeField]
    private Image slide;

    [SerializeField]
    private RectTransform textBox;

    [SerializeField]
    private RectTransform[] positions;

    [SerializeField]
    private CutsceneSlide[] sequence;

    private int cur_index = 0;
    private const int MAIN_GAME_INDEX = 2;

    private TextReveal textCrawler;

    private int slideCount;

    private void OnEnable()
    {
        TextReveal.OnRevealDone += DisplayNextSlide;
    }

    private void OnDisable()
    {
        TextReveal.OnRevealDone -= DisplayNextSlide;
    }

    // Start is called before the first frame update
    void Start()
    {
        textCrawler = textField.GetComponent<TextReveal>();
        
        cur_index = 0;
        DisplaySlide(cur_index);
    }

    private void DisplaySlide(int i)
    {
        textBox.gameObject.SetActive(true);
        var currentSlide = sequence[cur_index];
        slide.sprite = currentSlide.spriteGraphic;

        textBox.position = currentSlide.textLocation switch
        {
            TextBoxLocation.Top => positions[0].position,
            TextBoxLocation.Bottom => positions[1].position,
            TextBoxLocation.Left => positions[2].position,
            TextBoxLocation.Right => positions[3].position,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        if (currentSlide.text.Length == 0)
        {
            textField.enabled = false;
            textBox.gameObject.SetActive(false);
            DisplayNextSlide();
        }
        else
        {
            
            textField.text = currentSlide.text;
            textCrawler.StartTextReveal();
        }
        
        //StartCoroutine(QueueNextSlide(currentSlide.pauseTime));
    }

    private void DisplayNextSlide()
    {
        StartCoroutine(QueueNextSlide(sequence[cur_index].pauseTime));
        // if (cur_index >= sequence.Length - 1)
        // {
        //     SceneManager.LoadScene(MAIN_GAME_INDEX);
        // }
        // else
        // {
        //     DisplaySlide(cur_index++);
        // }
    }

    IEnumerator QueueNextSlide(float time)
    {
        yield return new WaitForSeconds(time);
        if (cur_index >= sequence.Length - 1)
        {
            EndSlide();
        }
        else
        {
            DisplaySlide(cur_index++);
        }

        slideCount++;
        if (slideCount >= sequence.Length)
        {
            if (endCutsceneEvent != null)
            {
                endCutsceneEvent.Raise();
                Debug.Log("end cutscene");
            }
        }
    }

    public void EndSlide()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if ( currentScene == 3)
            SceneManager.LoadScene(0);
        else if (currentScene == 1)
            SceneManager.LoadScene(MAIN_GAME_INDEX);
        else if (currentScene == 2)
        {
            gameObject.SetActive(false);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            EndSlide();
        }
        
    }
}
