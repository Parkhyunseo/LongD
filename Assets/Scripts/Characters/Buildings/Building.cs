using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Planets;

namespace Characters.Buildings
{
    [RequireComponent(typeof(Health))]
    public class Building : Attackable
    {
        [SerializeField]
        protected new string name;

        [SerializeField]
        protected int hp;

        [SerializeField]
        protected GameObject InstantiateParticle;

        [SerializeField]
        protected GameObject DestroyParticle;
        
        protected Planet _planet;

        Health _health;

        BuildingManager _bm;

        private void Awake()
        {
            _bm = BuildingManager.Instance;
            _health = GetComponent<Health>();
            _health.OnDie = () =>
            {
                Debug.Log("Building Current Health : " + _health.CurrentHealth);
                Destroy(gameObject);
            };
        }

        public override void TakeDamage(int damage, GameObject Insticator)
        {
            _health.TakeDamage(damage);
        }

        public void Build()
        {
            _planet = transform.parent.parent.GetComponent<Planet>();
            OnBuild();
        }
        
        public void Upgrade()
        {

        }

        public void DestroyBuilding()
        {
            OnCollapse();

            Destroy(gameObject);
        }

        protected virtual void OnBuild() { }
        protected virtual void OnCollapse() { }

    }

}
