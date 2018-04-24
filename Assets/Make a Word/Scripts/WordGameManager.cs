using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Fungus;
public class WordGameManager : MonoBehaviour
{
    public bool useDictionary;                      // Should we use the dictionary?
    public TextAsset dictionary;                    // If useDictionary is true, this is the dictionary that will be used.
    public string[] words;
    public bool rotateTiles = false;                // if true, tiles with letters will be rotated. This can make game harder.
    public GameObject letterTile;
    public GameObject letterHolder;
    public GameObject parentObject;
    public float spacing;                           // Spacing between the letter fields
    public Flowchart fl;
    // don't change following variables
    // if not sure what you are doing 
    private bool showMsg, showHint = false;
    private int hintPos;
    private Ray ray;
    private RaycastHit hit;
    private GameObject current, backObj, parentObj;
    private char[] _word;
    private Vector3 center;
    private string[] tempValues;
    [HideInInspector]
    public bool container = false;
    private string tileName = "tileName";
    private string holderName = "holderName";
    private string word, message, hintString;
    [HideInInspector]
    public string letterHolderValue;
    [HideInInspector]
    public int letterOrder;
    [HideInInspector]
    public bool selected = false;
    [HideInInspector]
    public bool isgameready = false;

    private int wordcount;
    public GameObject[] Dairy_book;
    //btn
    private Button BingoBtn;
    private Button HintBtn;
    private Button DontKnowBtn;
    private Button OpenDairyBtn;
    private Button closeDairyBtn;
    private Text MsgTxt;
    private Image MsgImg;

    //judgefinger
    private Vector3 startFingerPos;
    private Vector3 nowFingerPos;
    private float xMoveDistance;
    private AutoFlip mDiaryflip;

    public ParticleSystem Sign;
    public ParticleSystem Sign2;
    private bool isGameFinish = false;
    private bool isDairyOpen = false;

    public AudioSource closebookAudio;
    public AudioSource tearAudio;
    public AudioSource particalAudio;
    public AudioSource btnclickAudio;
    public AudioSource wordinAudio;

    private static WordGameManager instance;
    public static WordGameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        BingoBtn = GameObject.Find("BingoButton").GetComponent<Button>();
        BingoBtn.onClick.AddListener(Bingo);
        HintBtn = GameObject.Find("HintButton").GetComponent<Button>();
        HintBtn.onClick.AddListener(Hint);
        DontKnowBtn = GameObject.Find("DontKnowButton").GetComponent<Button>();
        DontKnowBtn.onClick.AddListener(DontKnow);
        OpenDairyBtn = GameObject.Find("OpenDairy").GetComponent<Button>();
        OpenDairyBtn.onClick.AddListener(showdairy);
        closeDairyBtn = GameObject.Find("closeDairyButton").GetComponent<Button>();
        closeDairyBtn.onClick.AddListener(closedairy);
        MsgTxt = GameObject.Find("MsgText").GetComponent<Text>();
        MsgImg = GameObject.Find("tips").GetComponent<Image>();
        Sign.Stop();
        OpenDairyBtn.interactable = false;

        // If we use dictionary, explode it into array...
        if (useDictionary)
        {
            words = dictionary.text.Split(';');
        }  // ...else stop if there are no user defined words.
        else if (words.Length == 0)
        {
            Debug.LogError("You don't have any words!");
            return;
        }

