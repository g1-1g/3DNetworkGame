using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Room : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private TMP_InputField _roomNameInputField;

    [SerializeField] private Button _createRoomButton;

    private void Start()
    {
        _createRoomButton.onClick.AddListener(() => MakeRoom());
    }
    private void MakeRoom()
    {
        string nickname = _nicknameInputField.text;
        string roomName = _roomNameInputField.text;

        PhotonRoomManager.Instance.MakeRoom(nickname, roomName);
    }
}
