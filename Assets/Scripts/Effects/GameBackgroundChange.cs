using UnityEngine;

public class GameBackgroundChange : MonoBehaviour
{
    public static GameBackgroundChange Instance;

    [SerializeField] private SpriteRenderer gameBackground;
    [SerializeField] private Color newColor;

    void Awake()
    {
        Instance = this;
    }

    public void ApplyEffect()
    {
        gameBackground.color = newColor;
    }

    public void RemoveEffect()
    {
        gameBackground.color = Color.white;
    }
}
