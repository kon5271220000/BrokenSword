using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackManager : MovementControl
{
    [SerializeField] public float _upWardForce = 600;
    [SerializeField] public float _defaultForce = 300;
    [SerializeField] public float _movementTime = 0.1f;
    public bool _meleeAttack;
    private Animator _meleeAnim;
    private Animator _anim;
    private MovementControl _character;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _character = GetComponent<MovementControl>();
        _meleeAnim = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _meleeAttack = true;
        }
        else
        {
            _meleeAttack = false;
        }

        if(_meleeAttack && Input.GetAxis("Vertical") > 0)
        {
            _anim.SetTrigger("UpwardMelee");
            _meleeAnim.SetTrigger("UpwardSwipe");
        }

        if(_meleeAttack && Input.GetAxis("Vertical") < 0 && !_character._onGround)
        {
            _anim.SetTrigger("DownwardMelee");
            _meleeAnim.SetTrigger("DownwardSwipe");
        }

        if((_meleeAttack && Input.GetAxis("Vertical")==0) || (_meleeAttack && Input.GetAxisRaw("Vertical") < 0 && _character._onGround))
        {
            _anim.SetTrigger("HorizontalMelee");
            _meleeAnim.SetTrigger("HorizontalSwipe");
        }
    }
}
