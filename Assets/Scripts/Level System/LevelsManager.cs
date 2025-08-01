using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public List<LevelController> levelControllers = new();

    [SerializeField] private RecordedMovements recordedMovements;
    private MovementRecorder movementRecorder;
    private MovementReplayer movementReplayer;

    private ButtonsManager buttonsManager;
    public int currentLevel;

    void Start()
    {
        levelControllers = FindObjectsByType<LevelController>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OrderBy(level => level.name)
            .ToList();

        movementRecorder = FindFirstObjectByType<MovementRecorder>();
        movementReplayer = FindFirstObjectByType<MovementReplayer>();

        currentLevel = 0;

        buttonsManager = FindFirstObjectByType<ButtonsManager>();

        SetSelectionState();
    }

    //Jogo começa no estado de seleção de fases
    //Nele os botões das fases estão ativos
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

    //Ao selecionar uma fase, vai para o estado de Recording
    //Nele o personagem da fase selecionada é ativado e o jogo começa a gravar os inputs
    //Acaba quando o jogador coletar a cenoura da fase
    public void SetRecordingState(CharacterController chosenCharacter)
    {
        //Desativa todos os botões
        buttonsManager.DisableButtons();

        recordedMovements.activeCharacter = chosenCharacter;

        movementRecorder.StartRecording(chosenCharacter);
    }

    //Após finalizar a fase selecionada, inicia o estado de Replaying
    //Nele todos os personagens diferentes do personagem que acabou de ser jogado imitarão os inputs deste
    //Acaba quando o replay terminar
    public void SetReplayingState(bool repeating)
    {
        currentLevel++;

        levelControllers[currentLevel].gameObject.SetActive(true);

        buttonsManager.DisableButtons();

        //Chamar o setup com os personagens que serão afetados
        //Se for em caso normal, pegar todos os ativos
        List<CharacterController> aux = new();

        if (!repeating)
        {
            for (int i = 0; i <= currentLevel; i++)
                aux.Add(levelControllers[i].GetCharacter());
        }

        //Se for o caso em que o está repetindo a conclusão de fases, pegar apenas o personagem que acavou de ser criado
        else
            aux.Add(levelControllers[currentLevel].GetCharacter());

        movementReplayer.Setup(aux);

        StartCoroutine(ReplayingRoutine());
    }

    private IEnumerator ReplayingRoutine()
    {
        yield return new WaitForSeconds(0.3f);

        movementReplayer.StartReplaying();
    }

    //Após o replay, vai para o estado de checagem de unlock
    //Se TODOS os personagens obteram sua respectiva cenoura, desbloqueia a próxima fase
    //Caso todas as fases já tenham sido desbloqueadas, acaba o jogo
    //Caso contrário, volta ao estado de seleção de fases
    public void SetUnlockingState()
    {
        int count = 0;

        foreach (LevelController level in levelControllers)
        {
            if (!level.gameObject.activeSelf)
                continue;

            if (level.carrotReached == false)
            {
                SetSelectionState();
                return;
            }

            count++;
        }

        //Se todas as fazes no total foram finalizadas
        if (count == levelControllers.Count)
            SceneManager.LoadScene("FinalMenu");

        //Se todas ATIVAS foram finalizadas
        else
            SetReplayingState(true);
    }
}
