using UnityEngine;
using UnityEngine.EventSystems;

public class GameConfigsMenuController : MonoBehaviour, IPointerExitHandler
{
    private Animator animator;

    public bool activated;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        activated = true;

        animator.SetBool("activated", activated);
    }

    public void Deactivate()
    {
        activated = false;

        animator.SetBool("activated", false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        activated = false;

        animator.SetBool("activated", activated);
    }
}