using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Stats
{
    [Serializable]
    public class StatForSerialize : EnumArray<StatEnum, string>
    {
        public string Shield;
        public string ShieldLife;
        public string StrickingPower;

        public void Serialize()
        {
            this[StatEnum.Shield] = Shield;
            this[StatEnum.ShieldLife] = ShieldLife;
            this[StatEnum.StrickingPower] = StrickingPower;
        }
    }
}