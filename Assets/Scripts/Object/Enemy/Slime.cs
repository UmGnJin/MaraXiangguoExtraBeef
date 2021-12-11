using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;
using ArcanaDungeon;


namespace ArcanaDungeon.Object
{
    public class Slime : Enemy
    {
        public GameObject poisontile;
        public GameObject taljin;
        public void Awake()
        {
            this.maxhp = 115;
            this.maxstamina = 100;
            this.power = 1;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "슬라임";
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
                if (this.GetStamina() < 20 && this.exhausted == false)
                    this.exhausted = true;
                else if (this.GetStamina() >= 60 && this.exhausted == true)
                    this.exhausted = false;

                Vision_research();
                if(this.exhausted == true)// 스태미나 회복 방식. 일반적으로는 특정 조건 만족 시 탈진에 걸리고, 일정 수치 이상의 스태미나까지 휴식만 한다.
                                          // 그렇게 일정 수치까지 회복한 이후, 탈진 상태이상이 제거되고, 기존의 행동 우선도대로 행동을 재개한다.
                {
                    GameObject exhau_image = Instantiate(taljin);//탈진 시 탄진 이펙트 발생
                    exhau_image.transform.position = this.transform.position;
                    exhau_image.GetComponent<exhaustController>().live = 120;
                    this.StaminaChange(20);
                }
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// 공격 거리 내에 플레이어가 존재 시, 기본 공격을 우선시한다.
                                                                                                                   // 슬라임의 기본 공격은 독을 부여한다.
                {
                    //Debug.Log(this.name+"이(가) 당신을 공격합니다.");
                    Dungeon.dungeon.Plr.be_hit(this.power);
                    Dungeon.dungeon.Plr.condition_add(3, 2);    //중독 2 부여
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





                this.Turnend();


            }
        }

        public override void die() // 슬라임 사망 효과 - 독과 관련있게
                                   // 1. 폭발 - 터지면서 주변에 약한 피해와 독 부여. 근접 계열 카운터형.
                                   // 2. 바닥 - 죽은 위치에 일정 턴동안 유지되는 / 한 번 밟을 때까지 유지되는 독 장판 생성. 위에 올라가면 독 1 부여. 이미 독이 있다면 중첩이 감소되지 않고 갱신됨.
        {
            Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1].Remove(this.gameObject);
            GameObject PoisonTile = Instantiate(poisontile);
            PoisonTile.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);

            Destroy(this.gameObject);


        }
    }
}