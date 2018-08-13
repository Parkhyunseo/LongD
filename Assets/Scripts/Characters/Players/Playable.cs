using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Planets;
using Characters.Buildings;

namespace Characters.Players
{
    public abstract class Playable : MonoBehaviour
    {
        /// <summary>
        /// players' Population Componnet
        /// </summary>
        public Population population;

        /// <summary>
        /// user's name or ai's name
        /// </summary>
        protected new string name;

        /// <summary>
        /// player's planet
        /// </summary>
        public Planet planet;

        /// <summary>
        /// 설치 할 수 있게 소지하고 있는 시설들
        /// </summary>
        protected List<Building> buildings;
    }

}
