using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class DominoManger : MonoBehaviour {

    public const float Dominooffset = 2.0f;
    //以后要是有地形的话
    public const float _verticalOffset = 1.0f;
    public const int MissionNumTotal = 5;
    public Flowchart fl;
    private GameObject TempDomino;
    private int mDonimoTotalNum;

    [Header("Prefabs")]
    public GameObject DominoPrefab;
    public GameObject Level_heart;
    public GameObject level_heart_help;


    private UIcontroller mUIcontroller;
    private DominoAudioManger mAudioM;

    private GameObject CurrentLvl;
    private GameObject AllDominos;

    [HideInInspector]
    public int MissionCompleteNum;
    [HideInInspector]
    public bool IsStartPush;
    [HideInInspector]
    public GameObject CurrentDomino;
    [HideInInspector]
    public Startdomino mStarter;

    private List<DominosController> ListDominos;

    //judgefinger
    private Vector3 startFingerPos;
    private Vector3 nowFingerPos;
    private float xMoveDistance;
    private float yMoveDistance;
    private int backValue = 0;

    private static DominoManger instance;
    public static DominoManger Instance
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
        mUIcontroller = GetComponent<UIcontroller>();
        mAudioM = DominoAudioManger.Instance;
        AllDominos = new GameObject();
        AllDominos.name = "AllDominos";
        ListDominos = new List<DominosController>();
        CurrentLvl = Instantiate(Level_heart);
        if(CurrentLvl != null)
        {
            mStarter = CurrentLvl.GetComponentInChildren<Startdomino>();
        }
        MissionCompleteNum = 0;
        mDonimoTotalNum = 0;
        IsStartPush = false;

    }

    void Update()
    {
        MousePick();
        //DoNavigation();
        PositioningDomino();
        CreatingDomino();
        if (TempDomino == null && CurrentDomino != null)
        {
            judgeFinger();
        }
        Finish();


    }

    void Finish()
    {
        if (MissionCompleteNum >= MissionNumTotal && IsStartPush)
        {
            Invoke("WintheGame",1.0f);
        }
    }

    void WintheGame()
    {
        fl.SendFungusMessage("over");

    }

    public void LosetheGame()
    {
        mUIcontroller.ShowFailedImage();
        IsStartPush = true;
    }

    void MousePick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Domino")
                {
                    CurrentDomino = hit.transform.gameObject;
                    //Color tempc = CurrentDomino.GetComponent<MeshRenderer>().material.color;
                    //CurrentDomino.GetComponent<MeshRenderer>().material.color = new Color(tempc.r, tempc.g, tempc.b, 0.55f);

                }
            }
        }
    }

    public void BuildingDonimo(string type)
    {
        CurrentDomino = null;
        if(IsStartPush)
        {
            return;
        }
        if (type == "Domino")
        {
            TempDomino = Instantiate(DominoPrefab);
            mAudioM.d_up.Play();
        }
    }

    public void DelcurrentDomino()
    {
        if (!IsStartPush)
        {
            if (CurrentDomino != null)
            {             
                if(ListDominos.Count != 0)
                {
                    ListDominos.Remove(CurrentDomino.GetComponent<DominosController>());
                }
                mDonimoTotalNum = ListDominos.Count;
                Destroy(CurrentDomino);
                mAudioM.d_delete.Play();
            }
        }
    }

    void PositioningDomino()
    {
        if (TempDomino == null)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            TempDomino.transform.position = new Vector3(hit.point.x - 1.5f*Dominooffset, _verticalOffset, hit.point.z + Dominooffset);
        }
    }

    

    void CreatingDomino()
    {
        if (TempDomino == null)
            return;

        if (Input.GetKeyUp("mouse 0"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //出界
            if (TempDomino.transform.position.x < -11 || TempDomino.transform.position.x > 11.5)
            {
                Destroy(TempDomino);
                //ErrorSound.Play();
                return;
            }
            
            if (Physics.Raycast(ray, out hit))
            {
                dominodone(TempDomino,new Vector3(hit.point.x - 1.5f * Dominooffset, _verticalOffset, hit.point.z + Dominooffset),Vector3.zero);
                CurrentDomino = TempDomino;
                ListDominos.Add(CurrentDomino.GetComponent<DominosController>());
                CurrentDomino.GetComponent<DominosController>().recondPosandEuler();
                mDonimoTotalNum = ListDominos.Count;
                TempDomino = null;
                mAudioM.d_down.Play();
                return;
            }
            Destroy(TempDomino);
        }
    }

    void judgeFinger()
    {
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
                return;
            }
            if (startFingerPos == nowFingerPos)
            {
                return;
            }
            xMoveDistance = Mathf.Abs(nowFingerPos.x - startFingerPos.x);

            yMoveDistance = Mathf.Abs(nowFingerPos.y - startFingerPos.y);


                if (nowFingerPos.x - startFingerPos.x > 0)
                {

                    backValue = -1; //沿着X轴负方向移动  

                }
                else
                {

                    backValue = 1; //沿着X轴正方向移动  

                }

            if (backValue == 1)
            {
                CurrentDomino.transform.Rotate(Vector3.up * Time.deltaTime * 120);
            }
            else if (backValue == -1)
            {
                CurrentDomino.transform.Rotate(Vector3.up * -1 * Time.deltaTime * 120);
            }
            CurrentDomino.GetComponent<DominosController>().recondPosandEuler();
        }
    }

    public void pushDonimo()
    {
        if (!IsStartPush)
        {
            if (CurrentDomino != null)
            {
                CurrentDomino = null;
            }
            IsStartPush = true;
            MissionCompleteNum = 0;
            mStarter.pushDonimo();
            mUIcontroller.turnButton();
        }
        mAudioM.d_startpush.Play();
    }

    public void backDonimo()
    {
        if (IsStartPush)
        {
            Destroy(CurrentLvl);
            CurrentLvl = Instantiate(Level_heart);
            mStarter = CurrentLvl.GetComponentInChildren<Startdomino>();
            MissionCompleteNum = 0;
            for (int i=0;i < mDonimoTotalNum; i++)
            {
                ListDominos[i].getupagain();
            }
            IsStartPush = false;
            mUIcontroller.turnButton();
            mUIcontroller.HideFailedImage();
            mAudioM.d_restart.Play();
        }
    }

    public void RestartDomino()
    {
        Destroy(CurrentLvl);
        Destroy(AllDominos);
        AllDominos = new GameObject();
        AllDominos.name = "AllDominos";
        CurrentLvl = Instantiate(Level_heart);
        mStarter = CurrentLvl.GetComponentInChildren<Startdomino>();
        MissionCompleteNum = 0;
        mDonimoTotalNum = 0;
        ListDominos.Clear();
        IsStartPush = false;
        mUIcontroller.ResetButton();
        mUIcontroller.HideFailedImage();
        mAudioM.d_restart.Play();
    }

    void helpme()
    {
        Destroy(CurrentLvl);
        Destroy(AllDominos);
        AllDominos = new GameObject();
        AllDominos.name = "AllDominos";
        CurrentLvl = Instantiate(level_heart_help);
        mStarter = CurrentLvl.GetComponentInChildren<Startdomino>();
        MissionCompleteNum = 0;
        mDonimoTotalNum = 0;
        ListDominos.Clear();
        IsStartPush = false;
        mUIcontroller.ResetButton();
        mUIcontroller.HideFailedImage();
        mAudioM.d_restart.Play();
    }

    public void SkipLevel()
    {
        mUIcontroller.HideFailedImage();
        helpme();
    }

    private void dominodone(GameObject TempDomino, Vector3 pos,Vector3 Euler)
    {
        TempDomino.GetComponent<BoxCollider>().enabled = true;
        TempDomino.transform.position = pos;
        TempDomino.transform.eulerAngles = Euler;
        TempDomino.GetComponent<Rigidbody>().useGravity = true;
        //原色
        Color tempc = TempDomino.GetComponent<MeshRenderer>().material.color;
        TempDomino.GetComponent<MeshRenderer>().material.color = new Color(tempc.r, tempc.g, tempc.b, 1);
        //彩虹
        //TempDomino.GetComponent<MeshRenderer>().material.color = new Color((255 - 30 * ((mDonimoTotalNum+1)%9))/255.0f, (((mDonimoTotalNum + 1)%9) * 15)/255.0f, (30 * ((mDonimoTotalNum + 1)%9))/255.0f, 0.9f);

        TempDomino.transform.parent = AllDominos.transform;
        TempDomino.name = mDonimoTotalNum.ToString();
    }
}
