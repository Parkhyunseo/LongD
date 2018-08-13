using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Characters.Cards;
using Characters.Players;
using Characters.Buildings;

using Managers;
using UI.CardManagement;

namespace UI
{
    public class UIManager : Singletons.Singleton<UIManager>
    {
        GameObject _leftSelect;
        GameObject _rightSelect;
        TurnEndButton _turnEndButton;
        
        CardEffect _selectedCard;
        
        GameManager _gm;

        HandManagement _playerHandManagement;
        HandManagement _opponentHandManagement;
        /*
        [SerializeField]
        Canvas _canvas;
        GraphicRaycaster _graphicRaycaster;
        PointerEventData _pointerEventData;
        */

        public CardEffect selectedCard
        {
            get
            {
                return _selectedCard;
            }
            set
            { 
                _selectedCard = value;
            }
        }

        protected override void Awake()
        {
            _gm = GameManager.Instance;
            //_graphicRaycaster = _canvas.GetComponent<GraphicRaycaster>();
            //_pointerEventData = new PointerEventData(null);
        }

        private void Start()
        {
            _leftSelect = FindManager.PreemptiveAttack;
            _rightSelect = FindManager.NonPreemptiveAttack;
            _turnEndButton = FindManager.TurnEndButton.GetComponent<TurnEndButton>();

            _leftSelect.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("선공");
                _gm.IsPlayerTurn = true;
                AnimationManager.Instance.PlaySlidingWindowAnimation(_leftSelect.transform, true, true, () =>
                {
                    SelectFirstEnd();
                    _gm.SetGame();
                    AnimationManager.Instance.PlaySetUI(FindManager.LogManager.transform.parent.parent, FindManager.TurnEndButton.transform);
                });
                AnimationManager.Instance.PlaySlidingWindowAnimation(_rightSelect.transform, false, false);

            });

            _rightSelect.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("후공");
                _gm.IsPlayerTurn = false;
                AnimationManager.Instance.PlaySlidingWindowAnimation(_leftSelect.transform, false, true);
                AnimationManager.Instance.PlaySlidingWindowAnimation(_rightSelect.transform, true, false, () =>
                {
                    SelectFirstEnd();
                    AnimationManager.Instance.PlaySetUI(FindManager.LogManager.transform.parent.parent, FindManager.TurnEndButton.transform);
                    _gm.SetGame();
                });
            });
            _playerHandManagement = FindManager.PlayerHands.GetComponent<HandManagement>();
            _opponentHandManagement = FindManager.OpponentHands.GetComponent<HandManagement>();
        }

        private void Update()
        {
            if(_selectedCard != null)
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Ray2D ray = new Ray2D(wp, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if(hit.collider != null)
                {
                    var slot = hit.transform.GetComponent<Slot>();
                    var building = hit.transform.GetComponent<Building>();

                    if(!slot.isAvailable)
                    {
                        // TODO :: 설치불가마우스
                    }

                    if(building != null)
                    {
                        // TODO :: Select Card가 BuildCard이면 안됌
                        if(_selectedCard.GetComponent<BuildCardEffect>() == null)
                        {
                            // TODO :: MouseCursor 변경
                        }
                    }
                    else if(slot != null)
                    {
                        // TODO :: Select Card가 BuildCard아니면 안됌
                        if (_selectedCard.GetComponent<BuildCardEffect>() != null)
                        {
                            // TODO :: MouseCursor 변경
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Ray2D ray = new Ray2D(wp, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                
                if (hit.collider != null)
                {
                    if (_selectedCard != null)
                    {
                        var targetable = hit.transform.GetComponent<Targetable>();
                        if (targetable != null)
                        {
                            var buildCardEffect = _selectedCard.GetComponent<BuildCardEffect>();
                            var building = targetable.transform.childCount;
                            var slot = targetable.GetComponent<Slot>();

                            if (slot != null)
                            {
                                if (!slot.isAvailable)
                                    return;
                            }

                            if( (building <= 0  && buildCardEffect != null) || (building > 0 && buildCardEffect == null))
                            {
                                _selectedCard.SetTarget(targetable);
                                _playerHandManagement.ReAssignHands();
                                _selectedCard.Activate();
                                _selectedCard.GetComponent<CardUI>().ResetCardClick(true);
                            }
                        }
                        else
                        {
                            _selectedCard.GetComponent<CardUI>().ResetCardClick();
                        }
                    }
                }
                else if (_selectedCard != null)
                {
                    _selectedCard.GetComponent<CardUI>().ResetCardClick();
                }
            }
 
        }

        public void ShowDialog(string message)
        {

        }

        public Playable PlayDetermineTurnGame()
        {
            return null;
        }

        public void SelectFirstEnd()
        {
            //_leftSelect.SetActive(false);
            //_rightSelect.SetActive(false);
        }

        public void ToggleTurnEndButton(bool enabled)
        {
            _turnEndButton.GetComponent<Button>().interactable = enabled;
        }

        public void GetHands(bool isPlayerTurn, Action callback=null)
        {
            if (!isPlayerTurn)
                _opponentHandManagement.GetHands(true, callback);
            else
                _playerHandManagement.GetHands();
        }

        public void SetHands(bool player, bool opponent)
        {
            AnimationManager.Instance.PlayHandAnimation(_playerHandManagement.cardContainer.transform, _playerHandManagement.baseY, true, player);
            AnimationManager.Instance.PlayHandAnimation(_opponentHandManagement.cardContainer.transform, _opponentHandManagement.baseY, false, opponent);            
        }

        public void ThrowAwayHand(bool isPlayer)
        {
            if (isPlayer)
                _playerHandManagement.ThrowAwayHand();
            else
                _opponentHandManagement.ThrowAwayHand();
        }

        public void FreezeUI()
        {

        }
    }

}
