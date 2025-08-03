using UnityEngine;

public class LevelMenuController : MonoBehaviour
{
    public static LevelMenuController Instance;

    private Animator levelMenuAnimator;

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

    public void ResetCurrentLevel()
    {
        levelsManager.ResetCurrentLevel();
    }
}