using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class BlockCard : Cards
    {
        private int playerBlock = 0;
        public BlockCard(int typ)
        {
            this.cardTape = typ;
            this.creatAtCard(typ);
            /*this.costChange(10);
            playerBlock = 10;
            this.setCardSprite("임시 방어", "방어카드", playerBlock + "만큼 방어도를 얻습니다.");*/

        }
        private void creatAtCard(int typ)
        {
            int co = typ % 1000000;
            int patto = co / 100000;
            co %= 100000;
            this.costChange(co / 1000); co %= 1000;
            this.setRange(co / 100); co %= 100;
            playerBlock = co;
            if (patto == 0)
            {
                this.setCardSprite("임시 방어", "방어카드", playerBlock + "만큼 방어도를 얻습니다.");
            }
        }
        public override void UseCard(player Plr, Enemy enemy)
        {
            Debug.Log("방어 카드 사용됨 : " + playerBlock + " / " + (Plr != null));
            if (Plr != null)
            {
                Plr.BlockChange(this.playerBlock);
                Plr.StaminaChange(-this.getCost());
            }
            else
                Debug.Log("player not found");
        }
    }
}
