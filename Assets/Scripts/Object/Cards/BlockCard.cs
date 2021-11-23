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
        }

        public override void UseCard(Thing thing) { }
        public void UseCard(player pl)
        {
            pl.BlockChange(this.playerBlock);
        }
    }
}
