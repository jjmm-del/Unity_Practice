using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 젠이츠의 Hitbox에 붙을 스크립트
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class AttackHitbox : MonoBehaviour
{
    public event Action OnHitEnemy;
    
    private float _damageToDeal;
    private List<Collider2D> _hitEnemies;
    
    public void Initialize(float damage)
    {
        this._damageToDeal = damage;
    }

    private void OnEnable()
    {
        if (_hitEnemies == null)
        {
            _hitEnemies = new List<Collider2D>();
        }
        else
        {
            _hitEnemies.Clear();
        }
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // [ ⚡️ 슈퍼 디버깅 로그 ⚡️ ]
        int otherLayerInt = otherCollider.gameObject.layer;
        string otherLayerName = LayerMask.LayerToName(otherLayerInt);
        string otherGameObjectName = otherCollider.gameObject.name;
        int enemyLayerInt = LayerMask.NameToLayer("Enemy");

        Debug.Log($"[Hitbox] 겹침 감지! 대상: {otherGameObjectName}, 레이어: {otherLayerName} (번호: {otherLayerInt})");
        Debug.Log($"[Hitbox] 내가 찾는 'Enemy' 레이어 번호: {enemyLayerInt}");

        // [ 1번 용의자 체크 ]
        if (otherLayerInt != enemyLayerInt)
        {
            Debug.LogError($"[Hitbox] 레이어 불일치! '{otherLayerName}'(이)랑 'Enemy'(을)를 비교했습니다. 리턴!");
            return;
        }

        // [ 2번 용의자 체크 ]
        if (_hitEnemies.Contains(otherCollider))
        {
            Debug.LogWarning("[Hitbox] 중복 타격! 리턴합니다.");
            return;
        }

        // [ 3번 용의자 체크 ]
        DemonHealth demon = otherCollider.GetComponentInParent<DemonHealth>();
        if (demon == null)
        {
            Debug.LogError("[Hitbox] DemonHealth 스크립트를 부모에서 못 찾음! 리턴!");
            return;
        }
        
        if (demon.IsDead)
        {
            Debug.LogWarning("[Hitbox] 이미 죽은 적입니다. 리턴!");
            return;
        }

        // [ 성공! ]
        Debug.Log($"[Hitbox] {otherGameObjectName}에게 {_damageToDeal} 데미지 입힘! (성공)");
        demon.TakeDamage(_damageToDeal);
        _hitEnemies.Add(otherCollider);

        OnHitEnemy?.Invoke();
    }
}
