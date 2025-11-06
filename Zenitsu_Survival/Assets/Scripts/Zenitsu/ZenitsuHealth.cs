using UnityEngine;
using System;

public class ZenitsuHealth : MonoBehaviour
{
    //<float, float> -> 현재체력, 최대체력 2개의 값을 함께 보냅니다.
    public event Action<float, float> OnHealthChanged;

    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;

    public float CurrentHealth
    {
        get {return _currentHealth;}
    }

    public bool IsDead
    {
        get {return _currentHealth <= 0;}
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        if (IsDead) return;
        _currentHealth -= damageAmount;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }
        Debug.Log("젠이츠 체력:"+_currentHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        if (IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("젠이츠 사망");
    }
}
