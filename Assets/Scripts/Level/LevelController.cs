using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelController : MonoBehaviour
{
    [SerializeField] private RecordedMovements recordedMovements;
    private MovementRecorder movementRecorder;
    private MovementReplayer movementReplayer;

    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CharacterController character;

    private PlayerInput playerInput;
    private LevelButtonsController levelButtonsController;


    [SerializeField] private List<GameObject> entities = new();

    void Awake()
    {
        movementRecorder = FindFirstObjectByType<MovementRecorder>();
        movementReplayer = FindFirstObjectByType<MovementReplayer>();

        playerInput = character.GetComponent<PlayerInput>();
    }

    void Start()
    {
        levelButtonsController = FindFirstObjectByType<LevelButtonsController>();
    }

    public void SelectLevel()
    {
        //Aumenta prioridade da câmera
        cinemachineCamera.Priority = 10;

        //Define o personagem da fase como o principal
        recordedMovements.activeCharacter = character.transform;

        //Faz o setup do replayer
        movementReplayer.Setup();

        //Começa a gravar os movimentos
        movementRecorder.StartRecording(character.transform);

        StartCoroutine(SelectRoutine());
    }

    private IEnumerator SelectRoutine()
    {
        yield return new WaitForSeconds(1.3f);

        //Ativa booleana do controlador do personagem
        character.isActive = true;

        //Ativa o input do jogador
        playerInput.enabled = true;
    }

    public void DesselectLevel()
    {
        cinemachineCamera.Priority = 0;

        playerInput.enabled = false;

        character.isActive = false;

        movementReplayer.StartReplaying();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            ResetLevel();
    }

    private void ResetLevel()
    {
        foreach (GameObject entity in entities)
        {
            IReset reset = entity.GetComponent<IReset>();

            reset.Reset();
        }
    }
}