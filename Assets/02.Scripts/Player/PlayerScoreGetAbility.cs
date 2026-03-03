using System;
using UnityEngine;

public class PlayerScoreGetAbility : PlayerAbility, IScoreGetable
{
    private void Start()
    {
        _owner.OnDie += HalveScore;
    }

    public bool TryGet(int score)
    {
        if (_owner.GameState != EGameState.Game) return false;

        if (_owner.PhotonView != null && _owner.PhotonView.IsMine)
        {
            PlayerScore.AddLocalScore(score);
        }
        return true;
    }

    private void HalveScore(EDieType type)
    {
        PlayerScore.HalveScore();  
    }
}
