using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using WolfGameLib;
using System.Text;
/// <summary>
/// 局域网玩家角色控制器
/// </summary>
public class PlayerController : NetworkBehaviour
{

    void Start()
    {
        gameObject.transform.parent = GameObject.Find("Canvas").transform;

    }

    void Update()
    {
        
    }

    [Command]
    void CmdSendClicked(Vector2 p)
    {
        GameObject.Find("BoardContainer").GetComponent<BoardController>().Clicked(p);
    }
    [ClientRpc]
    void RpcSendClicked(Vector2 p)
    {
        GameObject.Find("BoardContainer").GetComponent<BoardController>().Clicked(p);
    }

    public void RemoteClick(Vector2 p)
    {
        if (isServer)
        {
            RpcSendClicked(p);
        }
        else if(isClient)
        {
            CmdSendClicked(p);
        }

    }
}
