using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int _attackTypeHash = Animator.StringToHash("AttackType");
    private readonly int _reactionTriggerHash = Animator.StringToHash("Reaction");
    private readonly int _reactionTypeHash = Animator.StringToHash("ReactionType");
    private readonly int _dieTriggerHash = Animator.StringToHash("Die");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetSpeedRatio(float ratio)
    {
        _animator.SetFloat(_speedRatioHash, ratio);
    }

    public void SetAttackTrigger()
    {
        _animator.SetTrigger(_attackTriggerHash);
    }

    public void SetReactionTrigger()
    {
        _animator.SetTrigger(_reactionTriggerHash);
    }

    public void PlayReaction(int type)
    {
        _animator.SetInteger(_reactionTypeHash, type);
        SetReactionTrigger();
    }

    public void PlayAttack(EAttackType type)
    {
        _animator.SetInteger(_attackTypeHash, (int)type);
        SetAttackTrigger();
    }

    [PunRPC]
    public void SetDieTrigger()
    {
        _animator.SetTrigger(_dieTriggerHash);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            PlayReaction(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayReaction(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayReaction(2);
        }
    }
}
