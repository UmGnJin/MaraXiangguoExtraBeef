using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    // THing에 스테미나 상태이상 방어도 있음
    public enum CardType
    {
        ATTACK = 0,
        DEFENSE = 1
    }
    
    public class AnotherCards//명시된 스크립트 이름과 달라 Card로 통일.
    {
        private int cost = 0; // 카드 스테미나 코스트
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
            return ("이름 : " + this.name + ", 비용 : " + this.cost + ", 효과 : " + this.info);
        }
    }

}

