using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int CardDamage = 10; // 기본 데미지.

        public AttackCard()
        {
            this.cardTape = 1;
            this.costChange(10);
            this.illust = "sprites/Card/임시 공격";
        }
        public void IncreaseDMG(int DmgUp) // 공격력 증가.
        {
            CardDamage += DmgUp;
        }

        public override void UseCard(Thing thing)
        {
            if (thing != null)
            {
                Debug.Log("공격 전 체력" + thing.GetHp());
                thing.HpChange(-CardDamage);
                Debug.Log("공격 후 체력" + thing.GetHp());
            }
            else
                Debug.Log("적을 찾을 수 없습니다.");

        }
    }
}
