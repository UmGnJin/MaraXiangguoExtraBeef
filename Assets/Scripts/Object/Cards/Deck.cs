using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // 안쓴 카드와 쓴 카드를 놓을 공간
        private List<Cards> CardsDeck = new List<Cards>(); // 덱 리스트
        private List<Cards> UsedDeck = new List<Cards>(); // 사용한 카드 덱 리스트

        private int CardCount; // 덱의 카드 수

        public List<Cards> Hands = new List<Cards>();

        public const int MAX_HAND = 6;
        public int max_Hand = 3;
        public Deck()
        {
            SettingFstDeck();
            //Debug.Log("현재 덱에 있는 카드 수 :" + CardCount); // 로그
        }

        public void SettingFstDeck()// 만약 플레이어 직업 생기면 직업별 초기 카드 세팅 
        {
            for (int i = 0; i < 2; i++)
            {
                AttackCard AtCd = new AttackCard();
                CardsDeck.Add(AtCd);
                //Debug.Log("공격카드 덱 구성중");// 로그
            }
            for (int i = 2; i < 4; i++)
            {
                BlockCard BlCd = new BlockCard();
                CardsDeck.Add(BlCd);
                //Debug.Log("방어카드 덱 구성중");// 로그
            }
            CardCount = CardsDeck.Count;
        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // 덱 리스트의 맨 위 카드
            Cards Tempcard = CardsDeck[TopOfDeck];
            CardsDeck.RemoveAt(TopOfDeck); // 덱 리스트의 맨 위 카드 제거
            //Debug.Log("덱에서 카드 뽑는 중 : " + Tempcard.cardTape);// 로그

            return Tempcard;
        }

        public void ChangDeck()
        {
            CardsDeck = new List<Cards>(UsedDeck);
            UsedDeck = new List<Cards>();
            CardsDeck.OrderBy(a => Guid.NewGuid());
        }

        public List<Cards> showDeckList()
        {
            return CardsDeck;
        }
        public List<Cards> showCardSlot()
        {
            return Hands;
        }

        public void DrawCards() // 덱에 있는 맨 위부터 카드 정해진 수 만큼 가져오기
        {
            if (Hands.Count < max_Hand)
            {
                    CardCount--;
                    Hands.Add(CardsDeck[CardCount]);
                    CardsDeck.RemoveAt(CardCount);
            }
            //Debug.Log("핸드에 있는 카드 수 : 0" + " 카드 타입 :" + Hands[0].cardTape);
            //Debug.Log("핸드에 있는 카드 수 : 1" + " 카드 타입 :" + CardSlot[1].cardTape);
            //Debug.Log("핸드에 있는 카드 수 : 2" + " 카드 타입 :" + CardSlot[2].cardTape);
            //Debug.Log("핸드에 있는 카드 수 : 3" + " 카드 타입 :" + CardSlot[3].cardTape);
        }

        public int UsingCard(int SlotNum,Thing smthing )
        {
            int cost = Hands[SlotNum].getCost();
            //CardSlot[SlotNum].UseCard(smthing);
            //Debug.Log("카드 사용됨" + cost);
            UsedDeck.Add(Hands[SlotNum]);
            //Debug.Log(UsedDeck[0].cardTape + "사용된 카드타입");
            Hands.RemoveAt(SlotNum);
            return cost;
        }


    }
}