using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

using Characters.Players;
using Characters.Planets;
using Characters.Buildings;
using Characters.Cards;
using Tasks;

using UI;

namespace Managers
{
    public class GameManager : Singletons.Singleton<GameManager>
    {
        // TODO :: 만약에 엄청 큰 수가 생길 경우 어떻게 처리할 것인가
        public const int slotSize = 8;

        public Player Player;
        public Opponent Opponent;
        public int turnEndCount;

        bool _isPlayingActiveAnimation;
        bool _isPlayerTurn;
        bool _isChangingTurn;
        Playable _currentTurn;
        TaskScheduler _ts;
        AnimationManager _am;
        FullScreenSprite _background;
        Sprite[] _backgroundSprites;
        UIManager _um;

        public bool isPlayingActiveAnimation
        {
            get
            {
                return _isPlayingActiveAnimation;
            }
            set
            {
                _isPlayingActiveAnimation = value;
            }
        }

        public bool IsPlayerTurn
        {
            get
            {
                return _isPlayerTurn;
            }
            set
            {
                _isPlayerTurn = value;
            }
        }

        public FullScreenSprite backgorund
        {
            get
            {
                return _background;
            }
        }

        public Playable currentTurn
        {
            get
            {
                return _currentTurn;
            }
            set
            {
                _currentTurn = value;
            }
        }

        public enum State
        {
            Turn,
            TurnIsOver,
            IsAnimating,
            EndAnimatation,
        }

        protected override void Awake()
        {
            base.Awake();

            _ts = TaskScheduler.Instance;
            _um = UIManager.Instance;
            _am = AnimationManager.Instance;

            var backgorund = new GameObject("Background");
            (_background = backgorund.AddComponent<FullScreenSprite>())
                  .renderer = backgorund.AddComponent<SpriteRenderer>();
            _background.renderer.sortingLayerName = "Background";
            _backgroundSprites = Resources.LoadAll<Sprite>(Resource.backgrounds);
        }

        void Start()
        {
            Player = FindManager.PlayerPlanet.GetComponent<Player>();
            Opponent = FindManager.OpponentPlanet.GetComponent<Opponent>();
        }

        /// <summary>
        /// Turn을 정의하고 핸드를 받는다.
        /// </summary>
        public void SetGame()
        {
            if (_isPlayerTurn)
            {
                Debug.Log("나의 턴");
                _currentTurn = Player;
                Player.SetMyTurn();
                _um.GetHands(true);
                _um.SetHands(true, false);
            }
            else
            {
                Debug.Log("상대의 턴");
                _currentTurn = Opponent;
                Freeze();

                _um.GetHands(false, () => {
                    Opponent.PlayOpponentTurn(() =>
                    {
                        EndTurn();
                        Debug.Log("Opponent Turn End");
                    });
                });
                _um.SetHands(false, true);
            }
        }

        /// <summary>
        /// Player - 턴 종료 버튼을 눌렀을 때, Opponent - 더 이상 낼 카드가 없을 때
        /// </summary>
        public void EndTurn()
        {
            WaitEndTasks(true);

            _um.ThrowAwayHand(_currentTurn.Equals(Player) ? true: false);
            // 카드를 모두 뒤로 보낸다.
            _um.SetHands(false, false);

            ActiveTopSlot();
            if(_ts.IsUseable())
            {
                Debug.Log("TaskScheduler Useable");
                _am.PlayPlanetFullShotAnimation(Player.transform, Opponent.transform, () =>
                {
                    Debug.Log("TaskScheduler Play");
                    _ts.Play();
                });
            }else
            {
                TurnChange(false);
            }
        }

        public void TurnChange(bool isNeedAnimation=true)
        {
            if (!_isChangingTurn)
                _isChangingTurn = true;
            else
                return;

            if (isNeedAnimation)
            {
                _am.PlayPlanetHoverAnimation(Player.transform, Opponent.transform, false, true, () => 
                {
                    _currentTurn.transform.GetComponent<Planet>().Rotate(() => {
                        if (_isPlayerTurn)
                        {
                            _isPlayerTurn = false;
                            _am.PlayTurnNotification(FindManager.OpponentTurnNotification.transform, () =>
                            {
                                // 다음 턴의 시작 손댈 수 있게
                                WaitEndTasks(false);
                                turnEndCount += 1;
                                _isChangingTurn = false;
                                SetGame();

                                _um.ToggleTurnEndButton(false);
                            });
                        }
                        else
                        {
                            _isPlayerTurn = true;
                            _am.PlayTurnNotification(FindManager.MyTurnNotification.transform, () =>
                            {
                                // 다음 턴의 시작 손댈 수 있게
                                WaitEndTasks(false);
                                turnEndCount += 1;
                                _isChangingTurn = false;
                                SetGame();

                                _um.ToggleTurnEndButton(true);
                            });
                        }
                    });
                });
            }
            else
            {
                _currentTurn.transform.GetComponent<Planet>().Rotate(()=> {
                    if (_isPlayerTurn)
                    {
                        _isPlayerTurn = false;

                        _am.PlayTurnNotification(FindManager.OpponentTurnNotification.transform, () =>
                        {
                            // 다음 턴의 시작 손댈 수 있게
                            WaitEndTasks(false);
                            turnEndCount += 1;

                            _isChangingTurn = false;
                            SetGame();

                            _um.ToggleTurnEndButton(false);
                        });
                    }
                    else
                    {
                        _isPlayerTurn = true;

                        _am.PlayTurnNotification(FindManager.MyTurnNotification.transform, () => 
                        {
                            // 다음 턴의 시작 손댈 수 있게
                            WaitEndTasks(false);
                            turnEndCount += 1;

                            _isChangingTurn = false;
                            SetGame();

                            _um.ToggleTurnEndButton(true);
                        });
                    }
                });

            }
        }

        public void OnMouseHoverFromPlanet(bool isEnter)
        {
            if(!_isPlayingActiveAnimation)
                AnimationManager.Instance.PlayPlanetHoverAnimation(Player.transform, Opponent.transform, isEnter);
        }

        IEnumerator WaitForActiveAnimation(Action callback)
        {
            while(_isPlayingActiveAnimation)
                yield return null;
            callback();
        }

        /// <summary>
        /// Freeze는 여기저기 막 움직일 수 있지만 행동은 되지 않는다.
        /// WaitForTask는 Task행동이 끝날 때 까지 아무 행동도 할 수 없다.
        /// </summary>
        /// <param name="useable"></param>
        private void WaitEndTasks(bool useable)
        {
            // TODO :: UI 사용 불가능
            _isPlayingActiveAnimation = useable;
        }
        
        public void ActiveTopSlot()
        {   
            var planet = _currentTurn.transform.GetComponent<Planet>();

            if (planet.Slots[planet.activePoint].AlreadyWasBuilt)
            {
                var building = planet.Slots[planet.activePoint].transform.GetChild(0);

                if (building != null)
                {
                    var activeBuilding = building.GetComponent<ActiveBuilding>();
                    if (activeBuilding != null)
                        building.GetComponent<ActiveBuilding>().OnActive();
                }
            }
        }

        /// <summary>
        /// Freeze는 여기저기 막 움직일 수 있지만 행동은 되지 않는다.
        /// WaitForTask는 Task행동이 끝날 때 까지 아무 행동도 할 수 없다.
        /// </summary>
        private void Freeze()
        {
            _um.FreezeUI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="winner"></param>
        public void EndGame(Playable winner)
        {
            // TODO :: EndGameAnimation;
            Debug.Log("EndGame!!");
        }

    }

}
