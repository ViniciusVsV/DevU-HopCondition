using System.Collections;
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

        yield return new WaitForSeconds(activeDuration);

        laserStopped.Invoke();

        laserBeam.SetActive(false);

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

        laserCooldownTimer = laserCooldown;

        animator.Play(idleAnimation.name);

        if (reactivate)
            isActive = true;
    }
}