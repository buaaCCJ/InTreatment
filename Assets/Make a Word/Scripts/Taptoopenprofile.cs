using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Taptoopenprofile : MonoBehaviour
{
    private WordGameManager gameManger;

    void Start()
    {
        gameManger = WordGameManager.Instance;
    }

#if UNITY_EDITOR || (!UNITY_EDITOR && !(UNITY_IPHONE || UNITY_ANDROID))
    void OnMouseDown()
    {
        OpenProfile();
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

            OpenProfile();
            return;
        }
    }

    void OpenProfile()
    {
        if (transform.childCount != 0 && !gameManger.isgameready)
        {
            gameManger.Sign2.Stop();
            transform.GetChild(0).GetComponent<Animator>().Play("flytocenter");
            gameManger.closebookAudio.Play();
        }
    }

}


