using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSystem : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D _rb;
    private TrailRenderer _tr;

    [Header("Jump Varibles")]
    [SerializeField] private float _jumpTime = 0.4f;
    [SerializeField] private float _jumpForce = 18.0f;
    [SerializeField] private float _fallMultipler = 5.0f;
    [SerializeField] private float _jumpMultipler = 3.0f;
    private bool _doubleJump;
    bool _isJumping;
    float _jumpCounter;

    [Header("Layer Marsk")]
    [SerializeField]public Transform _groundCheck;
    [SerializeField]public LayerMask _groundLayer;
    private Vector2 vecGravity;

    [Header("Dash Variables")]
    [SerializeField] private float _dashingVelocity = 14.0f;
    [SerializeField] private float _dashingTime = 0.5f;
    private Vector2 _dashingDirection;
    private bool _isDashing;
    private bool _canDashing = true;

    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //Jump
        if(_isGrounded() && !Input.GetButton("Jump"))
        {
            _doubleJump = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded() || _doubleJump)
            {
                Jump();
                _isJumping = true;
                _jumpCounter = 0;
                _doubleJump = !_doubleJump;
            }
            
        }

        if(_rb.velocity.y > 0 && _isJumping)
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
                _rb.velocity += vecGravity * _currentJump * Time.deltaTime;
            } 
        }

        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
            _jumpCounter = 0;

            if(_rb.velocity.y > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y*0.6f);
            }
        }

        if(_rb.velocity.y < 0)
        {
            _rb.velocity -= vecGravity * _fallMultipler * Time.deltaTime;
        }

        //Dash
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

        if (_isGrounded())
        {
            _canDashing = true;
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    bool _isGrounded()
    {
        return Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1.09f, 0.22f), CapsuleDirection2D.Horizontal, 0, _groundLayer);
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _isDashing = false;
    }
}
