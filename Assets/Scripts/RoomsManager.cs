using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject roomPrefab;

    private readonly Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            if (!NewAndNotNull(roomList[i])) continue;

            rooms.Add(roomList[i].Name, CreateRoomPrefab(roomList[i]));
        }
    }

    private GameObject CreateRoomPrefab(RoomInfo roomInfo)
    {
        GameObject roomInstance = Instantiate(roomPrefab, container);

        if (roomInstance.TryGetComponent(out RoomView roomViewData))
        {
            roomViewData.SetData(roomInfo.Name);
        }

        return roomInstance;
    }

    private bool NewAndNotNull(RoomInfo roomInfo)
    {
        return roomInfo != null && !rooms.ContainsKey(roomInfo.Name);
    }

    public void DeleteDuplicateRooms(List<RoomInfo> roomList)
    {

    }

    public void DeleteDuplicateOrEmptyRooms(List<RoomInfo> roomList) { }
}
