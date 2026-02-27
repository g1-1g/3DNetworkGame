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

    [SerializeField]
    public Vector3 _groundCheckOffset = new Vector3(0, 0.1f,0);

    private float _moveSpeed;

    private float _speedOffset = 0.05f;

    private const float _gravity = 18f;

    private float _yVelocity = 0f;

    private CharacterController _controller;

    private PlayerAnimator _animator;

    public bool ShouldRun;

    public bool IsGrounded {  get; private set; }

    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_owner.GameState != EGameState.Game) return;

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
        IsGrounded = Physics.Raycast(transform.position + _groundCheckOffset, Vector3.down, 0.2f);
        Debug.DrawRay(transform.position, Vector3.down * 0.2f, IsGrounded ? Color.green : Color.red);

        if (IsGrounded && _yVelocity < 0)
        {
            _yVelocity = -1;
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            if (!_owner.Stat.CanJump()) return;

            _yVelocity = _jumpForce;
            _owner.Stat.ConsumeStamina(_owner.Stat.StaminaDrainOnJump);
        }

        Vector3 direction = new Vector3(0, _yVelocity, 0);

        _controller.Move(direction * Time.deltaTime);
    }

    private void SpeedUpdate(float moveScale)
    {
        ShouldRun = moveScale > 0 && Input.GetKey(KeyCode.LeftShift) && _owner.Stat.CanRun();
        float targetSpeed;

        if (ShouldRun)
        {
            targetSpeed = _owner.Stat.WalkSpeed * _owner.Stat.RunMultiplier;
            _owner.Stat.ConsumeStamina(_owner.Stat.StaminaDrainOnRun * Time.deltaTime);
        }
        else
        {
            targetSpeed = _owner.Stat.WalkSpeed;
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
