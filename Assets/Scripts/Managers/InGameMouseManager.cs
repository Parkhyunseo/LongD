using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    // TODO :: 다시 사용 고려
    public class InGameMouseManager : Singletons.Singleton<InGameMouseManager>
    {
        public delegate void OnMyPlanetClick();
        public delegate void OnBackgroundClick();
        public delegate void OnSlotClick();

        public enum ClickableObject
        {
            Nothing,
            MyPlanet,
            EnemyPlanet,
            Slot,
        }

        ClickableObject FocusedObject = ClickableObject.Nothing;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
    }

}
