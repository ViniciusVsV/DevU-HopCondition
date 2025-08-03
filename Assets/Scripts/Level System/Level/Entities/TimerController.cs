using System;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour, IActivate, IReset
{
    [SerializeField] private float timeLimit;
    [SerializeField] private CarrotController carrot;
    private float timeLimitTimer;

    [SerializeField] private TextMeshPro text;

    public bool isActive;

    private int minutes;
    private int seconds;

    void Awake()
    {
        timeLimitTimer = timeLimit;
    }

    void Update()
    {
        if (!isActive)
            return;

        if (carrot.collected)
            Deactivate();

        if (timeLimitTimer <= 0f)
        {
            timeLimitTimer = 0;

            carrot.Disable();

            minutes = Mathf.FloorToInt(timeLimitTimer / 60f);
            seconds = Mathf.FloorToInt(timeLimitTimer % 60f);

            text.text = $"{minutes}:{seconds:00}";

            return;
        }

        timeLimitTimer -= Time.deltaTime;

        minutes = Mathf.FloorToInt(timeLimitTimer / 60f);
        seconds = Mathf.FloorToInt(timeLimitTimer % 60f);

        text.text = $"{minutes}:{seconds:00}";
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void _Reset(bool reactivate)
    {
        isActive = false;

        timeLimitTimer = timeLimit;

        text.text = "1:00";

        if (reactivate)
            isActive = true;
    }
}
