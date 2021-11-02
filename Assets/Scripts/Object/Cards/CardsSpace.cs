using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class Cards
    {
        public int CardTape = 0; // 카드 종류(ex 공격,회복,드로우)마다 다른 값.
        /*
         * 0 디폴트 값 
         *  X( 1, 2, 3...)  공격카드 ,상태이상 공격 카드, 다중타격 카드등 enemy 값을 인자로 받아야하는 카드
         * 1X(11,12,13...)  회복, 방어도,등등의 앞으로 생길수 있는 플레이어 대상 카드
         */
        public virtual void UseCard(Enemy enemy)
        {
            
        }
    }

    public class AttackCard : Cards
    {
        private int CardDamege = 10; // 기본 데미지.

        public AttackCard()
        {
            this.CardTape = 1;
        }
        public void IncreaseDMG(int DmgUp) // 공격력 증가.
        {
            CardDamege += DmgUp; 
        }

        public override void UseCard(Enemy enemy)
        {
            if (enemy != null)
            {
                Debug.Log("공격 전 체력"+enemy.GetHp());
                enemy.HpChange(-CardDamege);
                Debug.Log("공격 후 체력"+enemy.GetHp());
            }
            else
                Debug.Log("적을 찾을 수 없습니다.");

        }
    }
}

