using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour, IReset
{
    private SpriteRenderer backgroundImage;
    [SerializeField] private Color levelCompletedColor;
    [SerializeField] private Color levelFailedColor;

    [Range(0, 255)][SerializeField] private float alpha;

    void Awake()
    {
        backgroundImage = GetComponent<SpriteRenderer>();
    }

    public void SetCompletedColor()
    {
        Color aux = levelCompletedColor;

        aux.a = alpha / 255f;
        backgroundImage.color = aux;
    }

    public void SetFailedColor()
    {
        Color aux = levelFailedColor;

        aux.a = alpha / 255f;
        backgroundImage.color = aux;
    }

    public void Reset()
    {
        Color c = Color.white;

        c.a = 0f;
        backgroundImage.color = c;
    }
}