using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{

    public class BasicConsCard : Cards
    {
        /*�����̻� ī�� ����
         * -ī��� ����
         *  *�����ڿ� ���� �ڵ带 �Է� �Ѵ�.
         *  *ī�� ���� ����ī�带 ����ϴ� �� �����ϴ�.
         * -�����̻� ȿ�� �� ��ü�� ���� Ŀ��Ǿ� �Լ��� ���.
         */
        int consTurn = 0;
        public BasicConsCard()
        {

        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    //�����̻� �߰� �� �κ�
                    enemy.condition_add(0, this.consTurn);
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("���� ã�� �� �����ϴ�.");
            }
            else
                Debug.Log("�÷��̾ ã�� �������ϴ�.");

        }
        public void conditionApp(Enemy enemy, int conKey, int conTurn)
        {
            if (enemy != null)
            {
                enemy.condition_add(conKey, conTurn);
            }
        }

    }

}

