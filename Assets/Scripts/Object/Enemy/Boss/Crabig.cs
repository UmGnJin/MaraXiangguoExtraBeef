using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    public class Crabig : Enemy
    {
        public GameObject[] warning;
        public GameObject taljin;
        bool canmove = true;
        int dir = 0;//0 12시 1 3시 2 6시 3 9시 방향
        int combo = 0;
        private int count = 0;
        private int cool = 0;

        public void Awake()
        {
            isboss = true;
            this.maxhp = 150;
            this.maxstamina = 60;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);
            BlockChange(100);
            this.power = 10;
            this.name = "Crabig";
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                HpChange(-70);//자해 테스트
        }
        public new void FixedUpdate()
        {
            if (this.hp <= 0)
            {

                this.die();
            }

            else if (isTurn > 0)
            {
                Debug.Log(canmove);
                if (cool > 0)
                {
                    Debug.Log("ㅁㄴㅇ");
                    cool--;
                    if (cool < 0)
                        cool = 0;
                }
                if(cool == 0 && combo == 0)
                {
                    canmove = true;
                }

                if (this.GetStamina() < 20 && this.exhausted == false)
                    this.exhausted = true;
                else if (this.GetStamina() >= 60 && this.exhausted == true)
                    this.exhausted = false;

                Vision_research();
                if (this.exhausted == true || count>0)// 스태미나 회복 방식. 일반적으로는 특정 조건 만족 시 탈진에 걸리고, 일정 수치 이상의 스태미나까지 휴식만 한다.
                                           // 그렇게 일정 수치까지 회복한 이후, 탈진 상태이상이 제거되고, 기존의 행동 우선도대로 행동을 재개한다.
                {
                    if (count > 0)
                    {
                        count--;
                        if (count < 0)
                            count = 0;
                    }
                    if(count == 0)
                    {
                        cool = 10;
                        combo = 0;
                        this.BlockChange(100);
                        this.StaminaChange(30);

                    }
                    GameObject exhau_image = Instantiate(taljin);//탈진 시 탈진 이펙트 발생
                    exhau_image.transform.position = this.transform.position;
                    exhau_image.GetComponent<exhaustController>().live = 300;
                    this.StaminaChange(20);
                }
                else if (combo == 2)
                {
                    count = 4;
                    diswarn();

                }
                else if(canmove == true && Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 3 & Plr_pos[0, 0] != -1)
                {
                    Normal_Attack();
                    Debug.Log("특공격");
                }
                
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1) 
                {

                    Dungeon.dungeon.Plr.be_hit(power);  //★Floor에 따라 변경되는 공격력을 변수에 집어넣어서 그 변수만큼만 깎아야 한다
                    Debug.Log("공격");
                    this.StaminaChange(-20);
                }
                else if (route_pos.Count > 0)
                {
                    Debug.Log("이동");
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                    this.StaminaChange(-10);
                }
                else
                {
                    this.StaminaChange(5);
                }
                




                this.Turnend();


            }
        }
        private void Charge_Attack()
        {

        }
        private void Normal_Attack()
        {
            
            int pla_x = Plr_pos[1, 0];
            int pla_y = Plr_pos[1, 1];
            float xminus = this.transform.position.x - pla_x;
            float yminus = this.transform.position.y - pla_y;

            if (Mathf.Abs(xminus) > Mathf.Abs(yminus))
            {
                if (xminus > 0)
                    dir = 3;
                else
                    dir = 1;
            }
            else
            {
                if (yminus > 0)
                    dir = 2;
                else
                    dir = 0;
            }
            if(Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1)
                route_pos.RemoveAt(0);
            else if (route_pos.Count > 0)
            {
                transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                route_pos.RemoveAt(0); 
            }
            if(combo == 1)
                diswarn();
            //-----공격의 벽------------------------------------------------------------------------------------------
            switch (dir) {
                case 0:
                    posattack(this.transform.position.x + 1, this.transform.position.y + 1, power);
                    posattack(this.transform.position.x, this.transform.position.y + 1, power);
                    posattack(this.transform.position.x - 1, this.transform.position.y + 1, power);
                    warnattack(this.transform.position.x + 1, this.transform.position.y + 1, 0);
                    warnattack(this.transform.position.x, this.transform.position.y + 1, 1);
                    warnattack(this.transform.position.x - 1, this.transform.position.y + 1, 2);
                    if (combo == 1)
                    {
                        posattack(this.transform.position.x + 1, this.transform.position.y + 2, power);
                        posattack(this.transform.position.x, this.transform.position.y + 2, power);
                        posattack(this.transform.position.x - 1, this.transform.position.y + 2, power);
                        warnattack(this.transform.position.x + 1, this.transform.position.y + 2, 3);
                        warnattack(this.transform.position.x, this.transform.position.y + 2, 4);
                        warnattack(this.transform.position.x - 1, this.transform.position.y + 2, 5);
                    }
                    break;
                case 1:
                    posattack(this.transform.position.x + 1, this.transform.position.y + 1, power);
                    posattack(this.transform.position.x + 1, this.transform.position.y, power);
                    posattack(this.transform.position.x + 1, this.transform.position.y - 1, power);
                    warnattack(this.transform.position.x + 1, this.transform.position.y + 1, 0);
                    warnattack(this.transform.position.x + 1, this.transform.position.y, 1);
                    warnattack(this.transform.position.x + 1, this.transform.position.y - 1, 2);
                    if (combo == 1)
                    {
                        posattack(this.transform.position.x + 2, this.transform.position.y + 1, power);
                        posattack(this.transform.position.x + 2, this.transform.position.y, power);
                        posattack(this.transform.position.x + 2, this.transform.position.y - 1, power);
                        warnattack(this.transform.position.x + 2, this.transform.position.y + 1, 3);
                        warnattack(this.transform.position.x + 2, this.transform.position.y, 4);
                        warnattack(this.transform.position.x + 2, this.transform.position.y - 1, 5);
                    }
                    break;
                case 2:
                    posattack(this.transform.position.x + 1, this.transform.position.y - 1, power);
                    posattack(this.transform.position.x, this.transform.position.y - 1, power);
                    posattack(this.transform.position.x - 1, this.transform.position.y - 1, power);
                    warnattack(this.transform.position.x + 1, this.transform.position.y - 1, 0);
                    warnattack(this.transform.position.x, this.transform.position.y - 1, 1);
                    warnattack(this.transform.position.x - 1, this.transform.position.y - 1, 2);
                    if (combo == 1)
                    {
                        posattack(this.transform.position.x + 1, this.transform.position.y - 2, power);
                        posattack(this.transform.position.x, this.transform.position.y - 2, power);
                        posattack(this.transform.position.x - 1, this.transform.position.y - 2, power);
                        warnattack(this.transform.position.x + 1, this.transform.position.y - 2, 3);
                        warnattack(this.transform.position.x, this.transform.position.y - 2, 4);
                        warnattack(this.transform.position.x - 1, this.transform.position.y - 2, 5);
                    }
                    break;
                case 3:
                    posattack(this.transform.position.x - 1, this.transform.position.y + 1, power);
                    posattack(this.transform.position.x - 1, this.transform.position.y, power);
                    posattack(this.transform.position.x - 1, this.transform.position.y - 1, power);
                    warnattack(this.transform.position.x - 1, this.transform.position.y + 1, 0);
                    warnattack(this.transform.position.x - 1, this.transform.position.y, 1);
                    warnattack(this.transform.position.x - 1, this.transform.position.y - 1, 2);
                    if (combo == 1)
                    {
                        posattack(this.transform.position.x - 2, this.transform.position.y + 1, power);
                        posattack(this.transform.position.x - 2, this.transform.position.y, power);
                        posattack(this.transform.position.x - 2, this.transform.position.y - 1, power);
                        warnattack(this.transform.position.x - 2, this.transform.position.y + 1, 3);
                        warnattack(this.transform.position.x - 2, this.transform.position.y, 4);
                        warnattack(this.transform.position.x - 2, this.transform.position.y - 1, 5);
                    }
                    break;

                default: break;

                }
            if (combo == 0)
                combo = 1;
            else if(combo == 1)
            {
                
                BlockChange(-999999);
                combo = 2;
                canmove = false;
                
            }
        }

        public void warnattack(float x, float y, int num)
        {
            warning[num].SetActive(true);
            warning[num].transform.position = new Vector2(x, y);
        }
        public void diswarn()
        {
            int i;
            for(i=0;i<6;i++)
            warning[i].SetActive(false);
           
        }




    }
}
