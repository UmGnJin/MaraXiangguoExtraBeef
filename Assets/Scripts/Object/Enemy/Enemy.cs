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

        bool isawaken;  //���� �Լ��� ���� �ʿ������� ���� �����غ���

        protected int[,] Plr_pos = new int[2, 2];  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ

        public bool[,] FOV;

        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //���⿡ �ʵ� ����
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
                if (this.exhausted == true)// ���¹̳� ȸ�� ���. Ư�� �ൿ�� �Ϸ� �� �� �� �ൿ�� �䱸�Ǵ� ���¹̳��� ���ٸ� �ൿ�� �� �� ���� ä�� �� �� �� ���¹̳��� ȸ���Ѵ�, Ż�� �� ���� ���¹̳� �ִ�ġ�� ����ϸ� ȸ�� ���� Ż���� �����ǰ� ������� �ൿ�Ѵ�
                                           // �ִ� ���¹̳� 0~40 > Ż�� 1��, �ִ� ���¹̳� 41~80 > Ż�� 2��, �ִ� ���¹̳� 81~120 > Ż�� 3��....
                {
                    this.StaminaChange(20);
                }
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// ���� �Ÿ� ���� �÷��̾ ���� ��, �⺻ ������ �켱���Ѵ�.
                {
                    //Debug.Log(this.name+"��(��) ����� �����մϴ�.");
                    //���⿡ ���ݳ��� �Է�
                    //this.StaminaChange(-20);
                }
                else if (route_pos.Count > 0) // �÷��̾� ����
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
            Debug.Log("���� ��");
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);

            Plr_pos[0, 0] = -1; //Plr_pos[0,0]�� -1�� �־�ΰ� �÷��̾ �����ϸ� �� ��ǥ�� �����Ѵ�, ���� �÷��̾ �������� �� �ϸ� -1�� ä�� ���� ������ Ȯ���� �� �ִ�
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    //�þ߿� ���̴� ��ġ�ε�, �÷��̾�� ��ǥ���� ������ �ű⿡ �ִ� �� �÷��̾�ϱ� Plr_pos�� �����Ѵ�
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

            //�ڸ����� �þ� ������ �Ķ�������, ������ cur_pos�� ������� ��Ÿ����, �翬�� ������ �þ� ������ ������ �ʿ䰡 �����Ƿ� ���߿� ������ ��
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
            //�ڸ��Ϳ��� �÷��̾�Է� ���� ��θ� �Ӱ� ǥ���Ѵ�, ���߿� ������ �ൿ�� �ڵ��Ǹ� ������ ��
            foreach (int ii in route_pos)
            {
                Dungeon.dungeon.currentlevel.temp_gameobjects[ii % Dungeon.dungeon.currentlevel.width, ii / Dungeon.dungeon.currentlevel.width].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
            }
        }

        protected void range_attack(int dest_x, int dest_y, int val, bool friendly_fire)
        { //���Ÿ� ����, pierce�� ���� ���� ���� ��� ���� �����ϴ� ���� ������ ��� true, friendly_fire�� �� �������� �Ʊ��� ���� ������ ��� true(���� 1�� ���� �� ������ ��ġ�� �߻��ϴ� ��ų�� ���� ����)
            Debug.Log("���� ���Ÿ� ���� ���� ��");
            //�� ������ ���� ��ǥ���� (dest_x,dest_y)���� �´�� �簢�� ��ǥ���� ���ؿ�
            List<float[]> result = new List<float[]>();
            //��ǥ ������ ���� ���̰� ���� �������̶�� x��ǥ�� �ٲ㰡�� result�� ����
            if (dest_y - transform.position.y == 0)
            {
                int temp_var = dest_x - transform.position.x > 0 ? 1 : -1;
                for (float i = transform.position.x; i*temp_var <= dest_x*temp_var; i+=temp_var)
                {
                    result.Add(new float[2] { i, dest_y });
                }
                //��ǥ ������ ���� ���̰� ���� �������̶�� y��ǥ�� �ٲ㰡�� result�� ����
            }
            else if (dest_x - transform.position.x == 0)
            {
                int temp_var = dest_y - transform.position.y > 0 ? 1 : -1;
                for (float i = transform.position.y; i*temp_var <= dest_y*temp_var; i+=temp_var)
                {
                    result.Add(new float[2] { dest_x, i });
                }
                //�������� �ƴ϶�� 
            }
            else
            {
                bool mirrored = false;
                float more_slope, less_slope, slope;
                int less_chn = dest_x - transform.position.x > 0 ? 1 : -1;
                int more_chn = dest_y - transform.position.y > 0 ? 1 : -1;
                //�߻� �������� ��ǥ �������� �̵��� �� x�� y ��ǥ �� �� ���� ���ϴ� ��ǥ���� ���� ��ǥ�� more_slope, �� ���� ���ϴ� ��ǥ���� ���� ��ǥ�� less_slope�� �����ϰ� �� ���� ���ϴ� ��ǥ�� ��ȭ��/�� ���� ���ϴ� ��ǥ�� ��ȭ���� slope�� ����
                if (Math.Abs(dest_x - transform.position.x) > Math.Abs(dest_y - transform.position.y))
                {
                    more_slope = transform.position.x;
                    less_slope = transform.position.y;
                    slope = Math.Abs(dest_x - transform.position.x) / Math.Abs(dest_y - transform.position.y);
                    less_chn += more_chn; more_chn = less_chn - more_chn; less_chn -= more_chn; //�� ������ ���� �¹ٲ۴�, ������ �а� ������ �ȶ��� ����� ����
                    mirrored = true;
                }
                else
                {
                    more_slope = transform.position.y;
                    less_slope = transform.position.x;
                    slope = Math.Abs(dest_y - transform.position.y) / Math.Abs(dest_x - transform.position.x);
                }

                Debug.Log(slope % 1);
                //����Ƽ ��ǥ ü��� �������� �߾��� �������� �ϱ� ������, less_slope�� 0.5 ��ȭ�ϴ� ������ ��ǥ���� ���� ���� ����
                result.Add(new float[2] { more_slope, less_slope }); //for (float i = more_slope; more_slope * more_chn < (i + (slope / 2) * more_chn + more_chn) * more_chn; more_slope += more_chn) { result.Add(new float[2] { more_slope, less_slope }); }
                less_slope += less_chn;
                if (slope % 2 == 0) { more_slope += more_chn; } //��less_slope�� ����� �� more_slope�� ������ �ƴϸ� -more_chn�� ����� �Ѵ�.. �ƴϸ� more_slope�� ������ �� +more_chn�ϴ���
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
                //����Ƽ ��ǥ ü��� �������� �߾��� �������� �ϱ� ������, less_slope�� 0.5 ��ȭ�ϴ� ������ ��ǥ���� ���������� ����
                if (slope % 1 == 0) { more_slope += more_chn; }
                result.Add(new float[2] { more_slope, less_slope });    //for (float i = more_slope; more_slope * more_slope < (i + (slope / 2) * more_chn + more_chn) * more_chn; more_slope += more_chn) { result.Add(new float[2] { more_slope, less_slope }); }

                //���� ����� ��� ã�Ƽ� ���ظ� �ش�, �ٸ� friendly_fire�� false��� �ڱ���(����)�� �� ������
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
                    //�̵�
                }
            }
        }

        public override void die()
        {

        }
    }
}
