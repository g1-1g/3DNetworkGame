using System.Collections.Generic;
using System.Linq;
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

        List<ScoreData> scoreDates = scores.Values.ToList();

        for (int i = 0; i < _scoreItems.Count; i++)
        {
            if (scoreDates.Count <= i)
            {
                _scoreItems[i].SetScore(string.Empty, 0);
                continue;
            }
            ScoreData data = scoreDates[i];
            if (data.Score == 200)
            {

            }
            _scoreItems[i].SetScore(data.Nickname, data.Score);
        }
    }

    public void OnDestroy()
    {
        ScoreManager.Instance.OnPlayerScoreChanged -= OnScoreUpdate;
    }
}
