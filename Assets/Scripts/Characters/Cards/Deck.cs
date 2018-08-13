using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Characters.Cards
{
    public class Deck : MonoBehaviour
    {
        DeckManager _dm;
        Queue<Card> _cards;

        private void Awake()
        {
            _dm = DeckManager.Instance;
        }

        public void SetDeck(Card[] cards)
        {
            _cards.Clear();
            foreach(var card in cards)
            {
                _cards.Enqueue(card);
            }
        }

        public void GetDeck()
        {
            
        }
    }

}
