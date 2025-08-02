using System.Collections;
using UnityEngine;

public class LaserController : MonoBehaviour, IReset
{
    [SerializeField] private GameObject laserBeam;

    [SerializeField] private float laserCooldown;
    [SerializeField] private float activeDuration;
    private float laserCooldownTimer;

    [SerializeField] private AnimationClip attackAnimation;
    [SerializeField] private AnimationClip idleAnimation;

    private Animator animator;
    private Coroutine routine;

    void Awake()
    {
        animator = GetComponent<Animator>();

        laserCooldownTimer = laserCooldown;

        routine = null;
    }

    void Update()
    {
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
        laserBeam.SetActive(true);

        yield return new WaitForSeconds(activeDuration);

        laserBeam.SetActive(false);

        animator.Play(idleAnimation.name);
    }

    public void Reset()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        laserBeam.SetActive(false);

        laserCooldownTimer = laserCooldown;

        animator.Play(idleAnimation.name);
    }
}