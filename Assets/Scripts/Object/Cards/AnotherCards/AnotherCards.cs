using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    // THing�� ���׹̳� �����̻� �� ����
    public enum CardType
    {
        ATTACK = 0,
        DEFENSE = 1
    }
    
    public class AnotherCards//��õ� ��ũ��Ʈ �̸��� �޶� Card�� ����.
    {
        private int cost = 0; // ī�� ���׹̳� �ڽ�Ʈ
        public Sprite illust;
        public string info, name;
        
        public AnotherCards(string Name = "TestName", string Info = "TestInfo", int Cost = 99)
        {
            Initialize(Name, Info, Cost);
        }
        public void Initialize(string Name = "TestName", string Info = "TestInfo", int Cost = 99)
        {
            this.cost = Cost;
            this.name = Name;
            this.info = Info;
        }
        public virtual void UseCard(Enemy enemy) { }
        public void costChange(int newcost)
        {
            cost = newcost;
        }
        public int getCost()
        {
            return cost;
        }
        public string Info()
        {
            return ("�̸� : " + this.name + ", ��� : " + this.cost + ", ȿ�� : " + this.info);
        }
    }

}

