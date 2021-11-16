using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ArcanaDungeon.cards
{
    public class AnotherDeck
    {
        // �Ⱦ� ī��� �� ī�带 ���� ����
        private List<AnotherCards> DeckList = new List<AnotherCards>(); // �� ��ü ����Ʈ

        private List<AnotherCards> Drawalbes = new List<AnotherCards>(); // ���� ��(=���� ī�� ����)
        public List<AnotherCards> Hand = new List<AnotherCards>(); // ��
        private List<AnotherCards> Discarded = new List<AnotherCards>(); // ���� ī��� ����

        public const int MAX_HAND = 6;
        public int max_Hand = 3;

        public AnotherDeck()
        {         
            Initialize();
            //Debug.Log("���� ���� �ִ� ī�� �� :" + Deck.Count); // �α�
        }

        public void Initialize()// ���� �÷��̾� ���� ����� ������ �ʱ� ī�� ���� 
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
            DeckList.Remove(Tempcard); // �� ����Ʈ�� �� �� ī�� ����
            //Debug.Log(Tempcard.Info());// �α�
        }
        public void Discard(AnotherCards card)
        {
            Discarded.Add(card);
            Hand.Remove(card);
            //Debug.Log(card.Info());// �α�
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