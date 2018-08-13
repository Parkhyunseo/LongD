using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Characters.Buildings
{
    /// <summary>
    /// Building생성, 업그레이드, 삭제를 담당
    /// </summary>
    public class BuildingBuilder : Singletons.Singleton<BuildingBuilder>
    { 
        [SerializeField]
        private GameObject[] _mySlots;

        private GameObject[] _yourSlots;

        private BuildingFactory BuildingFactory;

        BuildingManager _buildingManager;

        protected override void Awake()
        {
            base.Awake();
            //_buildingManager = BuildingManager.Instance;
            // 성능 <<< 생산성...?
            
        }

        private void Start()
        {
            /*
            _mySlots = new GameObject[GameManager.slotSize];
            var myPlanet = GameObject.Find("MyPlanet");
            int i = 0;
            foreach (Transform child in myPlanet.transform)
            {
                _mySlots[i] = child.gameObject;
                i++;
                Debug.Log(i);
            }
            */
            /*
            var yourPlanet = GameObject.Find("YourPlanet");
            i = 0;
            foreach (Transform child in yourPlanet.transform)
            {
                _yourSlots[i] = child.gameObject;
                i++;
            }*/
        }

        /// <summary>
        /// Null Check 필요
        /// 이름으로 OnActive
        /// </summary>
        /// <param name="buildingRequestment"></param>
        /// <returns></returns>
        public void Order(Slot target,GameObject building)
        {
            // TODO :: 추가적인 작업이 무엇인지 확인
            GameObject createdBuilding = Instantiate(building);
            createdBuilding.transform.parent = target.transform;
            createdBuilding.transform.localPosition = Vector3.zero;
            createdBuilding.transform.localRotation = target.transform.rotation;
            //_mySlots[buildingRequestment.index].SetActive(false);
            //GameObject building = BuildingFactory.Instance.CreateBuilding(buildingRequestment);
            //building.transform.LookAt((2*_mySlots[buildingRequestment.index].transform.position - buildingRequestment.owner.transform.position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingRequestment"></param>
        /// <returns></returns>
        private Vector3 CalculatePosition(BuildingRequestment buildingRequestment)
        {
            //buildingRequestment.index;


            return new Vector3(0, 0, 0);
        }

    }
}