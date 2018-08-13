using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using Managers;
using UI.CardManagement;
using Characters.Cards;

namespace Characters.Players
{
    [RequireComponent(typeof(AIBrain), typeof(Population))]
    public class Opponent : Playable
    {
        [SerializeField]
        HandManagement _handManagement;

        AIBrain _aIBrain;
        GameManager _gm;

        bool _isDoing;

        public bool isDoing
        {
            get
            {
                return _isDoing;
            }
            set
            {
                _isDoing = value;
            }
        }

        private void Awake()
        {
            _aIBrain = GetComponent<AIBrain>();
            _gm = GameManager.Instance;

            population = GetComponent<Population>();
        }

        public void PlayOpponentTurn(Action callback)
        {
            _isDoing = true;
            population.CurrentPopulation = population.MaximumPopulation;
            UseCard();
            StartCoroutine(WaitForOpponentBehavior(callback));
        }

        IEnumerator WaitForOpponentBehavior(Action callback)
        {
            while (_isDoing)
            {
                yield return null;
            };

            callback.Invoke();
        }

        public void UseCard()
        {
            Debug.Log("새로운 카드 찾기");
            Card card = _aIBrain.GetCard(); // parameter로 count 값을 넣어주기
            
            if (card != null)
            {
                var effect = card.GetComponent<CardEffect>();
                Debug.Log("사용될 카드의 이펙트 : " + effect);
                if (effect.GetComponent<BuildCardEffect>() != null || effect.GetComponent<WeaponCardEffect>() != null)
                    effect.SetTarget(_aIBrain.GetTarget());

                _handManagement.ReAssignHands();
                effect.Activate();
            }
            else
            {
                Debug.Log("카드 사용 종료");
                _isDoing = false;
            }
        }

        private void OnMouseEnter()
        {
            _gm.OnMouseHoverFromPlanet(true);
        }

        private void OnMouseExit()
        {
            _gm.OnMouseHoverFromPlanet(false);
        }
    }

}
