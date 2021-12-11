using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{

    public class Ali : Enemy
    {
        
        //public GameObject[] warning;
        public GameObject taljin;
        private bool moving = false;
        private int cooldown = 0;
        private int cooldown_c = 0;
        private int cooldown_w = 0;

        public void Awake()
        {
            this.maxhp = 200;
            this.maxstamina = 100;
            this.power = 10;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Ali";
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                HpChange(-70);//자해 테스트
        }
        public new void FixedUpdate()
        {
            if (this.hp <= 0)
                this.die();

            if (isTurn > 0)
            {
                if (this.GetStamina() < 20 && this.exhausted == false)
                {

                    this.exhausted = true;
                }
                else if (this.GetStamina() >= 60 && this.exhausted == true)
                {

                    this.exhausted = false;
                }
                Vision_research();

                if (this.exhausted == true)//탈진 상태에서 스태미나 회복, 일반적으로는 특정 조건 만족 시 탈진에 걸리고, 일정 수치 이상의 스태미나까지 휴식만 한다.
                                           //그렇게 일정 수치까지 회복한 이후, 탈진 상태이상이 제거되고, 기존의 행동 우선도대로 행동을 재개한다.
                {
                    GameObject exhau_image = Instantiate(taljin);//탈진 시 탄진 이펙트 발생
                    exhau_image.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
                    exhau_image.GetComponent<exhaustController>().live = 120;
                    this.StaminaChange(20);

                }
                else if (cooldown_c == 0 & Charge_Check() != -1)
                {
                    Charge_Attack(Charge_Check());
                }

                else
                {

                     if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                    {
                        Dungeon.dungeon.Plr.be_hit(power);
                        Dungeon.dungeon.Plr.condition_add(3, 1);
                        if (Dungeon.dungeon.Plr.GetStamina() <= 10 && cooldown == 0)
                        {
                            Dungeon.dungeon.Plr.condition_add(1, 2);
                            cooldown = 20;
                        }

                        this.StaminaChange(-10);
                    }
                    
                    else if (route_pos.Count > 0)
                    {   
                        if (moving == true)
                        {
                            transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                            route_pos.RemoveAt(0);
                            this.StaminaChange(-10);
                            moving = false;
                        }
                        else
                            moving = true;
                    }
                   
                    else //멍때리기
                    {
                        this.StaminaChange(5);
                    }
                }
                if (cooldown > 0)
                    cooldown--;
                if (cooldown_c > 0)
                    cooldown_c--;
                this.Turnend();

            }
        }
        private int Charge_Check()
        {
            float x1 = this.transform.position.x;
            float y1 = this.transform.position.y;
            float x2 = Dungeon.dungeon.Plr.transform.position.x;
            float y2 = Dungeon.dungeon.Plr.transform.position.y;
            if (Dungeon.distance_cal(this.transform, Dungeon.dungeon.Plr.transform) <= 3)
            {
                if (x1 == x2 && y1 < y2)
                    return 0;
                else if (x1 == x2 && y1 > y2)
                    return 4;
                else if (y1 == y2 && x1 < x2)
                    return 2;
                else if (y1 == y2 && x1 > x2)
                    return 6;
                else if (x1 - x2 == y1 - y2 && x1 < x2)
                    return 1;
                else if (x1 - x2 == y1 - y2 && x1 > x2)
                    return 5;
                else if (x1 - x2 == -(y1 - y2) && x1 < x2)
                    return 3;
                else if (x1 - x2 == -(y1 - y2) && x1 > x2)
                    return 7;
                else
                    return -1;
            }
            else
                return -1;
        }
        private void Charge_Attack(int dir)
        {
            Debug.Log(dir);
            int reach = Dungeon.distance_cal(this.transform, Dungeon.dungeon.Plr.transform)-1;
            
            switch (dir)
            {
                case 0: 
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + reach);
                    break;
                case 1:
                    this.transform.position = new Vector2(this.transform.position.x + reach, this.transform.position.y + reach);
                    break;
                case 2:
                    this.transform.position = new Vector2(this.transform.position.x + reach, this.transform.position.y);
                    break;
                case 3:
                    this.transform.position = new Vector2(this.transform.position.x + reach, this.transform.position.y - reach);
                    break;
                case 4:
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - reach);
                    break;
                case 5:
                    this.transform.position = new Vector2(this.transform.position.x - reach, this.transform.position.y - reach);
                    break;
                case 6:
                    this.transform.position = new Vector2(this.transform.position.x - reach, this.transform.position.y );
                    break;
                case 7:
                    this.transform.position = new Vector2(this.transform.position.x - reach, this.transform.position.y + reach);
                    break;
                default:
                    Debug.LogError("이상함");
                    break;
            }

            Dungeon.dungeon.Plr.be_hit(power);
            cooldown_c = 20;
            Vision_research();
        }
        


    }
}
