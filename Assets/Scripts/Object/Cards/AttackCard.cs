using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int CardDamege = 10; // �⺻ ������.

        public AttackCard()
        {
            this.cardTape = 1;
            this.costChange(10);
        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            CardDamege += DmgUp;
        }

        public override void UseCard(Enemy enemy)
        {
            if (enemy != null)
            {
                Debug.Log("���� �� ü��" + enemy.GetHp());
                enemy.HpChange(-CardDamege);
                Debug.Log("���� �� ü��" + enemy.GetHp());
            }
            else
                Debug.Log("���� ã�� �� �����ϴ�.");

        }
    }
}
