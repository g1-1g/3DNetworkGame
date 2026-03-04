using Photon.Pun;
using UnityEngine;

public class BearContext : MonoBehaviour
{
    public BearStat Stat { get; private set; }
    public PhotonView PhotonView { get; private set; }

    public BearAnimator Animator { get; private set; }

    void Awake()
    {
        Stat = GetComponent<BearStat>();
        PhotonView = GetComponent<PhotonView>();
        Animator = GetComponent<BearAnimator>();
    }
}
