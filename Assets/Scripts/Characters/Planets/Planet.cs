using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Buildings;
using Managers;
using Characters.Stats;

namespace Characters.Planets
{
    [RequireComponent(typeof(Health))]
    public class Planet : Attackable
    {
        Policy.RotationPolicy _rotationPolicy;

        Health _health;
        [SerializeField]
        GameObject slot_prefab;

        [SerializeField]
        float _radius;
        
        [SerializeField]
        int _activePoint;

        PlanetStat _stat;

        bool _activedShield;
        float _rotateAngle;

        Slot[] _slots;
        GameManager _gm;
        Players.Playable _owner;

        GameObject _hpObject;
        GameObject _populationObject;
        GameObject _shield;

        public Slot[] Slots
        {
            get
            {
                return _slots;
            }
        }

        public int activePoint
        {
            get
            {
                return _activePoint;
            }
            set
            {
                _activePoint = value;
            }
        }

        public bool activatedShield
        {
            get
            {
                return _activedShield;
            }
            set
            {
                _activedShield = value;
            }
        }

        public Health health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public Policy.RotationPolicy rotationPolicy
        {
            get
            {
                return _rotationPolicy;
            }
            set
            {
                _rotationPolicy = value;
            }
        }

        public GameObject shield
        {
            get
            {
                return _shield;
            }
        }

        private void Awake()
        {
            _gm = GameManager.Instance;
            _rotateAngle = CalculateAngle(GameManager.slotSize);

            _slots = new Slot[GameManager.slotSize];

            _rotationPolicy = new Policy.RotationPolicy
            {
                SpeedPerTurn = 1
            };
            //_stat = GetComponent<Stat>();
            _activePoint = 0; //NOTE :: 처음 시작할 때 가장 맨 머리위에있는 자식의 번호

            _owner = GetComponent<Players.Playable>();
            _health = GetComponent<Health>();
            _health.OnDie = () =>
            {
                _gm.EndGame(_owner);
            };
            
            if (GetComponent<Players.Player>() != null)
            {
                _hpObject = FindManager.PlayerPlanetStatic.transform.GetChild(0).gameObject;
                _populationObject = FindManager.PlayerPlanetStatic.transform.GetChild(1).gameObject;
                _shield = FindManager.PlayerPlanetStatic.transform.GetChild(2).gameObject;
            }
            else
            {
                _hpObject = FindManager.OpponentPlanetStatic.transform.GetChild(0).gameObject;
                _populationObject = FindManager.OpponentPlanetStatic.transform.GetChild(1).gameObject;
                _shield = FindManager.OpponentPlanetStatic.transform.GetChild(2).gameObject;
            }
    
            //MakeSlot(GameManager.slotSize);
        }

        private void Start()
        {
            MakeSlot(GameManager.slotSize);
            UpdateDate();
        }

        public GameObject SetShield(bool enabled)
        {
            AnimationManager.Instance.PlaySetShield(_shield.transform, enabled);
            _activedShield = enabled;

            return _shield;
        }
        
