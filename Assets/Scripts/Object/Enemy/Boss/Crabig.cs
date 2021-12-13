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
        int dir = 0;//0 12�� 1 3�� 2 6�� 3 9�� ����
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
                HpChange(-70);//���� �׽�Ʈ
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
                    Debug.Log("������");
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
                if (this.exhausted == true || count>0)// ���¹̳� ȸ�� ���. �Ϲ������δ� Ư�� ���� ���� �� Ż���� �ɸ���, ���� ��ġ �̻��� ���¹̳����� �޽ĸ� �Ѵ�.
                                           // �׷��� ���� ��ġ���� ȸ���� ����, Ż�� �����̻��� ���ŵǰ�, ������ �ൿ �켱����� �ൿ�� �簳�Ѵ�.
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
                    GameObject exhau_image = Instantiate(taljin);//Ż�� �� Ż�� ����Ʈ �߻�
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
                    Debug.Log("Ư����");
                }
                
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1) 
                {

                    Dungeon.dungeon.Plr.be_hit(power);  //��Floor�� ���� ����Ǵ� ���ݷ��� ������ ����־ �� ������ŭ�� ��ƾ� �Ѵ�
                    Debug.Log("����");
                    this.StaminaChange(-20);
                }
                else if (route_pos.Count > 0)
                {
                    Debug.Log("�̵�");
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
            //-----������ ��------------------------------------------------------------------------------------------
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
