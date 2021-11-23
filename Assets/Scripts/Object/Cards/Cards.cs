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
        public string illust;
        /* 카드종류 및 여러 효과 구현을 효율적으로 하기위해서는
         * 구조체로 다르게 만들 필요가 있음 
         * 예시) 상태이상 및 효과 클래스를 만들고
         *       공격카드 생성자에 타입 값에 맞는 효과 클래스를 저장
         *       카드 사용 메소드에서 클래스의 메소드 호출 - 당장 필요한게 아님
         * 0 디폴트 값 
         *  X( 1, 2, 3...)  공격카드 ,상태이상 공격 카드, 다중타격 카드등 enemy 값을 인자로 받아야하는 카드
         * 1X(11,12,13...)  회복, 방어도,등등의 앞으로 생길수 있는 플레이어 대상 카드
         */
        public virtual void UseCard(Thing thing){}
        public void costChange(int  newcost)
        {
            cardCost = newcost;
        }
        public int getCost()
        {
            return cardCost;
        }
        public string getIllust() { //함수 이름은 대문자 I, 소문자 l 2개를 사용한다
            return illust;        
        }
    }

}

