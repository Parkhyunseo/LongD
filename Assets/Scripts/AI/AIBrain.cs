using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Managers;
using Characters.Cards;
using Characters.Players;
using Characters.Planets;
using System.Threading;

using UI.CardManagement; // 수정하자
using Characters.Buildings;

namespace AI
{
    // TODO :: 비동기로 하고 Task에 넣어놔서 시간
    /// <summary>
    /// 
    /// </summary>
    public class AIBrain : MonoBehaviour
    {
        class PlanetState
        {
            bool _buildableState;
            bool _equipmentUseableState;

            public bool buildableState
            {
                get { return _buildableState; }
                set { _buildableState = value; }
            }

            public bool equipmentUseableState
            {
                get { return _equipmentUseableState; }
                set { _equipmentUseableState = value; }
            }
        }

        [SerializeField]
        AILevel aILevel;

        [SerializeField]
        Transform _cardContainer;

        Opponent _opponent;
        Queue<Queue<Card>> _selectedCard;
        Queue<Card> _currentHands;
        Queue<Targetable> _targets;

        PlanetState _planetState;
        int _turnCount;

        private void Awake()
        {
            _turnCount = -1;
            _opponent = GetComponent<Opponent>();
            _selectedCard = new Queue<Queue<Card>>();
            _targets = new Queue<Targetable>();
            _planetState = new PlanetState();
            Random.InitState(0);
        }

        public Targetable GetTarget()
        {
            if(_targets.Count <= 0)
            {
                Debug.Log("GetTarget Error");
                return null;
            }else
            {
                return _targets.Dequeue();
            }
        }

        /// <summary>
        /// Building을 설치할 장소를 고른다.
        /// 설치할 곳이 없다면 False 반환 아니면 True반환
        /// </summary>
        /// <returns></returns>
        private bool SelectTarget(bool isBuilding)
        {
            var slot = GetComponent<Planet>().Slots;

            for(int i=0; i < GameManager.slotSize; i++)
            {
                if(isBuilding)
                {
                    if (slot[i].isAvailable && !slot[i].AlreadyWasBuilt)
                    {
                        _targets.Enqueue(slot[i]);
                        return true;
                    }
                }else
                {
                    if(slot[i].transform.childCount >= 1 && slot[i].isAvailable)
                    {
                        if (slot[i].transform.GetChild(0).GetComponent<ActiveBuilding>() != null)
                        {
                            _targets.Enqueue(slot[i]);
                            return true;
                        }
                    }
                    
                }
                
            }
            return false;
        }

        /// <summary>
        /// 낼 카드가 없다면 null을 반환
        /// 카드가 필요할 때마다 그냥 찾기
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Card GetCard()
        {
            //Async 
            List<Card> cards = new List<Card>();
            for (var i = 0; i < _cardContainer.childCount; i++)
            {
                cards.Add(_cardContainer.GetChild(i).GetComponent<Card>());
            }
            
            Debug.Log("핸드에 있는 카드 수 :" + cards.Count);
            var selectedCard = SelectRandomCard(cards);

            if (selectedCard == null)
            {
                Debug.Log("카드 선택 종료");
                return null;
            }

            Debug.Log("이번에 사용할 카드 : " + selectedCard.name);
            return selectedCard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cards"></param>
        private Card SelectRandomCard(List<Card> cards)
        {
            var curPop = _opponent.population.CurrentPopulation;

            List<Card> useableCard = FilterUseable(cards, curPop);
            Debug.Log("선택 가능한 경우의 수 : " + useableCard.Count);

            if (useableCard.Count < 1)
                return null;

            float selectedValue = Random.Range(0.1f, useableCard.Count-0.1f);
            int index = (int)selectedValue;

            if (useableCard[index].cardNeededPeople <= curPop)
            {
                curPop -= useableCard[index].cardNeededPeople;
                //;
                if (useableCard[index].GetComponent<BuildCardEffect>() != null)
                    SelectTarget(true);
                else if (useableCard[index].GetComponent<WeaponCardEffect>() != null)
                    SelectTarget(false);
                cards.Remove(useableCard[index]); // 핸드에서 제거한다.
            }

            return useableCard[index];
        }

        private List<Card> FilterUseable(List<Card> hands, int curPop)
        {
            List<Card> cards = new List<Card>();

            UpdatePlanetState();

            for (int i = 0; i < hands.Count; i++)
            {
                if(hands[i].cardNeededPeople <= curPop)
                {
                    if(hands[i].GetComponent<BuildCardEffect>() != null)
                    {
                        if (_planetState.buildableState)
                            cards.Add(hands[i]);
                    }
                    else if(hands[i].GetComponent<WeaponCardEffect>() != null)
                    {
                        if (_planetState.equipmentUseableState)
                            cards.Add(hands[i]);
                    }
                    else
                    {
                        cards.Add(hands[i]);
                    }
                }
            }

            return cards;
        }
        
        private void UpdatePlanetState()
        {
            var slot = GetComponent<Planet>().Slots;

            //Test
            var buildableCount = 0;
            var equpmentUseableCount = 0;

            _planetState.buildableState = false;
            _planetState.equipmentUseableState = false;

            for (int i = 0; i < GameManager.slotSize; i++)
            {
                if (slot[i].isAvailable && !slot[i].AlreadyWasBuilt)
                {
                    buildableCount += 1;
                    _planetState.buildableState = true;
                    Debug.Log("건물의 인덱스 : " + slot[i].index);
                }

                if (slot[i].transform.childCount >= 1)
                {
                    if (slot[i].transform.GetChild(0).GetComponent<ActiveBuilding>() != null)
                    {
                        equpmentUseableCount += 1;
                        _planetState.equipmentUseableState = true;
                    }
                }
            }

            Debug.Log("건물 지을 수 있는 공간 :" + buildableCount);
            Debug.Log("무기 장착 가능한  공간 :" + equpmentUseableCount);
        }

        private void SelectCardGreedily(List<Card> cards)
        {
            /*
             *  1. 코스트가 낮은걸로
             *  2. 건물 -> 무기 -> 행동 처럼 특정 카드우선
             *  3. 적절한 알고리즘
             */
        }

        /// <summary>
        /// Deck을 통해
        /// </summary>
        /// <param name="deck"></param>
        private void SelectCardAsSearchNumberOfCase(List<Card> deck)
        {

        }
    }
}
