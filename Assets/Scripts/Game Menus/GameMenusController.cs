using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenusController : MonoBehaviour
{
    [Header("-------Level Menu-------")]
    [SerializeField] private Animator levelMenuAnimator;

    [Header("-------Replay Menu-------")]
    [SerializeField] private Animator replayMenuAnimator;

    public void ActivateLevelMenu()
    {
        levelMenuAnimator.Play("Activating");
    }
    public void DeactivateLevelMenu()
    {
        levelMenuAnimator.Play("Deactivating");
    }

    public void ActivateReplayMenu()
    {
        replayMenuAnimator.Play("Activating");
    }
    public void DeactivateReplayMenu()
    {
        replayMenuAnimator.Play("Deactivating");
    }

    public void ToggleFastForward()
    {
        Debug.Log("Fast forward!");
    }
    public void RestartReplay()
    {
        Debug.Log("Restart do replay!");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}