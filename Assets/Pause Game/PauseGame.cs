using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused;
    [SerializeField]private SimpleOrbitalCamera _camera;
    [SerializeField] private GameObject mainMenu;
    

    

   public void Pause()
    {
        Time.timeScale = 0;
        TimeManager.Instance.enabled = false;
        _camera.enabled = false;
        mainMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log($"Game paused");
    }

   public void ResumeGame()
    {
        Time.timeScale = 1;
        TimeManager.Instance.enabled = true;
        _camera.enabled = true;
        mainMenu.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log($"Game resumed");
    }

   

   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Pause();
            }
            else
            {
                ResumeGame();
            }
        }
    }
}
