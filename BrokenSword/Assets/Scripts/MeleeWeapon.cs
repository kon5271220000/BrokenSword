using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 20.0f;
    private MovementControl _character;
    private Rigidbody2D _rb;
    private MeleeAttackManager _meleeAttackManager;
    private Vector2 _direction;
    private bool _collided;
    private bool _downwardStrike;
    
    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponentInParent<MovementControl>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _meleeAttackManager = GetComponentInParent<MeleeAttackManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemieHealth>())
        {
            HandelCollision(collision.GetComponent<EnemieHealth>());
        }
    }

    private void HandelCollision(EnemieHealth objHeath)
    {
        if (Input.GetAxis("Vertical") < 0 && objHeath._giveUpwardForce && !_character._onGround)
        {
            _direction = Vector2.up;
            _downwardStrike = true;
            _collided = true;
        }

        if(Input.GetAxis("Vertical") > 0 && !_character._onGround)
        {
            _direction = Vector2.down;
            _collided = true;
        }

        if((Input.GetAxis("Vertical") <= 0 && _character._onGround) || Input.GetAxis("Vertical") == 0)
        {
            if (!_character._facingRight)
            {
                _direction = Vector2.right;
            }
            else
            {
                _direction = Vector2.left;
            }
            _collided = true;
        }
        
        objHeath.Damage(_damageAmount);
        StartCoroutine(NoLongerColliding());
    }

    private void HandleMovement()
    {
        if (_collided)
        {
            if (_downwardStrike)
            {
                _rb.AddForce(_direction * _meleeAttackManager._upWardForce);
            }
            else
            {
                _rb.AddForce(_direction * _meleeAttackManager._defaultForce);
            }
        }
    }
    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSeconds(_meleeAttackManager._movementTime);
        _collided = false;
        _downwardStrike = false;
    }
}
