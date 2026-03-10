using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance { get; private set; }

    private Dictionary<int, ScoreData> _scores = new();
    public ReadOnlyDictionary<int, ScoreData> Scores => new(_scores);

    public event Action OnPlayerScoreChanged;

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

    public void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PlayerScore.EnsureLocalScore();
        }
    }

    public override void OnJoinedRoom()
    {
        PlayerScore.EnsureLocalScore();
    }

    // [데이터 공유]
    // 1. OnSerializeView (+TransformView, AnimationView, ...)
    //     ㄴ c# 기본 타입, Vector,
    //        ㄴ PhtonNetwork...Rate...에 따라
    // 2. RPC -> 매개변소를 활용해서 데이터 동기화
    //          ㄴ 즈로 변화가 빈번하지 않는 데이터를 함수 호출을 이용해서 동기화
    // 3. 커스텀 프로퍼티 (Custom Property)
    //          ㄴ 주로 변화가 빈번하지 않은 데이터들을 해시 테이블로 동기화
    //          ㄴ (플레이어 준비 상태, 점수, 룸의 모드 등등)


    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayerScore.ScoreKey))
        {
            OnPlayerScoreChanged?.Invoke();
        }
    }
}
