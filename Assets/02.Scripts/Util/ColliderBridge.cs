using System;
using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    public event Action<Collider> OnTriggerEnterEvent;

    public Collider Collider {  get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }
}
