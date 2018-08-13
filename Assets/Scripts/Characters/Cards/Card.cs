using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Managers;

namespace Characters.Cards
{
    /// <summary>
    /// 카드에 대한 정보를 가지고 있다.
    /// </summary>
    public class Card : MonoBehaviour
    {
        [SerializeField]
        Sprite _cardSprite;

        [SerializeField]
        Sprite _cardBackSprite;

        [SerializeField]
        string _cardName;

        [SerializeField]
        string _cardDescription;

        [SerializeField]
        int _cardNeededPeople;

        [SerializeField]
        int _cardSTK;

        [SerializeField]
        int _cardHP;

        [SerializeField]
        GameObject _logPrefab;

        public AnimationManager _am;

        public Sprite cardSprite
        {
            get
            {
                return _cardSprite;
            }
            set
            {
                _cardSprite = value;
            }
        }

        public string cardName
        {
            get
            {
                return _cardName;
            }
            set
            {
                _cardName = value;
            }
        }

        public string cardDescription
        {
            get
            {
                return _cardDescription;
            }
            set
            {
                _cardDescription = value;
            }
        }
        public int cardNeededPeople
        {
            get
            {
                return _cardNeededPeople;
            }
            set
            {
                _cardNeededPeople = value;
            }
        }
        public int cardSTK
        {
            get
            {
                return _cardSTK;
            }
            set
            {
                _cardSTK = value;
            }
        }

        public int cardHP
        {
            get
            {
                return _cardHP;
            }
            set
            {
                _cardHP = value;
            }
        }

        public GameObject logPrefab
        {
            get
            {
                return _logPrefab;
            }
        }

        public Sprite cardBackSprite
        {
            get
            {
                return _cardBackSprite;
            }
        }

        private void Awake()
        {
            _am = AnimationManager.Instance;  
        }
    }

}
