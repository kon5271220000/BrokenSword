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
    bool _onGround;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _onGround = Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, _groundLayer);
        if (Input.GetButtonDown("Jump") && _onGround){
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }
}
