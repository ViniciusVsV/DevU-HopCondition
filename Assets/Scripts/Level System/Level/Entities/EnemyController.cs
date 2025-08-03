using UnityEngine;

public class EnemyController : MonoBehaviour, IActivate, IReset
{
    [SerializeField] private float moveSpeed;
    private int moveDirection = 1;

    private Vector2 initialPosition;
    private Vector2 initialScale;
    private Rigidbody2D rb;

    private bool isActive;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        initialScale = transform.localScale;
    }

    void FixedUpdate()
    {
        if (!isActive)
            return;

        rb.linearVelocityX = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
            Flip();
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

        moveDirection *= -1;

        
    }

    public void Activate()
    {
        isActive = true;
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.creatureSound, transform, SoundFXManager.Instance.lowVolume );
    }

    public void _Reset(bool reactivate)
    {
        isActive = false;

        rb.linearVelocity = Vector2.zero;

        moveDirection = 1;
        transform.position = initialPosition;
        transform.localScale = initialScale;

        if (reactivate)
            isActive = true;
    }
}