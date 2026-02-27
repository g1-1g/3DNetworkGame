using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerRotateAbility : PlayerAbility
{
    public Transform CameraRoot; 

    [SerializeField]
    private float _rotationSpeed = 5;

    [SerializeField]
    private PlayerContext _playerContext;

    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_owner.GameState != EGameState.Game) return;

        _mx += Input.GetAxis("Mouse X") * _rotationSpeed;
        _my += Input.GetAxis("Mouse Y") * _rotationSpeed;

        _my = Mathf.Clamp(_my, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, _mx, 0f);

        CameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }

}

