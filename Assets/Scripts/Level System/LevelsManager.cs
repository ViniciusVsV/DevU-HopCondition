using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public List<LevelController> levelControllers = new();

    [SerializeField] private RecordedMovements recordedMovements;
    private MovementRecorder movementRecorder;
    private MovementReplayer movementReplayer;

    private ButtonsManager buttonsManager;
    private LevelController currentLevel;
    private int unlockLevelsCounter;

    [Header("-------Selection Delays-------")]
    [SerializeField] private float levelResetDelay;

    [Header("------Replay Delays-------")]
    [SerializeField] private float showNewLevelDelay;
    [SerializeField] private float startReplayDelay;

    public bool isWaiting;

    void Start()
    {
        levelControllers = FindObjectsByType<LevelController>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OrderBy(level => level.name)
            .ToList();

        movementRecorder = FindFirstObjectByType<MovementRecorder>();
        movementReplayer = FindFirstObjectByType<MovementReplayer>();

        unlockLevelsCounter = 0;

        buttonsManager = FindFirstObjectByType<ButtonsManager>();

        StartCoroutine(SetSelectionState());
    }

    void Update()
    {
        if (isWaiting && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            isWaiting = false;
            StartCoroutine(SetSelectionState());
        }

        if (currentLevel != null && Input.GetKeyDown(KeyCode.R))
            ResetCurrentLevel();
    }

    private IEnumerator SetSelectionState()
    {
        foreach (LevelController level in levelControllers)
        {
            if (!level.gameObject.activeSelf)
                continue;

            yield return new WaitForSeconds(levelResetDelay);

            level.ResetLevel();
        }

        buttonsManager.EnableButtons();
    }

    public void SetRecordingState(LevelController currentLevel)
    {
        this.currentLevel = currentLevel;

        buttonsManager.DisableButtons();

        movementRecorder.StartRecording(currentLevel.GetCharacter());
    }

    public void SetReplayingState(bool repeating)
    {
        currentLevel = null;

        buttonsManager.DisableButtons();

        StartCoroutine(ReplayingRoutine(repeating));
    }

    private IEnumerator ReplayingRoutine(bool repeating)
    {
        yield return new WaitForSeconds(showNewLevelDelay);

        levelControllers[unlockLevelsCounter + 1].gameObject.SetActive(true);

        List<CharacterController> aux = new();

        if (!repeating)
        {
            for (int i = 0; i <= unlockLevelsCounter + 1; i++)
                aux.Add(levelControllers[i].GetCharacter());
        }
        else
            aux.Add(levelControllers[unlockLevelsCounter + 1].GetCharacter());

        movementReplayer.Setup(aux);

        yield return new WaitForSeconds(startReplayDelay);

        movementReplayer.StartReplaying();
    }

    public void SetUnlockingState()
    {
        int count = 0;

        foreach (LevelController level in levelControllers)
        {
            if (!level.gameObject.activeSelf)
                continue;

            if (level.carrotReached == false)
            {
                isWaiting = true;

                level.levelFailed.Invoke();
            }

            count++;
        }

        if (isWaiting)
            return;

        unlockLevelsCounter++;

        //Se todas as fases no total foram finalizadas
        if (count == levelControllers.Count)
            SceneManager.LoadScene("FinalMenu");

        //Se todas ATIVAS foram finalizadas
        else
            SetReplayingState(true);
    }

    public void ResetCurrentLevel()
    {
        if (currentLevel == null)
            return;

        recordedMovements.isRecording = false;

        currentLevel.ResetLevel();

        movementRecorder.StartRecording(currentLevel.GetCharacter());
    }
}