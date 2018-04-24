using UnityEngine;
using System.Collections;

public class HolderValue : MonoBehaviour {
    public int order;

    public void SetOrder(int value)
    {
        order = value;
    }

    public int GetOrder()
    {
        return order;
    }

    void OnMouseExit()
    {
        WordGameManager.Instance.letterOrder = -1;
    }

    void OnMouseOver()
    {
        WordGameManager.Instance.letterOrder = order;
    }
}
