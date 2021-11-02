using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;
using System;

namespace ArcanaDungeon.Object
{
    public class SlimeColony : Enemy
    {
        GameObject slime;
        Transform throw_coordinate;

        public void Awake()
        {
            this.maxhp = 300;
            this.maxstamina = 100;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.maxcooltime = 5;  //★임시 쿨타임
            this.cooltime = 0;

            this.name = "슬라임 군집";
            slime = Dungeon.dungeon.Mobs[Array.FindIndex(Dungeon.dungeon.Mobs, m => m.name == "Slime")];
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

                if (this.exhausted == true)// 스태미나 회복 방식. 일반적으로는 특정 조건 만족 시 탈진에 걸리고, 일정 수치 이상의 스태미나까지 휴식만 한다.
                                           // 그렇게 일정 수치까지 회복한 이후, 탈진 상태이상이 제거되고, 기존의 행동 우선도대로 행동을 재개한다.
                {
                    this.StaminaChange(20);
                }
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// 플레이어가 자신의 공격 범위 내에 있으면, 기본 공격을 가장 우선시한다.
                {
                    Debug.Log(this.name + "이(가) 당신을 공격합니다.");
                    Dungeon.dungeon.Plr.HpChange(-10);  //★Floor에 따라 변경되는 공격력을 변수에 집어넣어서 그 변수만큼만 깎아야 한다
                    this.StaminaChange(-20);
                }
                else if(Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) > 1 & Plr_pos[0, 0] != -1 & this.cooltime <= 0) //근접공격 안되고, 시야 내에 있고, 쿨다운 중이 아닐 경우 슬라임 소환
                {
                    throw_coordinate = Dungeon.dungeon.Plr.transform;
                    ThrowSlime(throw_coordinate.position);
                    //Dungeon.dungeon.currentlevel.temp_gameobjects[(int)throw_coordinate.position.x, (int)throw_coordinate.position.y].GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1); 이러면 플레이어턴에 시야 처리하면서 다시 되돌아온다
                    this.cooltime = this.maxcooltime;
                    this.StaminaChange(-30);
                }
                else if (route_pos.Count > 0)// 슬라임 군집은 특성상 다음 층으로 가는 길을 막고 있어야 하므로 움직이지 않는다.
                {
                    //transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    //route_pos.RemoveAt(0);
                }
                this.cooltime -= 1;
                isTurn -= 1;
            }
        }


        public void ThrowSlime(Vector2 pos) // 플레이어가 공격 범위 내에 없고 시야 범위 내에 있을 때, 현재 플레이어 위치에 그림자 표시. 다음 턴에 해당 위치에 공격 판정 및 슬라임 생성.
                                 // 플레이어 위치에 그림자 이펙트 생성. 그림자 이펙트는 다음 턴에 해당 타일에 플레이어가 있으면 피해를 주고, 근처 빈 타일에 슬라임을 스폰한다.
                                 // 슬라임 콜로니는 이 행동을 사용 시 현재 체력의 n%, 혹은 일정 체력량을 소모하고 그만큼의 체력을 가진 슬라임을 던진다.
        {
            GameObject thrown = Instantiate(slime, pos, Quaternion.identity);
            int health = (int)(this.hp * 0.1);
            if (health < 1)
                health = 1;

            thrown.GetComponent<Slime>().maxhp = (health);
            //Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor].Add(thrown);
        }

        public void HpChange(int val)
        {
            if (val > 0)
            {
                if (this.hp + val > this.maxhp)
                {
                    this.hp = this.maxhp;
                }
                else
                {
                    this.hp += val;
                }
            }
            else if (val != 0)
            {
                this.hp -= val;
                if (val > -15)//15 이하의 피해를 받으면, 받은 피해량의 절반을 체력으로 가지는 슬라임을 소환한다.
                {
                    Debug.Log("슬라임 군집에서 일부가 떨어져 나옵니다.");
                    Vector2 pos = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                    GameObject divided = Instantiate(slime, pos, Quaternion.identity);
                    divided.GetComponent<Slime>().maxhp = (int)(val / 2);
                    Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor].Add(divided);
                    //여기에 슬라임 소환
                }
                else
                    Debug.Log("슬라임이 떨어져 나오지만, 이내 힘없이 흩어집니다.");
                if (this.hp < 0)
                {
                    this.die();
                }

            }
        }
    }
}