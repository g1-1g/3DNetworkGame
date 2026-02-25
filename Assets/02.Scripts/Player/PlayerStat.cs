using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float Health;

    public float MaxHealth;

    public float Stamina;

    public float MaxStamina;
    public float StaminaDrainOnRun;    // 달리기 소모
    public float StaminaDrainOnJump;   // 점프 소모
    public float StaminaDrainOnAttack;   // 공격 소모
    public float StaminaRecoveryRate; // 회복 속도

    public float Damage;

    public float WalkSpeed;
    public float RunMultiplier;
}