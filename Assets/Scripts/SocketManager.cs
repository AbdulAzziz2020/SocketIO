using System;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Transports;
using MessagePack.Resolvers;
using UnityEngine;
using UnityEngine.Events;

public class ManagerSocket : SingletonPersistant<ManagerSocket>
{
    public int socketPort = 3000;
    
    private static Socket socket;
    public static Socket GetSocket => socket ?? null;
    public static SocketManager manager;

    public static UnityEvent<Socket> OnSocketReceiver = new UnityEvent<Socket>();

    public void OnConnectSocketIO()
    {
        if(manager != null) manager.Close();
        Debug.Log("Initialize Socket!");

        SocketOptions defaultOption = new SocketOptions();
        defaultOption.ConnectWith = TransportTypes.WebSocket;
        //var manager = new SocketManager(new Uri($"http://localhost:{socketPort}"));
        manager = new SocketManager(new Uri($"http://103.150.191.85:{socketPort}"), defaultOption);
        
        Debug.Log(manager != null);
        if (manager == null)
        {
            Debug.Log("Manager is null, please init the manager");
            return;
        }

        socket = manager.Socket;
        manager.Open();

        Debug.Log(socket.IsOpen);
        if (socket.IsOpen || socket != null)
        {
            Debug.Log("Socket already to use, Enjoy!");
        }
        
        SocketReceiver(socket);
    }

    public void OnDisconnectSocketIO()
    {
        if (manager != null)
        {
            manager.Close();
        }
    }

    public void SocketReceiver(Socket socket)
    {
        socket.On("connect", () =>
        {
            Debug.Log("Socket Connected, id: " + socket.Id);
        });
        
        socket.On("disconnect", () =>
        {
            Debug.Log("Socket Disconnected");
        });
        
    }

    public void Login(string username, Action<string> result)
    {
        socket.ExpectAcknowledgement<string>(OnReceive).Emit(Constants.LoginKey, username);

        void OnReceive(string raw)
        {
            result.Invoke(raw);
        }
    }
    
    static MessagePack.MessagePackSerializerOptions opt;

    public static MessagePack.MessagePackSerializerOptions MsgPackOpt()
    {
        if (opt == null)
        {
            var sc = StaticCompositeResolver.Instance;
            sc.Register(
                StandardResolver.Instance
            );

            opt = new MessagePack.MessagePackSerializerOptions(sc);
            opt.WithOmitAssemblyVersion(true);
        }

        return opt;
    }

}