using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;


namespace ArcanaDungeon.Object
{
    public class Slime : Enemy
    {
        public void Awake()
        {
            this.maxhp = 115;
            this.maxstamina = 100;
            this.power = 1;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "슬라임";
        }

        public void FixedUpdate()
        {
            if (isTurn > 0)
            {
                if (this.GetStamina() < 20 && this.exhausted == false)
                    this.exhausted = true;
                else if (this.GetStamina() >= 60 && this.exhausted == true)
                    this.exhausted = false;

                Vision_research();
                if(this.exhausted == true)// 스태미나 회복 방식. 일반적으로는 특정 조건 만족 시 탈진에 걸리고, 일정 수치 이상의 스태미나까지 휴식만 한다.
                                          // 그렇게 일정 수치까지 회복한 이후, 탈진 상태이상이 제거되고, 기존의 행동 우선도대로 행동을 재개한다.
                {
                    this.StaminaChange(20);
                }
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// 공격 거리 내에 플레이어가 존재 시, 기본 공격을 우선시한다.
                                                                                                                   // 슬라임의 기본 공격은 독을 부여한다.
                {
                    //Debug.Log(this.name+"이(가) 당신을 공격합니다.");
                    HpChange(Dungeon.dungeon.Plr, -this.power);  //★Floor에 따라 변경되는 공격력을 변수에 집어넣어서 그 변수만큼만 깎아야 한다
                    condition_add(Dungeon.dungeon.Plr, 3, 2);    //중독 2 부여
                    this.StaminaChange(-20);
                }
                else if (route_pos.Count > 0)
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                }
                isTurn -= 1;
            }
        }

        public override void die() // 슬라임 사망 효과 - 독과 관련있게
                                   // 1. 폭발 - 터지면서 주변에 약한 피해와 독 부여. 근접 계열 카운터형.
                                   // 2. 바닥 - 죽은 위치에 일정 턴동안 유지되는 / 한 번 밟을 때까지 유지되는 독 장판 생성. 위에 올라가면 독 1 부여. 이미 독이 있다면 중첩이 감소되지 않고 갱신됨.
        {

        }
    }
}