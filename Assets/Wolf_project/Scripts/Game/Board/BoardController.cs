using UnityEngine;
using System.Collections;
using WolfGameLib;
using UnityEngine.UI;
using System;
/// <summary>
/// 游戏棋盘控制器
/// </summary>

public class BoardController : MonoBehaviour
{

    public static GameBoard Board;

    public AudioSource PieceAudio;

    public GameObject Canvas;
    public GameObject Wolf;
    public GameObject Sheep;

    public Button TooltipButton;
    public Button GoBackButton;
    public Button DefeateButton;
    public Button DogfallButton;
    public Toggle SoundButton;
    public Toggle MusicButton;
    public Button HelpButton;

    public Text HPText;
    public Text MPText;

    public Image HPImage;
    public Image MPImage;

    public GameObject Enemy;

    public bool IsLANMode;

    public event OnGameOverEventHandler OnGameOver;
    public delegate void OnGameOverEventHandler(string message);

    public event OnGameMessageEventHandler OnGameMessage;
    public delegate void OnGameMessageEventHandler(string title,string message);

    void Start()
    {
        TooltipButton.onClick.AddListener(() => { OnTooltipClicked(); });
        GoBackButton.onClick.AddListener(() => { OnGoBackClicked(); });
        DefeateButton.onClick.AddListener(() => { OnDefeateClicked(); });
        DogfallButton.onClick.AddListener(() => { OnDogfallClicked(); });


        SoundButton.isOn = !MainController.soundOn;
        MusicButton.isOn = !MainController.musicOn;

        SoundButton.onValueChanged.AddListener((value) => { MainController.soundOn = !value; });
        MusicButton.onValueChanged.AddListener((value) => { MainController.musicOn = !value; });

    }

    private void OnTooltipClicked()
    {
        Board.AI.Move(Board);
    }

    private void OnGoBackClicked()
    {
        if (Board.Map.Movements.Count == 0)
        {
            OnGameMessage("游戏提示", "现在还不是悔棋的时候！");
        }
        if (Board.PlayerMode == PlayerMode.Single)
        {
            Board.GoBack(2);
        }
        else
        {
            Board.GoBack(1);
        }
    }

    private void OnDefeateClicked()
    {
        
            string message = "小红帽放弃了尝试！";
            OnGameOver(message);
        
    }

