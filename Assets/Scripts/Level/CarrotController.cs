using UnityEngine;

public class CarrotController : MonoBehaviour
{
    [SerializeField] private RecordedMovements recordedMovements;
    [SerializeField] private LevelController levelController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform == recordedMovements.activeCharacter)
            {
                Debug.Log("PORRA");
                levelController.DesselectLevel();
            }

            Destroy(gameObject);
        }
    }
}