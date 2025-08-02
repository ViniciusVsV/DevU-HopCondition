using UnityEngine;

public class MovementRecorder : MonoBehaviour
{
    [SerializeField] private RecordedMovements recordedMovements;

    private float recordTimer;
    private float timeStamp;

    public bool waitingForInput;

    private void Update()
    {
        if (waitingForInput)
        {
            if (recordedMovements.activeCharacter.moveDirection != Vector2.zero || recordedMovements.activeCharacter.jumpPressed)
            {
                recordedMovements.isRecording = true;
                waitingForInput = false;
            }
            else
                return;
        }

        if (recordedMovements.isRecording)
        {
            recordTimer += Time.deltaTime;
            timeStamp += Time.deltaTime;

            if (recordTimer >= 1 / recordedMovements.recordFrequency)
            {
                recordedMovements.timeStamps.Add(timeStamp);
                recordedMovements.moveDirections.Add(recordedMovements.activeCharacter.moveDirection);
                recordedMovements.jumpPressed.Add(recordedMovements.activeCharacter.jumpPressed);

                recordTimer = 0f;
            }
        }
    }

    public void StartRecording(CharacterController chosenCharacter)
    {
        timeStamp = 0f;
        recordTimer = 0f;

        recordedMovements.ResetData();

        recordedMovements.activeCharacter = chosenCharacter;

        recordedMovements.isReplaying = false;

        waitingForInput = true;
    }
}