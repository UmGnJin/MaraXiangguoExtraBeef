using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class FireCard : Cards
    {
        private int fireTurn = 0;
        private int fir = 0;
        public FireCard(int typ)
        {
            this.cardTape = typ;
            this.creatAtCard(typ);
        }
        private void creatAtCard(int typ)
        {
            int co = typ % 1000000;
            int patto = co / 100000;
            co %= 100000;
            this.costChange(co / 1000); co %= 1000;
            this.setRange(co / 100); co %= 100;
            this.fir = co / 10;
            this.fireTurn = co % 10;
            if (patto == 0)
            {
                this.setCardSprite("임시 발화", "발화카드", this.fireTurn + "턴 동안 피해를 줍니다.");
            }
        }
        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    //상태이상 추가 할 부분
                    enemy.condition_add(0, this.fireTurn);
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


