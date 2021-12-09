using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int cardDamage = 20; // �⺻ ������.

        public AttackCard(int cType)
        {
            this.cardTape = cType;
            creatAtCard(cType);
            /*this.costChange(20);
            setRange(1);
            this.setCardSprite("�ӽ� ����", "����ī��", "��Ÿ� 1ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");*/
        }
        private void creatAtCard(int typ)
        {
            int co = typ % 1000000;
            int patto = co / 100000;
            co %= 100000;
            this.costChange(co / 1000); co %= 1000;
            this.setRange(co / 100); co %= 100;
            cardDamage = co;
            Debug.Log("���� ī�� ����" + patto + "/ �� , �ڽ�Ʈ " + cardDamage + ", " + this.getCost());
            switch (patto)
            {
                case 0:
                    {
                        this.setCardSprite("�ӽ� ����", "����ī��", "��Ÿ� 1ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
                        break;
                    }
                case 1:
                    {
                        this.setCardSprite("�ӽ� ��Ÿ", "����ī��+", "��Ÿ� 1ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
                        break;
                    }
                case 2:
                    {
                        this.setCardSprite("ȭ�� ���", "ȭ�� ���", "��Ÿ� 5ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
                        break;
                    }

            }
        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            this.cardDamage += DmgUp;
            setRange(1);
            this.setCardSprite("�ӽ� ��Ÿ", this.cardName + "+", "��Ÿ� 1ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
        }
        public void BasicRange()
        {  //���Ÿ� ���� �⺻��
            this.cardTape = 1;
            setRange(5);
            this.setCardSprite("ȭ�� ���", "ȭ�� ���", "��Ÿ� 5ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    enemy.be_hit(cardDamage);
                    //�����̻� �߰� �� �κ�
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
