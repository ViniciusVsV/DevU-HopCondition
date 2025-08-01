using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelController : MonoBehaviour
{
    [SerializeField] private RecordedMovements recordedMovements;
    private MovementRecorder movementRecorder;
    private MovementReplayer movementReplayer;

    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private GameObject character;

    private PlayerInput playerInput;

    void Awake()
    {
        movementRecorder = FindFirstObjectByType<MovementRecorder>();
        movementReplayer = FindFirstObjectByType<MovementReplayer>();

        playerInput = character.GetComponent<PlayerInput>();
    }

    public void SelectLevel()
    {
        //Aumenta prioridade da câmera
        cinemachineCamera.Priority = 10;

        //Define o personagem da fase como o principal
        recordedMovements.activeCharacter = character.transform;

        //Ativa o input do jogador
        playerInput.enabled = true;

        //Faz o setup do replayer
        movementReplayer.Setup();

        //Começa a gravar os movimentos
        movementRecorder.StartRecording(character.transform);
    }

    public void DesselectLevel()
    {
        cinemachineCamera.Priority = 0;

        playerInput.enabled = false;

        movementReplayer.StartReplaying();
    }
}