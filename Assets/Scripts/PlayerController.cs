using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mozgás attribútumok")]
    public float moveSpeed = 5f;
    public float jumpForce = 8.5f;
    public float jumpContinuesForce = 1f;
    public float wallSlideSpeed = 2f;

    [Header("Environment ellenrzés")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float wallCheckDistance = 1f;

    [Header("Ugrás attribútumok")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.15f;
    public int extraJumpsValue = 1;

    [Header("Fizika korlátok")]
    public float maxFallSpeed = -20f;

    private bool isSneaking;

    // Privát változók a fizikához és állapotokhoz
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isRunningSoundPlaying = false;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private int extraJumps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        extraJumps = extraJumpsValue;

        Vector2 savedPos = Checkpoint.GetSavedPositionForCurrentScene();

        if (savedPos != Vector2.zero)
        {
            transform.position = savedPos;
        }
    }

    void Update()
    {
        HandleInput();
        UpdateAnimations();
        HandleRunningSound();
    }

    private void HandleRunningSound()
    {
        // Ha mozog, földön van, és nem csúszik falon
        bool isMoving = Mathf.Abs(rb.linearVelocityX) > 0.1f && isGrounded && !isWallSliding;

        if (isMoving && !isRunningSoundPlaying)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySound("Run", 0.4f);
            isRunningSoundPlaying = true;
        }
        else if (!isMoving && isRunningSoundPlaying)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.StopSound("Run");
            isRunningSoundPlaying = false;
        }
    }

    private void FixedUpdate()
    {
        // Fizikai ellenőrzések a FixedUpdate-ben a legstabilabbak
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.Raycast(transform.position, spriteRenderer.flipX ? Vector2.left : Vector2.right, wallCheckDistance, groundLayer);

        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    public void HandleInput()
    {
        //Vízszintes mozgás
        float moveInput = Input.GetAxis("Horizontal");
        Move(moveInput);

        //Coyote Time és Extra ugrás frissítése
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumps = extraJumpsValue;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //Jump Buffer frissítése
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //Ugrás logika hívása
        if (jumpBufferCounter > 0f)
        {
            Jump();
        }

        //Ugrás magasságának szabályozása
        if (Input.GetKey(KeyCode.Space) && rb.linearVelocityY > 0)
        {
            rb.AddForceY(jumpContinuesForce);
        }

        //Gravitáció 
        rb.gravityScale = (rb.linearVelocityY < 0) ? 3f : 2f;

        //Falon csúszás
        HandleWallSliding();
    }

    public void Move(float direction)
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Karakter megfordítása
        if (rb.linearVelocityX != 0 && !isTouchingWall)
        {
            spriteRenderer.flipX = rb.linearVelocityX < 0;
        }
    }

   
    public void Jump()
    {
        if (isWallSliding)
        {
            float direction = spriteRenderer.flipX ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * 8f, jumpForce);
            spriteRenderer.flipX = !spriteRenderer.flipX;
            jumpBufferCounter = 0f;
            AudioManager.Instance.PlaySound("Jump");
        }
        else if (coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
            AudioManager.Instance.PlaySound("Jump");
        }
        else if (extraJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            extraJumps--;
            jumpBufferCounter = 0f;
            AudioManager.Instance.PlaySound("Jump");
        }
    }

    public void Sneak(bool state)
    {
        isSneaking = state;
        // Később itt lehet felezni a sebességet, vagy átállítani a kollíziót
    }

    private void HandleWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.linearVelocityY < 0.15f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void UpdateAnimations()
    {
        if (isGrounded)
        {
            animator.Play(rb.linearVelocityX == 0 ? "Player_Idle" : "Player_Run");
        }
        else
        {
            if (isWallSliding) animator.Play("Player_WallSlide");
            else if (rb.linearVelocityY > 0) animator.Play("Player_jump");
            else animator.Play("Player_Fall");
        }
    }
}