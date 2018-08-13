using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Weapons;

using Tasks;

namespace Characters.Buildings
{
    public class ActiveBuilding : Building
    {
        [SerializeField]
        GameObject _weapon;
        TaskScheduler _ts;

        public GameObject @Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                _weapon = value;
            }
        }

        private void Awake()
        {
            _ts = TaskScheduler.Instance;   
        }

        public void OnActive()
        {
            if(_weapon != null)
                _ts.AddTask(new Task(
                    () =>
                    {
                        Debug.Log("Action!");
                        var obj = Instantiate(_weapon, transform);
                        obj.GetComponent<Weapon>().Launch();
                    }
                ));
        }

        protected override void OnBuild()
        {

        }

        protected override void OnCollapse()
        {

        }
    }

}
