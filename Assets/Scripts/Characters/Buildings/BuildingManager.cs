using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Characters.Buildings
{
    /// <summary>
    /// Building들을 총괄하고, 데이터들을 가지고 있는 곳
    /// </summary>
    public class BuildingManager : Singletons.Singleton<BuildingManager>
    {
        GameObject[] _fieldBuildings;
        Dictionary<string, int> _fieldBuildingsNameIndexDictionary;
        //PlayerData

        public GameObject[] fieldBuildings
        {
            get
            {
                return _fieldBuildings;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _fieldBuildingsNameIndexDictionary = new Dictionary<string, int>();
            LoadData();
        }

        private void Start()
        {
            for (int i = 0; i < _fieldBuildings.Length; i++)
            {
                //Debug.Log(_fieldBuildings[i].name);
                if (!_fieldBuildingsNameIndexDictionary.ContainsKey(_fieldBuildings[i].name))
                    _fieldBuildingsNameIndexDictionary.Add(_fieldBuildings[i].name, i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void LoadData()
        {
            // TODO :: 상대방과 내가 가지고 있는 빌딩들만 조사하여 Load시킨다. 
            _fieldBuildings = Resources.LoadAll<GameObject>(Resource.buildings);
        }

        /// <summary>
        /// string으로 Building Prefab을 찾아 리턴
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject Find(string name)
        {
            var building = _fieldBuildings[_fieldBuildingsNameIndexDictionary[name]];
            return building;          
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            
        }

        public void SetBuildingAt(int index, GameObject building)
        {

        }

        int BuildingNameToIndex(string name)
        {
            return _fieldBuildingsNameIndexDictionary[name];
        }

        //public 
    }
}
