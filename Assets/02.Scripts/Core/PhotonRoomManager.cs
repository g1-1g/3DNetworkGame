using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }

    public const string MasterSaveKey = "Room";

    private Room _room;
    public Room Room => _room;

    public event Action OnDataChanged;
    public event Action<List<RoomInfo>> OnRoomListUpdateEvent;
    public event Action<Player> OnPlayerEnter;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDied;

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료");

        Debug.Log($"룸 이름 : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 인원 : {PhotonNetwork.CurrentRoom.PlayerCount}");

        _room = PhotonNetwork.CurrentRoom;

        // AutomaticallySyncScene=true 일 때는 마스터만 LoadLevel 호출.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }

        /*if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            // 아무것도 하지 않아도.. 자동으로 방장이 있는 씬으로 옮겨진다.
        }*/

        OnDataChanged?.Invoke();

        //SceneManager.LoadScene("GameScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 입장 실패 : {returnCode} - {message}");

        // 랜덤 룸 입장에 실패하면 룸이 하나도 없으니 룸을 만들자

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20; // 룸 최대 접속자 수
        roomOptions.IsVisible = true; // 로비에서 룸을 보여줄 것인지
        roomOptions.IsOpen = true; // 룸의 오픈 여부

        // 룸 만들기
        PhotonNetwork.CreateRoom("test", roomOptions);
    }

    public void MakeRoom(string nickName, string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20; // 룸 최대 접속자 수
        roomOptions.IsVisible = true; // 로비에서 룸을 보여줄 것인지
        roomOptions.IsOpen = true; // 룸의 오픈 여부
        PhotonNetwork.NickName = nickName;

        roomOptions.CustomRoomProperties = new Hashtable
        {
            { MasterSaveKey, PhotonNetwork.NickName }
        };

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            MasterSaveKey
        };


        // 룸 만들기
        PhotonNetwork.CreateRoom(roomName, roomOptions);     
    }

    public void JoinRoom(string nickName, string roomName)
    {
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("룸 생성 완료");
        SetMasterName(PhotonNetwork.LocalPlayer);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerEnter?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerLeft?.Invoke(newPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        SetMasterName(newMasterClient);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        OnRoomListUpdateEvent?.Invoke(roomList);
    }

    [PunRPC]
    public void NotifyPlayerDeath(int attackerActorNumber)
    {
        string attackerNickName = _room.Players[attackerActorNumber].NickName;
        string victimNickName = PhotonNetwork.LocalPlayer.NickName;

        OnPlayerDied?.Invoke(attackerNickName, victimNickName);
    }


    public string GetMasterName(RoomInfo roomInfo)
    {
        if (roomInfo == null || roomInfo.CustomProperties == null)
        {
            return null;
        }
        if (roomInfo.CustomProperties.TryGetValue(MasterSaveKey, out var value))
        {
            if (value is string player) return player;
        }
        return null;
    }

    public void SetMasterName(Player player)
    {
        var props = new ExitGames.Client.Photon.Hashtable { { MasterSaveKey, player.NickName } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

}
