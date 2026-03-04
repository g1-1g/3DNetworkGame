using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Photon.Pun;
using UnityEngine;

public class UI_Scores : MonoBehaviour
{
    private List<UI_ScoreItem> _scoreItems;

    private void Start()
    {
        _scoreItems = GetComponentsInChildren<UI_ScoreItem>().ToList();
        ScoreManager.Instance.OnPlayerScoreChanged += OnScoreUpdate;
    }

    private void OnScoreUpdate()
    {
        var scores = ScoreManager.Instance.Scores;

        var sorted = PhotonNetwork.PlayerList.OrderByDescending
            (p => PlayerScore.GetScore(p)).ToList();

        for (int i = 0; i < _scoreItems.Count; i++)
        {
            if (sorted.Count <= i)
            {
                _scoreItems[i].SetScore(string.Empty, 0);
                continue;
            }
            _scoreItems[i].SetScore(sorted[i].NickName, PlayerScore.GetScore(sorted[i]));
        }
    }

    public void OnDestroy()
    {
        ScoreManager.Instance.OnPlayerScoreChanged -= OnScoreUpdate;
    }
}
