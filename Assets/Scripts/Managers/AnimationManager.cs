using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Managers
{
    public class AnimationManager : Singletons.PersistentSingleton<AnimationManager>
    {
        Sequence _handSequence;
        Sequence _cardSequence;
        Sequence _planetSequence;
        Sequence _sceneSequence;
        Sequence _turnEndSequence;

        public Sequence turnEndSequence
        {
            get
            {
                return _turnEndSequence;
            }
            set
            {
                _turnEndSequence = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DOTween.Init();
            //DOTween.defaultAutoPlay = AutoPlay.None;
            //DOTween.defaultAutoKill = false;  
        }

        // If you can, use decorator pattern
        private void CreateSequence(Sequence sequence)
        {
            if (sequence == null)
                sequence = DOTween.Sequence();              
        }

        private void PlayCallback(Sequence sequence, Action callback)
        {
            if (callback != null)
            {
                Debug.Log(sequence);
                sequence.onComplete += () =>
                {
                    callback.Invoke();
                };
            }
        }

        #region Card
        public void PlayCardHoverAnimation(RectTransform card, float afterY, bool isEnter=true, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(card.DOAnchorPosY(afterY, 0.3f))
                    .Join(card.DOScale(isEnter ? Vector3.one * 2 : Vector3.one, 0.3f))
                    .OnComplete(() => { if(callback!=null)callback.Invoke(); });
        }

        public void PlayCardActiveAnimation(Transform card, Vector3 pos, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(card.DOMove(pos, 0.3f))
                    .Join(card.DOScale(Vector2.one * 3f, 0.1f))
                    .OnComplete(() => { if (callback != null) callback.Invoke(); });
        }

        public void PlayCardDeActiveAnimation(Transform card, Vector3 pos, Action callback=null)
        {
            card.DOLocalMove(pos, 0.3f);
            card.DOScale(Vector2.one, 0.1f);
        }

        //deprecated
        public void PlayCardUseAnimation(Transform card, Action callback=null)
        {
            card.GetChild(0).GetComponent<Image>().DOFade(0, 0.6f);
        }

        public void PlayOpponentCard(RectTransform card, Vector3 viewPos, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(card.DOAnchorPosY(100, 0.3f))
                    .Join(card.DOScale(Vector3.one * 2, 0.3f))
                    .Append(card.DOMove(viewPos, 0.3f))
                    .Join(card.DOScale(Vector2.one * 3f, 0.1f))
                    .Append(card.DOLocalRotate(new Vector3(0, 90), 0.3f))
                    .AppendCallback(() => { card.GetComponent<UI.CardManagement.CardUI>().ChangeToFront(); })
                    .Append(card.DOLocalRotate(new Vector3(0, 180), 0.3f))
                    .OnComplete(() =>
                    {
                        if (callback != null)
                            callback.Invoke();
                    });
        }

        public void PlayCardGotoCemetery(Transform card, Transform cemetary, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();

            card.transform.SetParent(card.transform.parent.parent.parent);
            sequence.Append(card.DORotate(new Vector3(0, 0, -130), 0.3f))
                        .Join(card.DOMove(cemetary.position, 0.8f))
                        .Join(card.DOScale(0.4f, 0.8f))
                        .Join(card.GetChild(0).GetComponent<Image>().DOFade(0, 1.3f))
                        .OnComplete(() =>
                        {
                            if(callback!=null)
                            {
                                callback.Invoke();
                            }else
                            {
                                Destroy(card.gameObject);
                            }
                        });
        }
        #endregion

        #region GameSet
        public void PlaySlidingWindowAnimation(Transform window, bool isSelected, bool isLeft, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.onComplete += () => {
                if (callback != null)
                    callback.Invoke();
            };

            if(isSelected)
            {
                sequence.AppendInterval(1.5f);

            }
            else
            {
                sequence.Append(window.GetComponent<Image>().DOColor(Color.black, 0.3f));
            }

            if(isLeft)
            {
                sequence.Append(window.GetComponent<RectTransform>().DOLocalMoveX(-1000, 0.8f));
            }
            else
            {
                sequence.Append(window.GetComponent<RectTransform>().DOLocalMoveX(1000, 0.8f));
            }
        }

        public void PlayTurnNotification(Transform notification, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();
            var image = notification.GetComponent<Image>();

            //image.color = Color.white;
            sequence.Append(image.DOFade(1, 0.5f))
                    .AppendInterval(1.5f)
                    .Append(image.DOFade(0, 0.5f))
                    .OnComplete(() => { if (callback != null) callback.Invoke(); });
        }

        public void PlaySetUI(Transform log, Transform turnEndButton, Action callback=null)
        {
            log.GetComponent<RectTransform>().DOAnchorPosX(40, 0.8f);
            turnEndButton.GetComponent<RectTransform>().DOAnchorPosX(-50, 0.8f);
        }

        public void PlayHandAnimation(Transform hand, float baseY, bool upPadding, bool isShow=false, Action callback=null)
        {
            var padding = 70;
            hand.GetComponent<RectTransform>().DOAnchorPosY(isShow ? padding : -40, 0.3f);
        }

        public void PlayGetHandAnimation(Transform card, Vector3 to, Action callback = null)
        {
            var duration = 0.8f;//UnityEngine.Random.Range(0.5f, 1.3f);
            card.GetComponent<RectTransform>().DOAnchorPos(to, duration).OnComplete(() => {
                if (callback != null)
                {
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendInterval(1f)
                            .AppendCallback(() => callback());
                }
                 
            });            
        }

        #endregion

        #region Planet
        public void PlayPlanetHoverAnimation(Transform player, Transform opponent, bool isEnter = false, bool isTurnEnd = false, Action callback = null)
        {
            Sequence sequence = DOTween.Sequence();

            int playerScale = 0;
            int opponentScale = 0;
            int playerPosition = 0;
            int opponentPosition = 0;
            float speed = 0.3f;

            if (isEnter)
            {
                playerScale = 5;
                opponentScale = 8;
                playerPosition = -30;
                opponentPosition = 20;
            }
            else
            {
                playerScale = 8;
                opponentScale = 5;
                playerPosition = -20;
                opponentPosition = 30;
            }

            if (isTurnEnd)
            {
                speed = 1.5f;
            }

            sequence.Append(player.DOScale(playerScale, speed))
                .Join(player.DOLocalMoveY(playerPosition, speed))
                .Join(FindManager.PlayerPlanetStatic.transform.DOScale(playerScale, speed))
                .Join(FindManager.PlayerPlanetStatic.transform.DOLocalMoveY(playerPosition, speed))
                .Join(opponent.DOScale(opponentScale, speed))
                .Join(opponent.DOLocalMoveY(opponentPosition, speed))
                .Join(FindManager.OpponentPlanetStatic.transform.DOScale(opponentScale, speed))
                .Join(FindManager.OpponentPlanetStatic.transform.DOLocalMoveY(opponentPosition, speed))
                .OnComplete(() => callback());
        }
        
        public void PlaySetShield(Transform shield, bool enabled, Action callback=null)
        {
            shield.gameObject.SetActive(enabled);

            shield.GetComponent<SpriteRenderer>().DOFade(enabled ? 1 : 0, 0.5f);
            shield.DOScale(enabled ? Vector2.one * 2.5f : Vector2.one, 0.5f).OnComplete(() => {
                if (callback != null)
                    callback.Invoke();
            });
        }

        #endregion

        #region TurnEnd
        public void RotatePlanetAnimation(Transform planet, float angle, int count, Action callback = null)
        {
            Sequence sequence = DOTween.Sequence();
            Quaternion quaternion = planet.rotation;

            for (int i = 0; i < count; i++)
                quaternion = Quaternion.Euler(0, 0, angle) * quaternion;

            sequence.Append(planet.DORotateQuaternion(quaternion, 1f)).SetEase(Ease.InCubic)
                    .OnComplete(()=> {
                        if (callback != null)
                            callback.Invoke();
                    });
        }

        public void PlayPlanetFullShotAnimation(Transform player, Transform opoonent, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(player.DOScale(2.5f, 1))
                    .Join(player.DOLocalMoveY(-28, 1))
                    .Join(FindManager.PlayerPlanetStatic.transform.DOScale(2.5f, 1))
                    .Join(FindManager.PlayerPlanetStatic.transform.DOLocalMoveY(-28, 1))
                    .Join(opoonent.DOScale(2.5f, 1))
                    .Join(opoonent.DOLocalMoveY(28, 1))
                    .Join(FindManager.OpponentPlanetStatic.transform.DOScale(2.5f, 1))
                    .Join(FindManager.OpponentPlanetStatic.transform.DOLocalMoveY(28, 1))
                    .OnComplete(() => 
                    {
                        if (callback != null)
                            callback.Invoke();
                    });
        }

        public void PlayTurnEndButtonToggleAnimation(Transform button, bool isPlayerTurn, Action callback=null)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(button.DOScale(Vector3.one * 3, 0.6f))
                    .Join(button.DOLocalRotate(new Vector3(180, 0, 0), 0.6f))
                    .Append(button.DOScale(Vector3.one, 0.3f))
                    .Join(button.DOLocalRotate(new Vector3(360, 0, 0), 0.3f))
                    .Append(button.DOShakePosition(0.5f, 2, 20));
        }
    
        #endregion
    }

}
