using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }

    private Room _room;
    public Room Room => _room;


    public event Action OnDataChanged;
    

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

        OnDataChanged?.Invoke();

        SpawnManager.Instance.Spawn();
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
}
