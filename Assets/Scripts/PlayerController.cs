using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;
using System;
using NUnit.Framework;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField] private float gumSlowScale;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;

    [SerializeField, UnityEngine.Range(0, 1)]
    private float smoothDampTime = 0.125f;

    private Vector2 smoothDampVelocity = Vector2.zero;//for Vector2.smoothdamp to remember previous velocity


    [SerializeField] private float defaultGravityScale = 1.0f; // The standard gravity for jumping up
    [SerializeField] private float fallGravityMultiplier = 2.0f; // How much stronger gravity is when falling
    [SerializeField] private float maxFallSpeed = 10.0f; // Maximum downward velocity


    private InputAction moveAction;
    private InputAction jumpAction;

    private float xInput;
    [SerializeField]
    public float stickyEffectTime = 10f;

    private int facingDirection = 1;
    [SerializeField]
    private int Strawberry;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool isOnSlope;
    [SerializeField]
    private bool isJumping;
    [SerializeField]
    private bool canWalkOnSlope;
    [SerializeField]
    private bool canJump;
    [SerializeField]
    private bool isSticky = false;
    public bool haveStrawberry; //for demo only

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 boxColliderSize;

    private Vector2 slopeNormalPerp;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

    private SpriteRenderer spriteRenderer;

    private Color originalColor;
    public Color warningColor = Color.red;

    Animator animator;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb.gravityScale = defaultGravityScale; // Set initial gravity

        animator = GetComponent<Animator>();

        boxColliderSize = boxCollider2D.size;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        CheckInput();
        CheckGround();
        if(isSticky)
        {
            CheckWall();
        }
        SetAnimationVariables();
    }
    private void FixedUpdate()
    {
        //CheckGround();
        //SlopeCheck();
        ApplyMovement();
        ApplyGravityModifiers();//make player fall faster
    }

    #region Walk
    private void CheckInput()
    {
        xInput = moveAction.ReadValue<Vector2>().x;
        //xInput = Input.GetAxisRaw("Horizontal");

        if (xInput > 0 && facingDirection == -1)
        //if (xInput == 1 && facingDirection == -1)
        {
            Flip();
        }
        else if (xInput < 0 && facingDirection == 1)
        //else if (xInput == -1 && facingDirection == 1)
        {
            Flip();
        }

        if (jumpAction.triggered)
        {
            Jump();
        }

    }
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion

    private void CheckGround()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rb.linearVelocity.y <= 0.0f /*|| isGrounded*/)
        {
            isJumping = false;
        }
        

        /*if (isGrounded && !isJumping && isSticky) // มีสถานะ sticky
        {
            canJump = true;
        }*/
        if (isOnSlope && !isSticky)
        {
            canJump = false;
        }
        else if (isGrounded && !isJumping)
        {
            canJump = true;
        }



    }
    private void CheckWall()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rb.linearVelocity.y <= 0.0f || isGrounded)
        {
            isJumping = false;
        }

        /*if (isGrounded && !isJumping && isSticky) // มีสถานะ sticky
        {
            canJump = true;
        }*/
        if (isOnSlope && !isSticky)
        {
            canJump = false;
        }
        else if (isGrounded && !isJumping)
        {
            canJump = true;
        }

    }
    private void Jump()
    {
        if (canJump && isGrounded)
        {
            canJump = false;
            isJumping = true;
            //newVelocity.Set(0.0f, 0.0f);
            //rb.linearVelocity = newVelocity;
            newForce.Set(0.0f, jumpForce);
            rb.AddForce(newForce, ForceMode2D.Impulse);
        }
    }
    private void ApplyGravityModifiers()// make player fall faster
    {
        if (rb.linearVelocityY < 0)//check if player is falling
        {
            rb.gravityScale = defaultGravityScale * fallGravityMultiplier; // Player is falling, so increase gravity

            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -maxFallSpeed)); //limit max fall speed
        }
        else//player is not falling, so gravity is default (-9.81 * defaultGravityScale)
        {
            rb.gravityScale = defaultGravityScale;
        }
        Debug.Log(rb.linearVelocityY);
    }

    private void ApplyMovement()
    {
        newVelocity = Vector2.zero; // make sure newVelocity would not retain value from previous frame
        if (isGrounded && !isOnSlope && !isJumping) //if not on slope
        {
            Debug.Log("This one");
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            //rb.linearVelocity = newVelocity;
        }
        /*else if (isGrounded && isOnSlope && isSticky)
        {
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            rb.linearVelocity = newVelocity;
        }*/
        else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
        {
            newVelocity.Set(movementSpeed /** slopeNormalPerp.x*/ * xInput, rb.linearVelocity.y);
            //rb.linearVelocity = newVelocity;
        }
        else if (isGrounded && isOnSlope && !canWalkOnSlope && !isJumping)
        {
            newVelocity.Set(0.0f, -7f);

        }
        else if (!isGrounded) //If in air
        {
            newVelocity.Set(movementSpeed * xInput, rb.linearVelocity.y);
            //rb.linearVelocity = newVelocity;
        }

        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, newVelocity, ref smoothDampVelocity, smoothDampTime); //smoothDampVelocity is used to track the rate of velocity change
    }

    public void SetIsOnSlope(bool isOnSlope)
    {
        this.isOnSlope = isOnSlope;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Slope"))
        {
            rb.sharedMaterial = noFriction;
            if (!isSticky)
            {
                canWalkOnSlope = false;
            }

            SetIsOnSlope(true);

        }
        else if (collision.CompareTag("Sticky"))
        {
            StartCoroutine(StickyEffect(stickyEffectTime));
            collision.GetComponent<Sticky>().CollectSticky(stickyEffectTime);
            GameManager.GetInstance().GetComponent<BuffCooldownUI>().isStickyActive = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Slope"))
        {
            // normal fricctionn
            canWalkOnSlope = true;
            SetIsOnSlope(false);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    IEnumerator StickyEffect(float time)
    {
        isSticky = true;
        canWalkOnSlope = true;
        float temp = movementSpeed;
        movementSpeed *= gumSlowScale;

        float elapsedTime = 0f; // ตัวแปรนับเวลาที่ผ่านไป
        // วนลูปจนกว่าเวลาที่ผ่านไปจะเท่ากับ disappearDelay
        while (elapsedTime < time)
        {
            // เพิ่มเวลาที่ผ่านไปในแต่ละเฟรม
            elapsedTime += Time.deltaTime;

            // คำนวณค่า t (0 ถึง 1) เพื่อใช้ในการ Lerp สี
            float t = elapsedTime / time;

            // เปลี่ยนสีของ SpriteRenderer โดยค่อยๆ ผสมจากสีเดิม (warningColor) ไปยังสีเตือน (originalColor)
            spriteRenderer.color = Color.Lerp(warningColor, originalColor, t);

            // รอจนถึงเฟรมถัดไปแล้วค่อยทำงานต่อ
            yield return null;
        }

        //yield return new WaitForSeconds(time);
        isSticky = false;
        canWalkOnSlope = true;
        movementSpeed = temp;
    }
    private void SetAnimationVariables()
    {
        animator.SetBool("isJumpingAnim", jumpAction.triggered);
        animator.SetBool("isFallingAnim", rb.linearVelocityY < 0.1f);//not 0 because somtimes it does not fall when jump to platform
        animator.SetBool("isGroundAnim", isGrounded);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocityY);

    }

    public void AddStrawberry()
    {
        Strawberry++;
    }
    public int GetStrawberry()
    {
        return Strawberry;
    }
}
