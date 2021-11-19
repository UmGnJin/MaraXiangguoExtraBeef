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
            this.illust = "Assets/Resources/sprites/Card/임시 방어.png";
        }

        public override void UseCard(Enemy enemy) { }
        public void UseCard(player pl)
        {
            pl.BlockChange(this.playerBlock);
        }
    }
}
