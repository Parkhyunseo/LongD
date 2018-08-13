using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Cards
{
    public class RotationPolicyCard : BehaviorCardEffect
    {
        void Start() 
        {
            behavior = () =>
            {
                _owner.planet.rotationPolicy.SpeedPerTurn = 3;
                _owner.planet.Rotate(() =>
                {

                    _owner.planet.rotationPolicy.SpeedPerTurn = 1;
                    DeActivate();
                });
            };
        }
    }

}
