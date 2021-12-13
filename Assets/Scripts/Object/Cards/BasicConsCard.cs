using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class BasicConsCard : Cards
    {
        /*상태이상 카드 사용범
         * -카드로 사용시
         *  *생성자에 생성 코드를 입력 한다.
         *  *카드 사용시 유스카드를 사용하는 건 동일하다.
         * -상태이상 효과 그 자체일 때는 커디션앱 함수를 사용.
         */
        int consTurn = 0;
        int consType = 0;
        string[,] cardInfoList = new string[,] { { "임시 발화", "발화카드", "적에게 ", "턴 동안 피해를 줍니다." },
                                                 { "임시 기절", "기절카드", "적을 ", "턴 동안 기절시킵니다." },
                                                 { "급류", "급류", "", "턴 동안  매 턴 15만큼 스태미나를 추가로 회복합니다." },
                                                 { "임시 중독", "중독카드", "적에게 중독을 ", "중첩 시킵니다." } };
        public BasicConsCard(int typ)
        {
            cardTape = 8;
            settingCard(typ);
        }
        private void settingCard(int typ)
        {
            int co = typ % 1000000;
            //int patto = co / 100000; //상태이상 + 드로우 같은 카드 구현시 사용 될 코드 부분
            co %= 100000;
            this.costChange(co / 1000); co %= 1000;
            this.setRange(co / 100); co %= 100;
            this.consType = co / 10;
            this.consTurn = co % 10;
            this.setCardSprite(cardInfoList[this.consType, 0], cardInfoList[this.consType, 1], cardInfoList[this.consType, 2]+ this.consTurn + cardInfoList[this.consType, 3]);
        }
        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    //상태이상 추가 할 부분
                    enemy.condition_add(this.consType, this.consTurn);
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("적을 찾을 수 없습니다.");
                if(this.consType == 2)// 급류
                {
                    //상태이상 추가 할 부분
                    Plr.condition_add(this.consType, this.consTurn);
                    Plr.StaminaChange(-this.getCost());
                }
            }
            else
                Debug.Log("플레이어를 찾을 수없습니다.");

        }
        public void conditionApp(Enemy enemy, int conKey, int conTurn)
        {
            if (enemy != null)
            {
                enemy.condition_add(conKey, conTurn);
            }
        }

    }

}

