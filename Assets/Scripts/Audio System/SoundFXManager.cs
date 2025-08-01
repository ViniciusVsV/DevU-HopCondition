using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioSource _musicSource;

    [Header("--SoundSFX---")]
    public AudioClip jumpSound;

    [Header("--Musics---")]
    public AudioClip gameMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(_audioSource, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLenght = audioClip.length;

        //Destroi depois de tocar o clip;
        Destroy(audioSource.gameObject, clipLenght);


    }

    public void Music(AudioClip musicClip)
    {
        if (_musicSource.clip != musicClip)
        {
            _musicSource.clip = musicClip;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }

}
