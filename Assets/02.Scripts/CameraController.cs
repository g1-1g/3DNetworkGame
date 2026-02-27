using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerContext PlayerContext;

    private CinemachineCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
    }
    void Start()
    {
        PlayerContext.OnPlayerAssigned += PlayerSet;
    }

    private void PlayerSet(Transform transform)
    {
        Transform cameraRoot = transform.GetComponent<PlayerRotateAbility>().CameraRoot;
        _camera.Follow = cameraRoot;
    }

    private void OnDestroy()
    {
        if (PlayerContext != null)
        {
            PlayerContext.OnPlayerAssigned -= PlayerSet;    
        }
    }
}
