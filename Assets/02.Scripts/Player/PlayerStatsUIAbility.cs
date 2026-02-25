using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIAbility : PlayerAbility
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _staminaSlider;

    void Update()
    {
        _hpSlider.value = _owner.Stat.Health / _owner.Stat.MaxHealth;

        _staminaSlider.value = _owner.Stat.Stamina / _owner.Stat.MaxStamina;
    }
}
