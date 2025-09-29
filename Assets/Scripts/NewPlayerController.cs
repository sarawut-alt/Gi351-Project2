using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;
using System;
using NUnit.Framework;

public class NewPlayerController : MonoBehaviour
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

    [SerializeField, UnityEngine.Range(0, 1)]
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
    private Vector2 boxColliderSize;

    private Vector2 slopeNormalPerp;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

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
    }
    
}
