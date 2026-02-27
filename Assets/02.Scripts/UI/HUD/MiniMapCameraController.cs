using System;
using UnityEngine;

public class MiniMapCameraController : MonoBehaviour
{
    public PlayerBinder PlayerContext;
    [SerializeField] private float _offsetY = 10f;

    private Transform _target;
    
    private void Start()
    {
        PlayerContext.OnPlayerAssigned += SetTarget;
    }

    private void SetTarget(Transform transform)
    {
        _target = transform;
    }

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 targetPosition = _target.position;
        Vector3 finalPosition = _target.position + new Vector3(0f, _offsetY, 0f);

        transform.position = finalPosition;
        Vector3 targetAngle = _target.eulerAngles;
        targetAngle.x = 90;

        transform.eulerAngles = targetAngle;
    }

    private void OnDestroy()
    {
        if (PlayerContext != null)
        {
            PlayerContext.OnPlayerAssigned -= SetTarget;
        }
    }
}
