using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain = ArcanaDungeon.Terrain;
using ArcanaDungeon;
using ArcanaDungeon.util;

namespace ArcanaDungeon.Object
{
    public class Enemy : Thing
    {
        protected int cooltime;
        public int maxcooltime;

        protected int[,] Plr_pos = new int[2, 2];  //0번 인덱스는 실제 플레이어 위치, 1번 인덱스는 마지막으로 본 플레이어 위치
        public bool[,] FOV;

        private GameObject hp_background;
        private GameObject hp_fillarea;
        private GameObject st_background;
        private GameObject st_fillarea;

        public bool isboss = false;


        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //여기에 필드 복사
            e.isTurn = this.isTurn;

            return e;
        }

        public void Initiate() {    //몬스터 생성자, 진짜 생성자로 쓰면 Dungeon이 생성되기도 전에 실행되서 square을 가져올 수 없고 Awake로 실행하면 Dungeon의 Awake가 종료되기 전에 player의 Vision_marker가 실행돼서 오류난다
            //transform.position = pos;

            hp_background = Instantiate(Dungeon.dungeon.square) as GameObject;  //체력바 생성
            hp_fillarea = Instantiate(Dungeon.dungeon.square) as GameObject;
            hp_background.transform.SetParent(this.transform);  //체력바를 이 몬스터의 자식으로 설정
            hp_fillarea.transform.SetParent(this.transform);
            hp_background.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0f, 0f, 1f);    //체력바 색칠
            hp_fillarea.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
            hp_background.transform.localPosition = new Vector3(0, 0.6f, -0.1f);    //체력바 위치 조정
            hp_fillarea.transform.localPosition = new Vector3(0, 0.6f, -0.2f);

            st_background = Instantiate(Dungeon.dungeon.square) as GameObject;  //체력바 생성
            st_fillarea = Instantiate(Dungeon.dungeon.square) as GameObject;
            st_background.transform.SetParent(this.transform);  //체력바를 이 몬스터의 자식으로 설정
            st_fillarea.transform.SetParent(this.transform);
            st_background.GetComponent<SpriteRenderer>().color = new Color(0f, 0.2f, 0f, 1f);    //체력바 색칠
            st_fillarea.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 1f);
            st_background.transform.localPosition = new Vector3(0, 0.55f, -0.01f);    //체력바 위치 조정
            st_fillarea.transform.localPosition = new Vector3(0, 0.55f, -0.02f);

            this.status_update();
        }

        public void FixedUpdate()
        {

            if (isTurn > 0)
            {
                Vision_research();
                if (this.GetStamina() < 20 && this.exhausted == false)
                    this.exhausted = true;
                else if (this.GetStamina() >= 60 && this.exhausted == true)
                    this.exhausted = false;


                if (this.exhausted == true)// 스태미나 회복 방식. 특정 행동을 하려 할 때 그 행동에 요구되는 스태미나가 없다면 행동을 할 수 없는 채로 몇 턴 간 스태미나를 회복한다, 탈진 턴 수는 스태미나 최대치에 비례하며 회복 이후 탈진이 해제되고 원래대로 행동한다
                                           // 최대 스태미나 0~40 > 탈진 1턴, 최대 스태미나 41~80 > 탈진 2턴, 최대 스태미나 81~120 > 탈진 3턴....
                {
                    this.StaminaChange(20);
                }
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// 공격 거리 내에 플레이어가 존재 시, 기본 공격을 우선시한다.
                {
                    //Debug.Log(this.name+"이(가) 당신을 공격합니다.");
                    //여기에 공격내용 입력
                    //this.StaminaChange(-20);
                }
                else if (route_pos.Count > 0) // 플레이어 추적
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);

                }
                isTurn -= 1;
            }

            
        }

        public void status_update() {    //체력바, 스태미나, 상태이상 등 몬스터 위에 표시되는 UI들의 갱신과 보이게 하는 기능을 맡는다
            //체력바 갱신
            hp_background.SetActive(true);
            hp_fillarea.SetActive(true);
            hp_fillarea.transform.localScale = new Vector2((this.hp > 0 ? this.hp : 0) / (float)this.maxhp, 1);
            hp_fillarea.transform.localPosition = new Vector3(-0.5f * (this.hp > 0 ? 1f - (this.hp / (float)this.maxhp) : 1), 0.6f, -0.2f);
            //스태미나바 갱신
            st_background.SetActive(true);
            st_fillarea.SetActive(true);
            st_fillarea.transform.localScale = new Vector2((this.stamina > 0 ? this.stamina : 0) / (float)this.maxstamina, 1);
            st_fillarea.transform.localPosition = new Vector3(-0.5f * (this.stamina > 0 ? 1f - (this.stamina / (float)this.maxstamina) : 1), 0.55f, -0.2f);
        }
        public void status_hide() {     //체력바, 스태미나, 상태이상 등 몬스터 위에 표시되는 UI들을 숨긴다
            hp_background.SetActive(false);
            hp_fillarea.SetActive(false);
            st_background.SetActive(false);
            st_fillarea.SetActive(false);
        }

        protected void Vision_research()
        {
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), this.vision_distance, FOV);

            Plr_pos[0, 0] = -1; //Plr_pos[0,0]에 -1을 넣어두고 플레이어를 포착하면 그 좌표로 변경한다, 만약 플레이어를 포착하지 못 하면 -1인 채로 남기 때문에 확인할 수 있다
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    //시야에 보이는 위치인데, 플레이어랑 좌표까지 같으면 거기에 있는 게 플레이어니까 Plr_pos에 저장한다
                    if (FOV[i, j] & (i == Dungeon.dungeon.Plr.transform.position.x) & (j == Dungeon.dungeon.Plr.transform.position.y))
                    {
                        Plr_pos[0, 0] = Plr_pos[1, 0] = (int)Dungeon.dungeon.Plr.transform.position.x;
                        Plr_pos[0, 1] = Plr_pos[1, 1] = (int)Dungeon.dungeon.Plr.transform.position.y;
                        break;
                    }
                }
            }

            if (Plr_pos[0, 0] != -1)
            {
                route_BFS(Plr_pos[0, 0], Plr_pos[0, 1]);
            }

        }
        public void posattack(float x, float y, int dmg)
        {
            if (Plr_pos[0, 0] == x && Plr_pos[0, 1] == y)
            {
                Dungeon.dungeon.Plr.be_hit(dmg);
            }
        }
        
        public override void die()
        {
            Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1].Remove(this.gameObject);
            UI.uicanvas.log_add(this.name + "이(가) 쓰러졌습니다.");
            if (isboss)
            {
                if (Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1].Count == 0)
                {
                    UI.uicanvas.log_add("괴물의 힘이 당신을 회복시킵니다.");
                    Dungeon.dungeon.Plr.HpChange(50);
                    Dungeon.dungeon.currentlevel.locked = false;//보스 잡고 다음층으로 가게 함
                }
            }
            UI.uicanvas.Treasure(Dungeon.dungeon.Plr.allDeck.getRandomCardcode());
            Destroy(this.gameObject);
        }

        public override void Spawn() { }
    }
}
