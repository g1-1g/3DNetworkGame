using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private float _health;

    [SerializeField] private float _maxHealth;

    [SerializeField] private float _stamina;

    [SerializeField] private float _maxStamina;

    public float StaminaDrainOnRun;    // 달리기 소모
    public float StaminaDrainOnJump;   // 점프 소모
    public float StaminaDrainOnAttack;   // 공격 소모
    public float StaminaRecoveryRate; // 회복 속도

    public float Damage;

    public float WalkSpeed;
    public float RunMultiplier;

    public float Health => _health;
    public float Stamina => _stamina;

    public void Init()
    {
        _health = _maxHealth;
        _stamina = _maxStamina;
    }

    #region Health
    public float GetHealthRatio()
    {
        if (_maxHealth == 0) return 0;
        
        return _health / _maxHealth;
    }

    public void SetHealth(float value)
    {
        _health = Mathf.Clamp(value, 0, _maxHealth);
    }

    public void ConsumeHealth(float amount)
    {
        _health = Mathf.Clamp(_health - amount, 0, _maxHealth);
    }

    public void RegenHealth(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
    }

    #endregion

    #region Stamina
    public float GetStaminaRatio()
    {
        if (_maxStamina == 0) return 0;

        return _stamina / _maxStamina;
    }

    public void SetStamina(float value)
    {
        _stamina = Mathf.Clamp(value, 0, _maxStamina);
    }

    public void ConsumeStamina(float amount)
    {
        _stamina = Mathf.Clamp(_stamina - amount, 0, _maxStamina);
    }

    public void RegenStamina(float amount)
    {
        _stamina = Mathf.Clamp(_stamina + amount, 0, _maxStamina);
    }

    public bool CanJump()
    {
        return Stamina > StaminaDrainOnJump;
    }

    public bool CanRun()
    {
        return Stamina > StaminaDrainOnRun;
    }

    public bool CanAttack()
    {
        return Stamina > StaminaDrainOnAttack;
    }

    #endregion
}