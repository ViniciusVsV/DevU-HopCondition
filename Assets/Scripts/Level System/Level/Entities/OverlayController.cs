using UnityEngine;

public class OverlayController : MonoBehaviour, IReset
{
    private SpriteRenderer backgroundSprite;

    [SerializeField] private Sprite sprCompleted;
    [SerializeField] private Sprite sprFailed;

    void Awake()
    {
        backgroundSprite = GetComponent<SpriteRenderer>();
    }

    public void SetCompletedColor()
    {
        backgroundSprite.sprite = sprCompleted;
    }

    public void SetFailedColor()
    {
        if (backgroundSprite.sprite == sprFailed)
            return;

        backgroundSprite.sprite = sprFailed;

        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.failSound, transform, 1f);
    }

    public void _Reset(bool reactivate)
    {
        backgroundSprite.sprite = null;
    }
}