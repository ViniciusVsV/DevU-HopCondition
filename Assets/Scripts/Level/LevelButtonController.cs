using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Color originalColor;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color newColor = originalColor;
        newColor.a = 0.3f;

        buttonImage.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = originalColor;
    }
}
