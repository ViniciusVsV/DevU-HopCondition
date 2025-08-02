using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    public void setMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume",Mathf.Log10(level)*20);
    }

    public void setSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
    }

    public void setMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
    }
}
