using System;
using UnityEngine;
using System.Collections;
public class DemonAI_Melee : MonoBehaviour
{
    private enum AiState
    {
        Chasing,
        Attack_Dash,
        Attack_Jump,
        Cooldown
    }

    [Header("AI State")]
    [SerializeField] private AiState _currentState;
    
    [Header("대상(젠이츠)")]
    [SerializeField] private Transform _playerTransform;
    
    [Header("컴포넌트 참조")]
    private Rigidbody2D _rigidbody;
    private DemonHealth _health;
    
    [Header("1.Chasing(추격)설정")]
    [SerializeField] private float _chaseSpeed = 3.0f;
    [SerializeField] private float _stopDistance = 1.5f;
    
    [Header("2.Dashing(돌진)설정")]
    [SerializeField] private float _dashSpeed = 15.0f;
    [SerializeField] private float _dashAttackRange = 7.0f;
    [SerializeField] private float _dashCooldown = 5.0f;
    [SerializeField] private float _dashDuration = 0.5f;
    private float _lastDashTime = -999f;

    [SerializeField] private GameObject _dashHitbox;
    
    [Header("3.Jumping(점프)설정")]
    [SerializeField] private float _jumpForce = 12.0f;
    [SerializeField] private float _jumpAttackRange = 5.0f;
    [SerializeField] private float _jumpCooldown = 5.0f;
    private float _lastJumpTime = -999f;

    private bool _isGrounded = true;
    private Vector2 _dashDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<DemonHealth>();

        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if(_dashHitbox != null) _dashHitbox.SetActive(false);
    }

    private void Start()
    {
        _currentState = AiState.Chasing;
        _lastDashTime = -_dashCooldown;
        _lastJumpTime = -_jumpCooldown;
    }

    private void Update()
    {
        if (_playerTransform == null || _health.IsDead)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        switch (_currentState)
        {
            case AiState.Chasing:
                LookForAttackOpportunity();
                break;
            case AiState.Cooldown:
                break;
            case AiState.Attack_Dash:
            case AiState.Attack_Jump:
                break;
        }
    }


    private void FixedUpdate()
    {
        if (_playerTransform == null || _health.IsDead) return;
        switch (_currentState)
        {
            case AiState.Chasing:
                ChasePhysics();
                break;
            case AiState.Attack_Dash:
                DashPhysics();
                break;
            case AiState.Attack_Jump:
                //점프는 순간일 뿐
                break;
            case AiState.Cooldown:
                _rigidbody.linearVelocity =new Vector2(0, _rigidbody.linearVelocityY);
                break;
        }

        FacePlayer();
    }
    private void LookForAttackOpportunity()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        bool canDash = (Time.time >= _lastDashTime + _dashCooldown);
        if (canDash && distance <= _dashAttackRange)
        {
            StartCoroutine(DashAttackCoroutine());
            return;
        }
        bool canJump = (Time.time >= _lastJumpTime + _jumpCooldown);
        if (canJump && distance <= _jumpAttackRange && _isGrounded)
        {
            StartJumpAttack();
            return;
        }
        //추격
    }


    private IEnumerator DashAttackCoroutine()
    {
        _currentState = AiState.Attack_Dash;
        _lastDashTime = Time.time;

        _dashDirection = (_playerTransform.position - transform.position).normalized;
        _rigidbody.linearVelocity = Vector2.zero;
        if(_dashHitbox != null) _dashHitbox.SetActive(true);
        
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(_dashDuration);
        
        if(_dashHitbox != null) _dashHitbox.SetActive(false);
        _currentState = AiState.Cooldown;
        yield return new WaitForSeconds(1.0f);
        _currentState = AiState.Chasing;
    }
    private void StartJumpAttack()
    {
        _currentState = AiState.Attack_Jump;
        _lastJumpTime = Time.time;
        
        Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
        Vector2 jumpVector = (directionToPlayer * 0.5f + Vector2.up * 1.0f).normalized;
        
        _rigidbody.AddForce(jumpVector * _jumpForce, ForceMode2D.Impulse);
        _isGrounded = false;
        //점프공격 히트박스 켜기

        StartCoroutine(JumpcooldownCoroutine());
    }

    private IEnumerator JumpcooldownCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        
        _isGrounded = true;
        _currentState = AiState.Cooldown;

        yield return new WaitForSeconds(1.0f);
        _currentState = AiState.Chasing;
    }

    private void ChasePhysics()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        if (distance > _stopDistance)
        {
            Vector2 direction = (_playerTransform.position - transform.position).normalized;
            _rigidbody.linearVelocity = new Vector2(direction.x * _chaseSpeed, _rigidbody.linearVelocityY);
        }
        else
        {
            _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocityY);
        }
    }


    private void DashPhysics()
    {
        _rigidbody.linearVelocity = _dashDirection * _dashSpeed;
    }
    private void JumpPhysics()
    {
        throw new NotImplementedException();
    }


    private void FacePlayer()
    {
        throw new NotImplementedException();
    }
    
}
