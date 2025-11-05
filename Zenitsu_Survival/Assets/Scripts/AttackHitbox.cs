using System;
using UnityEngine;

/// <summary>
/// 젠이츠의 Hitbox에 붙을 스크립트
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 20f;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //DemonHealth demon = otherCollider.GetComponent<DemonHealth>();
        
        //if(demon != null)
        {
            Debug.Log(otherCollider.name + "에게"+ _damageAmount + "데미지 입힘!");
            //demon.TakeDamage(_damageAmount);
        }
    }
}
