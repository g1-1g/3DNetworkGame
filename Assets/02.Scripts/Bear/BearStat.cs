using UnityEngine;

public class BearStat : MonoBehaviour
{
    [SerializeField] private float _health;

    [SerializeField] private float _maxHealth;

    public float Damage;

    public float WalkSpeed;

    public float RunMultiplier;

    public float RunSpeed {  get; private set; }

    public float Health => _health;


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

    private void Start()
    {
        RunSpeed = WalkSpeed * RunMultiplier;
    }
}
