using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Characters.Cards;
using Managers;

namespace UI.CardManagement
{
    public class CardUI : Subcomponent<Card>
    {
        [SerializeField]
        HandManagement _handManagement;

        [SerializeField]
        Sprite _pointer;

        [SerializeField]
        Texture2D _mousePointer;
        Vector2 _hotspot;

        Vector3 baseScale;
        Vector3 _baseLocalPosition;
        Vector3 afterHoverPosition;
        Vector3 afterHoverScale;

        int _siblingIndex;
        bool _isAvailable;

        [SerializeField]
        GameObject _image;
        [SerializeField]
        GameObject _name;
        [SerializeField]
        GameObject _description;
        [SerializeField]
        GameObject _neededPeople;
        [SerializeField]
        GameObject _stk;
        [SerializeField]
        GameObject _hp;

        public int siblingIndex
        {
            get
            {
                return _siblingIndex;
            }
            set
            {
                _siblingIndex = value;
            }
        }

        public bool isAvailable
        {
            get
            {
                return _isAvailable;
            }
            set
            {
                _isAvailable = value;
            }
        }
        
        public Vector3 baseLocalPosition
        {
            get
            {
                return _baseLocalPosition;
            }
            set
            {
                _baseLocalPosition = value;
            }
        }

        public  GameObject _back;

        protected override void Awake()
        {
            base.Awake();
            baseScale = transform.localScale;
            afterHoverPosition = new Vector2(0, 100);
            afterHoverScale = Vector2.one * 2;
            _hotspot = new Vector2(_mousePointer.width / 2, _mousePointer.height / 2);

            var cardInformation = GetComponent<Card>();
        }

        protected void Start()
        {
            _handManagement = FindManager.PlayerHands.GetComponent<HandManagement>();
        }

        public void OnCardHoverEnter()
        {
            GetComponent<RectTransform>().SetAsLastSibling();
            AnimationManager.Instance.PlayCardHoverAnimation(_image.GetComponent<RectTransform>(), 100);
        }

        public void OnCardHoverExit()
        {
            AnimationManager.Instance.PlayCardHoverAnimation(_image.GetComponent<RectTransform>(), 0, false);
        }

        public void ChangeToFront()
        {
            transform.GetChild(0).GetComponent<Image>().sprite = _super.cardSprite;
        }

        public void OnCardClick()
        {
            UIManager.Instance.selectedCard = GetComponent<CardEffect>();
            AnimationManager.Instance.PlayCardActiveAnimation(transform, FindManager.PlayerCardViewPosition.transform.position);
            AnimationManager.Instance.PlayHandAnimation(_handManagement.cardContainer.transform, _handManagement.baseY, true, false);

            _handManagement.UpDownHand();
            if(GetComponent<RotationPolicyCard>() == null)
                Cursor.SetCursor(_mousePointer, _hotspot, CursorMode.Auto);
            else
            {
                var behaviorCardEffect = GetComponent<RotationPolicyCard>();
                Debug.Log("card" + behaviorCardEffect);
                if (behaviorCardEffect != null)
                {
                    _handManagement.ReAssignHands();
                    behaviorCardEffect.Activate();
                    ResetCardClick(true);
                }

            }
        }

        public void ResetCardClick(bool useCard=false)
        {
            UIManager.Instance.selectedCard = null;

            if (!useCard)
                AnimationManager.Instance.PlayCardDeActiveAnimation(transform, baseLocalPosition);
            else
                transform.parent = _handManagement.transform.parent;

            AnimationManager.Instance.PlayHandAnimation(_handManagement.cardContainer.transform, _handManagement.baseY, true, true);

            _handManagement.UpDownHand(true);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

}
