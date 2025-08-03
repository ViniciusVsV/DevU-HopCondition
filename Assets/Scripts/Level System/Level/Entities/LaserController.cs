using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LaserController : MonoBehaviour, IActivate, IReset
{
    [SerializeField] private GameObject laserBeam;

    [SerializeField] private float laserCooldown;
    [SerializeField] private float activeDuration;
    private float laserCooldownTimer;

    [SerializeField] private AnimationClip attackAnimation;
    [SerializeField] private AnimationClip idleAnimation;

    private Animator animator;
    private Coroutine routine;

    private bool isActive;

    public UnityEvent laserStarted;
    public UnityEvent laserStopped;

    //Universal laser para o som
    [SerializeField] private GameObject universalLaser;


    void Awake()
    {
        animator = GetComponent<Animator>();

        laserCooldownTimer = laserCooldown;

        routine = null;
    }

    void Update()
    {
        if (!isActive)
            return;

        laserCooldownTimer -= Time.deltaTime;

        if (laserCooldownTimer < 0f)
        {
            laserCooldownTimer = laserCooldown + activeDuration + attackAnimation.length;
            animator.Play(attackAnimation.name);
        }
    }

    public void Attack()
    {
        routine = StartCoroutine(LaserRoutine());
    }

    private IEnumerator LaserRoutine()
    {
        laserStarted.Invoke();

        laserBeam.SetActive(true);

        //Play sound Laser
        if (gameObject != universalLaser)
            SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.laserSound, transform, SoundFXManager.Instance.lowVolume * 0.75f, SoundFXManager.Instance.lowPitch);

        yield return new WaitForSeconds(activeDuration);

        laserBeam.SetActive(false);

        laserStopped.Invoke();

        animator.Play(idleAnimation.name);
    }

    public void Activate()
    {
        isActive = true;
    }

    public void _Reset(bool reactivate)
    {
        isActive = false;

        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        laserBeam.SetActive(false);

        laserStopped.Invoke();

        laserCooldownTimer = laserCooldown;

        animator.Play(idleAnimation.name);

        if (reactivate)
            isActive = true;
    }
}