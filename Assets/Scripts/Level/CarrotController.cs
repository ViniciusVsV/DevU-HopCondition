using System.Collections;
using UnityEngine;

public class CarrotController : MonoBehaviour, IReset
{
    [SerializeField] private RecordedMovements recordedMovements;
    [SerializeField] private LevelController levelController;

    [SerializeField] private float desselectionDelay;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform == recordedMovements.activeCharacter)
            {
                Debug.Log("PORRA");
                StartCoroutine(Routine());
            }

            //dESATIVA colider
            col.enabled = false;

            //desativa sprite renderer
            spriteRenderer.enabled = false;
        }
    }

    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(desselectionDelay);

        levelController.DesselectLevel();
    }

    public void Reset()
    {
        spriteRenderer.enabled = true;

        col.enabled = true;
    }
}