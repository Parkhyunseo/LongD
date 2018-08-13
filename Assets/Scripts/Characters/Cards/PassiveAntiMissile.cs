using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Buildings;

namespace Characters.Cards
{
    public class PassiveAntiMissile : BuildCardEffect
    {
        protected override void OnActivate()
        {
            // TODO :: 시전자의 카운터 함수에 넣어놓는다.
            //_target.transform.GetComponent<Planets.Planet>().onIntercept += OnAttack(damage, _target)
        }

        protected void OnAttack(ref double damage, Building target)
        {

        }
    }

}
