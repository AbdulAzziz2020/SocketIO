using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO3;
using MessagePack;
using NaughtyAttributes;
using UnityEngine;

public class AccountData : MonoBehaviour
{
    public string username = "test";
    public string message = "Hello World";
    
    [Button("Send")]
    public void Send()
    {
        AccountMessage newMessage = new AccountMessage(username, message);
        byte[] serialize = MessagePackSerializer.Serialize(newMessage);
        ManagerSocket.GetSocket.ExpectAcknowledgement<byte[]>(OnReceive).Emit(Constants.LoginKey, serialize);

        void OnReceive(byte[] response)
        {
            AccountMessage result = MessagePackSerializer.Deserialize<AccountMessage>(response);
            Debug.Log("Result: " + result.name + " || " + result.message);
        }
    }
}

[System.Serializable, MessagePackObject(keyAsPropertyName: true)]
public class AccountMessage
{
    public string name;
    public string message;

    public AccountMessage(string name, string message)
    {
        this.name = name;
        this.message = message;
    }
}

