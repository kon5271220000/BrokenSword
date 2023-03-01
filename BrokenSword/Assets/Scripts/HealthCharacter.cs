using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCharacter : MonoBehaviour
{
    [Header("Health Variables")]
    [SerializeField]public int _maxHealth = 100;
    public int _currentHealth;

    [SerializeField] public HealthBar _healthBar;
   
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Healing(10);
        }
    }

    void TakeDamage(int _damg)
    {
        _currentHealth -= _damg;
        _healthBar.SetHealth(_currentHealth);
    }

    void Healing(int _heal)
    {
        _currentHealth += _heal;
        _healthBar.SetHealth(_currentHealth);
    }
}
