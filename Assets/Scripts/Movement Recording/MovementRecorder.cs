using UnityEngine;

public class MovementRecorder : MonoBehaviour
{
    [SerializeField] private RecordedMovements recordedMovements;

    private float recordTimer;
    private float timeStamp;

    private void Update()
    {
        if (recordedMovements.isRecording)
        {
            recordTimer += Time.deltaTime;
            timeStamp += Time.deltaTime;

            if (recordTimer >= 1 / recordedMovements.recordFrequency)
            {
                recordedMovements.timeStamps.Add(timeStamp);
                recordedMovements.positions.Add(recordedMovements.activeCharacter.transform.position);

                recordTimer = 0f;
            }
        }
    }

    public void StartRecording(CharacterController chosenCharacter)
    {
        timeStamp = 0f;

        recordedMovements.ResetData();

        recordedMovements.activeCharacter = chosenCharacter;

        recordedMovements.isRecording = true;
        recordedMovements.isReplaying = false;
    }
}