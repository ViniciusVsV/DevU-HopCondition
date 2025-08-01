using System.Net.Security;
using Unity.Cinemachine;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private MovementRecorder movementRecorder;
    private MovementReplayer movementReplayer;

    private CharacterController[] characterControllers;
    private CharacterController chosenCharacter;

    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void Start()
    {
        movementRecorder = FindFirstObjectByType<MovementRecorder>();
        movementReplayer = FindFirstObjectByType<MovementReplayer>();

        characterControllers = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SelectCharacter(characterControllers[0]);

        else if (Input.GetKeyDown(KeyCode.R))
            DesselectCharacter();

        else if (Input.GetKeyDown(KeyCode.Z))
            cinemachineCamera.Priority = 10;
    }

    public void SelectCharacter(CharacterController chosenCharacter)
    {
        this.chosenCharacter = chosenCharacter;

        movementRecorder.StartRecording(chosenCharacter.transform);
        movementReplayer.Setup();

        chosenCharacter.GetSelected();
    }

    public void DesselectCharacter()
    {
        chosenCharacter.GetDesselected();

        movementReplayer.StartReplaying();
    }
}