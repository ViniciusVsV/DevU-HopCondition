using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameConfigsMenuController configsMenu;

    private MovementReplayer movementReplayer;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        movementReplayer = FindFirstObjectByType<MovementReplayer>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("activated", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (configsMenu.activated)
            return;

        animator.SetBool("activated", false);
    }

    public void ToggleFastForward()
    {
        movementReplayer.ToggleFastForward();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("InitialMenu");
    }

    public void OpenConfigs()
    {
        configsMenu.Activate();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
}