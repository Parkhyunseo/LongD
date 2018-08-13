using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Buildings
{
    // TODO :: 트리형태의 Type구조를 어떻게 나타낼 것인가.
    public enum BuildingClass : int
    {
        Unknown = 0,
        AttackBuilding,
        DefenseBuilding,
        ResourceBuilding,
        SpecificPurposeBuilding
    }
}
