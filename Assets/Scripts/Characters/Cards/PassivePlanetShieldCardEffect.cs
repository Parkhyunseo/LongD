using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Cards
{
    public abstract class PassivePlanetShieldCardEffect: BuildCardEffect
    {
        protected Action OnShieldActivate;

        protected override void OnActivate()
        {
            base.OnActivate();
            OnShieldActivate.Invoke();
            //_owner.GetComponent<Planet>
            //효과 발생
        }

        protected override void OnDeActivate()
        {
            base.OnDeActivate();
        }
    }

}
