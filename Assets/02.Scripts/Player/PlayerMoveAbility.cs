using Photon.Pun.Demo.SlotRacer;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 7f;
    [SerializeField]
    public float _jumpForce = 2.5f;

    private const float _gravity = 9.17f;

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

        _controller.Move(direction * _moveSpeed * Time.deltaTime);
    }
}
