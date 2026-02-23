using Photon.Pun.Demo.SlotRacer;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed = 7f;

    [SerializeField]
    private float _runMultiply = 2f;

    [SerializeField]
    private float _speedChangeRate = 3;

    [SerializeField]
    public float _jumpForce = 25f;


    private float _moveSpeed;
    private float _speedOffset = 0.05f;

    private const float _gravity = 9.8f;

    private float _yVelocity = 0f;

    private CharacterController _controller;

    private PlayerAnimator _animator;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
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
            _yVelocity = _jumpForce;
        }

        Vector3 direction = new Vector3(0, _yVelocity, 0);

        _controller.Move(direction * Time.deltaTime);
    }

    private void SpeedUpdate(float moveScale)
    {
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? _walkSpeed * _runMultiply : _walkSpeed;

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

        if (_moveSpeed < _walkSpeed)
        {
            return _moveSpeed / _walkSpeed;
        }

        else
        {
            float excess = _moveSpeed - _walkSpeed;
            float runRange = _walkSpeed*_runMultiply - _walkSpeed;
            if (Mathf.Approximately(runRange, 0f)) return 1f;
            return 1 + excess / runRange;
        }
    }
}
