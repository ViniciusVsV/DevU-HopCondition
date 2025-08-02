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

        SetSelectionState();
    }

    void Update()
    {
        if (isWaiting && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            isWaiting = false;
            SetSelectionState();
        }

        if (currentLevel != null && Input.GetKeyDown(KeyCode.R))
            ResetCurrentLevel();
    }

    public void SetSelectionState()
    {
        //Reseta todas as fases
        foreach (LevelController level in levelControllers)
        {
            if (level.gameObject.activeSelf)
                level.ResetLevel();
        }

        //Ativa todos os botões
        buttonsManager.EnableButtons();
    }

    public void SetRecordingState(LevelController currentLevel)
    {
        this.currentLevel = currentLevel;

        //Desativa todos os botões
        buttonsManager.DisableButtons();

        movementRecorder.StartRecording(currentLevel.GetCharacter());
    }

    public void SetReplayingState(bool repeating)
    {
        currentLevel = null;

        levelControllers[unlockLevelsCounter + 1].gameObject.SetActive(true);

        buttonsManager.DisableButtons();

        //Chamar o setup com os personagens que serão afetados
        //Se for em caso normal, pegar todos os ativos
        List<CharacterController> aux = new();

        if (!repeating)
        {
            for (int i = 0; i <= unlockLevelsCounter + 1; i++)
                aux.Add(levelControllers[i].GetCharacter());
        }
        //Se for o caso em que o está repetindo a conclusão de fases, pegar apenas o personagem que acavou de ser criado
        else
            aux.Add(levelControllers[unlockLevelsCounter + 1].GetCharacter());

        movementReplayer.Setup(aux);

        StartCoroutine(ReplayingRoutine());
    }

    private IEnumerator ReplayingRoutine()
    {
        yield return new WaitForSeconds(0.3f);

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