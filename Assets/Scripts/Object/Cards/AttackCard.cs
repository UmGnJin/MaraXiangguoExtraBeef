using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int cardDamage = 20; // �⺻ ������.

        public AttackCard()
        {
            this.cardTape = 1;
            this.costChange(20);
            setRange(1);
            this.illust = "sprites/Card/�ӽ� ����";
            this.cardName = "����ī��";
            this.cardInfo = "��Ÿ� 1ĭ, "+cardDamage + "��ŭ ���ظ� �ݴϴ�.";
        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            this.cardDamage += DmgUp;
            setRange(1);
            this.illust = "sprites/Card/�ӽ� ��Ÿ";
            this.cardName += "+";
            this.cardInfo = "��Ÿ� 1ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.";
        }
        public void BasicRange() {  //���Ÿ� ���� �⺻��
            this.cardTape = 1;
            setRange(5);
            this.illust = "sprites/Card/ȭ�� ���";
            this.cardName = "ȭ�� ���";
            this.cardInfo = "��Ÿ� 5ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.";
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
