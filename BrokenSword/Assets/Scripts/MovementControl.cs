using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D _rb;
    private TrailRenderer _tr;

    [Header("Movement Variables")]
    [SerializeField] private float _acceleration = 70.0f;
    [SerializeField] private float _maxMoveSpeed = 12.0f;
    [SerializeField] private float _linearDrag = 7.0f;
    private float _horizontalDirection;
    bool _facingRight = true;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0);

    [Header("Jump Varibles")]
    [SerializeField] private float _jumpTime = 0.4f;
    [SerializeField] private float _jumpForce = 18.0f;
    [SerializeField] private float _fallMultipler = 5.0f;
    [SerializeField] private float _jumpMultipler = 3.0f;
    private bool _doubleJump;
    bool _isJumping;
    float _jumpCounter;

    [Header("Layer Marsk")]
    [SerializeField] public Transform _groundCheck;
    [SerializeField] public LayerMask _groundLayer;
    private Vector2 _vecGravity;

    [Header("Dash Variables")]
    [SerializeField] private float _dashingVelocity = 14.0f;
    [SerializeField] private float _dashingTime = 0.5f;
    private Vector2 _dashingDirection;
    private bool _isDashing;
    private bool _canDashing = true;

    // Start is called before the first frame update
    void Start()
    {
        _vecGravity = new Vector2(0, -Physics2D.gravity.y);
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalDirection = GetInput().x;

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            _doubleJump = false;
        }

        //Jump Control
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || _doubleJump)
            {
                Jump();
                _isJumping = true;
                _jumpCounter = 0;
                _doubleJump = !_doubleJump;
            }

        }

        if (_rb.velocity.y > 0 && _isJumping)
        {
            _jumpCounter += Time.deltaTime;
            if (_jumpCounter > _jumpTime)
            {
                _isJumping = false;

                float t = _jumpCounter / _jumpTime;
                float _currentJump = _jumpMultipler;

                if (t > 0.5f)
                {
                    _currentJump = _jumpMultipler * (1 - t);
                }
                _rb.velocity += _vecGravity * _currentJump * Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
            _jumpCounter = 0;

            if (_rb.velocity.y > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.6f);
            }
        }

        if (_rb.velocity.y < 0)
        {
            _rb.velocity -= _vecGravity * _fallMultipler * Time.deltaTime;
        }

        var _dashInput = Input.GetButtonDown("Dash");

        if (_dashInput && _canDashing)
        {
            _isDashing = true;
            _canDashing = false;
            _tr.emitting = true;
            _dashingDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (_dashingDirection == Vector2.zero)
            {
                _dashingDirection = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }

        if (_isDashing)
        {
            _rb.velocity = _dashingDirection.normalized * _dashingVelocity;
            return;
        }

        if (IsGrounded())
        {
            _canDashing = true;
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        ApplyLinearDrag();

        if (_horizontalDirection < 0 && _facingRight)
        {
            Flip();
        }
        if (_horizontalDirection > 0 && !_facingRight)
        {
            Flip();
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0.0f) * _acceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
        }
    }

    private void ApplyLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _linearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1.09f, 0.21f), CapsuleDirection2D.Horizontal, 0, _groundLayer);
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _isDashing = false;
    }
}
