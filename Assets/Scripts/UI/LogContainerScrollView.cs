using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LogContainerScrollView : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        //GetComponent<ScrollRect>().verticalScrollbar.
    }
    public void OnChangeValue()
    {
        //GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        //scrollRect.verticalNormalizedPosition = 0.5f;
        //scrollRect.
    }
}