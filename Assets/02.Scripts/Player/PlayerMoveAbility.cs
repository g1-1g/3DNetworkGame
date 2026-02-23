using Photon.Pun.Demo.SlotRacer;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 7f;
    [SerializeField]
    public float _jumpForce = 25f;

    private const float _gravity = 9.8f;

    private float _yVelocity = 0f;

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        direction.Normalize();

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
}
