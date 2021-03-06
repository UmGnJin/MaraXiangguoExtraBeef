using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class AttackCard : Cards
    {
        private int cardDamage = 20; // 기본 데미지.

        public AttackCard(int cType)
        {
            this.cardTape = cType;
            settingCard(cType);
            /*this.costChange(20);
            setRange(1);
            this.setCardSprite("임시 공격", "공격카드", "사거리 1칸, " + cardDamage + "만큼 피해를 줍니다.");*/
        }
        private void settingCard(int typ)
        {
            int co = typ % 1000000;
            int patto = co / 100000;
            co %= 100000;
            this.costChange(co / 1000); co %= 1000;
            this.setRange(co / 100); co %= 100;
            cardDamage = co;
            Debug.Log("공격 카드 종류" + patto + "/ 딜 , 코스트 " + cardDamage + ", " + this.getCost());
            switch (patto)
            {
                case 0:
                    {
                        this.setCardSprite("임시 공격", "공격카드", "사거리 1칸, " + cardDamage + "만큼 피해를 줍니다.");
                        break;
                    }
                case 1:
                    {
                        this.setCardSprite("임시 강타", "공격카드+", "사거리 1칸, " + cardDamage + "만큼 피해를 줍니다.");
                        break;
                    }
                case 2:
                    {
                        this.setCardSprite("화살 쏘기", "화살 쏘기", "사거리 5칸, " + cardDamage + "만큼 피해를 줍니다.");
                        break;
                    }

            }
        }
        public void IncreaseDMG(int DmgUp) // 공격력 증가.
        {
            this.cardDamage += DmgUp;
            setRange(1);
            this.setCardSprite("임시 강타", this.cardName + "+", "사거리 1칸, " + cardDamage + "만큼 피해를 줍니다.");
        }
        public void BasicRange()
        {  //원거리 공격 기본형
            this.cardTape = 1;
            setRange(5);
            this.setCardSprite("화살 쏘기", "화살 쏘기", "사거리 5칸, " + cardDamage + "만큼 피해를 줍니다.");
        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    enemy.be_hit(cardDamage);
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
