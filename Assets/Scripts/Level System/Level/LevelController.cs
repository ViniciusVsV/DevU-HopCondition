using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CharacterController character;

    private PlayerInput playerInput;

    [SerializeField] private float selectionDelay;

    [Header("-------Entities-------")]
    [SerializeField] private List<GameObject> entities = new();
    [SerializeField] private GameObject entitiesFather;

    [Header("-------Events-------")]
    public UnityEvent<LevelController> levelSelected;
    public UnityEvent<bool> levelFinished;
    public UnityEvent levelFailed;

    public bool carrotReached;

    void Awake()
    {
        playerInput = character.GetComponent<PlayerInput>();
    }

    void Start()
    {
        LevelActivated.Instance.ApplyEffect(this);
    }

    public void SelectLevel()
    {
        cinemachineCamera.Priority = 10;

        levelSelected.Invoke(this);

        StartCoroutine(SelectRoutine());
    }

    private IEnumerator SelectRoutine()
    {
        yield return new WaitForSeconds(selectionDelay);

        ActivateLevel();

        character.isActive = true;

        playerInput.enabled = true;
    }

    public void FinishLevel()
    {
        levelFinished.Invoke(false);

        cinemachineCamera.Priority = 0;

        playerInput.enabled = false;

        character.isActive = false;
    }

    public void ActivateLevel()
    {
        foreach (GameObject entity in entities)
        {
            IActivate activate = entity.GetComponent<IActivate>();

            if (activate != null)
                activate.Activate();
        }
    }

    public void ResetLevel(bool reactivate)
    {
        carrotReached = false;

        foreach (GameObject entity in entities)
        {
            IReset reset = entity.GetComponent<IReset>();

            reset._Reset(reactivate);
        }
    }
    public void ActivateEntities()
    {
        entitiesFather.SetActive(true);
    }

    public CharacterController GetCharacter()
    {
        return character;
    }
    public CinemachineCamera GetCamera()
    {
        return cinemachineCamera;
    }
}