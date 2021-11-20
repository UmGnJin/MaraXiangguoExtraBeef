using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class CardSlots
    {
        private List<Cards> CardSlot = new List<Cards>();
        private int LimitCardsNum = 10; // 가질 수 있는 최대 카드 수  
        private int StartTurnHands = 4; //이 함수는 필요없을 듯함, 드로우는 2턴에 1장 고정으로 쭉 진행할 예정이라 from Cjh
        public Enemy DetectedEnemy = new Enemy();// 시야 코딩 완성되면 받기. Enemy는 MonoBehaviour 기반이므로 new로 생성하면 안됨.
        public void DrawCards(Deck deck_to_draw, int CardsNum_to_draw) // 덱에 있는 맨 위부터 카드 정해진 수 만큼 가져오기
        {
            for(int CardsNum = 0; CardsNum < CardsNum_to_draw; CardsNum++)
            {
                if (CardSlot.Count < LimitCardsNum)
                    CardSlot.Add(deck_to_draw.HandOverCards());
                else
                    break;
            }
            //Debug.Log("핸드에 있는 카드 수 : 0" + " 카드 타입 :" + CardSlot[0].cardTape);
            //Debug.Log("핸드에 있는 카드 수 : 1" + " 카드 타입 :" + CardSlot[1].cardTape);
            //Debug.Log("핸드에 있는 카드 수 : 2" + " 카드 타입 :" + CardSlot[2].cardTape);
            //Debug.Log("핸드에 있는 카드 수 : 3" + " 카드 타입 :" + CardSlot[3].cardTape);
        }
        public int UsingCard(int SlotNum)
        {
            CardSlot[SlotNum].UseCard(DetectedEnemy);
            return CardSlot[SlotNum].getCost();
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
        public int GetLimit() {
            return this.StartTurnHands;//LimitCardsNum;  ★LimitCardsNum이 6 이하로 바뀌면 주석으로 교체할 것
        }
    }
}

