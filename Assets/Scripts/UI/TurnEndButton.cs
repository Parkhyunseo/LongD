using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Managers;
using UnityEngine.UI;

public class TurnEndButton : MonoBehaviour {
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GetComponent<Button>().interactable = false;
            GameManager.Instance.EndTurn();
            AnimationManager.Instance.PlayTurnEndButtonToggleAnimation(transform.GetComponent<RectTransform>(), true);
        });
    }
}
