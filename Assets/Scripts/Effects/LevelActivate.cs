using System.Collections;
using UnityEngine;

public class LevelActivate : MonoBehaviour
{
    public static LevelActivate Instance;

    [SerializeField] private float duration;
    [SerializeField] private float activateEntitiesDelay;

    void Awake()
    {
        Instance = this;
    }

    public void ApplyEffect(LevelController levelController)
    {
        StartCoroutine(Routine(levelController));
    }

    private IEnumerator Routine(LevelController levelController)
    {
        Transform levelTransform = levelController.transform;

        float timer = 0f;
        Vector3 initialScale = levelTransform.localScale;
        Vector3 targetScale = Vector3.one;

        while (timer < duration)
        {
            float t = timer / duration;
            levelTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            timer += Time.deltaTime;
            yield return null;
        }

        levelTransform.localScale = targetScale;

        yield return new WaitForSeconds(activateEntitiesDelay);

        HiderDissolve.Instance.ApplyEffect(levelController.GetHider());

        levelController.ActivateEntities();
    }
}