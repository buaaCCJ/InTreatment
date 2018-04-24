using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
/// <summary>
/// 局域网房间
/// </summary>
public class LocalDiscovery : NetworkDiscovery
{

    public GameObject RoomText;
    public GameObject Container;
    public GameObject NetManger;

    private Dictionary<String, int> BroadCastTicks;

    void Start()
    {
        this.showGUI = MainController.NetworkTest;
        BroadCastTicks = new Dictionary<String, int>();

        Initialize();

        RoomText = GameObject.Find("RoomText");
        Container = GameObject.Find("RoomContainer");
        NetManger = GameObject.Find("NetworkManager");
    }
    public void EnterLooby()
    {
        if (isClient || isServer) this.StopBroadcast();
        StartAsClient();
        InvokeRepeating("RefreshRoom", 1, 3);
        InvokeRepeating("TickTags", 1, 1);
    }

    public void RefreshRoom()
    {
        RoomText = GameObject.Find("RoomText");
        Container = GameObject.Find("RoomContainer");

        foreach (GameObject SubText in GameObject.FindGameObjectsWithTag("RoomItem"))
        {
            Destroy(SubText);
        }
        foreach (string key in broadcastsReceived.Keys)
        {
            NetworkBroadcastResult value;
            if (broadcastsReceived.TryGetValue(key, out value))
            {
                string val = DetectIPv4(key);
                string dat = Encoding.Unicode.GetString(value.broadcastData);
                GameObject tempText = MonoBehaviour.Instantiate(RoomText, Container.transform) as GameObject;
                tempText.transform.GetChild(0).GetComponent<Text>().text = dat + "(" + val + ")";
                tempText.GetComponent<RoomItemController>().Info.HostName = val;
                tempText.GetComponent<RoomItemController>().Info.BroadcastData = dat;
                tempText.tag = "RoomItem";
            }
        }
    }

    public void CreateRoom(string name = "default")
    {
        StopRefresh();
        Initialize();
        if (isClient || isServer) this.StopBroadcast();
        this.broadcastData = name;
        this.StartAsServer();
        NetManger.GetComponent<LocalNetwork>().StartRoomServer();
    }

    public void JoinRoom(string key = "localhost")
    {
        StopRefresh();
        NetManger.GetComponent<LocalNetwork>().StartRoomClient(key);
    }
    public void StopRefresh()
    {
        CancelInvoke();
    }
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //更新的广播超时计数器
        if (!BroadCastTicks.ContainsKey(fromAddress))
        {
            BroadCastTicks.Add(fromAddress, 3);
        }
        else
        {
            BroadCastTicks.Remove(fromAddress);
            BroadCastTicks.Add(fromAddress, 3);
        }

    }

    private void TickTags()
    {
        //检测超时并移除失效的广播信息
        int c = BroadCastTicks.Keys.Count;
        if (c == 0) return;

        string[] array = new string[c];
        BroadCastTicks.Keys.CopyTo(array, 0);
        foreach (string key in array)
        {
            int value;
            if (BroadCastTicks.TryGetValue(key, out value))
            {
                if (value <= 0)
                {
                    BroadCastTicks.Remove(key);
                    broadcastsReceived.Remove(key);
                }
                else
                {
                    BroadCastTicks.Remove(key);
                    BroadCastTicks.Add(key, value - 1);
                }
            }
        }
    }

    private string DetectIPv4(string text)
    {
        Regex reg = new Regex("(?<=(\\b|\\D))(((\\d{1,2})|(1\\d{2})|(2[0-4]\\d)|(25[0-5]))\\.){3}((\\d{1,2})|(1\\d{2})|(2[0-4]\\d)|(25[0-5]))(?=(\\b|\\D))", RegexOptions.IgnoreCase);
        Match match = reg.Match(text);
        if (match.Length > 0)
        {
            return match.Groups[0].Value;
        }
        return "";
    }
}
