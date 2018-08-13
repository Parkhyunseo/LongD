using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Buildings;

namespace Characters.Cards
{
    public class BuildCardEffect : CardEffect
    {
        [SerializeField]
        protected GameObject _building;

        /// <summary>
        /// CardEffect가 
        /// </summary>
        protected override void OnActivate()
        {
            if (_target != null)
                Build();
            else
                Debug.Log("Target이 존재하지 않습니다.");
        }

        protected override void OnDeActivate()
        {
            _isActivated = false;
        }

        /// <summary>
        /// Slot의 gameObject의 하위에 building을 생성
        /// </summary>
        /// <param name="target"></param>
        private void Build()
        {
            _target.Target( () => {
                var building = Instantiate(_building);
                var slot = _target.GetComponent<Slot>();
                //building.transform.position = _target.GetComponent<Slot>().buildingPosition;
                // NOTE :: 빌딩이 땅에 붙어 있는 것 처럼 보이기 위해
                //building.transform.rotation = Quaternion.Euler(0, 0, building.transform.rotation.z + _target.transform.rotation.z);
                building.transform.SetParent(_target.transform);
                building.transform.localPosition = slot.buildingPosition;
                building.transform.localRotation = slot.buildingQuaternion;

                slot.AlreadyWasBuilt = true;
                Debug.Log(slot.index + "번 건물에 설치");
                building.GetComponent<Building>().Build();
                OnDeActivate();
            });
        }

    }

}
