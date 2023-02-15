using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSystem : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D _rb;

    [Header("Jump Varibles")]
    [SerializeField] private float _jumpTime = 0.4f;
    [SerializeField] private float _jumpForce = 18.0f;
    [SerializeField] private float _fallMultipler = 5.0f;
    [SerializeField] private float _jumpMultipler = 3.0f;
    private bool _doubleJump;

    [Header("Layer Marsk")]
    [SerializeField]public Transform _groundCheck;
    [SerializeField]public LayerMask _groundLayer;
    private Vector2 vecGravity;

    bool _isJumping;
    float _jumpCounter;
    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        _rb = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
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
}
