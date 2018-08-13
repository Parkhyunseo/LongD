using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Stats
{
    public class BuildingStat : MonoBehaviour
    {
        bool _needUpdate;

        Stat _finalStat;
        StatBase _baseStat;
        List<Stat> _bonuses;

        private void Start()
        {
            _bonuses = new List<Stat>();
            _baseStat = GetComponent<StatBase>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>building is die?</returns>
        public bool IsDieAfterDamage(int damage)
        {
            return true;
        }
    }

}
