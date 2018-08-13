using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Characters.Cards;
using Managers;

namespace UI.CardManagement
{
    public class HandManagement : MonoBehaviour
    {
        [SerializeField]
        GameObject _cardContainer;

        GameObject[] cards;

        CardUI _selectedUI;
        const int _maxHandSize = 5;
        int _curHandSize;

        // For Hand Animation
        private float _baseY;

        public float baseY
        {
            get
            {
                return _baseY;
            }
        }

        public GameObject cardContainer
        {
            get
            {
                return _cardContainer;
            }
        }

        private void Awake()
        {
            _baseY = transform.GetComponent<RectTransform>().anchoredPosition.y;
            cards = Resources.LoadAll<GameObject>(Resource.cards);
        }

        public void UpDownHand(bool up=false)
        {
            for (int i = 0; i < _cardContainer.transform.childCount; i++)
                _cardContainer.transform.GetChild(i).GetComponent<Image>().raycastTarget = up;
        }

        public void DownDeckExceptedOnlyOneCard(GameObject selectedCard)
        {
            for(int i =0; i < _cardContainer.transform.childCount; i++)
            {
                var card = _cardContainer.transform.GetChild(i).gameObject;
                if (!card.Equals(selectedCard))
                    card.GetComponent<Image>().raycastTarget = false;
            }
        }

        public void ReWindCardDeck()
        {
            for (int i = 0; i < _cardContainer.transform.childCount; i++)
                _cardContainer.transform.GetChild(i).GetComponent<Image>().raycastTarget = true;

        }

        /// <summary>
        /// TODO :: 덱에서 불러오는 것으로 수정할 것
        /// </summary>
        /// <param name="isBack"></param>
        public void GetHands(bool isBack=false, Action callback = null)
        {   
            var width = _cardContainer.GetComponent<RectTransform>().rect.width;
            var height = _cardContainer.GetComponent<RectTransform>().rect.height;

            var padding = 10;

            var margin = width / 4;
            
            //var minCardWidth = 70;
            //var minCardHeight = 90;

            var cardWidth = 100;//Mathf.Max((width-4*padding-2*margin)/5, minCardWidth);
            var cardHeight = 130;//Mathf.Max(height + 20, minCardHeight);
            
            var startPosition = GameManager.Instance.IsPlayerTurn ? FindManager.PlayerDeck.transform.position : FindManager.OpponentDeck.transform.position;

            for (int i = 0; i < 5; i++)
            {
                var obj = Instantiate(cards[i % 5], _cardContainer.transform);
                var basePosition = new Vector2(width/2 - cardWidth*2 - 2*padding + (cardWidth + padding) * i, -70);
                obj.GetComponent<RectTransform>().position = startPosition;
                obj.GetComponent<CardUI>().baseLocalPosition = basePosition;

                obj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
                if (isBack)
                {
                    obj.GetComponent<Image>().raycastTarget = false;
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = obj.GetComponent<Card>().cardBackSprite;
                }

                if (i == 4)
                    AnimationManager.Instance.PlayGetHandAnimation(obj.transform, basePosition, () => { callback.Invoke(); });
                else
                    AnimationManager.Instance.PlayGetHandAnimation(obj.transform, basePosition);
            }
        }

        public void ReAssignHands()
        {
            var width = _cardContainer.GetComponent<RectTransform>().rect.width;
            var height = _cardContainer.GetComponent<RectTransform>().rect.height;
            var padding = 10;

            var margin = width/4;

            //var minCardWidth = 70;
            //var minCardHeight = 90;

            var cardWidth = 100;//Mathf.Max((width - 4*padding - 2 * margin) / _cardContainer.transform.childCount, minCardWidth);
            var cardHeight = 130; //Mathf.Max(height + 20, minCardHeight);

            Debug.Log("내 손에 있는 카드 수 : " + _cardContainer.transform.childCount);

            if (_cardContainer.transform.childCount >= 1)
            {
                for (var i = 0; i < _cardContainer.transform.childCount; i++)
                {
                    Vector2 basePosition;
                    if (_cardContainer.transform.childCount % 2 == 0)
                        basePosition = new Vector2(width/2 - ((_cardContainer.transform.childCount-1)/2)*(cardWidth + padding) +(cardWidth + padding) * i, -70);
                    else
                        basePosition = new Vector2(width/2 - (_cardContainer.transform.childCount/2)*(cardWidth + padding) + (cardWidth/2) + (cardWidth + padding) * i, -70);
                    var obj = _cardContainer.transform.GetChild(i);
                    obj.GetComponent<RectTransform>().anchoredPosition = basePosition;
                    obj.GetComponent<CardUI>().baseLocalPosition = obj.GetComponent<RectTransform>().localPosition;
                    obj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
                }
            }
        }

        public void ThrowAwayHand()
        {
            Debug.Log("ThrowAwayHand Before: " + _cardContainer.transform.childCount);
            int i = 0;
            GameObject[] allChildren = new GameObject[_cardContainer.transform.childCount];

            foreach (Transform child in _cardContainer.transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                DestroyImmediate(child);
            }
            //_cardContainer.transform.DetachChildren();
            Debug.Log("ThrowAwayHand After: " + _cardContainer.transform.childCount);
        }
    }
}
