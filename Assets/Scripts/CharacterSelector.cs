using System.Net.Security;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private MovementRecorder movementRecorder;
    private MovementReplayer movementReplayer;

    private CharacterController[] characterControllers;
    private CharacterController chosenCharacter;

    //Ao escolher um personagem:
    //Desativar os inputs dos outros personagens
    //Desativar os recorders dos outros personagens
    //Desativar o replayer do personagem selecionado
    //Setar o personagem selecionado no scriptable object

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