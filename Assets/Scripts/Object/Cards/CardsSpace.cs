using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    // THing�� ���׹̳� �����̻� �� ����
    public class Cards
    {
        public int cardTape = 0; // ī�� ����(ex ����,ȸ��,��ο�)���� �ٸ� ��.
        private int cardCost = 0; // ī�� ���׹̳� �ڽ�Ʈ
        /* ī������ �� ���� ȿ�� ������ ȿ�������� �ϱ����ؼ���
         * ����ü�� �ٸ��� ���� �ʿ䰡 ���� 
         * ����) �����̻� �� ȿ�� Ŭ������ �����
         *       ����ī�� �����ڿ� Ÿ�� ���� �´� ȿ�� Ŭ������ ����
         *       ī�� ��� �޼ҵ忡�� Ŭ������ �޼ҵ� ȣ�� - ���� �ʿ��Ѱ� �ƴ�
         * 0 ����Ʈ �� 
         *  X( 1, 2, 3...)  ����ī�� ,�����̻� ���� ī��, ����Ÿ�� ī��� enemy ���� ���ڷ� �޾ƾ��ϴ� ī��
         * 1X(11,12,13...)  ȸ��, ��,����� ������ ����� �ִ� �÷��̾� ��� ī��
         */
        public virtual void UseCard(Enemy enemy){}
        public void costChange(int  newcost)
        {
            cardCost = newcost;
        }
        public int getCost()
        {
            return cardCost;
        }
    }

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
                Debug.Log("���� �� ü��"+enemy.GetHp());
                enemy.HpChange(-CardDamege);
                Debug.Log("���� �� ü��"+enemy.GetHp());
            }
            else
                Debug.Log("���� ã�� �� �����ϴ�.");

        }
    }

    public class  BlockCard : Cards
    {
        private int playerBlock = 0;
        public BlockCard()
        {
            this.cardTape = 2;
            playerBlock = 10;
        }

        public override void UseCard(Enemy enemy){}
        public void UseCard(player pl)
        {
            pl.BlockChange(this.playerBlock);
        }
    }
}

