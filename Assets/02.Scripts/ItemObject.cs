using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemObject : MonoBehaviour
{
    private PhotonView _view;
    private bool _destroyRequested;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger with {other.name}, layer ={ other.gameObject.layer}");

        if (_destroyRequested)
        {
            return;
        }

        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            Debug.Log(controller.GameState);
            if (controller.GameState != EGameState.Game) return;

            _destroyRequested = true;

            if (_view != null && _view.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
                return;
            }

            var target = _view != null && _view.Owner != null
                ? _view.Owner
                : PhotonNetwork.MasterClient;

            if (_view != null && target != null)
            {
                _view.RPC(nameof(RequestDestroy), target);
            }
        }
    }

    [PunRPC]
    private void RequestDestroy()
    {
        if (_view != null && (_view.IsMine || PhotonNetwork.IsMasterClient))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
