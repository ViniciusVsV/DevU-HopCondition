using System.Collections;
using UnityEngine;

public class CarrotController : MonoBehaviour, IReset
{
    [SerializeField] private RecordedMovements recordedMovements;
    [SerializeField] private LevelController levelController;

    [SerializeField] private float desselectionDelay;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private bool collided;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        collided = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collided)
        {
            collided = true;

            col.enabled = false;

            spriteRenderer.enabled = false;

            levelController.carrotReached = true;

            if (other.gameObject == recordedMovements.activeCharacter.gameObject)
                StartCoroutine(Routine());
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