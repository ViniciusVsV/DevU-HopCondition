using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementReplayer : MonoBehaviour
{
    [SerializeField] private RecordedMovements recordedMovements;

    public List<CharacterController> characterControllers = new();

    private float replayTimer;
    private int currentIndex;

    [SerializeField] private float fastForwardSpeed = 2f;
    public bool isFastForwarded;

    public UnityEvent replayFinished;

    //Furtado mudancas para o som
    public bool isReplayingNow = false;

    private void Update()
    {
        if (recordedMovements.isReplaying)
        {
            Time.timeScale = isFastForwarded ? fastForwardSpeed : 1f;

            if (replayTimer >= recordedMovements.timeStamps[^1])
            {
                recordedMovements.isReplaying = false;

                replayFinished.Invoke();

                return;
            }

            replayTimer += Time.deltaTime;

            while (currentIndex < recordedMovements.timeStamps.Count &&
                recordedMovements.timeStamps[currentIndex] <= replayTimer)
            {
                Vector2 moveDir = recordedMovements.moveDirections[currentIndex];
                bool jumpPressed = recordedMovements.jumpPressed[currentIndex];

                foreach (var controller in characterControllers)
                {
                    controller.moveDirection = moveDir;

                    if (jumpPressed)
                        controller.ApplyJump();
                }

                currentIndex++;
            }
        }
        else
            Time.timeScale = 1f;

        //Mudanacas Som
       isReplayingNow = recordedMovements.isReplaying;
}

    ///Permite passar os personagens que serão afetados pelo replay
    public void Setup(List<CharacterController> replayedCharacters)
    {
        characterControllers = replayedCharacters;

        characterControllers.Remove(recordedMovements.activeCharacter);
    }

    public void StartReplaying()
    {
        replayTimer = 0f;
        currentIndex = 0;

        recordedMovements.isRecording = false;
        recordedMovements.isReplaying = true;
    }

    public void ToggleFastForward()
    {
        isFastForwarded = !isFastForwarded;
    }
}