using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class FindManager : MonoBehaviour
    {
        public GameObject[] FindObject = new GameObject[0];

        public static GameObject PlayerCardViewPosition;
        public static GameObject OpponentCardViewPosition;
        public static GameObject PreemptiveAttack;
        public static GameObject NonPreemptiveAttack;
        public static GameObject TurnEndButton;
        public static GameObject LogManager;

        public static GameObject PlayerDeck;
        public static GameObject OpponentDeck;

        public static GameObject PlayerCemetery;
        public static GameObject OpponentCemetery;

        public static GameObject PlayerPlanet;
        public static GameObject OpponentPlanet;

        public static GameObject PlayerHands;
        public static GameObject OpponentHands;

        public static GameObject MyTurnNotification;
        public static GameObject OpponentTurnNotification;

        public static GameObject PlayerPlanetStatic;
        public static GameObject OpponentPlanetStatic;

        void Awake()
        {
            PlayerCardViewPosition = FindObject[0];
            OpponentCardViewPosition = FindObject[1];
            PreemptiveAttack = FindObject[2];
            NonPreemptiveAttack = FindObject[3];
            TurnEndButton = FindObject[4];
            LogManager = FindObject[5];

            PlayerDeck = FindObject[6];
            OpponentDeck = FindObject[7];

            PlayerCemetery = FindObject[8];
            OpponentCemetery = FindObject[9];

            PlayerPlanet = FindObject[10];
            OpponentPlanet = FindObject[11];

            PlayerHands = FindObject[12];
            OpponentHands = FindObject[13];

            MyTurnNotification = FindObject[14];
            OpponentTurnNotification = FindObject[15];

            PlayerPlanetStatic = FindObject[16];
            OpponentPlanetStatic = FindObject[17];
        }
    }

}
