using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private ScrollRect _scrollRect;
    void Start()
    {
        _logText.text = "방에 입장했습니다.";

        PhotonRoomManager.Instance.OnPlayerEnter += OnPlayerEnter;
        PhotonRoomManager.Instance.OnPlayerLeft += OnPlayerLeft;
        PhotonRoomManager.Instance.OnPlayerDied += PlayerDeathLog;
    }

    private void PlayerDeathLog(string attackerNickName, string victimNickName)
    {
        _logText.text += "\n" + $"{attackerNickName}님이 {victimNickName}님을 처치하였습니다.";
        OnContentChanged();
    }

    private void OnPlayerEnter(Player player)
    {
        _logText.text += "\n" + $"{player.NickName}님이 입장하였습니다.";
        OnContentChanged();
    }

    private void OnPlayerLeft(Player player)
    {
        _logText.text += "\n" + $"{player.NickName}님이 퇴장하였습니다.";
        OnContentChanged();
    }

    public void OnContentChanged()
    {
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    private void OnDestroy()
    {
        PhotonRoomManager.Instance.OnPlayerEnter -= OnPlayerEnter;
        PhotonRoomManager.Instance.OnPlayerLeft -= OnPlayerLeft;
        PhotonRoomManager.Instance.OnPlayerDied -= PlayerDeathLog;
    }
}
