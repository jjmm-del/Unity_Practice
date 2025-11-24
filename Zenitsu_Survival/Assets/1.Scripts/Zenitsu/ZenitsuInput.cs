using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// [신규 Input System 버전]
/// </summary>
public class ZenitsuInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool SkillInput { get; private set; }

    private ZenitsuControls _zenitsuControls;

    private void Awake()
    {
        _zenitsuControls = new ZenitsuControls();
    }

    private void OnEnable()
    {
        _zenitsuControls.Player.Enable();
    }

    private void OnDisable()
    {
        _zenitsuControls.Player.Disable();
    }

    private void Update()
    {
        MoveInput = _zenitsuControls.Player.Move.ReadValue<Vector2>();
        AttackInput = _zenitsuControls.Player.Attack.WasPressedThisFrame();
        JumpInput = _zenitsuControls.Player.Jump.WasPressedThisFrame();
        SkillInput = _zenitsuControls.Player.Skill.WasPressedThisFrame();
    }
}
