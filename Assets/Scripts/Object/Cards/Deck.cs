using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // 안쓴 카드와 쓴 카드를 놓을 공간
        private List<Cards> CardsDeck = new List<Cards>(); // 덱 리스트
        private int CardCount; // 덱의 카드 수
        public Deck()
        {
            
            SettingFstDeck();
            Debug.Log("현재 덱에 있는 카드 수 :" + CardCount); // 로그
        }

        public void SettingFstDeck()// 만약 플레이어 직업 생기면 직업별 초기 카드 세팅 
        {
            for(int i = 0; i < 2; i++)
            {
                AttackCard AtCd = new AttackCard();
                CardsDeck.Add(AtCd);
                Debug.Log("공격카드 덱 구성중");// 로그
            }
            for (int i = 2; i <4; i++)
            {
                BlockCard BlCd = new BlockCard();
                CardsDeck.Add(BlCd);
                Debug.Log("방어카드 덱 구성중");// 로그
            }
            CardCount = CardsDeck.Count;
        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // 덱 리스트의 맨 위 카드
            Cards Tempcard = CardsDeck[TopOfDeck]; 
            CardsDeck.RemoveAt(TopOfDeck); // 덱 리스트의 맨 위 카드 제거
            Debug.Log("덱에서 카드 뽑는 중 : " + Tempcard.cardTape);// 로그

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