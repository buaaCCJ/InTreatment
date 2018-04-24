using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 元素缩放脚本，支持鼠标滚轮和双指触摸
/// </summary>
public class ScaleScript : MonoBehaviour
{
    /// <summary>
    /// 目标对象
    /// </summary>
    public GameObject Target;
    /// <summary>
    /// 最大缩放比例
    /// </summary>
    public float MaxScale = 1.4f;
    /// <summary>
    /// 最小缩放比例
    /// </summary>
    public float MinScale = 0.7f;
    /// <summary>
    /// 鼠标滚轮缩放速度
    /// </summary>
    public float MouseWheelScaleSpeed = 0.05f;

    private float beginDist;
    private float endDist;
    List<Touch> touchList = new List<Touch>();

    private float scrDist;
    private float zoom=1.0f;
    ///DTY
    //private float zoom=1.3f;
    private float constant_zoom = 1.0f;
    ///end
    private void Start()
    {
        scrDist = Vector2.Distance(new Vector2(0, 0), new Vector2(Screen.width, Screen.height));
    }
    void Update()
    {
        int fingerCount = 0;
        touchList.Clear();
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                touchList.Add(touch);
                fingerCount++;
            }

        }
        if (fingerCount == 2)
        {
            if (touchList[0].phase == TouchPhase.Began || touchList[1].phase == TouchPhase.Began)
            {
                beginDist = Vector2.Distance(touchList[0].position, touchList[1].position);
            }
            else
            {
                endDist = Vector2.Distance(touchList[0].position, touchList[1].position);
                zoom =zoom + zoom * (endDist - beginDist) / scrDist;
                beginDist = endDist;
                if (zoom > MaxScale) zoom = MaxScale;
                if (zoom < MinScale) zoom = MinScale;
                ///DTY
                zoom = constant_zoom;
                ///end
                Target.transform.localScale = new Vector3(zoom, zoom, 1f);
            }

        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            zoom += MouseWheelScaleSpeed;
            if (zoom > 1.4f) zoom = 1.4f;
            ///DTY
            zoom = constant_zoom;
            ///end
            Target.transform.localScale = new Vector3(zoom, zoom, 1f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            zoom -= MouseWheelScaleSpeed;
            if (zoom < 0.7f) zoom = 0.7f;
            ///DTY
            zoom = constant_zoom;
            ///end
            Target.transform.localScale = new Vector3(zoom, zoom, 1f);
        }
    }

}
