using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;


public class PlayerVoiceAbility : PlayerAbility
{
    public GameObject SpeakingIcon;
    [SerializeField] private PhotonVoiceView _voiceView;
    private Recorder _recorder;

    private void Start()
    {
        _recorder = FindAnyObjectByType<Recorder>();
        _voiceView = GetComponent<PhotonVoiceView>();

        _recorder.VoiceDetection = true; // 음성 감지 활성화
        _recorder.VoiceDetectionThreshold = 0.01f; // 음성 감지 임계값 설정
        _recorder.VoiceDetectionDelayMs = 300; // 음성 감지 지연 시간 설정 
    }
    void Update()
    {
        bool isSpeaking = false;

        if (_owner.PhotonView.IsMine)
        {
            isSpeaking = _recorder.IsCurrentlyTransmitting;
        }
        else
        {
            isSpeaking = _voiceView.IsSpeaking;
        } 

        SpeakingIcon.gameObject.SetActive(isSpeaking);

        if (Input.GetKeyDown(KeyCode.K))
        {
            //음소거 토글
            _recorder.TransmitEnabled = !_recorder.TransmitEnabled;
        }
    }
}
