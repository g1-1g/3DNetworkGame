using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

public class UI_RoomList : MonoBehaviour
{
    private UI_RoomItem[] _roomItems;

    private Dictionary<string, RoomInfo> _rooms = new Dictionary<string, RoomInfo>();

    private void Awake()
    {
        _roomItems = GetComponentsInChildren<UI_RoomItem>();
        HideAllRoomUI();
    }

    private void OnEnable()
    {
        PhotonRoomManager.Instance.OnRoomListUpdateEvent += UpdateRoomList;
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        HideAllRoomUI();

        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                _rooms.Remove(room.Name);
            }
            else
            {
                _rooms[room.Name] = room;
            }
        }
        
        List<RoomInfo> rooms = _rooms.Values.ToList();

        for (int i = 0; i < rooms.Count; i++)
        {
            _roomItems[i].gameObject.SetActive(true);
            _roomItems[i].Init(rooms[i]);
        }
    }

    private void HideAllRoomUI()
    {
        foreach (UI_RoomItem roomItem in _roomItems){
            roomItem.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (PhotonRoomManager.Instance == null) return;
        PhotonRoomManager.Instance.OnRoomListUpdateEvent -= UpdateRoomList;
    }
}

