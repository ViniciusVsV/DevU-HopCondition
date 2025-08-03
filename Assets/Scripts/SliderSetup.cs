using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderSetup : MonoBehaviour
{
    public AudioMixer audioMixer;                  // Arraste o AudioMixer aqui pelo Inspector
    public string exposedParam = "masterVolume";   // Nome do parâmetro no AudioMixer
    public Slider slider;                          // Arraste o Slider aqui, ou use GetComponent

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        if (audioMixer.GetFloat(exposedParam, out float valueInDb))
        {
            slider.value = DbToLinear(valueInDb);
        }
    }

    private float DbToLinear(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }
}
