using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieHealth : MonoBehaviour
{
    [SerializeField] private bool _damageable = true;
    [SerializeField] private float _health = 100.0f;
    [SerializeField] private float _invulnerabilityTime = .2f;
    [SerializeField] public bool _giveUpwardForce = true;
    [SerializeField] private bool _hit;
    [SerializeField] private float _currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _health;
    }

    public void Damage(float amount)
    {
        if(_damageable && _currentHealth > 0 && !_hit)
        {
            _hit = true;
            _currentHealth -= amount;
            
            if(_currentHealth <= 0)
            {
                _currentHealth = 0;
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(TurnOfHit());
            }
        }
    }

    private IEnumerator TurnOfHit()
    {
        yield return new WaitForSeconds(_invulnerabilityTime);
        _hit = false;
    }
}
