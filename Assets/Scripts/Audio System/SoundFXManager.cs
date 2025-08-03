using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [HideInInspector]public float lowVolume;
    [HideInInspector] public float lowPitch;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private float lowVolumeSave;

    [Header("--SoundSFX---")]
    public AudioClip jumpSound;
    public AudioClip laserSound;
    public AudioClip successSound;
    public AudioClip failSound;
    public AudioClip creatureSound;
    public AudioClip trasitionSound;
    

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

        lowVolume = lowVolumeSave;
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float pitch = 1f)
    {
        AudioSource audioSource = Instantiate(_audioSource, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.pitch = pitch;

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


    public void setlowVolume()
    {
        lowVolume = lowVolumeSave;
        lowPitch = 0.80f;
    }
    public void unlowVolume()
    {
        lowVolume = 1f;
        lowPitch= 1f;
    }
}
