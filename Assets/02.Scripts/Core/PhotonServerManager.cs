using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.Cinemachine;
using UnityEngine;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    // MonoBehaviour : Unity의 다양한 '이벤트' 콜백 함수를 오버라이드 할 수 있다.
    // MonoBehaviourPunCallbacks : PUN의 다양한 '서버 이벤트' 콜백 함수를 오버라이드할 수 있다.
    // - 서버 접속 성공 / 실패
    // - 방 입장 성공 / 실패
    // - 누군가 방에 입장 등등 ...

    private string _version = "0.0.1";
    private string _nickName = "G1";

    private void Start()
    {
        _nickName = $"{UnityEngine.Random.Range(0, 999)}";

        PhotonNetwork.GameVersion = _version;
        PhotonNetwork.NickName = _nickName;

        PhotonNetwork.SendRate = 30; // 얼마나 자주 데이터를 송수신할 것인지 (실제 송수신)
        PhotonNetwork.SerializationRate = 30; // 얼마나 자주 데이터를 직렬화 할 것인지 (송수신 준비)


        // 방장이 로드한 씬 게임에 다른 유저들도 똑같이 그 씬을 로드하도록 동기화
        // 방장 (마스터 클라이언트) : 방을 만든 '소유자' (방에는 하나의 마스터 클라이언트가 존재)
        // 방장이 씬 이동 시, 모두가 자동으로 씬 이동
        PhotonNetwork.AutomaticallySyncScene = true;

        // 위에 설정한 값들을 이용해서 서버로 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //포톤 서버에 접속하면 자동으로 호출되는 이벤트 함수
    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속 완료!");
        // 네임 서버 (AppId, GameVersion 등으로 구분되는 서버)

        Debug.Log(PhotonNetwork.CloudRegion);
        // 현재 어느 지역의 서버에 연결됐나?
        // ping 테스트를 통해 가장 빠른 리전으로 자동 연결 ( kr : korea )
    }

    public override void OnConnectedToMaster()
    {
        // 포톤 서버는 로비(=채널)이라는 개념 존재
        //TypedLobby lobby = new TypedLobby("3channel", LobbyType.Default);

        PhotonNetwork.JoinLobby(); // Default 로비 입장 시도
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
        Debug.Log(PhotonNetwork.InLobby);

        PhotonNetwork.JoinRandomRoom();
    }
}
