using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
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

    [SerializeField, Range(0, 1)]
    private float smoothDampTime = 0.125f;

    private Vector2 smoothDampVelocity = Vector2.zero;//for Vector2.smoothdamp to remember previous velocity

    
    [SerializeField] private float defaultGravityScale = 1.0f; // The standard gravity for jumping up
    [SerializeField] private float fallGravityMultiplier = 2.0f; // How much stronger gravity is when falling
    [SerializeField] private float maxFallSpeed = 10.0f; // Maximum downward velocity
    

    private InputAction moveAction;
    private InputAction jumpAction;

    private float xInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    [SerializeField]
    private float stickyEffectTime = 10f;

    private int facingDirection = 1;

    [SerializeField]
    private bool isGrounded;
    private bool isOnSlope;
    [SerializeField]
    private bool isJumping;
    private bool canWalkOnSlope;
    [SerializeField]
    private bool canJump;
    [SerializeField] 
    private bool isSticky = false;
    public bool haveStrawberry; //for demo only

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 capsuleColliderSize;

    private Vector2 slopeNormalPerp;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        rb.gravityScale = defaultGravityScale; // Set initial gravity

        capsuleColliderSize = cc.size;
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        CheckGround();
        SlopeCheck();
        ApplyMovement();
        ApplyGravityModifiers();//make player fall faster
    }

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
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rb.linearVelocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if(isGrounded && !isJumping && isSticky) // มีสถานะ sticky
        {
            canJump = true;
        }
        else if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }

    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (isSticky)
        {
            canWalkOnSlope = true;
        }
        else if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        // แก้ติดกำแพงเดินออกไม่ได้
        bool verticalWallNextTo = (Mathf.RoundToInt(slopeSideAngle) > 89);
        if (verticalWallNextTo)
        {
            canWalkOnSlope = true;
            if (isSticky)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }


        // before add sticky
        /*if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }*/

        if (isSticky)
        {
            rb.sharedMaterial = fullFriction; // เหนียวตลอด
        }
        else
        {
            if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
            {
                rb.sharedMaterial = fullFriction;
            }
            else
            {
                rb.sharedMaterial = noFriction;
            }
        }
    }

    private void Jump()
    {
        if (canJump)
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
            newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
            //rb.linearVelocity = newVelocity;
        }
        else if (!isGrounded) //If in air
        {
            newVelocity.Set(movementSpeed * xInput, rb.linearVelocity.y);
            //rb.linearVelocity = newVelocity;
        }

        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, newVelocity, ref smoothDampVelocity, smoothDampTime); //smoothDampVelocity is used to track the rate of velocity change
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // trigger ของเหนียว
        if (other.CompareTag("Sticky"))
        {
            StartCoroutine(StickyEffect(stickyEffectTime));
            Destroy(other.gameObject);
        }
    }
    

    IEnumerator StickyEffect(float time)
    {
        isSticky = true;

        yield return new WaitForSeconds(time);

        isSticky = false;
    }

}
