using System;
using System.Collections.Generic;
using BestHTTP.SocketIO3;
using MessagePack;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class LobbyScreen : MonoBehaviour
{
    public RoomItem roomItemPrefab;
    public Transform roomItemHolder;
    
    public string username;
    public TMP_Text usernameText;
    public Canvas lobbyScreen;
    
    public string currentRoomRequest;
    public Socket socket;

    public TMP_Text test;

    public void Set(string username)
    {
        lobbyScreen.gameObject.SetActive(true);
        usernameText.text = $"Welcome, {username} in Socket IO!";
        this.username = username;
        test.text = null;
        socket = ManagerSocket.GetSocket;
        GameManager.Instance.LobbyScreen.test.text = username;
        //SocketManager.OnSocketReceiver.AddListener(OnRequestRoom);
    }

    public void Reset()
    {
        lobbyScreen.gameObject.SetActive(false);
        usernameText.text = null;
        username = null;
        test.text = null;
    }

    public void LogOut()
    {
        ManagerSocket.Instance.OnDisconnectSocketIO();
        GameManager.Instance.loginScreen.Set();
        Reset();
    }

    public void TestClass()
    {
        test.text = null;
        Room room = new Room();
        room.id = "asdaqweIUY*72nn23";
        room.username = username;
        room.dataLength = 12039703.12f;

        string json = JsonUtility.ToJson(room);
        byte[] serialize = MessagePackSerializer.Serialize(json);
        socket.ExpectAcknowledgement<byte[]>(OnReceive).Emit(Constants.Room, serialize);

        void OnReceive(byte[] raw)
        {
            string json = MessagePackSerializer.Deserialize<string>(raw);
            Room room = JsonUtility.FromJson<Room>(json);
            test.text = "Room: " + JsonUtility.ToJson(room);
        }
    }
    
    public void TestClassV1()
    {
        test.text = null;
        Room room = new Room();
        room.id = "asdaqweIUY*72nn23";
        room.username = username;
        room.dataLength = 12039703.12f;
        
        Debug.Log(room);
        socket.ExpectAcknowledgement<Room>(OnReceive).Emit(Constants.Room1, room);

        void OnReceive(Room room)
        {
            Debug.Log(JsonUtility.ToJson(room));
            test.text = JsonUtility.ToJson(room);
        }
    }
    
    public void TestClassV2()
    {
        test.text = null;
        Room room = new Room();
        room.id = "asdaqweIUY*72nn23";
        room.username = username;
        room.dataLength = 12039703.12f;

        /*string json = JsonUtility.ToJson(room);
        Debug.Log(json);*/
        byte[] serialize = MessagePackSerializer.Serialize(room);
        Debug.Log(serialize.Length);
        socket.ExpectAcknowledgement<byte[]>(OnReceive).Emit(Constants.Room2, serialize);

        void OnReceive(byte[] raw)
        {
            string json = MessagePackSerializer.Deserialize<string>(raw);
            test.text = "JSON: \n" + JsonUtility.ToJson(json);
            Room room = JsonUtility.FromJson<Room>(json);
            test.text += "\n Room: " + JsonUtility.ToJson(room);

        }
    }

    public void TestStruct()
    {
        test.text = null;
        User user = new User();
        user.id = "1927n ksdfjl1-8asd";

        byte[] serialize = MessagePackSerializer.Serialize(user);
        socket.ExpectAcknowledgement<byte[]>(OnReceive).Emit(Constants.User, serialize);

        void OnReceive(byte[] raw)
        {
            try
            {
                User user = new User();
                user = MessagePackSerializer.Deserialize<User>(raw);
                Debug.Log(JsonUtility.ToJson(user));
                test.text = JsonUtility.ToJson(user);
            }
            catch (Exception e)
            {
                test.text = e.ToString();
            }
        }
    }
    

    public void TestVariable()
    {
        string user = username;
        byte[] serialize = MessagePackSerializer.Serialize(user);
        socket.ExpectAcknowledgement<byte[]>(OnReceive).Emit(Constants.Variable, serialize);

        void OnReceive(byte[] raw)
        {
            try
            {
                string username = MessagePackSerializer.Deserialize<string>(raw);
                test.text = username;
            }
            catch (Exception e)
            {
                test.text = e.ToString();
            }
        }
    }

    /*public void OnRequestRoom(Socket socket)
    {
        this.socket = socket;
        
        socket.On<byte[]>(Constants.RoomUpdate, (raw) =>
        {
            Room room = MessagePackSerializer.Deserialize<Room>(raw);

            if (!string.IsNullOrEmpty(room.id))
            {
                RoomItem roomItem = Instantiate(roomItemPrefab, roomItemHolder);
                roomItem.Set(room);
            }
        });
    }*/
}