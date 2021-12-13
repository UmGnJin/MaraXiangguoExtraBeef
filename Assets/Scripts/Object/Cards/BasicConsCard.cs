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
        int consType = 0;
        string[,] cardInfoList = new string[,] { { "�ӽ� ��ȭ", "��ȭī��", "������ ", "�� ���� ���ظ� �ݴϴ�." },
                                                 { "�ӽ� ����", "����ī��", "���� ", "�� ���� ������ŵ�ϴ�." },
                                                 { "�޷�", "�޷�", "", "�� ����  �� �� 15��ŭ ���¹̳��� �߰��� ȸ���մϴ�." },
                                                 { "�ӽ� �ߵ�", "�ߵ�ī��", "������ �ߵ��� ", "��ø ��ŵ�ϴ�." } };
        public BasicConsCard(int typ)
        {
            cardTape = 8;
            settingCard(typ);
        }
        private void settingCard(int typ)
        {
            int co = typ % 1000000;
            //int patto = co / 100000; //�����̻� + ��ο� ���� ī�� ������ ��� �� �ڵ� �κ�
            co %= 100000;
            this.costChange(co / 1000); co %= 1000;
            this.setRange(co / 100); co %= 100;
            this.consType = co / 10;
            this.consTurn = co % 10;
            this.setCardSprite(cardInfoList[this.consType, 0], cardInfoList[this.consType, 1], cardInfoList[this.consType, 2]+ this.consTurn + cardInfoList[this.consType, 3]);
        }
        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    //�����̻� �߰� �� �κ�
                    enemy.condition_add(this.consType, this.consTurn);
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("���� ã�� �� �����ϴ�.");
                if(this.consType == 2)// �޷�
                {
                    //�����̻� �߰� �� �κ�
                    Plr.condition_add(this.consType, this.consTurn);
                    Plr.StaminaChange(-this.getCost());
                }
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

