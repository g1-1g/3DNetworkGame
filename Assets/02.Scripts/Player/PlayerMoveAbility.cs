using Photon.Pun.Demo.SlotRacer;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveAbility : PlayerAbility
{
    [SerializeField]
    private float _speedChangeRate = 3;

    [SerializeField]
    public float _jumpForce = 25f;

    private float _moveSpeed;

    private float _speedOffset = 0.05f;

    private const float _gravity = 18f;

    private float _yVelocity = 0f;

    private CharacterController _controller;

    private PlayerAnimator _animator;

    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = transform.rotation * new Vector3(h, 0, v);
        direction.Normalize();

        SpeedUpdate(direction.magnitude);

        _controller.Move(direction * _moveSpeed * Time.deltaTime);

       
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded)
        {
            _yVelocity = -1;
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space) && _controller.isGrounded)
        {
            if (_owner.Stat.Stamina < _owner.Stat.StaminaDrainOnJump) return;

            _yVelocity = _jumpForce;
            _owner.Stat.Stamina -= _owner.Stat.StaminaDrainOnJump;
        }

        Vector3 direction = new Vector3(0, _yVelocity, 0);

        _controller.Move(direction * Time.deltaTime);
    }

    private void SpeedUpdate(float moveScale)
    {
        bool shouldRun = Input.GetKey(KeyCode.LeftShift) && _owner.Stat.Stamina > _owner.Stat.StaminaDrainOnRun;
        float targetSpeed;

        if (shouldRun)
        {
            targetSpeed = _owner.Stat.WalkSpeed * _owner.Stat.RunMultiplier;
            _owner.Stat.Stamina -= _owner.Stat.StaminaDrainOnRun * Time.deltaTime;
        }
        else
        {
            targetSpeed = _owner.Stat.WalkSpeed;
            if (_owner.Stat.Stamina < _owner.Stat.MaxStamina) _owner.Stat.Stamina = Mathf.Min(_owner.Stat.Stamina + _owner.Stat.StaminaRecoveryRate * Time.deltaTime, _owner.Stat.MaxStamina);
        }


        if (moveScale < 0.1f)
        {
            targetSpeed = 0f;
        }

        if (_moveSpeed < targetSpeed - _speedOffset || _moveSpeed > targetSpeed + _speedOffset)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, targetSpeed, _speedChangeRate * Time.deltaTime);
            _animator.SetSpeedRatio(CalculateBlendTreeParameter());
        }
        else
        {
            _moveSpeed = targetSpeed;
        }
    }

    private float CalculateBlendTreeParameter()
    {
        if (_moveSpeed < 0.01f)
        {
            return 0;
        }

        if (_moveSpeed < _owner.Stat.WalkSpeed)
        {
            return _moveSpeed / _owner.Stat.WalkSpeed;
        }

        else
        {
            float excess = _moveSpeed - _owner.Stat.WalkSpeed;
            float runRange = _owner.Stat.WalkSpeed * _owner.Stat.RunMultiplier - _owner.Stat.WalkSpeed;
            if (Mathf.Approximately(runRange, 0f)) return 1f;
            return 1 + excess / runRange;
        }
    }
}
