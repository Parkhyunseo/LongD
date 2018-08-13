using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Planets;

namespace Characters.Players
{
    public class Player : Playable
    {
        Planet _planet;

        private void Awake()
        {
            population = GetComponent<Population>();
        }

        public void SetMyTurn()
        {
            population.CurrentPopulation = population.MaximumPopulation;
        }
    }
}
