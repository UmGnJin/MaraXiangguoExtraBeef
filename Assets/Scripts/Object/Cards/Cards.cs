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
        private int cardCost = 0; // ī�� ���׹̳� �ڽ�Ʈ
        public string illust;
        /* ī������ �� ���� ȿ�� ������ ȿ�������� �ϱ����ؼ���
         * ����ü�� �ٸ��� ���� �ʿ䰡 ���� 
         * ����) �����̻� �� ȿ�� Ŭ������ �����
         *       ����ī�� �����ڿ� Ÿ�� ���� �´� ȿ�� Ŭ������ ����
         *       ī�� ��� �޼ҵ忡�� Ŭ������ �޼ҵ� ȣ�� - ���� �ʿ��Ѱ� �ƴ�
         * 0 ����Ʈ �� 
         *  X( 1, 2, 3...)  ����ī�� ,�����̻� ���� ī��, ����Ÿ�� ī��� enemy ���� ���ڷ� �޾ƾ��ϴ� ī��
         * 1X(11,12,13...)  ȸ��, ��,����� ������ ����� �ִ� �÷��̾� ��� ī��
         */
        public virtual void UseCard(Thing thing){}
        public void costChange(int  newcost)
        {
            cardCost = newcost;
        }
        public int getCost()
        {
            return cardCost;
        }
        public string getIllust() { //�Լ� �̸��� �빮�� I, �ҹ��� l 2���� ����Ѵ�
            return illust;        
        }
    }

}

