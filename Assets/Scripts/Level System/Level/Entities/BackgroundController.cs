using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour, IReset
{
    private SpriteRenderer backgroundImage;
    [SerializeField] private Color levelCompletedColor;
    [SerializeField] private Color levelFailedColor;

    void Awake()
    {
        backgroundImage = GetComponent<SpriteRenderer>();
    }

    public void SetCompletedColor()
    {
        backgroundImage.color = levelCompletedColor;
    }

    public void SetFailedColor()
    {
        backgroundImage.color = levelFailedColor;
    }

    public void Reset()
    {
        backgroundImage.color = Color.white;
    }
}