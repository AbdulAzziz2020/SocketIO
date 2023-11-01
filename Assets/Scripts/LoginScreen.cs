using System;
using BestHTTP.SocketIO3;
using MessagePack;
using TMPro;
using UnityEngine;

public class LoginScreen : MonoBehaviour
{
    public Canvas loginScreen;
    public TMP_InputField inputField;
    public string tmp;

    public void Set()
    {
        loginScreen.gameObject.SetActive(true);
        inputField.text = string.Empty;
        tmp = null;
        
        ManagerSocket.Instance.OnConnectSocketIO();
    }

    public void Reset()
    {
        loginScreen.gameObject.SetActive(false);
        inputField.text = string.Empty;
        tmp = null;
    }

    public void OnTypeInput(string value)
    {
        tmp = value;
    }

    public void OnLogin()
    {
        if (string.IsNullOrEmpty(tmp))
        {
            Debug.Log("check user");
            return;
        }
        
        ManagerSocket.Instance.Login(tmp, (result) =>
        {
            GameManager.Instance.LobbyScreen.Set(result);
            Reset();
        });
    }

 
}