using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class BlockCard : Cards
    {
        private int playerBlock = 0;
        public BlockCard()
        {
            this.cardTape = 2;
            this.costChange(10);
            playerBlock = 10;
            this.illust = "sprites/Card/임시 방어";
            this.cardName = "방어카드";
            this.cardInfo = playerBlock + "만큼 방어도를 얻습니다.";
        }

        public override void UseCard(player Plr, Enemy enemy)
        {
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
