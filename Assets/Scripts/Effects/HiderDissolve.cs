using System.Collections;
using UnityEngine;

public class HiderDissolve : MonoBehaviour
{
    public static HiderDissolve Instance;

    [SerializeField] private Material dissolveMaterial;
    public float duration;

    void Awake()
    {
        Instance = this;
    }

    public void ApplyEffect(GameObject hider)
    {
        StartCoroutine(Routine(hider));
    }

    private IEnumerator Routine(GameObject hider)
    {
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.trasitionSound, transform, 0.6f);

        float elapsed = 0f;
        float startValue = 0f;
        float endValue = 1.1f;

        dissolveMaterial.SetFloat("_DissolveAmount", startValue);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            float value = Mathf.Lerp(startValue, endValue, t);

            dissolveMaterial.SetFloat("_DissolveAmount", value);

            yield return null;
        }

        dissolveMaterial.SetFloat("_DissolveAmount", endValue);

        hider.SetActive(false);

        dissolveMaterial.SetFloat("_DissolveAmount", 0f);
    }
}