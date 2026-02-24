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
        // 포톤에서 가장 놓치기 쉽고 버그를 많이 일으키는 요소
        if (!_owner.PhotonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;

        CinemachineCamera vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = CameraRoot.transform;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        _mx += Input.GetAxis("Mouse X") * _rotationSpeed;
        _my += Input.GetAxis("Mouse Y") * _rotationSpeed;

        _my = Mathf.Clamp(_my, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, _mx, 0f);

        CameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }

}

