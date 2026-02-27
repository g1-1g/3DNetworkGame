using UnityEngine;

public class PlayerScoreGetAbility : PlayerAbility, IScoreGetable
{
    public bool TryGet(int score)
    {
        if (_owner.GameState != EGameState.Game) return false;

        if (_owner.PhotonView != null && _owner.PhotonView.IsMine)
        {
            PlayerScore.AddLocalScore(score);
        }
        return true;
    }
}
