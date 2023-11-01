using BestHTTP.SocketIO3;
using MessagePack;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public Room room;
    public TMP_Text roomId;
    public TMP_Text roomUser;

    public void Set(Room room)
    {
        this.room = room;
        roomId.text = room.id;
        roomUser.text = room.username;
    }

    public void OnJoinRoom(LobbyScreen lobbyScreen)
    {
        if (string.IsNullOrEmpty(room.id))
        {
            Debug.Log("Room not valid");
            return;
        }

        if (!string.IsNullOrEmpty(lobbyScreen.currentRoomRequest))
        {
            Debug.Log("wait for requesting join room!");
            return;
        }

        byte[] serialize = MessagePackSerializer.Serialize(lobbyScreen.username);
        lobbyScreen.socket.ExpectAcknowledgement<byte[]>(OnReceive).Emit(Constants.JoinRoom, serialize);

        void OnReceive(byte[] raw)
        {
            string result = MessagePackSerializer.Deserialize<string>(raw);

            if (result.Contains("Success"))
            {
                GameManager.Instance.roomScreen.Set();
                lobbyScreen.Reset();
            }
            
            lobbyScreen.currentRoomRequest = null;
        }
    }
}

[System.Serializable, MessagePackObject(keyAsPropertyName: true)]
public class Room
{
    public string id;
    public string username;
    public float dataLength;
}

[System.Serializable, MessagePackObject(keyAsPropertyName: true)]
public struct User
{
    public string id;
}