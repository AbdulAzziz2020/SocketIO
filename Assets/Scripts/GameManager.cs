using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string username;
    public LoginScreen loginScreen;
    public LobbyScreen LobbyScreen;
    public RoomScreen roomScreen;

    private void Start()
    {
        loginScreen.Set();
    }
}
