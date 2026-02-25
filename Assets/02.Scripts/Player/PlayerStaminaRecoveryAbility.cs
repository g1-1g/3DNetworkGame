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

        if (_owner.Stat.GetStaminaRatio() < 1)
        {
            _owner.Stat.RegenStamina(_owner.Stat.StaminaRecoveryRate * Time.deltaTime);

        }
    }
}
