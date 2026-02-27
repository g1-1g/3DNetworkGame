using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nickNameText;
    void Start()
    {
        PhotonRoomManager.Instance.OnPlayerScoreChanged += OnScoreUpdate;
    }

    private void OnScoreUpdate(Player player, int score)
    {
        if (player.IsLocal)
        {
            _nickNameText.text = player.NickName;
            _scoreText.text = score.ToString();
        }
    }
}
