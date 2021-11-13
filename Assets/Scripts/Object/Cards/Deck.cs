using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // �Ⱦ� ī��� �� ī�带 ���� ����
        private List<Cards> CardsDeck = new List<Cards>(); // �� ����Ʈ
        private int CardCount; // ���� ī�� ��
        public Deck()
        {
            
            SettingFstDeck();
            Debug.Log("���� ���� �ִ� ī�� �� :" + CardCount); // �α�
        }

        public void SettingFstDeck()// ���� �÷��̾� ���� ����� ������ �ʱ� ī�� ���� 
        {
            for(int i = 0; i < 2; i++)
            {
                AttackCard AtCd = new AttackCard();
                CardsDeck.Add(AtCd);
                Debug.Log("����ī�� �� ������");// �α�
            }
            for (int i = 2; i <4; i++)
            {
                BlockCard BlCd = new BlockCard();
                CardsDeck.Add(BlCd);
                Debug.Log("���ī�� �� ������");// �α�
            }
            CardCount = CardsDeck.Count;
        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // �� ����Ʈ�� �� �� ī��
            Cards Tempcard = CardsDeck[TopOfDeck]; 
            CardsDeck.RemoveAt(TopOfDeck); // �� ����Ʈ�� �� �� ī�� ����
            Debug.Log("������ ī�� �̴� �� : " + Tempcard.cardTape);// �α�

            return Tempcard;
        }

        public void ChangDeck(List<Cards> UsedDeck)
        {
            CardsDeck = UsedDeck;
        }

        public List<Cards> ShowDeckList()
        {
            return CardsDeck;
        }


    }
}