using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class CardSlots
    {
        private List<Cards> CardSlot;
        private int LimitCardsNum = 10; // 가질 수 있는 최대 카드 수  
        private int StartTurnHands = 4;
        public Enemy DetectedEnemy = new Enemy();// 시야 코딩 완성되면 받기
        public void DrawCards(Deck deck_to_draw, int CardsNum_to_draw) // 덱에 있는 맨 위부터 카드 정해진 수 만큼 가져오기
        {
            for(int CardsNum = 0; CardsNum < CardsNum_to_draw; CardsNum++)
            {
                if (CardSlot.Count < LimitCardsNum)
                    CardSlot.Add(deck_to_draw.HandOverCards());
                else
                    break;
            }
        }
        public void UsingCard(int SlotNum)
        {
            CardSlot[SlotNum].UseCard(DetectedEnemy);
        }

        public int StartTurnCardsNum()
        {
            return StartTurnHands;
        }
        public void UpStarHandsNum(int AddedDraw)
        {
            StartTurnHands += AddedDraw;
            if (StartTurnHands > LimitCardsNum)
                StartTurnHands = LimitCardsNum;
        }
    }
}

