using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
            Debug.Log($"Scene reloaded {SceneManager.GetActiveScene().buildIndex}");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
            Debug.Log("Game closed");
        }
    }
}
