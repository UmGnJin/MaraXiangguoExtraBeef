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

        bool isawaken;  //★이 함수가 정말 필요한지는 차차 생각해보자

        protected int[,] Plr_pos = new int[2, 2];  //0번 인덱스는 실제 플레이어 위치, 1번 인덱스는 마지막으로 본 플레이어 위치

        public bool[,] FOV;

        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //여기에 필드 복사
            e.isTurn = this.isTurn;

            return e;
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

        public override void Spawn()
        {

        }
        public void Spawn(Vector2 pos)
        {
            transform.position = pos;
            Vision_research();
        }

        public override void turn()
        {

        }

        protected void Vision_research()
        {
            Debug.Log("실행 중");
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);

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

            //★몬스터의 시야 범위를 파랑색으로, 몬스터의 cur_pos는 녹색으로 나타낸다, 당연히 몬스터의 시야 범위를 보여줄 필요가 없으므로 나중에 삭제할 것
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    if (FOV[i, j])
                    {
                        Dungeon.dungeon.currentlevel.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1, 1);
                    }
                    else {
                        Dungeon.dungeon.currentlevel.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                    }
                }
            }
            Dungeon.dungeon.currentlevel.temp_gameobjects[(int)transform.position.x, (int)transform.position.y].GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 1);
            //★몬스터에서 플레이어에게로 가는 경로를 붉게 표시한다, 나중에 몬스터의 행동이 코딩되면 삭제할 것
            foreach (int ii in route_pos)
            {
                Dungeon.dungeon.currentlevel.temp_gameobjects[ii % Dungeon.dungeon.currentlevel.width, ii / Dungeon.dungeon.currentlevel.width].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
            }
        }

        protected void range_attack(int dest_x, int dest_y, int val, bool friendly_fire)
        { //원거리 공격, pierce는 공격 범위 안의 모든 적을 공격하는 관통 공격일 경우 true, friendly_fire는 이 공격으로 아군도 공격 가능할 경우 true(보통 1턴 충전 뒤 지정한 위치로 발사하는 스킬에 사용될 예정)
            Debug.Log("몬스터 원거리 공격 실행 중");
            //이 몬스터의 현재 좌표부터 (dest_x,dest_y)까지 맞닿는 사각형 좌표들을 구해옴
            List<float[]> result = new List<float[]>();
            //목표 지점과 몬스터 사이가 수평 일직선이라면 x좌표만 바꿔가며 result에 저장
            if (dest_y - transform.position.y == 0)
            {
                int temp_var = dest_x - transform.position.x > 0 ? 1 : -1;
                for (float i = transform.position.x; i*temp_var <= dest_x*temp_var; i+=temp_var)
                {
                    result.Add(new float[2] { i, dest_y });
                }
                //목표 지점과 몬스터 사이가 수직 일직선이라면 y좌표만 바꿔가며 result에 저장
            }
            else if (dest_x - transform.position.x == 0)
            {
                int temp_var = dest_y - transform.position.y > 0 ? 1 : -1;
                for (float i = transform.position.y; i*temp_var <= dest_y*temp_var; i+=temp_var)
                {
                    result.Add(new float[2] { dest_x, i });
                }
                //일직선이 아니라면 
            }
            else
            {
                bool mirrored = false;
                float more_slope, less_slope, slope;
                int less_chn = dest_x - transform.position.x > 0 ? 1 : -1;
                int more_chn = dest_y - transform.position.y > 0 ? 1 : -1;
                //발사 지점에서 목표 지점까지 이동할 때 x와 y 좌표 중 더 많이 변하는 좌표축의 현재 좌표를 more_slope, 더 적게 변하는 좌표축의 현재 좌표를 less_slope에 저장하고 더 많이 변하는 좌표의 변화량/더 적게 변하는 좌표의 변화량을 slope에 저장
                if (Math.Abs(dest_x - transform.position.x) > Math.Abs(dest_y - transform.position.y))
                {
                    more_slope = transform.position.x;
                    less_slope = transform.position.y;
                    slope = Math.Abs(dest_x - transform.position.x) / Math.Abs(dest_y - transform.position.y);
                    less_chn += more_chn; more_chn = less_chn - more_chn; less_chn -= more_chn; //두 변수의 값을 맞바꾼다, 세상은 넓고 나보다 똑똑한 사람은 많다
                    mirrored = true;
                }
                else
                {
                    more_slope = transform.position.y;
                    less_slope = transform.position.x;
                    slope = Math.Abs(dest_y - transform.position.y) / Math.Abs(dest_x - transform.position.x);
                }

                Debug.Log(slope % 1);
                //유니티 좌표 체계는 프리팹의 중앙을 기준으로 하기 때문에, less_slope가 0.5 변화하는 동안의 좌표들을 가장 먼저 저장
                result.Add(new float[2] { more_slope, less_slope }); //for (float i = more_slope; more_slope * more_chn < (i + (slope / 2) * more_chn + more_chn) * more_chn; more_slope += more_chn) { result.Add(new float[2] { more_slope, less_slope }); }
                less_slope += less_chn;
                if (slope % 2 == 0) { more_slope += more_chn; } //★less_slope가 변경될 때 more_slope가 정수가 아니면 -more_chn을 해줘야 한다.. 아니면 more_slope가 정수일 때 +more_chn하던가
                if (mirrored)
                {
                    while (less_slope * less_chn < dest_y * less_chn)  // | more_slope + slope <= dest_x 
                    {
                        for (float i = more_slope; more_slope * more_chn < (i + slope * more_chn) * more_chn; more_slope += more_chn) { result.Add(new float[2] { more_slope, less_slope }); } // - 0.5f 
                        less_slope += less_chn;
                    }
                }
                else
                {
                    while (less_slope * less_chn <= dest_x * less_chn)  // | more_slope + slope <= dest_y
                    {
                        for (float i = more_slope; more_slope * more_chn < (i + slope * more_chn) * more_chn; more_slope += more_chn) { result.Add(new float[2] { less_slope, more_slope }); } // - 0.5f
                        less_slope += less_chn;
                    }
                }
                //유니티 좌표 체계는 프리팹의 중앙을 기준으로 하기 때문에, less_slope가 0.5 변화하는 동안의 좌표들을 마지막으로 저장
                if (slope % 1 == 0) { more_slope += more_chn; }
                result.Add(new float[2] { more_slope, less_slope });    //for (float i = more_slope; more_slope * more_slope < (i + (slope / 2) * more_chn + more_chn) * more_chn; more_slope += more_chn) { result.Add(new float[2] { more_slope, less_slope }); }

                //가장 가까운 대상 찾아서 피해를 준다, 다만 friendly_fire가 false라면 자기편(몬스터)은 안 때린다
                Thing closest = null;
                int closest_distance = 999;
                foreach (float[] r in result)
                {
                    Debug.Log("result : " + r[0] + " , " + r[1]);
                    foreach (GameObject t in Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1])
                    {
                        Debug.Log("mob : " + t.transform.position.x + " , " + t.transform.position.y + " // " + (t != this.gameObject));
                        if (t != this.gameObject & r[0] == t.transform.position.x & r[1] == t.transform.position.y & Dungeon.distance_cal(transform, t.transform) < closest_distance)
                        {
                            closest = t.GetComponent<Enemy>();
                            closest_distance = Dungeon.distance_cal(transform, t.transform);
                        }
                    }
                    Debug.Log(Dungeon.dungeon.Plr.transform.position);
                    if (r[0] == Dungeon.dungeon.Plr.transform.position.x & r[1] == Dungeon.dungeon.Plr.transform.position.y & Dungeon.distance_cal(transform, Dungeon.dungeon.Plr.transform) < closest_distance)
                    {
                        closest = Dungeon.dungeon.Plr;
                        closest_distance = Dungeon.distance_cal(transform, Dungeon.dungeon.Plr.transform);
                    }
                }
                if (closest == Dungeon.dungeon.Plr | friendly_fire)
                {
                    closest.HpChange(-val);
                    dest_x = (int)closest.transform.position.x;
                    dest_y = (int)closest.transform.position.y;

                    Dungeon.dungeon.GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
                    Dungeon.dungeon.GetComponent<LineRenderer>().SetPosition(1, new Vector3(dest_x, dest_y, -1));
                    Dungeon.dungeon.GetComponent<LineRenderer>().SetColors(new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 1f));
                }
                else
                {
                    //이동
                }
            }
        }

        public override void die()
        {

        }
    }
}
