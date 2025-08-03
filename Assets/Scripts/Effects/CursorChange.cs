using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public static CursorChange Instance;

    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D deactivatedCursor;

    void Awake()
    {
        Instance = this;
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }
    public void SetDeactivatedCursor()
    {
        Cursor.SetCursor(deactivatedCursor, Vector2.zero, CursorMode.Auto);
    }
}
