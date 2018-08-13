using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Characters.Buildings;
using Characters.Planets;
using DG.Tweening;

using Characters.Players;
using UI;

namespace Characters.Cards
{
    /// <summary>
    /// 카드의 효과에 대한 추상화 클래스
    /// </summary>
    [RequireComponent(typeof(Card))]
    public abstract class CardEffect : Subcomponent<Card>
    {
        [SerializeField]
        protected Targetable _target;
        [SerializeField]
        protected bool _deactivateOnDie = true;
        [SerializeField]
        protected AudioClip _sound;

        protected GameManager _gm;
        protected EffectManager _em;
        protected SoundManager _sm;
        protected AnimationManager _am;

        protected Action OnCardActive;
        protected Action OnCardDeactive;

        protected Players.Playable _owner;

        protected bool _isActivated;

        public bool isActivated
        {
            get
            {
                return _isActivated;
            }
        }

        public bool deactivteOnDie
        {
            get
            {
                return _deactivateOnDie;
            }
            set
            {
                _deactivateOnDie = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _gm = GameManager.Instance;
            _em = EffectManager.Instance;
            _sm = SoundManager.Instance;
            _am = AnimationManager.Instance;
            _super = GetComponent<Card>();
        }

        public void Activate()
        {
            // TODO :: 카드 사용하는 애니메이션
            if (_isActivated)
                return;

            if (_gm.currentTurn == _gm.Player)
                _owner = _gm.Player;
            else
                _owner = _gm.Opponent;

            Debug.Log("************Activate : " + _owner);

            var isUseable = _owner.population.UsePopulation(_super.cardNeededPeople);

            if (!isUseable)
                return;// TODO :: 안된다는 소리

            _isActivated = true;

            if(_gm.currentTurn == _gm.Player)
            {
                _am.PlayCardGotoCemetery(transform, FindManager.PlayerCemetery.transform);
                FindManager.LogManager.GetComponent<LogManager>().PresentLog(_super.logPrefab);
                OnActivate();
            }
            else
            {
                Debug.Log("Opponent Turn Card Use Animation");
                _am.PlayOpponentCard(GetComponent<RectTransform>(), FindManager.OpponentCardViewPosition.transform.position, () =>
                {
                    Debug.Log("Opponent PlayCardGotoCemetery");
                    _am.PlayCardGotoCemetery(transform, FindManager.OpponentCemetery.transform,() =>
                    {
                        // TODO :: UIManager로 돌리자
                        FindManager.LogManager.GetComponent<LogManager>().PresentLog(_super.logPrefab);
                        OnActivate();
                        StartCoroutine(WaitForDeActivate(() =>_gm.Opponent.UseCard()));
                    });
                });
            }
        }

        IEnumerator WaitForDeActivate(Action action)
        {
            while(_isActivated)
            {
                yield return null; 
            }
            Debug.Log("DeActivated");
            action();
            DeActivate();
        }
        
        public void DeActivate()
        {
            OnDeActivate();

            StopAllCoroutines();

            Destroy(gameObject);
        }

        public void SetTarget(Targetable target)
        {
            _target = target;
        }

        protected abstract void OnActivate();
        protected abstract void OnDeActivate();

        /// <summary>
        /// CardEffect Sound
        /// </summary>
        protected void PlaySound()
        {
            _sm.PlaySound(_sound, transform.position);
        }
    }

}

;