using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 房间列表按钮
/// </summary>
public class RoomItemController : MonoBehaviour
{
    public RoomInformation Info;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => { OnClick(); });
    }

    private void OnClick()
    {
        if (Info.HostName != string.Empty && Info.HostName != null)
        {
            GameObject.Find("GameView").GetComponent<GameController>().SelectedRoom = Info;
        }
    }
}
