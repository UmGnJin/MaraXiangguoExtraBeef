using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int CardDamege = 10; // 기본 데미지.

        public AttackCard()
        {
            this.cardTape = 1;
            this.costChange(10);
        }
        public void IncreaseDMG(int DmgUp) // 공격력 증가.
        {
            CardDamege += DmgUp;
        }

        public override void UseCard(Enemy enemy)
        {
            if (enemy != null)
            {
                Debug.Log("공격 전 체력" + enemy.GetHp());
                enemy.HpChange(-CardDamege);
                Debug.Log("공격 후 체력" + enemy.GetHp());
            }
            else
                Debug.Log("적을 찾을 수 없습니다.");

        }
    }
}
