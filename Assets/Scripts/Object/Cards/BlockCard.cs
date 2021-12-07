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
            this.setCardSprite("�ӽ� ���", "���ī��", playerBlock + "��ŭ ���� ����ϴ�.");
        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            Debug.Log("��� ī�� ���� : " + playerBlock + " / "+(Plr != null));
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
