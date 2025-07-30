using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("-------Movement-------")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;

    [Header("-------Jump-------")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float jumpCutMultiplier;

    [Header("-------GroundCheck-------")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    [Header("-------Coyote Time-------")]
    [SerializeField] private float coyoteTimeLimit;
    private float coyoteTimeCounter;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        coyoteTimeCounter = coyoteTimeLimit;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (isGrounded)
            coyoteTimeCounter = coyoteTimeLimit;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = moveDirection.x * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Caso caia de uma paltaforma, tem um tempinho extra para poder pular ainda
            if (rb.linearVelocityY < 0f && coyoteTimeCounter >= Mathf.Epsilon)
                rb.linearVelocityY = jumpStrength;

            //Caso esteja no chão mesmo, pula normalmente
            else if (isGrounded)
                rb.linearVelocityY = jumpStrength;
        }

        else if (rb.linearVelocityY > 0f && context.canceled)
            rb.linearVelocityY *= jumpCutMultiplier;
    }
}