using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CharacterController character;

    private PlayerInput playerInput;

    [SerializeField] private List<GameObject> entities = new();

    public bool carrotReached;

    public UnityEvent<CharacterController> levelSelected;
    public UnityEvent<bool> levelFinished;

    void Awake()
    {
        playerInput = character.GetComponent<PlayerInput>();
    }

    public void SelectLevel()
    {
        //Aumenta prioridade da câmera
        cinemachineCamera.Priority = 10;

        levelSelected.Invoke(character);

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
        levelFinished.Invoke(false);

        cinemachineCamera.Priority = 0;

        playerInput.enabled = false;

        character.isActive = false;
    }

    public void ResetLevel()
    {
        carrotReached = false;

        foreach (GameObject entity in entities)
        {
            IReset reset = entity.GetComponent<IReset>();

            reset.Reset();
        }
    }

    public CharacterController GetCharacter()
    {
        return character;
    }
}