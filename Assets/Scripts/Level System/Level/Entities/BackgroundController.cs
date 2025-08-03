using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class BackgroundController : MonoBehaviour, IReset
{
    private SpriteRenderer backgroundImage;
    //[SerializeField] private Color levelCompletedColor;
    //[SerializeField] private Color levelFailedColor;
    [SerializeField]private Sprite sprCompleted;
    [SerializeField]private Sprite sprFailed;

    [Range(0, 255)][SerializeField] private float alpha;

    void Awake()
    {
        backgroundImage = GetComponent<SpriteRenderer>();
    }

    public void SetCompletedColor()
    {

        //Color aux = levelCompletedColor;

        //aux.a = alpha / 255f;
        //backgroundImage.color = aux;

        Color aux = Color.white;
        aux.a = alpha / 255f;

        backgroundImage.color = aux;
        backgroundImage.sprite = sprCompleted;
    }

    public void SetFailedColor()
    {
        /*
        Color aux = levelFailedColor;

        aux.a = alpha / 255f;
        backgroundImage.color = aux;
        */
        Color aux = Color.white;
        aux.a = alpha / 255f;

        backgroundImage.color = aux;
        backgroundImage.sprite = sprFailed;

        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.failSound, transform, 1f);
    }

    public void _Reset(bool reactivate)
    {
        Color c = Color.white;

        c.a = 0f;
        backgroundImage.color = c;
    }
}