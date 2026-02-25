using System;
using Photon.Pun;
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

            if (!_owner.Stat.CanAttack()) return;

            _isAttacking = true;

            _owner.Stat.ConsumeStamina(_owner.Stat.StaminaDrainOnAttack);

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

                    _owner.PhotonView.RPC(nameof(PlayerAttackAnimation), RpcTarget.All, _currentAttackType);

                    break;
                case EAttackMode.Random:

                    // RPX 매서드 호출 방식
                    // 다른 컴퓨터에 있는 내 플레이어 오브젝트의 PlayerAttack 메서드를 실행한다.
                    _owner.PhotonView.RPC(nameof(PlayerAttackAnimation), RpcTarget.All, (EAttackType)UnityEngine.Random.Range(0, (int)EAttackType.Count));
     
                    break;
            }

            _isBuffered = false;
        }
    }

    // 트랜스폼(위치, 회전, 스케일), 애니메이션(float 파라미터)와 같이 상시로 동기화가 필요한 데이터는 : IPun옵저블
    // 애니메이션 트리거처럼 간헐적으로 특정한 이벤트가 발생했을때만 변화하는 데이터 동기화는 데이터 동기화가 아닌 이벤트 동기화 : RPC
    // RPC : Remote Procedure Call (원격 함수 호출)
    // ㄴ 물리적으로 떨어져 있는 다른 디바이스의 내 포튼뷰의 함수를 호출하는 기능
    // RPX로 호출할 함수는 반드시 [PunRPC] 어트리뷰트를 함수 앞에 명시해야 함
    // 트리거 값을 공유하는 것이 아닌 함수 실행을 공유하는 것

    [PunRPC]
    private void PlayerAttackAnimation(EAttackType type)
    {
        _animator.PlayAttack(type);
    }

    public void OnAttackFinish()
    {
        _isAttacking = false;
    }
}
