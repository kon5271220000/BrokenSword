using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Slider _slider;
    [SerializeField] public Gradient gradient;
    [SerializeField] public Image _fill;

    public void SetHealth(int _health)
    {
        _slider.value = _health;
        _fill.color = gradient.Evaluate(_slider.normalizedValue);
    }

    public void SetMaxHealth(int _health)
    {
        _slider.maxValue = _health;
        _slider.value = _health;

        _fill.color = gradient.Evaluate(_slider.normalizedValue);
    }
   
}
