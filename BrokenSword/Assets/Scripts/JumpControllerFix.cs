using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpControllerFix : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider2D;

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce;

    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && OnGround())
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private bool OnGround()
    {
        float _extraHeightText = 0.05f;
        RaycastHit2D _raycastHit = Physics2D.Raycast(_boxCollider2D.bounds.center, Vector2.down, _boxCollider2D.bounds.extents.y + _extraHeightText);
        Color _rayColor;
        if(_raycastHit.collider != null)
        {
            _rayColor = Color.green;
        } else
        {
            _rayColor = Color.red;
        }
        Debug.DrawRay(_boxCollider2D.bounds.center, Vector2.down * (_boxCollider2D.bounds.extents.y + _extraHeightText));
        Debug.Log(_raycastHit.collider);
        return _raycastHit.collider != null;
    }
}
