using TMPro;
using UnityEngine;

namespace photonMenuLobby
{
    public class RoomItem : MonoBehaviour
    {

        public TMP_Text roomName;

        LobbyManager manager;

        void Start()
        {
            manager = GameObject.FindObjectOfType<LobbyManager>();
        }
        public void SetRoomName(string _roomName)
        {
            roomName.text = _roomName;
        }

        public void OnClickItem()
        {
            manager.JoinRoom(roomName.text);
        }
    }
}