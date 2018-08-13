using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Cards
{
    public class CardLog : MonoBehaviour
    {
        private Stack<Card> _log;

        void Awake()
        {
            _log = new Stack<Card>();
        }

        public void AddLog(Card card)
        {
            _log.Push(card);
            //CardStcak에 추가
        }
    }

}
