using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance;

    private float currentTimeScale;

    public bool isPaused;

    void Awake()
    {
        Instance = this;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = currentTimeScale;

            isPaused = false;
        }
        else
        {
            isPaused = true;

            currentTimeScale = Time.timeScale;

            Time.timeScale = 0f;
        }
    }
}