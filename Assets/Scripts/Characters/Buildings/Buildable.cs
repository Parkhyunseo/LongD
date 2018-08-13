using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Planets;

namespace Characters.Buildings
{
    public class Buildable : Subcomponent<Reusable>
    {
        [SerializeField]
        [Information("", InformationAttribute.InformationType.None, false)]
        bool _subordinateToOwner;

        bool _activateOnStart;
        bool _activated;

        [SerializeField]
        protected Modifiers.ModifierList _onActivate;
        [SerializeField]
        protected Modifiers.ModifierList _onUnsummon;

        public bool Activated
        {
            get
            {
                return _activated;
            }
        }

        public Planet Owner
        {
            get;
            private set;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            if (_activateOnStart)
                Activate();
        //        _onActivate();
        }

        public void Activate()
        {
            if (_activated)
                return;

            _activated = true;
         //   _onActivate.Invoke(owner, owner);
        }

        public Buildable CreateBuilding(BuildingRequestment requestment)
        {
            //return Building
            return CreateBuilding(requestment, requestment.owner.transform.position);
        }

        /// <summary>
        /// Buliding을 생성하는 곳
        /// </summary>
        /// <param name="buildingComponent"></param>
        /// <param name="owner"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Buildable CreateBuilding(BuildingRequestment requestment, Vector3 position)
        {
            //return Building
            _super.gameObject.SetActive(false);

            var building = SimplePool.Spawn(_super, position).GetComponent<Buildable>();
            building._activated = false;
            building.Owner = requestment.owner;
            building.transform.parent = requestment.owner.transform;
            building.gameObject.SetActive(true); // OnEnable이 호출되는 순서를 제어하기 위함

            return building;
        }

        public void DestroyBuilding()
        {
            if (!_super.isSpawned)
                return;

        //    _onDestroy.Invoke(owner, owner);

            if (_subordinateToOwner)
         //       owner.onDie -= DestroyBuilding;

            SimplePool.Despawn(_super);
        }


    }

}
