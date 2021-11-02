using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // 안쓴 카드와 쓴 카드를 놓을 공간
        private List<Cards> CardsDeck; // 덱 리스트
        private int CardCount; // 덱의 카드 수
        public Deck()
        {
            /*
            SettingFstDeck();
            CardCount = CardsDeck.Count;
            */
        }

        public void SettingFstDeck()// 만약 플레이어 직업 생기면 직업별 초기 카드 세팅 
        {
            for(int i = 0; i < 5; i++)
            {
                AttackCard AtCd = new AttackCard();
                CardsDeck.Add(AtCd);
                CardCount = CardsDeck.Count;
            }
        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // 덱 리스트의 맨 위 카드
            Cards Tempcard = CardsDeck[TopOfDeck]; 
            CardsDeck.RemoveAt(TopOfDeck); // 덱 리스트의 맨 위 카드 제거
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