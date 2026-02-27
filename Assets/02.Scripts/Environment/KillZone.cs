using System.Collections;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public float KillDelay = 5;
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<IDamageable>(out IDamageable damageable);

        StartCoroutine(KillCoroutine(damageable)); 
        
    }

    private IEnumerator KillCoroutine(IDamageable damageable)
    {
        yield return new WaitForSeconds(KillDelay);

        damageable.Kill(EDieType.InstantRespawn);
    }
}