        wordcount = 0;
        btnhide();

    }

    public void LoadGame()
    {
        isgameready = true;
        btnshow();
        GameObject tempObj;
        Vector2 pos;

        // define center position
        center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane));
        showHint = false;
        // Instantiate parent object for other game objects
        parentObj = Instantiate(parentObject, transform.position, Quaternion.identity) as GameObject;
        // randomize the words in our list...
        //Shuffle(words);
        // ...pick one word...

        mDiaryflip = Dairy_book[wordcount].GetComponent<AutoFlip>();
        word = words[wordcount++];//words[UnityEngine.Random.Range(0, words.Length)];
        // ...and split to the array
        _word = word.ToCharArray();
        Shuffle(_word);
        // spacing between tiles can never be below 1
        spacing = Mathf.Clamp(spacing, 1, float.MaxValue);

        // center field elements as much as possible
        bool left = false;
        int leftv = 0, rightv = 1;
        Dictionary<GameObject, float> fields = new Dictionary<GameObject, float>();
        for (int i = 0; i < _word.Length; i++)
        {
            left = !left;
            if (left)
            {
                tempObj = Instantiate(letterHolder, Camera.main.ScreenToWorldPoint(new Vector3(((Screen.width / 2)) - (leftv * Screen.width/24 * letterHolder.transform.localScale.x * spacing), center.y + Screen.height / 8, 200)), Quaternion.identity) as GameObject;
                leftv++;
            }
            else
            {
                tempObj = Instantiate(letterHolder, Camera.main.ScreenToWorldPoint(new Vector3(((Screen.width / 2)) + (rightv * Screen.width / 24 * letterHolder.transform.localScale.x * spacing), center.y + Screen.height / 8, 200)), Quaternion.identity) as GameObject;
                rightv++;
            }
            fields.Add(tempObj, tempObj.transform.position.x);
        }

        // sort field elements
        fields = fields.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        leftv = 0;
        foreach (KeyValuePair<GameObject, float> targ in fields)
        {
            targ.Key.GetComponent<HolderValue>().SetOrder(leftv);
            targ.Key.name = holderName + leftv.ToString();
            targ.Key.transform.parent = parentObj.transform;
            Vector3 p = Camera.main.WorldToScreenPoint(targ.Key.transform.position);
            leftv++;
        }

        tempValues = new string[_word.Length];

        // finnaly, instantiate letters
        for (int i = 0; i < _word.Length; i++)
        {
            pos = new Vector2(UnityEngine.Random.Range(Screen.width/6 + i*(Screen.width*2/3/_word.Length) + Screen.width/20, Screen.width*5/6 - (_word.Length-i-1) * (Screen.width * 2 / 3 / _word.Length)- Screen.width / 20), UnityEngine.Random.Range(Screen.height *5/6, Screen.height /2));
            tempObj = Instantiate(letterTile, Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10)), Quaternion.identity) as GameObject;
            tempObj.GetComponent<LetterValue>().SetValue(_word[i].ToString());
            tempObj.name = tileName + i.ToString();
            tempObj.transform.parent = parentObj.transform;
            if (rotateTiles)
            {
                tempObj.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
            }
        }
    }

    void Update()
    {
#if UNITY_EDITOR || (!UNITY_EDITOR && !(UNITY_IPHONE || UNITY_ANDROID))
        if (Input.GetKeyDown(KeyCode.T))
        {
            isGameFinish = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isgameready && !isDairyOpen)
            {
                wordcorrect();
            }
        }
