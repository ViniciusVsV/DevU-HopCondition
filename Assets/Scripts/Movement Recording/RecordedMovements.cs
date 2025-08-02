using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecordedMovements", menuName = "Scriptable Objects/RecordedMovements")]
public class RecordedMovements : ScriptableObject
{
    public float recordFrequency;

    public List<float> timeStamps;
    public List<Vector2> moveDirections;
    public List<bool> jumpPressed;

    public CharacterController activeCharacter;

    public bool isRecording;
    public bool isReplaying;

    private void OnEnable()
    {
        isRecording = false;
        isReplaying = false;
    }

    public void ResetData()
    {
        timeStamps.Clear();
        moveDirections.Clear();
        jumpPressed.Clear();

        activeCharacter = null;
    }
}