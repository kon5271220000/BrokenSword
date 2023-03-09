using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] public float _moveSpeed = 5.0f;
    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
    }
}
