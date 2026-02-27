using System;
using UnityEngine;

public class PlayerBinder : MonoBehaviour
{
    public GameObject Player;

    public event Action<Transform> OnPlayerAssigned;

    public void Start()
    {
        if (Player != null)
        {
            SetPlayer(Player);
        }
    }
    public void SetPlayer(GameObject player)
    {
        Player = player;
        OnPlayerAssigned?.Invoke(player.transform);
    }
}
