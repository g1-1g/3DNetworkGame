using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameTextUI;
    [SerializeField] private TextMeshProUGUI _playerCountTextUI;
    [SerializeField] private Button _roomExitButton;

    void Start()
    {
        _roomExitButton.onClick.AddListener(ExitRoom);

        if (PhotonNetwork.InRoom)
        {
            Refresh();
        }
    }

    void OnEnable()
    {
        PhotonRoomManager.Instance.OnDataChanged += Refresh;
    }

    void OnDisable()
    {
        PhotonRoomManager.Instance.OnDataChanged -= Refresh;
    }

    private void Refresh()
    {
        Room room = PhotonRoomManager.Instance.Room;

        _roomNameTextUI.text = room.Name;
        _playerCountTextUI.text = $"{room.PlayerCount} / {room.MaxPlayers}";
    }

    private void ExitRoom()
    {
        //todo
    }
}
