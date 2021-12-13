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

        protected int[,] Plr_pos = new int[2, 2];  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ
        public bool[,] FOV;

        private GameObject hp_background;
        private GameObject hp_fillarea;
        private GameObject st_background;
        private GameObject st_fillarea;

        public bool isboss = false;


        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //���⿡ �ʵ� ����
            e.isTurn = this.isTurn;

            return e;
        }

        public void Initiate() {    //���� ������, ��¥ �����ڷ� ���� Dungeon�� �����Ǳ⵵ ���� ����Ǽ� square�� ������ �� ���� Awake�� �����ϸ� Dungeon�� Awake�� ����Ǳ� ���� player�� Vision_marker�� ����ż� ��������
            //transform.position = pos;

            hp_background = Instantiate(Dungeon.dungeon.square) as GameObject;  //ü�¹� ����
            hp_fillarea = Instantiate(Dungeon.dungeon.square) as GameObject;
            hp_background.transform.SetParent(this.transform);  //ü�¹ٸ� �� ������ �ڽ����� ����
            hp_fillarea.transform.SetParent(this.transform);
            hp_background.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0f, 0f, 1f);    //ü�¹� ��ĥ
            hp_fillarea.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
            hp_background.transform.localPosition = new Vector3(0, 0.6f, -0.1f);    //ü�¹� ��ġ ����
            hp_fillarea.transform.localPosition = new Vector3(0, 0.6f, -0.2f);

            st_background = Instantiate(Dungeon.dungeon.square) as GameObject;  //ü�¹� ����
            st_fillarea = Instantiate(Dungeon.dungeon.square) as GameObject;
            st_background.transform.SetParent(this.transform);  //ü�¹ٸ� �� ������ �ڽ����� ����
            st_fillarea.transform.SetParent(this.transform);
            st_background.GetComponent<SpriteRenderer>().color = new Color(0f, 0.2f, 0f, 1f);    //ü�¹� ��ĥ
            st_fillarea.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 1f);
            st_background.transform.localPosition = new Vector3(0, 0.55f, -0.01f);    //ü�¹� ��ġ ����
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

        public void status_update() {    //ü�¹�, ���¹̳�, �����̻� �� ���� ���� ǥ�õǴ� UI���� ���Ű� ���̰� �ϴ� ����� �ô´�
            //ü�¹� ����
            hp_background.SetActive(true);
            hp_fillarea.SetActive(true);
            hp_fillarea.transform.localScale = new Vector2((this.hp > 0 ? this.hp : 0) / (float)this.maxhp, 1);
            hp_fillarea.transform.localPosition = new Vector3(-0.5f * (this.hp > 0 ? 1f - (this.hp / (float)this.maxhp) : 1), 0.6f, -0.2f);
            //���¹̳��� ����
            st_background.SetActive(true);
            st_fillarea.SetActive(true);
            st_fillarea.transform.localScale = new Vector2((this.stamina > 0 ? this.stamina : 0) / (float)this.maxstamina, 1);
            st_fillarea.transform.localPosition = new Vector3(-0.5f * (this.stamina > 0 ? 1f - (this.stamina / (float)this.maxstamina) : 1), 0.55f, -0.2f);
        }
        public void status_hide() {     //ü�¹�, ���¹̳�, �����̻� �� ���� ���� ǥ�õǴ� UI���� �����
            hp_background.SetActive(false);
            hp_fillarea.SetActive(false);
            st_background.SetActive(false);
            st_fillarea.SetActive(false);
        }

        protected void Vision_research()
        {
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), this.vision_distance, FOV);

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
            UI.uicanvas.log_add(this.name + "��(��) ���������ϴ�.");
            if (isboss)
            {
                if (Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1].Count == 0)
                {
                    UI.uicanvas.log_add("������ ���� ����� ȸ����ŵ�ϴ�.");
                    Dungeon.dungeon.Plr.HpChange(50);
                    Dungeon.dungeon.currentlevel.locked = false;//���� ��� ���������� ���� ��
                }
            }
            UI.uicanvas.Treasure(Dungeon.dungeon.Plr.allDeck.getRandomCardcode());
            Destroy(this.gameObject);
        }

        public override void Spawn() { }
    }
}
