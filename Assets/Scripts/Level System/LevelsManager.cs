using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public List<LevelController> levelControllers = new();

    [SerializeField] private RecordedMovements recordedMovements;
    private MovementRecorder movementRecorder;
    private MovementReplayer movementReplayer;

    private ButtonsManager buttonsManager;
    private LevelController currentLevel;
    private LevelController lastLevel;
    private int unlockLevelsCounter;

    [Header("-------Selection Delays-------")]
    [SerializeField] private float cameraTransitionDelay;
    [SerializeField] private float levelActivatedDelay;
    [SerializeField] private float levelResetDelay;

    [Header("------Replay Delays-------")]
    [SerializeField] private float showNewLevelDelay;
    [SerializeField] private float showNewLevelRepeatingDelay;
    [SerializeField] private float startReplayDelay;

    public bool isWaiting;
    private bool isOnStart;
    public bool isReplaying;

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
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (isWaiting && !isPointerOverUI && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            isWaiting = false;

            ReplayMenuController.Instance.DeactivateReplayMenu();

            StartCoroutine(SetSelectionState());
        }

        if (currentLevel != null && Input.GetKeyDown(KeyCode.R))
            ResetCurrentLevel();

        if (currentLevel != null && !recordedMovements.isRecording && Input.GetMouseButtonDown(1))
            StartCoroutine(CancelSelection());
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

    private IEnumerator CancelSelection()
    {
        currentLevel.GetCamera().Priority = 0;

        Debug.Log(currentLevel.name);

        currentLevel.GetCharacter().isActive = false;
        currentLevel.ResetLevel(false);
        currentLevel = null;

        LevelMenuController.Instance.ActivatePauseButton();
        LevelMenuController.Instance.DeactivateLevelMenu();

        yield return new WaitForSeconds(cameraTransitionDelay);

        buttonsManager.EnableButtons();
    }

    public void SetRecordingState(LevelController currentLevel)
    {
        buttonsManager.DisableButtons();

        StartCoroutine(RecordingRoutine(currentLevel));
    }

    private IEnumerator RecordingRoutine(LevelController currentLevel)
    {
        LevelMenuController.Instance.DeactivatePausebutton();

        yield return new WaitForSeconds(cameraTransitionDelay);

        this.currentLevel = currentLevel;
        lastLevel = currentLevel;

        movementRecorder.StartRecording(currentLevel.GetCharacter());

        LevelMenuController.Instance.ActivateLevelMenu();

        //Som
        SoundFXManager.Instance.unlowVolume();
    }

    public void SetReplayingState(bool repeating, bool skipIf, bool skipDelay)
    {
        if (!skipIf && isReplaying)
            return;

        isReplaying = true;
        isWaiting = false;

        recordedMovements.isRecording = false;

        currentLevel = null;

        buttonsManager.DisableButtons();

        LevelMenuController.Instance.ActivatePauseButton();
        LevelMenuController.Instance.DeactivateLevelMenu();
        ReplayMenuController.Instance.ActivateReplayMenu();

        SoundFXManager.Instance.setlowVolume();

        StartCoroutine(ReplayingRoutine(repeating, skipDelay));
    }

    private IEnumerator ReplayingRoutine(bool repeating, bool skipDelay)
    {
        if (!skipDelay)
        {
            if (repeating)
                yield return new WaitForSeconds(showNewLevelRepeatingDelay);
            else
                yield return new WaitForSeconds(showNewLevelDelay);
        }

        GameBackgroundChange.Instance.ApplyEffect();

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
        {
            GameBackgroundChange.Instance.RemoveEffect();

            isReplaying = false;

            return;
        }

        unlockLevelsCounter++;

        //Se todas as 9 fases foram finalizadas
        if (count == levelControllers.Count)
            SceneManager.LoadScene("FinalMenu");

        //Se todas ATIVAS foram finalizadas
        else
            SetReplayingState(true, true, false);
    }

    public void ResetCurrentLevel()
    {
        if (currentLevel == null)
            return;

        recordedMovements.isRecording = false;

        currentLevel.ResetLevel(true);

        movementRecorder.StartRecording(currentLevel.GetCharacter());
    }

    public void ResetAllButLastLevel()
    {
        foreach (LevelController level in levelControllers)
        {
            if (!level.gameObject.activeSelf || level == lastLevel)
                continue;

            level.ResetLevel(true);
        }
    }
}