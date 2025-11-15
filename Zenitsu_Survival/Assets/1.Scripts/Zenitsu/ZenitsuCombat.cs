using UnityEngine;
using System.Collections; 
using UnityEngine.VFX;
public class ZenitsuCombat : MonoBehaviour
{
   [Header("Normal Attack")]
   [SerializeField] private float _attackDamage = 20f; //공격 데미지
   [SerializeField] private float _attackCooldown = 1.0f; //공격 쿨타임
   [SerializeField] private GameObject _attackHitboxObject; //공격 히트박스
   [SerializeField] private float _attackDashDuration = 0.3f;
   [SerializeField] private VisualEffect _attackEffect;
   
   
   [Header("Skill(Honoikazuchi no Kami)")]
   [SerializeField] private float _skillDamage = 100f; //스킬 데미지
   //[SerializeField] private float _skillCooldown = 10.0f; //스킬 쿨타임
   [SerializeField] private int _skillHitTarget = 10;
   [SerializeField] private GameObject _skillHitboxObject;
   [SerializeField] private float _skillDashDuration = 0.6f;
   [SerializeField] private GameObject _skillEffectPrefab; //화뢰신 이펙트 프리팹

   private ZenitsuInput _input;
   private float _attackCooldownTimer = 0f;
  // private float _skillCooldownTimer = 0f;
   private int _currentSkillHitCount = 0;
   private ZenitsuMovement _movement;
   private AttackHitbox _skillHitbox;
   private AttackHitbox _attackHitbox;
   public float AttackCoolDownRemaining
   {
      get
      {
         return Mathf.Max(0f, _attackCooldown - _attackCooldownTimer);
      }
   }

   public float SkillCooldownRemaining
   {
      get
      {
         return (float)Mathf.Max(0f,_skillHitTarget - _currentSkillHitCount);
      }
   }
   public float AttackCooldownTotal { get{ return _attackCooldown; }}
   public float SkillCooldownTotal { get{ return (float)_skillHitTarget; }}
   private void Awake()
   {
      _input = GetComponent<ZenitsuInput>();
      _movement = GetComponent<ZenitsuMovement>();

      if (_attackHitboxObject != null)
      {
         _attackHitbox = _attackHitboxObject.GetComponent<AttackHitbox>();
         _attackHitbox.OnHitEnemy += HandleAttackHit;
         
         _attackHitboxObject.SetActive(false);

      }

      if (_skillHitboxObject != null)
      {
         _skillHitbox = _skillHitboxObject.GetComponent<AttackHitbox>();
         _skillHitbox.OnHitEnemy += HandleAttackHit;
         _skillHitboxObject.SetActive(false);
         

      }
      //_attackEffect?.SetActive(false);
   }

   private void HandleAttackHit()
   {
      if (_currentSkillHitCount >= _skillHitTarget) return;

      _currentSkillHitCount++;
      Debug.Log($"[ZenitsuCombat]스킬 충전!({_currentSkillHitCount}/{_skillHitTarget})");
   }
   private void Update()
   {
      if (_attackCooldownTimer > 0f)
      {
         _attackCooldownTimer -= Time.deltaTime;
      }

      // if (_skillCooldownTimer > 0f)
      // {
      //    _skillCooldownTimer -= Time.deltaTime;
      // }
      
      if (_input.AttackInput && _attackCooldownTimer <= 0f)
      {
         PerformAttack();
      }

      if (_input.SkillInput && _currentSkillHitCount >= _skillHitTarget)
      {
         PerformSkill();
      }
   }

   private void PerformAttack()
   {
      if (_movement == null || _movement.IsDashing)
      {
         return;
      }

      StartCoroutine(DashAttackCoroutine());
   }
   private IEnumerator DashAttackCoroutine()
   {
      Debug.Log("벽력일섬");
      _attackCooldownTimer = _attackCooldown;
      if (_attackEffect != null)
      {
         _attackEffect.Play();
      }
      _movement.StartDash();

      //if (_attackEffect != null) _attackEffect.SetActive(true);
      if (_attackHitbox != null)
      {
         _attackHitbox.Initialize(_attackDamage);
         _attackHitboxObject.SetActive(true);
      }

      yield return new WaitForSeconds(_attackDashDuration);
      _movement.StopDash();
      if (_attackEffect != null)
      {
         _attackEffect.Stop();
      }
      //if (_attackEffect != null) _attackEffect.SetActive(false);
      if(_attackHitboxObject != null) _attackHitboxObject.SetActive(false);
     
      
      Debug.Log("...대시 종료");
   }

   private void PerformSkill()
   {
      if (_movement == null || _movement.IsDashing)
      {
         return;
      }
      StartCoroutine(DashSkillCoroutine());
   }

   private IEnumerator DashSkillCoroutine()
   {
      Debug.Log("화뢰신(火雷神)!");
      _currentSkillHitCount = 0;
      _movement.StartDash();


      if (_skillHitbox != null)
      {
         _skillHitbox.Initialize(_skillDamage);
         _skillHitboxObject.SetActive(true);
      }

      if (_skillEffectPrefab != null)
      {
         Instantiate(_skillEffectPrefab, transform.position,Quaternion.identity);
      }
      yield return new WaitForSeconds(_skillDashDuration);
      _movement.StopDash();
      if(_skillHitboxObject != null) _skillHitboxObject.SetActive(false);
   }

   private void OnDestroy()
   {
      if (_attackHitbox != null)
      {
         _attackHitbox.OnHitEnemy -= HandleAttackHit;
      }

      if (_skillHitbox != null)
      {
         _skillHitbox.OnHitEnemy -= HandleAttackHit;
      }
   }
   

}
