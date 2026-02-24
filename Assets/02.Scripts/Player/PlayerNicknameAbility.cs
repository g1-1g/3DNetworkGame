using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    void Start()
    {
        _nicknameTextUI.text = _owner.PhotonView.Owner.NickName;

        _nicknameTextUI.color = _owner.PhotonView.IsMine ? Color.white : Color.aliceBlue;
    }

    // Update is called once per frame
    void Update()
    {
        _nicknameTextUI.transform.forward = Camera.main.transform.forward;
    }
}
