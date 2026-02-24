using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStat Stat { get; private set; }
    public PhotonView PhotonView { get; private set; }
    void Awake()
    {
        Stat = GetComponent<PlayerStat>();
        PhotonView = GetComponent<PhotonView>();
    }
}