    private void OnDogfallClicked()
    {
        OnGameMessage("跳过游戏", "进入游戏下一部分");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas.transform as RectTransform, Input.mousePosition, null, out pos))
            {
                Wolf.GetComponent<RectTransform>().localPosition = pos;
                MouseDownWithPoint(Wolf.GetComponent<RectTransform>().anchoredPosition);
            }
        }
    }

    public void StartGame(int pm, int dm, int cm)
    {
        Board = new GameBoard();
        //Board.PlayerMode = (pm == 0 ? PlayerMode.Single : PlayerMode.Couple);
        Board.PlayerMode = PlayerMode.Single;
        //Board.AI.Difficulty = (dm == 0 ? DifficultyMode.Easy : DifficultyMode.Normal);
        Board.AI.Difficulty = DifficultyMode.Easy;
        //Board.PlayerCamp = (cm == 0 ? Camp.Wolf : Camp.Sheep);
        Board.PlayerCamp = Camp.Sheep;
        Board.MapChanged += Board_MapChanged;
        Board.GameOver += Board_GameOver;
        Board.Start();
    }

    public void Clicked(Vector2 p)
    {
        if (Board != null)
        {
            Board.Clicked(new VectorInt((int)p.x, (int)p.y));
            //Board.ClickedRaw((int)p.x, (int)p.y,80);
        }
    }

    private void Board_MapChanged(object sender, MapChangedEventArgs e)
    {
        int sheepCount = Board.Map.SheepRemaining;
        foreach (GameObject SubPiece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            Destroy(SubPiece);
        }

        foreach (IPiece SubPiece in Board.Map.Pieces)
        {
            if (SubPiece == null) continue;
            int bw = 79;
            int bwp = 39;
            int dx = (SubPiece.Location.Y < 2 || SubPiece.Location.Y > 6) ? bwp : bw;
            int dy = (SubPiece.Location.Y - 1) * bw;
            if (SubPiece.Location.Y < 2) dy = SubPiece.Location.Y * bwp;
            if (SubPiece.Location.Y > 6) dy = (SubPiece.Location.Y - 6) * bwp + 5 * bw;

            bool isOpposite = SubPiece.Camp == Camp.Wolf ? true : false;
            bool isScale = SubPiece.Location.Y < 2 || SubPiece.Location.Y > 6 ? true : false;
            if (isOpposite)
            {
                GameObject tempWolf = MonoBehaviour.Instantiate(Wolf, Canvas.transform) as GameObject;
                tempWolf.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(bw * 2 + (SubPiece.Location.X - 2) * dx, -dy, 0);
                tempWolf.transform.localScale = new Vector3(1f, 1f, 1f);
                if (isScale) tempWolf.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
                if (Board.Map.ActivedCamp != Camp.Wolf) tempWolf.GetComponent<Selectable>().interactable = false;
                tempWolf.tag = "Piece";
            }
            else
            {
                sheepCount += 1;
                GameObject tempSheep = MonoBehaviour.Instantiate(Sheep, Canvas.transform) as GameObject;
                tempSheep.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(bw * 2 + (SubPiece.Location.X - 2) * dx, -dy, 0);
                tempSheep.transform.localScale = new Vector3(1f, 1f, 1f);
                if (isScale) tempSheep.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
                if (Board.Map.ActivedCamp != Camp.Sheep || Board.Map.SheepRemaining > 0)
                {
                    tempSheep.GetComponent<Selectable>().interactable = false;
                }
                tempSheep.tag = "Piece";
            }
        }

        HPText.text = sheepCount.ToString() + "/24";
        MPText.text=Board.Map.SheepRemaining.ToString() + "/16";

        if (Board.Map.ActivedCamp == Camp.Wolf)
        {
            Enemy.SetActive(true);
        }
        else
        {
            Enemy.SetActive(false);
        }

        HPImage.GetComponent<Image>().transform.localScale = new Vector3(((float)sheepCount) / 24, 1, 1);
        MPImage.GetComponent<Image>().transform.localScale = new Vector3(((float)Board.Map.SheepRemaining) / 16, 1, 1);

        if (MainController.soundOn) { PieceAudio.Play(); }
    }

    private void Board_GameOver(object sender, GameOverEventArgs e)
    {
        //string message = e.Winner == Camp.Wolf ? "狼赢得了比赛!" : "羊赢得了比赛!";
        string message = e.Winner == Camp.Wolf ? "你输了这场比赛!" : "你赢得了比赛!";
        if (message == "你赢得了比赛!")
        {
            OnGameMessage("获得胜利", "");
        }
        else
        {
            OnGameOver(message);
        }

    }

    private void MouseDownWithPoint(Vector3 point)
    {
        int bw = 79;
        int bwp = 39;

        float px = point.x;
        float py = -point.y;

        int x = Mathf.RoundToInt(px / bw);
        int y = Mathf.RoundToInt(py / bw) + 1;
        if (py <= bw - bwp / 2)
        {
            x = 2 + Mathf.RoundToInt((px - bw * 2) / bwp);
            y = Mathf.RoundToInt(py / bwp);
        }
        else if (py >= bw * 5 + bwp / 2)
        {
            x = 2 + Mathf.RoundToInt((px - bw * 2) / bwp);
            y = Mathf.RoundToInt((py - (bw * 5)) / bwp) + 6;
        }
        Debug.Log(point.ToString() + "," + x.ToString() + "," + y.ToString());

        if (IsLANMode)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("NetPlayer");
            foreach (var item in temp)
            {
                PlayerController pc = item.GetComponent<PlayerController>();
                if ((pc.isServer && Board.Map.ActivedCamp == Camp.Wolf) ||
                     (pc.isClient && Board.Map.ActivedCamp == Camp.Sheep && !pc.isServer))
                {
                    if (pc.isLocalPlayer)
                    {
                        pc.RemoteClick(new Vector2(x, y));
                        Clicked(new Vector2(x, y));
                    }
                }
            }
        }
        else
        {
            Clicked(new Vector2(x, y));
        }
    }

}







