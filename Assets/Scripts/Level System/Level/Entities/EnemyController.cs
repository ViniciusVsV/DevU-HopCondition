using UnityEngine;

public class EnemyController : MonoBehaviour, IReset
{
    [SerializeField] private float moveSpeed;
    private int moveDirection = 1;

    private Vector2 initialPosition;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
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

    public void Reset()
    {
        moveDirection = 1;
        transform.position = initialPosition;
    }
}