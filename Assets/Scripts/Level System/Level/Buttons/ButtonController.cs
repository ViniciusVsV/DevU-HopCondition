using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Image buttonImage;
    private Color originalColor;

    void Awake()
    {
        button = GetComponent<Button>();
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

    public void Disable()
    {
        button.enabled = false;
        buttonImage.enabled = false;
    }

    public void Enable()
    {
        buttonImage.enabled = true;
        button.enabled = true;
    }
}
