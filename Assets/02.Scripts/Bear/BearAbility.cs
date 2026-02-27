using UnityEngine;

public class BearAbility : MonoBehaviour
{
    protected BearContext _context { get; private set; }

    protected virtual void Awake()
    {
        _context = GetComponent<BearContext>();
    }
}
