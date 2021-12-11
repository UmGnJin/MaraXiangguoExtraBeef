using ArcanaDungeon.Object;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class ThiefAttackCard : Cards
{
        private int cardDamage = 20; // 기본 데미지.

        public ThiefAttackCard(int typ)
        {
            this.costChange(20);
            this.setRange(2);
            settingCard(typ);
            /*
            this.setCardSprite("임시 공격", "공격카드", "사거리 1칸, " + cardDamage + "만큼 피해를 줍니다.");*/
        }
        private void settingCard(int typ)
        {
            int t = typ;
            cardDamage = t / 100;t %= 100;
            this.setWeakness(t / 10);
            this.cardTape = t % 10;
            switch (this.cardTape)
            {
                case 0:
                    {
                        this.setCardSprite("임시 공격", "급소 찌르기", "사거리 2칸, " + cardDamage + "만큼 피해를 주고, 약화 " + this.getWeaknessSt() +  "을 부여합니다.");
                        break;
                    }
                case 1:
                    {
                        this.setCardSprite("임시 강타", "수 읽기", "카드를 한 장 뽑습니다.");
                        break;
                    }
                case 2:
                    {
                        this.setCardSprite("화살 쏘기", "화살 쏘기", "사거리 2칸, " + cardDamage + "만큼 피해를 두번 줍니다.");
                        break;
                    }
                case 3:
                    {
                        this.setCardSprite("화살 쏘기", "화살 쏘기", "사거리 2칸, " + cardDamage + "만큼 피해를 줍니다.");
                        break;
                    }
                case 4:
                    {
                        this.setCardSprite("화살 쏘기", "화살 쏘기", "사거리 2칸, " + cardDamage + "만큼 피해를 줍니다.");
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

