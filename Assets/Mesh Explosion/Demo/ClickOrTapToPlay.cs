using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClickOrTapToPlay : MonoBehaviour
{
    private WordGameManager gameManger;
    private Animator manimator;

    void Start()
    {
        gameManger = WordGameManager.Instance;
        manimator = transform.GetComponent<Animator>();
    }


#if UNITY_EDITOR || (!UNITY_EDITOR && !(UNITY_IPHONE || UNITY_ANDROID))
    void OnMouseDown()
    {
        StartPlay();
    }
#endif

    void Update()
    {
        foreach (var i in Input.touches)
        {
            if (i.phase != TouchPhase.Began)
            {
                continue;
            }

            // It's kinda wasteful to do this raycast repeatedly for every ClickToExplode in the
            // scene, but since this component is just for testing I don't think it's worth the
            // bother to figure out some shared static solution.
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(i.position), out hit))
            {
                continue;
            }
            if (hit.collider != GetComponent<Collider>())
            {
                continue;
            }

            StartPlay();
            return;
        }
    }

    void StartPlay()
    {
        if (!gameManger.isgameready)
        {
            //if (manimator.GetCurrentAnimatorStateInfo(0).IsName("voidstate"))
            //{
            //    manimator.Play("flytocenter");
            //}
            if (manimator.GetCurrentAnimatorStateInfo(0).IsName("flytocenter") && manimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                BroadcastMessage("Explode");
                gameManger.tearAudio.Play();
                transform.GetComponent<MeshRenderer>().enabled = false;
                transform.GetComponent<BoxCollider>().enabled = false;
                Invoke("startwordgame", 2.0f);
            }
        }
    }

    void startwordgame()
    {
        gameManger.LoadGame();
        Destroy(gameObject);
    }

}
