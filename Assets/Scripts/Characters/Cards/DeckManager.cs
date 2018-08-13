using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Characters.Cards
{ 
    public class DeckManager : Singletons.PersistentSingleton<DeckManager>
    {
        private readonly static System.Random random = new System.Random();
        public const int maxDeckSize = 25;

        Managers.GameManager _gm;

        Card[] _allCard;

        Deck _playerDeck;
        Deck _opponentDeck;

        Cemetary _playerCemetary;
        Cemetary _opponentCemetary;

        Dictionary<Card, int> _opponent1DeckInformation;
        Dictionary<Card, int> _opponent2DeckInformation;
        Dictionary<Card, int> _opponent3DeckInformation;

        protected override void Awake()
        {
            base.Awake();
            LoadDeckData();

            _gm = Managers.GameManager.Instance;
            _opponent1DeckInformation = new Dictionary<Card, int>()
            {
                //{ _allCard[Data.CardInformation.BehaviorCardRotation], 10},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
                { _allCard[0], 1},
            };
        }

        private void LoadDeckData()
        {
            _allCard = Resources.LoadAll<Card>(Resource.cards);

            _playerDeck.SetDeck(_allCard);
        }
        
        public List<Card> GetHands(int size)
        {
            List<Card> hands = new List<Card>();

            for (var i = 0; i < size; i++)
            {
                //hands.Add(_myDeck.Dequeue());
            }

            return hands;
        }

        public void SuffleDeck()
        {

        }


        /// <summary>
        /// 파괴된 건물 같은 것을 덱에 추가 해준다.
        /// </summary>
        /// <param name="card"></param>
        public void AddCardToDeck(Card card)
        {

        }

        /// <summary>
        /// 파괴된 건물 같은 것들을 덱에서 제거 해준다.
        /// 특히 게임이 끝났을 때 제거 할 것
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCardFromDeck(Card card)
        {

        }
        
        /// <summary>
        /// 게임에서 승리할 경우 카드를 교체 
        /// </summary>
        /// <param name="trash"></param>
        public void ChangeCardInDeck(Card trash)
        {

        }


        private void Shuffle(List<Card> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (System.Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void PseudoShuffle(List<Card> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
