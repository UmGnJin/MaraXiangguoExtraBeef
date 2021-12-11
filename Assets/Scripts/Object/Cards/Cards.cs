using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    // THing�� ���׹̳� �����̻� �� ����
    public class Cards//��õ� ��ũ��Ʈ �̸��� �޶� Card�� ����.
    {
        public int cardTape = 0; // ī�� ����(ex ����,ȸ��,��ο�)���� �ٸ� ��.
        public int weakness = 0;
        private int cardCost = 0; // ī�� ���׹̳� �ڽ�Ʈ
        private int range = 0;
        public string illust;
        public string cardName = "�⺻ī��";
        public string cardInfo = "�̰� �⺻ī�忡��. �׷��� ���� �����.";
        /* ī������ �� ���� ȿ�� ������ ȿ�������� �ϱ����ؼ���
         * ����ü�� �ٸ��� ���� �ʿ䰡 ���� 
         * ����) �����̻� �� ȿ�� Ŭ������ �����
         *       ����ī�� �����ڿ� Ÿ�� ���� �´� ȿ�� Ŭ������ ����
         *       ī�� ��� �޼ҵ忡�� Ŭ������ �޼ҵ� ȣ�� - ���� �ʿ��Ѱ� �ƴ�
         * 
         * ī�� Ÿ�� �ڵ� ����
         *  ī�� Ÿ�� ,2 |ī�� �ڽ�Ʈ ,2 | ī�� ��Ÿ� ,1 | ī�� ��ġ(ex���ݷ� ,2 |
         *  1,30,5,10,1,4 13051014
         */
        public virtual void UseCard(player Plr, Enemy enemy) { }

        public virtual void settingCard() { }
        public void costChange(int newcost)
        {
            this.cardCost = newcost;
        }
        public int getCost()
        {
            return cardCost;
        }
        public int getRange()
        {
            return this.range;
        }
        public void setRange(int v)
        {
            this.range = v;
        }
        public int getWeaknessSt()
        {
            return this.weakness;
        }
        public void setWeakness(int weak)
        {
            this.weakness = weak;
        }
        public string getIllust()
        { //�Լ� �̸��� �빮�� I, �ҹ��� l 2���� ����Ѵ�
            return illust;
        }

        public void setCardSprite(string illu, string cname, string cinfo)
        {
            this.illust = "sprites/Card/" + illu; // ī�� ��� + ���� �̹���
            this.cardName = cname; // ī�� �̸�
            this.cardInfo = cinfo; // ī�� ����
        }

        public void conditionApp(player Plr, Enemy enemy, int conKey, int conTurn)
        {

            if (enemy != null)
            {
                enemy.condition_add(conKey, conTurn);
            }
            else
            {
                Plr.condition_add(conKey, conTurn);
            }
        }
    }

}

