using System.Collections.Generic;
using UnityEngine;

public class MovementReplayer : MonoBehaviour
{
    [SerializeField] private RecordedMovements recordedMovements;

    public List<Transform> characterTransforms = new();
    public List<Vector2> initialDisplacements = new();

    private float timeStamp;
    private int index1, index2;

    private void Update()
    {
        if (recordedMovements.isReplaying)
        {
            timeStamp += Time.deltaTime;

            GetIndexes();
            ApplyMovements();
        }
    }

    public void Setup()
    {
        CharacterController[] characters = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);

        foreach (CharacterController character in characters)
        {
            if (character.transform == recordedMovements.activeCharacter)
                continue;

            characterTransforms.Add(character.transform);
            initialDisplacements.Add(character.transform.position - recordedMovements.activeCharacter.position);
        }
    }

    public void StartReplaying()
    {
        timeStamp = 0f;

        recordedMovements.isRecording = false;
        recordedMovements.isReplaying = true;
    }

    private void GetIndexes()
    {
        for (int i = 0; i < recordedMovements.timeStamps.Count - 1; i++)
        {
            if (recordedMovements.timeStamps[i] <= timeStamp && timeStamp < recordedMovements.timeStamps[i + 1])
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = recordedMovements.timeStamps.Count - 1;
        index2 = recordedMovements.timeStamps.Count - 1;
    }

    private void ApplyMovements()
    {
        for (int i = 0; i < characterTransforms.Count; i++)
        {
            if (index1 == index2)
                characterTransforms[i].position = recordedMovements.positions[index1] + initialDisplacements[i];

            else
            {
                float aux1 = recordedMovements.timeStamps[index1];
                float aux2 = recordedMovements.timeStamps[index2];

                float interpolationFactor = (timeStamp - aux1) / (aux2 - aux1);

                characterTransforms[i].position = Vector2.Lerp
                (
                    recordedMovements.positions[index1] + initialDisplacements[i],
                    recordedMovements.positions[index2] + initialDisplacements[i],
                    interpolationFactor
                );
            }
        }
    }
}