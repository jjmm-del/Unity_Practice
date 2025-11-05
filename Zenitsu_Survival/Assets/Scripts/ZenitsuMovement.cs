using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ZenitsuMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 7.0f;
    
    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 15.0f;

    [Header("Dashing(벽력일섬용)")]
    [SerializeField] private float _dashSpeed = 25.0f; //벽력일섬용 속도
        
    [Header("GroundCheck")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] LayerMask _groundLayer;
    
    private Rigidbody2D _rigidbody;
    private ZenitsuInput _input;

    private bool _isGrounded;
    private bool _jumpBuffer = false;
    private float _originalGravityScale;

    public bool IsFacingRight { get; private set; } = true;
    public bool IsDashing { get; private set; } = false;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<ZenitsuInput>();
        
        _originalGravityScale = _rigidbody.gravityScale;
    }

    private void Update()
    {
        if (IsDashing)
        {
            _jumpBuffer = false;
            return;
        }
        if (_input.JumpInput)
        {
            _jumpBuffer = true;
        }
    }
    private void FixedUpdate()
    {
        if (IsDashing)
        {
            return;
        }
        
        
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);

        float horizontalInput = _input.MoveInput.x;
        _rigidbody.linearVelocity = new Vector2(horizontalInput * _moveSpeed, _rigidbody.linearVelocityY);

        if (_isGrounded && _jumpBuffer)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpBuffer = false;
        }

        if (horizontalInput > 0f && !IsFacingRight)
        {
            Flip();
        }

        if (horizontalInput < 0f && IsFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void StartDash()
    {
        IsDashing = true;
        _rigidbody.gravityScale = 0f;

        float dashDirection = IsFacingRight ? 1f : -1f;
        _rigidbody.linearVelocity = new Vector2(dashDirection * _dashSpeed, 0f);
    }

    public void StopDash()
    {
        IsDashing = false;
        _rigidbody.gravityScale = _originalGravityScale;
        _rigidbody.linearVelocity = Vector2.zero;
    }
}
