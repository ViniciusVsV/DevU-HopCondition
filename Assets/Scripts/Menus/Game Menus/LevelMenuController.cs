using UnityEngine;

public class LevelMenuController : MonoBehaviour
{
    public static LevelMenuController Instance;

    private Animator levelMenuAnimator;
    [SerializeField] private Animator pauseButtonAnimator;

    private LevelsManager levelsManager;

    void Awake()
    {
        Instance = this;

        levelMenuAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        levelsManager = FindFirstObjectByType<LevelsManager>();
    }

    public void ActivateLevelMenu()
    {
        levelMenuAnimator.Play("Activating");
    }
    public void DeactivateLevelMenu()
    {
        levelMenuAnimator.Play("Deactivating");
    }

    public void ActivatePauseButton()
    {
        pauseButtonAnimator.Play("Activating");
    }
    public void DeactivatePausebutton()
    {
        pauseButtonAnimator.Play("Deactivating");
    }

    public void ResetCurrentLevel()
    {
        levelsManager.ResetCurrentLevel();
    }
}