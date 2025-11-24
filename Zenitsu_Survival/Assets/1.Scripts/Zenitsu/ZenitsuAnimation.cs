using UnityEngine;

public class ZenitsuAnimation : MonoBehaviour
{
    private readonly int _isWalkingHash = Animator.StringToHash("IsRunning");
    private readonly int _isDashingHash = Animator.StringToHash("IsDashing");

    private Animator _animator;
    private ZenitsuInput _input;
    private ZenitsuMovement _movement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _input = GetComponentInParent<ZenitsuInput>();
        _movement = GetComponentInParent<ZenitsuMovement>();
    }

    private void LateUpdate()
    {
        UpdateRunState();
        UpdateDashState();
    }

    private void UpdateRunState()
    {
        if (_input == null) return;
        bool isRunning = _input.MoveInput.sqrMagnitude > 0.01f;
        
        _animator.SetBool(_isWalkingHash, isRunning);
    }

    private void UpdateDashState()
    {
        if (_movement == null) return;
        bool isDashing = _movement.IsDashing;
        
        _animator.SetBool(_isDashingHash, isDashing);
    }
}
