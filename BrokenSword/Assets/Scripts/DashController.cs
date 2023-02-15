using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("Dash Variables")]
    private bool _canDash = true;
    private bool _isDashing;
    [SerializeField] private float _dashingForce = 24.0f;
    [SerializeField] private float _dashingTime = 0.2f;
    [SerializeField] private float _dashingCooldown = 1.0f;
    [SerializeField] private TrailRenderer _tr;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDashing)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            return;
        }
 
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        float _originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0.0f;
        _rb.velocity = new Vector2(transform.localScale.x * _dashingForce, 0f);
        _tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = _originalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
    }
}
