using UnityEngine;
using System;
public class DemonHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 50;
    [SerializeField] private int _scoreValue = 10;
    [SerializeField] private int _expValue;
    
    public event Action<DemonHealth> OnDeath;
    
    private float _currentHealth;
    public bool IsDead { get; private set; }

    private void Start()
    {
        _currentHealth = _maxHealth;
        IsDead = false;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead) return;
        
        IsDead = true;
        //GameManager에 점수 보고
        GameManager.Instance.AddScore(_scoreValue);
        //WaveManager에 죽음 방송
        OnDeath?.Invoke(this);
        
        //죽음 애니메이션, 이펙트 재생 로직
        
        
        //오브젝트 파괴
        Destroy(this.gameObject, 2.0f);
    }
}
