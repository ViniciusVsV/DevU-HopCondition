using UnityEngine;

public class FairyController : MonoBehaviour, IReset
{
    [SerializeField] private Transform followPoint;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [SerializeField] private float stopDistance;

    private Rigidbody2D rb;
    private float currentSpeed;
    private Vector2 moveDirection;
    private Vector2 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = followPoint.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= stopDistance)
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);

        else
        {
            float targetSpeed = maxSpeed;

            if (distance < 1f)
                targetSpeed = Mathf.Lerp(0, maxSpeed, distance / 1f);

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }

        moveDirection = direction.normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * currentSpeed;
    }

    public void _Reset(bool reactivate)
    {
        transform.position = initialPosition;
    }
}