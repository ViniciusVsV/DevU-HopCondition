using UnityEngine;
using UnityEngine.UI;

public class ReplayMenuController : MonoBehaviour
{
    public static ReplayMenuController Instance;

    private Animator replayMenuAnimator;
    private MovementReplayer movementReplayer;
    private LevelsManager levelsManager;

    [SerializeField] private Color pressedColor;

    void Awake()
    {
        Instance = this;

        replayMenuAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        movementReplayer = FindFirstObjectByType<MovementReplayer>();
        levelsManager = FindFirstObjectByType<LevelsManager>();
    }

    public void ActivateReplayMenu()
    {
        replayMenuAnimator.Play("Activating");
    }
    public void DeactivateReplayMenu()
    {
        replayMenuAnimator.Play("Deactivating");
    }

    public void ToggleFastForward(Image buttonImage)
    {
        if (movementReplayer.isFastForwarded)
            buttonImage.color = Color.white;
        else
            buttonImage.color = pressedColor;

        movementReplayer.ToggleFastForward();
    }
    public void RestartReplay()
    {
        if (levelsManager.isReplaying)
            return;

        levelsManager.ResetAllButLastLevel();
        levelsManager.SetReplayingState(false, false, true);
    }
}