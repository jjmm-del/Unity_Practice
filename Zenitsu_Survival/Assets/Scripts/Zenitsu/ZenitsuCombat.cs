using UnityEngine;
using System.Collections; 
public class ZenitsuCombat : MonoBehaviour
{
   [Header("Normal Attack")]
   [SerializeField] private float _attackDamage = 20f; //공격 데미지
   [SerializeField] private float _attackCooldown = 1.0f; //공격 쿨타임
   [SerializeField] private GameObject _attackHitbox; //공격 히트박스
   [SerializeField] private float _attackDashDuration = 0.3f;
   [SerializeField] private GameObject _attackEffect;
   
   
   [Header("Skill(Honoikazuchi no Kami)")]
   [SerializeField] private float _skillDamage = 100f; //스킬 데미지
   [SerializeField] private float _skillCooldown = 10.0f; //스킬 쿨타임
   [SerializeField] private GameObject _skillHitbox;
   [SerializeField] private float _skillDashDuration = 0.6f;
   [SerializeField] private GameObject _skillEffectPrefab; //화뢰신 이펙트 프리팹

   private ZenitsuInput _input;
   private float _lastAttackTime = -999f;
   private float _lastSkillTime = -999f;

   private ZenitsuMovement _movement;

   public float AttackCoolDownRemaining
   {
      get
      {
         float remaining = (_lastAttackTime + _attackCooldown) - Time.time;
         return remaining < 0f ? 0f:remaining;
      }
   }

   public float SkillCooldownRemaining
   {
      get
      {
         float remaining = (_lastSkillTime + _skillCooldown) - Time.time;
         return remaining < 0f ? 0f:remaining;
      }
   }
   public float AttackCooldownTotal { get{return _attackCooldown;}}
   public float SkillCooldownTotal { get{return _skillCooldown;}}
   private void Awake()
   {
      _input = GetComponent<ZenitsuInput>();
      _movement = GetComponent<ZenitsuMovement>();
      
      if (_attackHitbox != null) _attackHitbox.SetActive(false);
      if(_skillHitbox != null) _skillHitbox.SetActive(false);
      _attackEffect?.SetActive(false);
   }

   private void Update()
   {
      if (_input.AttackInput)
      {
         PerformAttack();
      }

      if (_input.SkillInput)
      {
         PerformSkill();
      }
   }

   private void PerformAttack()
   {
      if (Time.time < _lastAttackTime + _attackCooldown)
      {
         Debug.Log("벽력일섬 쿨타임..."+(_lastAttackTime + _attackCooldown - Time.time));
         return;
      }
      if (_movement == null || _movement.IsDashing)
      {
         return;
      }

      StartCoroutine(DashAttackCoroutine());
   }
   private IEnumerator DashAttackCoroutine()
   {
      Debug.Log("벽력일섬");

      _lastAttackTime = Time.time;
      _movement.StartDash();

      if (_attackEffect != null) _attackEffect.SetActive(true);
      if(_attackHitbox != null) _attackHitbox.SetActive(true);

      yield return new WaitForSeconds(_attackDashDuration);
      _movement.StopDash();
      if (_attackEffect != null) _attackEffect.SetActive(false);
      if(_attackHitbox != null) _attackHitbox.SetActive(false);
      
      Debug.Log("...대시 종료");
   }

   private void PerformSkill()
   {
      if (Time.time < _lastSkillTime + _skillCooldown)
      {
         Debug.Log("화뢰신 쿨타임..."+(_lastSkillTime + _skillCooldown - Time.time));
         return;
      }

      if (_movement == null || _movement.IsDashing)
      {
         return;
      }
      StartCoroutine(DashSkillCoroutine());
   }

   private IEnumerator DashSkillCoroutine()
   {
      Debug.Log("화뢰신(火雷神)!");
      _lastSkillTime = Time.time;
      _movement.StartDash();
      if(_skillHitbox != null) _skillHitbox.SetActive(true);

      if (_skillEffectPrefab != null)
      {
         Instantiate(_skillEffectPrefab, transform.position,Quaternion.identity);
      }
      yield return new WaitForSeconds(_skillDashDuration);
      _movement.StopDash();
      if(_skillHitbox != null) _skillHitbox.SetActive(false);
   }
   
   

}
