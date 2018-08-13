using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Characters.Buildings;
using Characters.Stats;
using Characters.Planets;
using Managers;

namespace Characters.Weapons
{
    [RequireComponent(typeof(Health))]
    public abstract class Weapon : Attackable
    {
        [SerializeField]
        protected GameObject _initialParticle;
        [SerializeField]
        protected GameObject _boomParticle;
        [SerializeField]
        protected int _power;

        GameManager _gm;
        Stat _stat;

        public Action OnCompletedAction;
        protected Attackable _target;
        protected Vector3 _destination;
        protected Action _trailFunc;
        protected Railroad _railRoad;

        public Attackable target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        static void AutoWeaponIcon()
        {

        }

        protected void Awake()
        {
            _railRoad = GetComponent<Railroad>();
        }

        private void Activate(Building building)
        {
            OnActivate();
        }

        private void Deactivate()
        {
            Deactivate();
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();

        public abstract void Launch();

        /// <summary>
        /// 기본적으로는 상대의 activePoint 그러니까 마주보고 있는 곳만 공격한다.
        /// </summary>
        public virtual void SetTarget()
        {
            Planet planet;
            if (GameManager.Instance.IsPlayerTurn)
                planet = GameManager.Instance.Opponent.GetComponent<Planet>();
            else
                planet = GameManager.Instance.Player.GetComponent<Planet>();
            _target = planet.GetAttackable();
            _destination = new Vector3(_target.transform.position.x, _target.transform.position.y);

        }

        /// <summary>
        /// 특정 대상을 지정하여 공격한다.
        /// </summary>
        /// <param name="targetable"></param>
        public virtual void SetTarget(Targetable targetable)
        {

        }

        public void Damage(int damage, GameObject instigator)
        {
            //_stat.hp -= damage;
        }

        public void ShowDetail()
        {
            
        }

        protected void InitializePlaySound(AudioClip audioClip)
        {
            SoundManager.Instance.PlaySound(audioClip, transform.position);
        }

        protected void InitializePlayAnimate()
        {

        }
    }
}
