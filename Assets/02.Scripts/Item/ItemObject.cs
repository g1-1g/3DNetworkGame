using Photon.Pun;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private PhotonView _view;
    private bool _destroyRequested;
    [SerializeField] private int _scoreValue = 1;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(
         $"[ItemTrigger] self={name} id={GetInstanceID()} scene ={ gameObject.scene.name}active ={ gameObject.activeInHierarchy}" +
          $"viewId={_view?.ViewID} owner={_view?.Owner?.NickName} " +
          $"other={other.name} otherLayer={other.gameObject.layer}otherRoot ={ other.transform.root.name}"
      );
        if (_destroyRequested)
        {
            return;
        }

        if (other.TryGetComponent<IScoreGetable>(out IScoreGetable scoreGetable))
        {
            if (!scoreGetable.TryGet(_scoreValue)) return;

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