#endif

        // if left button is clicked and there's no selected letter
        if (Input.GetMouseButtonDown(0) && !selected)
        {
            container = false;
            letterOrder = -1;
            letterHolderValue = "";
            // check if we have clicked the letter...
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // ...and select it
                if (hit.transform.gameObject.name.Contains(tileName))
                {
                    current = hit.transform.gameObject;
                    current.layer = 2;
                    selected = true;
                }
            }
        }

        // selected letter draging
        if (Input.GetMouseButton(0) && selected)
        {
            current.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, current.transform.position.z));
        }

        // on left mouse release
        if (Input.GetMouseButtonUp(0) && selected)
        {
            current.layer = 0;
            // if we are not over field, drop letter
            if (letterOrder == -1)
            {
                selected = false;
                return;
            } // else place letter in right fieled
            else
            {
                current.transform.eulerAngles = new Vector3(0, 0, 0);
                tempValues[letterOrder] = current.GetComponent<LetterValue>().GetValue();
                current.transform.position = new Vector3(GameObject.Find(holderName + letterOrder.ToString()).transform.position.x, current.transform.position.y, GameObject.Find("holderName" + letterOrder.ToString()).transform.position.z);
                selected = false;
                wordinAudio.Play();
            }
        }
        MsgShow();
        if (isDairyOpen)
        {
            judgeFinger();
        }
    }

    // randomaize elements in array
    private void Shuffle(char[] letters)
    {
        for (int t = 0; t < letters.Length; t++)
        {
            char tmp = letters[t];
            int r = UnityEngine.Random.Range(t, letters.Length);
            letters[t] = letters[r];
            letters[r] = tmp;
        }
    }

    // randomaize elements in array
    private void Shuffle(string[] letters)
    {
        for (int t = 0; t < letters.Length; t++)
        {
            string tmp = letters[t];
            int r = UnityEngine.Random.Range(t, letters.Length);
            letters[t] = letters[r];
            letters[r] = tmp;
        }
    }

    // check if our word is correct
    private bool Check()
    {
        bool result = true;
        string answer = "";
        for (int j = 0; j < tempValues.Length; j++)
        {
            answer += tempValues[j];
        }

        if (answer.ToLower() != word.ToLower())
        {
            result = false;
        }

        return result;
    }

    // show short message
    private IEnumerator ShowMsg(string msg)
    {
        message = msg;
        showMsg = true;
        MsgImg.enabled = true;
        yield return new WaitForSeconds(3);
        message = "";
        showMsg = false;
        MsgImg.enabled = false;
    }

    public void showdairy()
    {
        Sign.Stop();
        Dairy_book[wordcount-1].transform.parent.GetComponent<Animator>().Play("opendiary");
        Dairy_book[wordcount - 1].transform.parent.localPosition= new Vector3(0,-400,0);
        isDairyOpen = true;
        OpenDairyBtn.interactable = false;
        closeDairyBtn.interactable = true;
        btnhide();
        closebookAudio.Play();
}
    public void closedairy()
    {
        if (isDairyOpen)
        {
            if (Dairy_book[wordcount - 1].GetComponent<Book>().currentPage == Dairy_book[wordcount - 1].GetComponent<Book>().bookPages.Length)
            {
                Dairy_book[wordcount - 1].transform.parent.GetComponent<Animator>().Play("closediary");
                closeDairyBtn.interactable = false;
                isDairyOpen = false;
                isgameready = false;
                closebookAudio.Play();
                if (wordcount == 4)
                {
                    //游戏结束！！！！！！！！！！！！！
                    fl.SendFungusMessage("start");
                    isGameFinish = true;
                }
                else
                {
                    Sign2.Play();
                }
            }
        }

    }

    void btnshow()
    {
        MsgTxt.text = "";
        MsgTxt.transform.localScale = Vector3.one;
        BingoBtn.gameObject.SetActive(true);
        HintBtn.gameObject.SetActive(true);
        DontKnowBtn.gameObject.SetActive(true);
    }

    void btnhide()
    {
        MsgTxt.text = "";
        MsgImg.enabled = false;
        MsgTxt.transform.localScale = Vector3.zero;
        BingoBtn.gameObject.SetActive(false);
        HintBtn.gameObject.SetActive(false);
        DontKnowBtn.gameObject.SetActive(false);
    }

    void MsgShow()
    {
        if (showMsg)
        {
            MsgTxt.text = message;
            MsgImg.enabled = true;
        }
        else
        {
            if (showHint)
            {
                MsgTxt.text = hintString;
                MsgImg.enabled = true;
            }
            else
            {
                MsgTxt.text = "";
                MsgImg.enabled = false;
            }
        }

    }

    void Bingo()
    {
        if (Check())
        {
            wordcorrect();
        }
        else
        {
            StartCoroutine(ShowMsg("X!"));
        }
        btnclickAudio.Play();
    }

    void wordcorrect()
    {
        showMsg = false;
        showHint = false;
        Destroy(parentObj);
        btnhide();
        Sign.Play();
        particalAudio.Play();
        OpenDairyBtn.interactable = true;
    }

    void Hint()
    {
        if (showHint) return;
        hintString = "";
        for (int i = 0; i < word.Length; i++)
        {
            hintString += "*";
        }
        hintPos = UnityEngine.Random.Range(0, word.Length);
        hintString = hintString.Remove(hintPos, 1);
        hintString = hintString.Insert(hintPos, word.Substring(hintPos, 1));
        showHint = true;
        showMsg = false;
        btnclickAudio.Play();
    }

    void DontKnow()
    {
        showMsg = false;
        StartCoroutine(ShowMsg(word));
        btnclickAudio.Play();
    }


    void judgeFinger()
    {
#if UNITY_EDITOR || (!UNITY_EDITOR && !(UNITY_IPHONE || UNITY_ANDROID))
        if (Input.GetKeyDown(KeyCode.Space))
        {
            closedairy();
            mDiaryflip.FlipRightPage();  //沿着X轴正方向移动
        }
#endif

        //没有触摸  
        if (Input.touchCount <= 0)
        {
            return;
        }

        if (Input.touchCount == 1)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)//(Input.GetKey("mouse 0"))
            {
                startFingerPos = Input.GetTouch(0).position;
                //startFingerPos = Input.mousePosition;
            }
            nowFingerPos = Input.GetTouch(0).position;
            //nowFingerPos = Input.mousePosition;

            if ((Input.GetTouch(0).phase == TouchPhase.Stationary) || (Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                startFingerPos = nowFingerPos;
                //Debug.Log("======释放触摸=====");  
                return;
            }
            if (startFingerPos == nowFingerPos)
            {
                return;
            }
            xMoveDistance = Mathf.Abs(nowFingerPos.x - startFingerPos.x);
            if(xMoveDistance>100)
            {
                if (nowFingerPos.x - startFingerPos.x > 0)
                {
                    mDiaryflip.FlipLeftPage(); //沿着X轴负方向移动  
                }
                else
                {
                    closedairy();
                    mDiaryflip.FlipRightPage();  //沿着X轴正方向移动                
                }
            }         
        }
    }
}
