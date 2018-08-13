using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Buildings
{
    public class BuildingRequestment
    {
        public readonly Planets.Planet owner;
        public readonly string name;
        public readonly BuildingClass buildingClass;
        public readonly int index;

        public BuildingRequestment(Planets.Planet owner, string name, BuildingClass buildingClass, int index)
        {
            this.owner = owner;
            this.name = name;
            this.buildingClass = buildingClass;
            this.index = index;
        }
    }
}
