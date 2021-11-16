using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ArcanaDungeon.cards
{
    public class AnotherDeck
    {
        // 안쓴 카드와 쓴 카드를 놓을 공간
        private List<AnotherCards> DeckList = new List<AnotherCards>(); // 덱 전체 리스트

        private List<AnotherCards> Drawalbes = new List<AnotherCards>(); // 현재 덱(=뽑을 카드 더미)
        public List<AnotherCards> Hand = new List<AnotherCards>(); // 패
        private List<AnotherCards> Discarded = new List<AnotherCards>(); // 버린 카드들 더미

        public const int MAX_HAND = 6;
        public int max_Hand = 3;

        public AnotherDeck()
        {         
            Initialize();
            //Debug.Log("현재 덱에 있는 카드 수 :" + Deck.Count); // 로그
        }

        public void Initialize()// 만약 플레이어 직업 생기면 직업별 초기 카드 세팅 
        {
            for (int i = 0; i < 10; i++)
            {
                AnotherCards card = new AnotherCards("TestName", "Testing", i + 1);
                DeckList.Add(card);
            }
            Drawalbes = DeckList;
        }

        public void DrawCard()
        {
            AnotherCards Tempcard = DeckList[DeckList.Count - 1];
            Hand.Add(Tempcard);
            DeckList.Remove(Tempcard); // 덱 리스트의 맨 위 카드 제거
            //Debug.Log(Tempcard.Info());// 로그
        }
        public void Discard(AnotherCards card)
        {
            Discarded.Add(card);
            Hand.Remove(card);
            //Debug.Log(card.Info());// 로그
        }
        public void Shuffle()
        {
            foreach(AnotherCards card in Discarded)
            {
                Drawalbes.Add(card);
                Discarded.Remove(card);
            }
            Drawalbes.OrderBy(a => Guid.NewGuid());
        }
        public void ChangeDeck(List<AnotherCards> UsedDeck)
        {
            DeckList = UsedDeck;
        }

        public List<AnotherCards> ShowDeckList()
        {
            return DeckList;
        }


    }
}