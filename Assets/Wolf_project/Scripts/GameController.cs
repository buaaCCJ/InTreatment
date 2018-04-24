using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WolfGameLib;

/// <summary>
/// 游戏场景控制器
/// </summary>
public class GameController : MonoBehaviour
{

    public Button PauseButton;          //暂停游戏
    public Button ResumeButton;         //继续游戏
    public Button HomeButton;           //返回主页

    public Button ReplayButton;         //重新开始
    public Button HomeButtonOver;		//返回主页

    public Button SingleButton;         //单人模式按钮
    public Button CoupleButton;         //双人模式按钮

    public Button EasyButton;           //简单模式
    public Button NormalButton;         //困难模式

    public Button WolfButton;           //狼阵营
    public Button SheepButton;          //羊阵营

    public Button ConsoleButton;        //单机模式
    public Button LANButton;            //联机模式

    public Button CreateButton;         //创建房间
    public Button JoinButton;           //加入房间

    public Button ReadyButton;          //准备比赛
    public Button BeginButton;          //开始比赛
    public Button WatchButton;          //观看比赛

    public Button MessageConfirmButton;
    public Button MessageCancelButton;

    public GameObject PausedScreen;     //游戏暂停界面
    public GameObject OverScreen;       //游戏结束界面
    public GameObject SelectScreen;     //玩家模式选择界面	
    public GameObject DifficultyScreen; //游戏难度选择界面
    public GameObject CampScreen;       //玩家阵营选择界面
    public GameObject NetModeScreen;    //双人网络模式界面
    public GameObject LobbyScreen;      //联机游戏大厅界面
    public GameObject RoomScreen;       //联机游戏房间界面
    public GameObject GameMessageScreen;       //游戏消息提示界面

    public GameObject BoardScreen;      //棋局界面
    public GameObject BoardView;        //棋盘视图

    public GameObject NetDiscovery;

    public RoomInformation SelectedRoom;    //选择的房间


    public GameObject RoomInputField;   //房间文本框

    public Text OverText;               //游戏结束提示消息

    public Text MessageTitleText;       //消息标题
    public Text MessageContentText;     //消息内容


    private int PlayerMode = 0;
    private int PlayerCamp = 0;
    private int DifficultyMode = 0;

    void Start()
    {
		
        PauseButton.onClick.AddListener(() => { OnGamePaused(); });
        ResumeButton.onClick.AddListener(() => { OnGameResumed(); });

        ReplayButton.onClick.AddListener(() => { SceneManager.LoadScene("wolf_red"); });
        HomeButton.onClick.AddListener(() => { OnGoHome(); });


        //HomeButtonOver.onClick.AddListener(() => { OnGoHome(); });
        PauseButton.onClick.AddListener(() => { OnGamePaused(); });

        SingleButton.onClick.AddListener(() => { SelectMode(0); });
        CoupleButton.onClick.AddListener(() => { SelectMode(1); });

        EasyButton.onClick.AddListener(() => { SelectDiffculty(0); });
        NormalButton.onClick.AddListener(() => { SelectDiffculty(1); });

        WolfButton.onClick.AddListener(() => { SelectCamp(0); });
        SheepButton.onClick.AddListener(() => { SelectCamp(1); });

        ConsoleButton.onClick.AddListener(() => { SelectNetMode(0); });
        LANButton.onClick.AddListener(() => { SelectNetMode(1); });

        CreateButton.onClick.AddListener(() => { SelectRoom(0); });
        JoinButton.onClick.AddListener(() => { SelectRoom(1); });

        BeginButton.onClick.AddListener(() => { ExcuteInRoom(0); });
        ReadyButton.onClick.AddListener(() => { ExcuteInRoom(1); });
        WatchButton.onClick.AddListener(() => { ExcuteInRoom(2); });

        MessageConfirmButton.onClick.AddListener(() => { ShowBox(null); });
        MessageCancelButton.onClick.AddListener(() => { ShowBox(null); });
		
        //ShowBox(SelectScreen);
		StartGame();
    }

    public void StartGame()
    {
        ShowBox(BoardScreen);
        BoardController bc = BoardView.GetComponent<BoardController>();
        bc.OnGameOver += OnGameOver;
        bc.OnGameMessage += OnGameMessage;
        bc.StartGame(PlayerMode, DifficultyMode, PlayerCamp);
    }

    private void OnGameMessage(string title, string message)
    {
        ShowBox(GameMessageScreen);
        MessageTitleText.text = title;
        MessageContentText.text = message;
    }

    private void OnGamePaused()
    {
        ShowBox(PausedScreen);
    }

    private void OnGameResumed()
    {
        ShowBox(BoardScreen);
    }

    public void OnGameOver(string message = "")
    {
        OverText.text = message;
        ShowBox(OverScreen, false);
    }

    private void OnGoHome()
    {
		SceneManager.LoadScene("wolf_red");
		//SceneManager.LoadScene("MainScene");
    }

    private void SelectMode(int mode)
    {
        PlayerMode = mode;
        if (mode == 0)
        {
            ShowBox(DifficultyScreen);
        }
        else
        {
            ShowBox(NetModeScreen);
        }
    }

    private void SelectDiffculty(int mode)
    {
        DifficultyMode = mode;
        ShowBox(CampScreen);
    }

    private void SelectCamp(int mode)
    {
        PlayerCamp = mode;
        StartGame();
    }

    private void SelectNetMode(int mode)
    {
        if (mode == 0)
        {
            BoardView.GetComponent<BoardController>().IsLANMode = false;
            StartGame();
        }
        else
        {
            BoardView.GetComponent<BoardController>().IsLANMode = true;
            ShowBox(LobbyScreen);
            NetDiscovery.GetComponent<LocalDiscovery>().EnterLooby();
        }
    }

    private void SelectRoom(int mode)
    {
        if (mode == 0)
        {
            NetDiscovery.GetComponent<LocalDiscovery>().StopRefresh();
            ShowBox(RoomScreen);
        }
        else
        {
            if (SelectedRoom.HostName == null || SelectedRoom.HostName == string.Empty) return;
            //ShowBox(RoomScreen);
            NetDiscovery.GetComponent<LocalDiscovery>().JoinRoom(SelectedRoom.HostName);
            //RoomInputField.GetComponent<InputField>().text = SelectedRoom.HostName;
        }
    }

    private void ExcuteInRoom(int mode)
    {
        if (mode == 0)
        {
            NetDiscovery.GetComponent<LocalDiscovery>().CreateRoom(RoomInputField.GetComponent<InputField>().text);
        }
        else if (mode == 1)
        {

        }
        else
        {

        }
    }


    private void ShowBox(GameObject target, bool hide = true)
    {
        if (hide)
        {
            PausedScreen.SetActive(false);     //游戏暂停界面
            OverScreen.SetActive(false);       //游戏结束界面
            SelectScreen.SetActive(false);    //玩家模式选择界面	
            DifficultyScreen.SetActive(false); //游戏难度选择界面
            CampScreen.SetActive(false);       //玩家阵营选择界面
            NetModeScreen.SetActive(false);    //双人网络模式界面
            LobbyScreen.SetActive(false);    //联机游戏大厅界面
            RoomScreen.SetActive(false);     //联机游戏房间界面
            GameMessageScreen.SetActive(false);     //联机游戏房间界面
        }
        if (target != null) { target.SetActive(true); }
    }


}
