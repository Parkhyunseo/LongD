using System.Collections;
using System.Collections.Generic;
using Characters.Buildings;
using UnityEngine;

using Characters.Planets;

namespace Characters.Cards.Implementations
{
    public class ThrowawayShieldCardEffect : PassivePlanetShieldCardEffect
    {
        protected override void OnActivate()
        {
            OnShieldActivate += () =>
            {
                _owner.planet.SetShield(true);
            };
            base.OnActivate();
            //_owner.GetComponent<Planet>
            //효과 발생
        }

        protected override void OnDeActivate()
        {
            base.OnDeActivate();
        }
    }
}

