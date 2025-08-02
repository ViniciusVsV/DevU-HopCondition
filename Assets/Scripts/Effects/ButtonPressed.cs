using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = sprite;
    }
}
