using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Collider _collider;

    public event Action<Collider> OnHit;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
    
    public void Activate()
    {
        _collider.enabled = true;
    }

    public void Deactivate()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit?.Invoke(other);
    }
}
