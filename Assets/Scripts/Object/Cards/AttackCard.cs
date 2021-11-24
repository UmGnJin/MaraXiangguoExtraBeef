using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int cardDamage = 10; // 기본 데미지.

        public AttackCard()
        {
            this.cardTape = 1;
            this.costChange(10);
            this.illust = "sprites/Card/임시 공격";
            this.cardName = "공격카드";
            this.cardInfo = cardDamage + "만큼 피해를 줍니다.";
        }
        public void IncreaseDMG(int DmgUp) // 공격력 증가.
        {
            cardDamage += DmgUp;
        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    enemy.HpChange(-cardDamage);
                    //상태이상 추가 할 부분
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
