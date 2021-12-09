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
        public BasicConsCard()
        {

        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    //상태이상 추가 할 부분
                    enemy.condition_add(0, this.consTurn);
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("적을 찾을 수 없습니다.");
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

