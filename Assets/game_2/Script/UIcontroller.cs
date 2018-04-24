using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour {

    private DominoManger gameManager;

    private Button BuildBtn;
    private Button PlayBtn;
    private Button BackBtn;
    private Button DeleteBtn;
    private Button RestartBtn;
    private Button SkipBtn;
    //private Text MsgTxt;
    private Image FailedImage;
    private bool isFailedImageShow;
    private Button FailedRestartBtn;
    private Button FailedSkipBtn;

    void Awake()
    {
        gameManager = DominoManger.Instance;

        if (gameManager == null)
        {
            Debug.LogError("Startdomino not found!");
            enabled = false;
            return;
        }
    }

    void Start () {

        BuildBtn = GameObject.Find("BuildBtn").GetComponent<Button>();
        //BuildBtn.onClick.AddListener(CreateDomino);

        PlayBtn = GameObject.Find("PlayBtn").GetComponent<Button>();
        PlayBtn.onClick.AddListener(gameManager.pushDonimo);

        BackBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        BackBtn.onClick.AddListener(gameManager.backDonimo);

        DeleteBtn = GameObject.Find("DelBtn").GetComponent<Button>();
        DeleteBtn.onClick.AddListener(gameManager.DelcurrentDomino);

        RestartBtn = GameObject.Find("RestartBtn").GetComponent<Button>();
        RestartBtn.onClick.AddListener(gameManager.LosetheGame);

        SkipBtn = GameObject.Find("SkipBtn").GetComponent<Button>();
        SkipBtn.onClick.AddListener(gameManager.SkipLevel);

        FailedImage = GameObject.Find("FailedImage").GetComponent<Image>();
        isFailedImageShow = false;

        FailedRestartBtn = FailedImage.transform.Find("FailedRestartBtn").GetComponent<Button>();
        FailedRestartBtn.onClick.AddListener(gameManager.RestartDomino);

        FailedSkipBtn = FailedImage.transform.Find("FailedSkipBtn").GetComponent<Button>();
        FailedSkipBtn.onClick.AddListener(gameManager.SkipLevel);

        //MsgTxt = GameObject.Find("MsgText").GetComponent<Text>();
        //MsgTxt.enabled = false;

        ResetButton();
    }
	
	void Update () {
	}

    public void CreateDomino()
    {
        if (gameManager == null)
            return;
        gameManager.BuildingDonimo("Domino");
    }

    //void StartPushDomino()
    //{
    //    gameManager.pushDonimo();
    //}

    //void DelcurrentDomino()
    //{
    //    gameManager.DelcurrentDomino(); 
    //}

    //void RestartDomino()
    //{
    //    gameManager.RestartDomino();
    //}

    //void SkipLvl()
    //{
    //    gameManager.SkipLevel();
    //}

    public void turnButton()
    {
        if (BackBtn.gameObject.activeInHierarchy)
        {
            BackBtn.gameObject.SetActive(false);
            PlayBtn.gameObject.SetActive(true);
        }
        else if (PlayBtn.gameObject.activeInHierarchy)
        {
            BackBtn.gameObject.SetActive(true);
            PlayBtn.gameObject.SetActive(false);
        }
    }

    public void ResetButton()
    {
        BackBtn.gameObject.SetActive(false);
        PlayBtn.gameObject.SetActive(true);
    }

    //public void ShowResultText(string str)
    //{
    //    MsgTxt.enabled = true;
    //    MsgTxt.text = str;
    //}
    //public void HideWindText()
    //{
    //    MsgTxt.enabled = false;
    //}
    public void ShowFailedImage()
    {
        FailedImage.GetComponent<Animator>().Play("flyin");
        isFailedImageShow = true;
    }

    public void HideFailedImage()
    {
        if(isFailedImageShow)
        {
            FailedImage.GetComponent<Animator>().Play("flyout");         
        }
    }
}
