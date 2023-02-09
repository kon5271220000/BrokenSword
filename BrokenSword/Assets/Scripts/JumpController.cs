using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce;

    [Header("Ground Check")]
    public Transform _groundCheck;
    public LayerMask _groundLayer;

    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    private bool _onGround;
    

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        if (Input.GetButtonDown("Jump") && _onGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void CheckCollisions()
    {
        _onGround = Physics2D.Raycast(transform.position * _groundRaycastLength, Vector2.down, _groundRaycastLength, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundRaycastLength);
    }
}
