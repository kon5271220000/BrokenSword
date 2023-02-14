using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSystem : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D _rb;

    [Header("Jump Varibles")]
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallMultipler;
    [SerializeField] private float _jumpMultipler;

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
        
        if (Input.GetButtonDown("Jump") && _isGrounded())
        {
            Jump();
            _isJumping = true;
            _jumpCounter = 0;
        }

        if(_rb.velocity.y > 0 && _isJumping)
        {
            _jumpCounter += Time.deltaTime;
            if (_jumpCounter > _jumpTime)
            {
                _isJumping = false;
            }
            _rb.velocity += vecGravity * _jumpMultipler * Time.deltaTime;
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
