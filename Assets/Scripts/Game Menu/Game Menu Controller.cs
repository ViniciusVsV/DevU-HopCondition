using UnityEngine;
using UnityEngine.EventSystems;

public class GameMenuController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("activated", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("activated", false);
    }
}