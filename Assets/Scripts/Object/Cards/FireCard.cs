using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class FireCard : Cards
    {
        private int fireTurn = 0;
        public FireCard()
        {
            fireTurn = 3;
            this.costChange(20);
            this.illust = "sprites/Card/임시 공격";
            this.cardName = "발화카드";
            this.cardInfo = this.fireTurn + "턴 동안 피해를 줍니다.";
        }
        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    //상태이상 추가 할 부분
                    Plr.condition_add(0, this.fireTurn);
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("적을 찾을 수 없습니다.");
            }
            else
                Debug.Log("플레이어를 찾을 수없습니다.");

        }
    }
}


