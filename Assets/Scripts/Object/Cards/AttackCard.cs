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
            this.illust = "Assets/Resources/sprites/Card/�ӽ� ����.png";
        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            CardDamage += DmgUp;
        }

        public override void UseCard(Enemy enemy)
        {
            if (enemy != null)
            {
                Debug.Log("���� �� ü��" + enemy.GetHp());
                enemy.HpChange(-CardDamage);
                Debug.Log("���� �� ü��" + enemy.GetHp());
            }
            else
                Debug.Log("���� ã�� �� �����ϴ�.");

        }
    }
}
