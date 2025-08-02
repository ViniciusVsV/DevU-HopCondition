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

    [SerializeField] private float levelActivatedDelay;

    [Header("-------Selection Delays-------")]
    [SerializeField] private float levelResetDelay;

    [Header("------Replay Delays-------")]
    [SerializeField] private float showNewLevelDelay;
    [SerializeField] private float showNewLevelRepeatingDelay;
    [SerializeField] private float startReplayDelay;

    public bool isWaiting;
    private bool isOnStart;

    void Start()
    {
        levelControllers = FindObjectsByType<LevelController>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OrderBy(level => level.name)
            .ToList();

        movementRecorder = FindFirstObjectByType<MovementRecorder>();
        movementReplayer = FindFirstObjectByType<MovementReplayer>();

        unlockLevelsCounter = 0;

        buttonsManager = FindFirstObjectByType<ButtonsManager>();

        isOnStart = true;

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
        if (isOnStart)
        {
            yield return new WaitForSeconds(levelActivatedDelay);
            isOnStart = false;
        }

        foreach (LevelController level in levelControllers)
        {
            if (!level.gameObject.activeSelf)
                continue;

            yield return new WaitForSeconds(levelResetDelay);

            level.ResetLevel(false);
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
        recordedMovements.isRecording = false;

        currentLevel = null;

        buttonsManager.DisableButtons();

        StartCoroutine(ReplayingRoutine(repeating));
    }

    private IEnumerator ReplayingRoutine(bool repeating)
    {
        if (repeating)
            yield return new WaitForSeconds(showNewLevelRepeatingDelay);
        else
            yield return new WaitForSeconds(showNewLevelDelay);

        levelControllers[unlockLevelsCounter + 1].gameObject.SetActive(true);

        yield return new WaitForSeconds(levelActivatedDelay);

        List<CharacterController> auxCharacters = new();
        List<LevelController> auxLevels = new();

        if (!repeating)
        {
            for (int i = 0; i <= unlockLevelsCounter + 1; i++)
            {
                auxCharacters.Add(levelControllers[i].GetCharacter());
                auxLevels.Add(levelControllers[i]);
            }
        }
        else
        {
            auxCharacters.Add(levelControllers[unlockLevelsCounter + 1].GetCharacter());
            auxLevels.Add(levelControllers[unlockLevelsCounter + 1]);
        }

        movementReplayer.Setup(auxCharacters);

        yield return new WaitForSeconds(startReplayDelay);

        foreach (LevelController level in auxLevels)
            level.ActivateLevel();

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

        //Se todas as 9 fases foram finalizadas
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

        currentLevel.ResetLevel(true);

        movementRecorder.StartRecording(currentLevel.GetCharacter());
    }
}