using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance;

    private Animator animator;
    private float savedTimeScale;

    public bool isPaused;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject blocker;

    void Awake()
    {
        Instance = this;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        if (isPaused)
        {
            animator.Play("Deactivating");

            Time.timeScale = savedTimeScale;

            isPaused = false;

            blocker.SetActive(false);
        }
        else
        {
            blocker.SetActive(true);

            animator.Play("Activating");

            isPaused = true;

            savedTimeScale = Time.timeScale;

            Time.timeScale = 0f;
        }
    }
}