using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    public class Gnoll : Enemy
    {
        public GameObject taljin;
        public void Awake() {
            this.maxhp = 90;
            this.maxstamina = 100;
            this.power = 10;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Gnoll";
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
                    exhau_image.transform.position = new Vector3( this.transform.position.x,this.transform.position.y,-1);
                    exhau_image.GetComponent<exhaustController>().live = 120;
                    this.StaminaChange(20);

                }
                else
                {
                    Debug.Log("Plr_pos : "+Plr_pos[0,0]+ " / "+Plr_pos[0,1]);
                    if (Plr_pos[0, 0] != -1)
                    {
                        Thing temp_target = range_attack(Plr_pos[0, 0], Plr_pos[0, 1], false);  //★공격력 10은 임시값이다, Floor에 따라 5/10/15로 증가하는 공격력을 변수에 집어넣어서 그 변수를 공격력 삼아야 한다
                        temp_target.be_hit(power);
                        UI.uicanvas.range_shot(this.gameObject, temp_target.gameObject);
                        this.StaminaChange(-30);
                       
                        
                    }
                    else if (route_pos.Count > 0)
                    {
                        transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                        route_pos.RemoveAt(0);
                        this.StaminaChange(-10);
                    }
                    else //멍때리기
                    {
                        this.StaminaChange(5);
                    }
                }

                this.Turnend();

            }
        }
    }
}
