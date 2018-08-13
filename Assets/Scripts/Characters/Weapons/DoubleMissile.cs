using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Characters.Weapons
{
    public class DoubleMissile : Weapon
    {
        Vector3 _leftPath1;
        Vector3 _leftPath2;
        Vector3 _rightPath1;
        Vector3 _rightPath2;

        private void Start()
        {
            OnCompletedAction += () =>
            {
                var particle = Instantiate(_boomParticle, transform.position, transform.rotation);
                Destroy(particle, 3.5f);
                Debug.Log("Double Missile의 타겟 : " + _target.name);
                _target.TakeDamage(_power, gameObject);
                gameObject.SetActive(false);
                Destroy(gameObject, 0.5f);
            };
        }

        /// <summary>
        /// 무기를 사용한다.
        /// </summary>
        public override void Launch()
        {
            base.SetTarget();
            var friends = Instantiate(gameObject, transform.parent);
            friends.GetComponent<DoubleMissile>().SecondLaunch();

            var particle = Instantiate(_initialParticle, friends.transform.position, Quaternion.identity);
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(particle, 2);

            _leftPath1 = new Vector3(-30, 0, 0);
            _leftPath2 = new Vector3(-25, _destination.y-10, 0);

            _railRoad.Animation(new Vector3[4] { new Vector3(transform.position.x, transform.position.y+5, 0), _leftPath1, _leftPath2, _destination });
        }

        public void SecondLaunch()
        {
            base.SetTarget();
            var particle = Instantiate(_initialParticle, transform.position, Quaternion.identity);
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(particle, 2);

            _rightPath1 = new Vector3(30, 0, 0);
            _rightPath2 = new Vector3(25, _destination.y - 10, 0);

            _railRoad.Animation(new Vector3[4] { new Vector3(transform.position.x, transform.position.y + 5, 0), _rightPath1, _rightPath2, _destination });
        }

        protected override void OnActivate()
        {

        }

        protected override void OnDeactivate()
        {
        }

        public override void TakeDamage(int damage, GameObject Insticator)
        {

        }
    }

}
