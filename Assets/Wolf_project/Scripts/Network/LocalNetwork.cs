using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LocalNetwork : NetworkManager
{

    public bool IsServer;

    public void Start()
    {
        this.StopServer();
        this.StopClient();
    }
    public void StartRoomServer()
    {
        this.StopHost();
        this.networkAddress = "localhost";
        IsServer = true;
        this.StartHost();
    }

    public void StartRoomClient(string ipAddress = "localhost")
    {
        this.StopHost();
        this.networkAddress = ipAddress;
        IsServer = false;
        this.StartClient();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if (!IsServer)
        {
            GameObject.Find("GameView").GetComponent<GameController>().StartGame();
        }
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        if (IsServer)
        {
            GameObject.Find("GameView").GetComponent<GameController>().StartGame();
        }
    }
}

