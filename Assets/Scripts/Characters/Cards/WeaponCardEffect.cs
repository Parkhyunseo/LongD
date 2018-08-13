using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Buildings;

namespace Characters.Cards
{
    /// <summary>
    /// 
    /// </summary>
    public class WeaponCardEffect : CardEffect
    {
        [SerializeField]
        GameObject _weapon;

        protected override void OnActivate()
        {
            if (_target.transform.childCount == 0)
            {
                Debug.Log("대상 슬롯에 건물이 존재 하지 않음.");
                return;
            }

            Debug.Log("무기를 장착할 건물 : " + _target.name);
            Debug.Log("무기가 장착 가능한가? : " + (_target.transform.childCount > 0).ToString());
            var building = _target.transform.GetChild(0).GetComponent<ActiveBuilding>();

            if (building != null)
                building.Weapon = _weapon;
            else
                Debug.Log("대상 슬롯의 건물이 Active건물이 아님");

            OnDeActivate();
        }

        protected override void OnDeActivate()
        {
            _isActivated = false;
        }

    }

}

