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
            playerBlock = 10;
        }

        public override void UseCard(Enemy enemy) { }
        public void UseCard(player pl)
        {
            pl.BlockChange(this.playerBlock);
        }
    }
}
