using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Buildings
{
    public class PassiveBuilding : Building
    {
        [SerializeField]
        bool _shieldBuilding;

        [SerializeField]
        bool _resourceBuilding;

        /// <summary>
        /// If ShieldBuilding, point is Shield Health Point
        /// If ResourceBuilding, point is Plus Population Point
        /// </summary>
        [SerializeField]
        int _point;

        protected override void OnBuild()
        {
            if (_shieldBuilding)
            {
                var shield = _planet.SetShield(true);
                shield.GetComponent<Shield>().Setting(_point, _planet);
                // 보호막 생성
            }

            if (_resourceBuilding)
            {
                var population = _planet.GetComponent<Population>();
                population.MaximumPopulation += _point;
                // 인구수 증가
            }
        }

        protected override void OnCollapse()
        {
            if (_shieldBuilding)
            {
                var shield = _planet.SetShield(false);
                // 보호막 생성
            }

            if (_resourceBuilding)
            {
                var population = _planet.GetComponent<Population>();
                population.MaximumPopulation -= _point;
                // 인구수 증가
            }
        }
    }

}
