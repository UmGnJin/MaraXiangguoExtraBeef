using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int CardDamage = 10; // �⺻ ������.

        public AttackCard()
        {
            this.cardTape = 1;
            this.costChange(10);
            this.illust = "sprites/Card/�ӽ� ����";
        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            CardDamage += DmgUp;
        }

        public override void UseCard(Thing thing)
        {
            if (thing != null)
            {
                Debug.Log("���� �� ü��" + thing.GetHp());
                thing.HpChange(-CardDamage);
                Debug.Log("���� �� ü��" + thing.GetHp());
            }
            else
                Debug.Log("���� ã�� �� �����ϴ�.");

        }
    }
}
