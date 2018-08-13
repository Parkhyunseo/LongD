using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UI;
using Characters.Planets;

using DG.Tweening;

namespace Characters.Buildings
{
    public class Slot : Targetable
    {
        Planet _owner;
        Building _building;
        Vector3 _buildingPosition;
        Quaternion _buildingQuaternion;
        Sprite _buildingSpriteInDark;

        [SerializeField]
        int _index;
        [SerializeField]
        bool _isAvailable;
        [SerializeField]
        bool _alreadyWasBuilt;

        public Planet owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        public Building building
        {
            get
            {
                return _building;
            }
            set
            {
                _building = value;
            }
        }

        public Vector3 buildingPosition
        {
            get
            {
                return _buildingPosition;
            }
            set
            {
                _buildingPosition = value;
            }
        }

        public Quaternion buildingQuaternion
        {
            get
            {
                return _buildingQuaternion;
            }
            set
            {
                _buildingQuaternion = value;
            }
        }

        public int index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        public bool isAvailable
        {
            get
            {
                return _isAvailable;
            }
            set
            {
                _isAvailable = value;
            }
        }

        public bool AlreadyWasBuilt
        {
            get
            {
                return _alreadyWasBuilt;
            }
            set
            {
                _alreadyWasBuilt = value;
            }
        }

        public Sprite buildingSpriteInDark
        {
            get
            {
                return _buildingSpriteInDark;
            }
            set
            {
                _buildingSpriteInDark = value;
            }
        }

        public override void Target(Action todo)
        {
            todo.Invoke();

            AlreadyWasBuilt = true;
            _building = transform.GetChild(0).GetComponent<Building>();
        }

        public void SetDayAndNight(bool isNight)
        {
            var black = Color.black;

            if (isNight)
            {
                isAvailable = true;
                GetComponent<SpriteRenderer>().DOColor(Color.black, 1);
            }
            else
            {
                isAvailable = false;
                GetComponent<SpriteRenderer>().DOColor(Color.white, 1);
                
                //GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, black, 3);
            }
        }

        /// <summary>
        /// 설명창을 따로 둬서 설명을 띄어줌
        /// </summary>
        public void OnMouseUp()
        {
            if(_alreadyWasBuilt && UIManager.Instance.selectedCard == null)
            {
                // Show Log
                var activeBuilding = _building.GetComponent<ActiveBuilding>();
                if (activeBuilding!= null)
                {
                    // Show WeaponLog
                }
            }

        }

    }

}