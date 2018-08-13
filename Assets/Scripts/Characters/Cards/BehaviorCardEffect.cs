using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Characters.Cards
{
    public abstract class BehaviorCardEffect : CardEffect
    {
        protected Action behavior;

        protected override void OnActivate()
        {
            if (behavior != null)
                behavior();
            else
                Debug.Log("Target have no ActiveBuilding Script");

            OnDeActivate();
        }

        protected override void OnDeActivate()
        {
            _isActivated = false;
        }

    }

}
