using System;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviour
{
    public AttackMode AttackMode;

    private bool _isBuffered;

    private bool _isAttacking;

    private AttackType _currentAttackType;
    
    private PlayerAnimator _animator;

    private void Start()
    {
        _animator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || _isBuffered)
        {
            if (_isAttacking)
            {
                _isBuffered = true;
                return; 
            }

            _isAttacking = true;

            switch (AttackMode)
            {
                case AttackMode.Sequential:
                    if (_currentAttackType >= AttackType.Count - 1)
                    {
                        _currentAttackType = 0;
                    }
                    else
                    {
                        _currentAttackType ++;
                    }
                        _animator.PlayAttack(_currentAttackType);
                    break;
                case AttackMode.Random:
                    _animator.PlayAttack((AttackType)UnityEngine.Random.Range(0, (int)AttackType.Count));
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
