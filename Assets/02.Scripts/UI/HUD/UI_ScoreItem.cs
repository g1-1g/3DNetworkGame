using System;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UI_ScoreItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nickNameText;


    public void SetScore(String player, int score)
    {
        _nickNameText.text = player;
        _scoreText.text = score.ToString();
    }
}
