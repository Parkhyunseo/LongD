using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UI.CardManagement;
using Managers;

namespace UI
{
    using UnityEngine.UI;

    public class Log : MonoBehaviour
    {
        [SerializeField]
        Sprite _sprite;
        Image _detailImage;

        private void Start()
        {
            _detailImage = FindManager.PlayerCardViewPosition.GetComponent<Image>();
        }
        
        public void OnMouseHoverEnter()
        {
            //_detailImage.enabled = true;
            Debug.Log("MouseEnter");
            _detailImage.color = Color.white;
            _detailImage.sprite = _sprite;
            // TODO :: Show Detail
        }

        public void OnMouseHoverExit()
        {
            //_detailImage.enabled = false;
            _detailImage.color = new Color(1, 1, 1, 0);
            _detailImage.sprite = null;
            // TODO :: HideDetail
        }

    }

}
