using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetColor(Color.blue);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}