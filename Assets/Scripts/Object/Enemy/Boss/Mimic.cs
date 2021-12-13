using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;
using ArcanaDungeon;
using Random = System.Random;


namespace ArcanaDungeon.Object
{
    public enum Bosang
    {
        KEY = 0,
        GGWANG = 1,
        BOMUL = 2

    }

        public class Mimic : Enemy
        {
        bool Activate = true;
        public Bosang bosang;
        public GameObject taljin;
        public static bool[] type = { false, false, false};
            public static Random rand = new Random();
        public void Awake()
        {
            isboss = true;
            this.maxhp = 115;
            this.maxstamina = 100;
            this.power = 1;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);
            int index = rand.Next(0,3);
            while (type[index])
            {
                 index = rand.Next(0, 3);
            }
            this.bosang = (Bosang)index;
            type[index] = true;
            this.name = "미믹";
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                HpChange(-70);//자해 테스트
        }
        public new void FixedUpdate()
        {
            if (this.hp < this.maxhp)
            {
                Debug.Log(Activate);
                Activate = true;
            }
            if (this.hp <= 0)
            {

                this.die();
            }
            
            else if (isTurn > 0)
            {
                    if (Activate == true)
                    {
                        if (this.GetStamina() < 20 && this.exhausted == false)
                            this.exhausted = true;
                        else if (this.GetStamina() >= 60 && this.exhausted == true)
                            this.exhausted = false;

                        Vision_research();
                        if (this.exhausted == true)// 스태미나 회복 방식. 일반적으로는 특정 조건 만족 시 탈진에 걸리고, 일정 수치 이상의 스태미나까지 휴식만 한다.
                                                   // 그렇게 일정 수치까지 회복한 이후, 탈진 상태이상이 제거되고, 기존의 행동 우선도대로 행동을 재개한다.
                        {
                            GameObject exhau_image = Instantiate(taljin);//탈진 시 탄진 이펙트 발생
                            exhau_image.transform.position = this.transform.position;
                            exhau_image.GetComponent<exhaustController>().live = 120;
                            this.StaminaChange(20);
                        }
                        else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)                  {
                        //Debug.Log(this.name+"이(가) 당신을 공격합니다.");

                        Dungeon.dungeon.Plr.condition_add(0, 2);    //발화 2 부여
                            this.StaminaChange(-20);
                        }
                        else if (route_pos.Count > 0)
                        {
                            transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                            route_pos.RemoveAt(0);
                            this.StaminaChange(-10);
                        }
                        else
                        {
                            this.StaminaChange(5);
                        }



                    }

                this.Turnend();


            }
        }

        public override void die() 
        {
                loot();
            Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1].Remove(this.gameObject);
            
            if (Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1].Count == 0)
            {
                UI.uicanvas.log_add("Boss Clear!");
                Dungeon.dungeon.currentlevel.locked = false;//보스 잡고 다음층으로 가게 함
            }
               
            Destroy(this.gameObject);


        }
            public void loot()
            {
                switch (bosang)
                {
                    case Bosang.KEY:
                        Debug.Log("희귀 열쇠 카드!");
                        break;
                    case Bosang.GGWANG:
                        Debug.Log("일반 꽝 카드.");
                        break;
                    case Bosang.BOMUL:
                        Debug.Log("황금 보물 카드!!!!!!");
                        break;

                }
            }
    }
}

