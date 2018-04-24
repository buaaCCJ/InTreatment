using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
/// <summary>
/// 主场景控制器
/// </summary>
public class MainController : MonoBehaviour
{
    public static bool soundOn = true;
    public static bool musicOn = true;

    public static bool NetworkTest = false;

    public Button StartButton;              //开始按钮
    public Button SettingsButton;           //设置按钮
    public Button HelpButton;               //帮助按钮
    public Button AboutButton;              //关于按钮
    public Button ExitGameButton;           //退出按钮

    public Button SettingsCancelButton;
    public Button HelpCancelButton;
    public Button AboutCancelButton;
    public Button ExitGameCancelButton;

    public Button ExitGameConfirmButton;

    public Toggle SoundButton = null;
    public Toggle MusicButton = null;
    public Image SoundBackground = null;
    public Image MusicBackground = null;
    public Sprite TickSprite = null;
    public Sprite UntickSprite = null;

    public GameObject SettingsScreen;
    public GameObject HelpScreen;
    public GameObject AboutScreen;
    public GameObject ExitGameScreen;

    public Toggle NetworkButton = null;

    void Start()
    {
        StartButton.onClick.AddListener(() => { SceneManager.LoadScene("GameScene"); });

        SettingsButton.onClick.AddListener(() => { ShowBox(SettingsScreen); });
        HelpButton.onClick.AddListener(() => { ShowBox(HelpScreen); });
        AboutButton.onClick.AddListener(() => { ShowBox(AboutScreen); });
        ExitGameButton.onClick.AddListener(() => { ShowBox(ExitGameScreen); });

        SettingsCancelButton.onClick.AddListener(() => { ShowBox(null); });
        HelpCancelButton.onClick.AddListener(() => { ShowBox(null); });
        AboutCancelButton.onClick.AddListener(() => { ShowBox(null); });
        ExitGameCancelButton.onClick.AddListener(() => { ShowBox(null); });

        ExitGameConfirmButton.onClick.AddListener(() => { Application.Quit(); });

        MainController.soundOn = (PlayerPrefs.GetInt("SoundOn", 1) == 1) ? true : false;
        MainController.musicOn = (PlayerPrefs.GetInt("MusicOn", 1) == 1) ? true : false;


        if (MainController.soundOn)
        {
            SoundBackground.sprite = TickSprite;
            SoundButton.isOn = true;
            PlayerPrefs.SetInt("SoundOn", 1);
        }
        else
        {
            SoundBackground.sprite = UntickSprite;
            SoundButton.isOn = false;
            PlayerPrefs.SetInt("SoundOn", 0);
        }
        if (MainController.musicOn)
        {
            MusicBackground.sprite = TickSprite;
            MusicButton.isOn = true;
            PlayerPrefs.SetInt("MusicOn", 1);
        }
        else
        {
            MusicBackground.sprite = UntickSprite;
            MusicButton.isOn = false;
            PlayerPrefs.SetInt("MusicOn", 0);
        }

        SoundButton.onValueChanged.AddListener((value) => OnSoundChange(value));
        MusicButton.onValueChanged.AddListener((value) => OnMusicChange(value));

        NetworkButton.isOn = MainController.NetworkTest;
        NetworkButton.onValueChanged.AddListener((value) => OnNetworkChanged(value));

    }

    private void OnNetworkChanged(bool value)
    {
        MainController.NetworkTest = value;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsScreenActived())
            {
                ShowBox(null);
            }
            else
            {
                ShowBox(ExitGameScreen);
            }
        }
    }

    private void OnSoundChange(bool value)
    {
        if (value)
        {
            SoundBackground.sprite = TickSprite;
            SoundButton.isOn = true;
            MainController.soundOn = true;
        }
        else
        {
            SoundBackground.sprite = UntickSprite;
            SoundButton.isOn = false;
            MainController.soundOn = false;
        }
    }

    private void OnMusicChange(bool value)
    {
        if (value)
        {
            MusicBackground.sprite = TickSprite;
            MusicButton.isOn = true;
            MainController.musicOn = true;
        }
        else
        {
            MusicBackground.sprite = UntickSprite;
            MusicButton.isOn = false;
            MainController.musicOn = false;
        }
    }

    private void ShowBox(GameObject target, bool hide = true)
    {
        if (hide)
        {
            SettingsScreen.SetActive(false);
            HelpScreen.SetActive(false);
            AboutScreen.SetActive(false);
            ExitGameScreen.SetActive(false);
        }

        if (target != null)
        {
            target.SetActive(true);
        }
    }
    private bool IsScreenActived()
    {
        if (SettingsScreen.activeSelf || HelpScreen.activeSelf || AboutScreen.activeSelf || ExitGameScreen.activeSelf)
        {
            return true;
        }
        return false;
    }
}
