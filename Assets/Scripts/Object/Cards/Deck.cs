using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using ArcanaDungeon;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // �Ⱦ� ī��� �� ī�带 ���� ����
        private List<Cards> CardsDeck = new List<Cards>(); // �� ����Ʈ
        private List<Cards> UsedDeck = new List<Cards>(); // ����� ī�� �� ����Ʈ

        private int CardCount; // ���� ī�� ��

        public List<Cards> Hands = new List<Cards>();

        public const int MAX_HAND = 6;
        public int max_Hand = 3;
        public Deck()
        {
            SettingFstDeck();
            ChangDeck();
            //DrawCards(); //UI.uicanvas.card_draw(Hands[Hands.Count - 1]);
            //DrawCards(); //UI.uicanvas.card_draw(Hands[Hands.Count - 1]); //���׽�Ʈ�� ���� �ӽ÷� �и� ����, ī�� ��ο� ��� ������ �Ϸ�Ǹ� �����ص� ��, �����ϸ� UI�� card_draw�� �̷��� ���� �θ��� �ʾƵ� DrawCards���� �ڵ����� ó���ϸ� ���� ��
        }

        public void SettingFstDeck()// ���� �÷��̾� ���� ����� ������ �ʱ� ī�� ���� 
        {
            
            for (int i = 0; i < 17; i++)
            {
                AttackCard AtCd = new AttackCard();
                AtCd.IncreaseDMG(20);
                AtCd.costChange(30);
                CardsDeck.Add(AtCd);
            }
            for (int i = 17; i < 34; i++)
            {
                AttackCard AtC = new AttackCard();
                CardsDeck.Add(AtC);
            }
            for (int i = 34; i < 50; i++)
            {
                FireCard FrCd = new FireCard();
                CardsDeck.Add(FrCd);
            }
            
            CardCount = CardsDeck.Count;
        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // �� ����Ʈ�� �� �� ī��
            Cards Tempcard = CardsDeck[TopOfDeck];
            CardsDeck.RemoveAt(TopOfDeck); // �� ����Ʈ�� �� �� ī�� ����
            //Debug.Log("������ ī�� �̴� �� : " + Tempcard.cardTape);// �α�

            return Tempcard;
        }

        public void ChangDeck() //�ڱ����̰� ���� �ӽ� �� ����
        {
            for(int i =0; i < CardsDeck.Count(); i++)
            {
                int ra1 = Dungeon.random.Next(0, CardsDeck.Count());
                int ra2 = Dungeon.random.Next(0, CardsDeck.Count());
                Cards temp = CardsDeck[ra1];
                CardsDeck[ra1] = CardsDeck[ra2];
                CardsDeck[ra2] = temp;
            }
            Debug.Log("���� �� �� ��"+CardsDeck.Count());
        }

        public List<Cards> showDeckList()
        {
            return CardsDeck;
        }
        public List<Cards> showCardSlot()
        {
            return Hands;
        }

        public void DrawCards() // ���� �ִ� �� ������ ī�� ������ �� ��ŭ ��������
        {
            if (Hands.Count < max_Hand)
            {
                    CardCount--;
                    Hands.Add(CardsDeck[CardCount]);
                    CardsDeck.RemoveAt(CardCount);
            }
            //Debug.Log("�ڵ忡 �ִ� ī�� �� " + Hands.Count);
            //Debug.Log("�ڵ忡 �ִ� ī�� �� : 1" + " ī�� Ÿ�� :" + CardSlot[1].cardTape);
            //Debug.Log("�ڵ忡 �ִ� ī�� �� : 2" + " ī�� Ÿ�� :" + CardSlot[2].cardTape);
            //Debug.Log("�ڵ忡 �ִ� ī�� �� : 3" + " ī�� Ÿ�� :" + CardSlot[3].cardTape);
        }

        public int UsingCard(int SlotNum, player PLR, Enemy EMY )
        {
            int effectType;
            switch (Hands[SlotNum].cardTape)
            {
                case 1:
                    {
                        Hands[SlotNum].UseCard(PLR, EMY);
                        effectType = 1;
                        break;
                    }
                case 2:
                    {
                        Hands[SlotNum].UseCard(PLR, EMY);
                        effectType = 2;
                        break;
                    }
                default:
                    {
                        Hands[SlotNum].UseCard(PLR, EMY);
                        effectType = 0;
                        break;
                    }
            }
            //CardSlot[SlotNum].UseCard(smthing);
            //Debug.Log("ī�� ����" + cost);
            UsedDeck.Add(Hands[SlotNum]);
            //Debug.Log(UsedDeck[0].cardTape + "���� ī��Ÿ��");
            Hands.RemoveAt(SlotNum);
            return effectType;
        }


    }
}