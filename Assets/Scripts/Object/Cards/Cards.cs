using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    // THing에 스테미나 상태이상 방어도 있음
    public class Cards//명시된 스크립트 이름과 달라 Card로 통일.
    {
        public int cardTape = 0; // 카드 종류(ex 공격,회복,드로우)마다 다른 값.
        private int cardCost = 0; // 카드 스테미나 코스트
        private int range;
        public string illust;
        public string cardName = "sprites/Card/";
        public string cardInfo = "이건 기본카드에오. 그래오 쓸모 없어오.";
        /* 카드종류 및 여러 효과 구현을 효율적으로 하기위해서는
         * 구조체로 다르게 만들 필요가 있음 
         * 예시) 상태이상 및 효과 클래스를 만들고
         *       공격카드 생성자에 타입 값에 맞는 효과 클래스를 저장
         *       카드 사용 메소드에서 클래스의 메소드 호출 - 당장 필요한게 아님
         * 
         * 카드 타입 코드 예시
         *  카드 타입 ,1 |카드 코스트 ,2 | 카드 사거리 ,1 | 카드 수치(ex공격력 ,2 | 
         * 000(0) 기본 카드, 100 공격 카드, 200 방어카드 300 상태이상 카드 백의 자리 수로 나타냄
         * *10
         */
        public virtual void UseCard(player Plr, Enemy enemy){}

        public virtual void settingCard(){}
        public void costChange(int  newcost)
        {
            this.cardCost = newcost;
        }
        public int getCost()
        {
            return cardCost;
        }
        public int getRange() {
            return this.range;
        }
        public void setRange(int v) {
            this.range = v;
        }
        public string getIllust() { //함수 이름은 대문자 I, 소문자 l 2개를 사용한다
            return illust;        
        }

        public void setCardSprite(string illu, string cname, string cinfo)
        {
            this.illust = "sprites/Card/" + illu; // 카드 경로 + 목적 이미지
            this.cardName = cname; // 카드 이름
            this.cardInfo = cinfo; // 카드 설명
        }
    }

}

