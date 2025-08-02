using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int frameRateCap = 120;
    [SerializeField] private bool disableVSync = true;

    void Awake()
    {
        QualitySettings.vSyncCount = disableVSync ? 1 : 0;

        Application.targetFrameRate = frameRateCap;
    }
}
