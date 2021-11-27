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
            this.illust = "sprites/Card/�ӽ� ����";
            this.cardName = "����ī��";
            this.cardInfo = cardDamage + "��ŭ ���ظ� �ݴϴ�.";
        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            this.cardDamage += DmgUp;
            this.illust = "sprites/Card/�ӽ� ��Ÿ";
            this.cardName += "+";
            this.cardInfo = cardDamage + "��ŭ ���ظ� �ݴϴ�.";
        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    enemy.HpChange(-cardDamage);
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
