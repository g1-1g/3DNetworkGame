using System;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    public EAttackMode AttackMode;

    private bool _isBuffered;

    private bool _isAttacking;

    private EAttackType _currentAttackType;
    
    private PlayerAnimator _animator;

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        if (Input.GetMouseButtonDown(0) || _isBuffered)
        {
            if (_isAttacking)
            {
                _isBuffered = true;
                return; 
            }

            if (_owner.Stat.Stamina < _owner.Stat.StaminaDrainOnAttack) return;

            _isAttacking = true;

            _owner.Stat.Stamina -= _owner.Stat.StaminaDrainOnAttack;

            switch (AttackMode)
            {
                case EAttackMode.Sequential:
                    if (_currentAttackType >= EAttackType.Count - 1)
                    {
                        _currentAttackType = 0;
                    }
                    else
                    {
                        _currentAttackType ++;
                    }
                        _animator.PlayAttack(_currentAttackType);
                    break;
                case EAttackMode.Random:
                    _animator.PlayAttack((EAttackType)UnityEngine.Random.Range(0, (int)EAttackType.Count));
                    break;
            }

            _isBuffered = false;
        }
    }

    public void OnAttackFinish()
    {
        _isAttacking = false;
    }
}
