using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Characters.Weapons
{
    public class Missile : Weapon
    {
        private void Start()
        {
            OnCompletedAction += () =>
            {
                var particle = Instantiate(_boomParticle, transform.position, transform.rotation);
                Destroy(particle, 3.5f);
                _target.TakeDamage(10, gameObject);
                gameObject.SetActive(false);
                Destroy(gameObject, 0.5f);
            };
        }

        public override void Launch()
        {
            base.SetTarget();
            var particle = Instantiate(_initialParticle, transform.position, Quaternion.identity);
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(particle, 2);
            _railRoad.Animation(new Vector3[1] { _destination });
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
