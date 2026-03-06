using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TextMeshProUGUI _masterNicknameTextUI;
    [SerializeField] private TextMeshProUGUI _playerCountText;
    [SerializeField] private Button _roomNameButton;
    [SerializeField] private TMP_InputField _nickNameInputField;

    private RoomInfo _roomInfo;

    public void OnEnable()
    {
        _roomNameButton.onClick.AddListener(OnClickJoinButton);
    }

    private void OnClickJoinButton()
    {
        PhotonRoomManager.Instance.JoinRoom(_nickNameInputField.text, _roomInfo.Name);
    }

    public void Init(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _masterNicknameTextUI.text = PhotonRoomManager.Instance.GetMasterName(roomInfo);
        _roomNameText.text = roomInfo.Name;
        _playerCountText.text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;

    }


    private void OnDisable()
    {
    }
}