        public Attackable GetAttackable()
        {
            if (_activedShield)
                return _shield.GetComponent<Attackable>(); // 쉴드
            else if (_slots[_activePoint].AlreadyWasBuilt)
                return _slots[_activePoint].building.GetComponent<Attackable>(); // 건물
            else
                return GetComponent<Attackable>(); // 행성
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private float CalculateAngle(int n)
        {
            float r = (360 / n);

            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public void Rotate(Action callback=null)
        {
            _activePoint = (_activePoint - _rotationPolicy.SpeedPerTurn);
            if (_activePoint < 0)
                _activePoint = GameManager.slotSize - _rotationPolicy.SpeedPerTurn;
            Debug.Log("활성화 자식 인덱스 : " + _activePoint);
            Debug.Log("회전 속도 : " + _rotationPolicy.SpeedPerTurn);
            Quaternion startingRotation = transform.rotation;

            AnimationManager.Instance.RotatePlanetAnimation(transform,  -_rotateAngle, _rotationPolicy.SpeedPerTurn, () => {
                UpdateDate();
                if (callback != null)
                    callback.Invoke();
            });
            
            //StartCoroutine(CRotate(_rotateAngle, _rotationPolicy.SpeedPerTurn));
        }

        /// <summary>
        /// 자전하는 애니메이션
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerator CRotate(float angle, int count = 1)
        {
            Quaternion startingRotation = transform.rotation;
            
            for (int i = 0; i < count; i++)
            {
                Quaternion finalRotation = Quaternion.Euler(0, 0, -angle) * startingRotation;
                float accel = 1.0f;

                UpdateDate();
                while (transform.rotation != finalRotation)
                {
                    accel += 0.1f;
                    transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.deltaTime * accel);
                    yield return 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        private void MakeSlot(int n)
        {
            Sprite[] slotSprites = Resources.LoadAll<Sprite>(Resource.slots);
            float point = transform.localScale.y;
            Vector3 pos = new Vector3(0, point, 0);
            pos = Quaternion.AngleAxis(_rotateAngle, Vector3.forward) * pos;
            var angle = 0;
            // Note : 포인트 기준으로 360 / n 도 만큼 회전 하여 슬롯 배치

            for (int i = 0; i < n; i++)
            {
                pos = Quaternion.AngleAxis(-_rotateAngle, Vector3.forward) * pos;

                var slot = Instantiate(slot_prefab);

                slot.GetComponent<SpriteRenderer>().sprite = slotSprites[i];
                _slots[i] = slot.GetComponent<Slot>();
                _slots[i].transform.parent = transform;

                //_slots[i].transform.localPosition = new Vector3(-0.58f, 0.44f, 0);
                _slots[i].transform.localPosition = new Vector3(0, 0, 0);
                _slots[i].transform.localScale = new Vector3(1, 1, 1);

                _slots[i].buildingPosition = new Vector3(pos.x, pos.y, 0) * (_radius+0.8f) + new Vector3(transform.position.x, transform.position.y, 0);
                _slots[i].buildingPosition = _slots[i].transform.InverseTransformPoint(_slots[i].buildingPosition);

                _slots[i].buildingQuaternion = Quaternion.Euler(0, 0, angle);//Quaternion.AngleAxis(angle, Vector3.forward);
                angle -= 360 / n;

                _slots[i].isAvailable = true;
                _slots[i].index = i;
                _slots[i].owner = this;

                slot.AddComponent<PolygonCollider2D>();
            }

            if (GetComponent<Players.Opponent>() != null)
            {
                transform.localScale = new Vector3(5, 5, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
                
            /*
            for(int i = 0; i < n; i++)
            {
                pos = Quaternion.AngleAxis(_rotateAngle, Vector3.forward) * pos;

                var slot = Instantiate(slot_prefab, new Vector3(pos.x, pos.y , 0) * _radius, Quaternion.identity);
                _slots[i] = slot.GetComponent<Slot>();

                _slots[i].transform.parent = transform;

                _slots[i].transform.Translate(transform.position.x, transform.position.y, 0);
                
                var dir = transform.position - _slots[i].transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                _slots[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                _slots[i].isAvailable = true;
                _slots[i].index = i;
                //_slots[i].gameObject.SetActive(false);
            }*/

                //if (GetComponent<Players.Opponent>() != null)
                //    transform.localRotation = Quaternion.AngleAxis(180, Vector3.forward);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotSize"></param>
        private void ShowSlot(int slotSize)
        {
            for (int i = 0; i < slotSize; i++)
            {
                if (_slots[i].GetComponent<Slot>().isAvailable)
                    _slots[i].gameObject.SetActive(true);
                
            }
        }

        public void UpdateDate()
        {
            for(int i = 0; i < GameManager.slotSize; i++)
            {
                if ( (_activePoint == i) || 
                    ((_activePoint+1) % GameManager.slotSize == i) ||
                    ((_activePoint-1) % GameManager.slotSize == i) ||
                    ((_activePoint == 0) && (i == GameManager.slotSize-1)))
                    _slots[i].GetComponent<Slot>().SetDayAndNight(false);
                else
                {
                    _slots[i].GetComponent<Slot>().SetDayAndNight(true);
                }
            }
        }

        public override void TakeDamage(int damage, GameObject Insticator)
        {
            Debug.Log("이전 체력 : " + _health.CurrentHealth);
            _health.TakeDamage(damage);
            Debug.Log("이후 체력 : " + _health.CurrentHealth);
        }
    }
}