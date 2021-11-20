using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class CardSlots
    {
        private List<Cards> CardSlot = new List<Cards>();
        private int LimitCardsNum = 10; // ���� �� �ִ� �ִ� ī�� ��  
        private int StartTurnHands = 4; //�� �Լ��� �ʿ���� ����, ��ο�� 2�Ͽ� 1�� �������� �� ������ �����̶� from Cjh
        public Enemy DetectedEnemy = new Enemy();// �þ� �ڵ� �ϼ��Ǹ� �ޱ�. Enemy�� MonoBehaviour ����̹Ƿ� new�� �����ϸ� �ȵ�.
        public void DrawCards(Deck deck_to_draw, int CardsNum_to_draw) // ���� �ִ� �� ������ ī�� ������ �� ��ŭ ��������
        {
            for(int CardsNum = 0; CardsNum < CardsNum_to_draw; CardsNum++)
            {
                if (CardSlot.Count < LimitCardsNum)
                    CardSlot.Add(deck_to_draw.HandOverCards());
                else
                    break;
            }
            //Debug.Log("�ڵ忡 �ִ� ī�� �� : 0" + " ī�� Ÿ�� :" + CardSlot[0].cardTape);
            //Debug.Log("�ڵ忡 �ִ� ī�� �� : 1" + " ī�� Ÿ�� :" + CardSlot[1].cardTape);
            //Debug.Log("�ڵ忡 �ִ� ī�� �� : 2" + " ī�� Ÿ�� :" + CardSlot[2].cardTape);
            //Debug.Log("�ڵ忡 �ִ� ī�� �� : 3" + " ī�� Ÿ�� :" + CardSlot[3].cardTape);
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
            return this.StartTurnHands;//LimitCardsNum;  ��LimitCardsNum�� 6 ���Ϸ� �ٲ�� �ּ����� ��ü�� ��
        }
    }
}

