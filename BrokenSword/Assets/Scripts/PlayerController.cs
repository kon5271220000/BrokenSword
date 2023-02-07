using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;
    private Animator _anim;

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration = 70.0f;
    [SerializeField] private float _maxMoveSpeed = 12.0f;
    [SerializeField] private float _LinearDrag = 7.0f;
    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);


    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalDirection = GetInput().x;
    }

    private void FixedUpdate()
    {
        MoveCharactor();
        ApplyLinearDrag();
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharactor()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0.0f) * _movementAcceleration);

        if(Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
        }
    }

    private void ApplyLinearDrag()
    {
        if(Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _LinearDrag;
        }
        else
        {
            _rb.drag = 0.0f;
        }
    }
}
