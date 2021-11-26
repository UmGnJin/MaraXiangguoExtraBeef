using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class FireCard : Cards
    {
        private int fireTurn = 0;
        public FireCard()
        {
            fireTurn = 3;
            this.costChange(20);
            this.illust = "sprites/Card/�ӽ� ����";
            this.cardName = "��ȭī��";
            this.cardInfo = this.fireTurn + "�� ���� ���ظ� �ݴϴ�.";
        }
        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    //�����̻� �߰� �� �κ�
                    Plr.condition_add(0, this.fireTurn);
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("���� ã�� �� �����ϴ�.");
            }
            else
                Debug.Log("�÷��̾ ã�� �������ϴ�.");

        }
    }
}


