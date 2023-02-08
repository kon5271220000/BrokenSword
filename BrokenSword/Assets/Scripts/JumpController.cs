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
    

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetButtonDown("Jump") && OnGround()){
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }

    bool OnGround()
    {
        return Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1.26f, 0.21f), CapsuleDirection2D.Horizontal, 0, _groundLayer);
    }


}
