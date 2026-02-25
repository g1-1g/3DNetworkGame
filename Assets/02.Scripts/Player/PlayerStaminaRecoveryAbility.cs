using UnityEngine;

public class PlayerStaminaRecoveryAbility : PlayerAbility
{
    private PlayerMoveAbility moveAbility;

    protected override void Awake()
    {
        base.Awake();

        moveAbility = GetComponent<PlayerMoveAbility>();
    }
    void Update()
    {
        if (moveAbility == null || moveAbility.ShouldRun) return;

        if (_owner.Stat.Stamina < _owner.Stat.MaxStamina)
        {
            _owner.Stat.Stamina = Mathf.Min(_owner.Stat.Stamina + _owner.Stat.StaminaRecoveryRate * Time.deltaTime, _owner.Stat.MaxStamina);
        }
    }
}
