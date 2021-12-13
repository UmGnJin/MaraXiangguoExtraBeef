using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace ArcanaDungeon.Object
{

    public class Zera : Enemy
    {
        public static Random random = new Random();
        Vector2 beamtarget = new Vector2();
        public GameObject[] warning;

        private bool charged = false;
        private int charged_T = 0;
        private int cooldown = 0;
        private int cooldown_Tel = 0;
        public void Awake()
        {
            isboss = true;
            this.maxhp = 80;
            this.maxstamina = 100;
            this.power = 10;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "�� �ּ���";
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                HpChange(-70);//���� �׽�Ʈ
        }
        public new void FixedUpdate()
        {
            if (this.hp <= 0)
                this.die();

            if (isTurn > 0)
            {
                warning[1].SetActive(false);
                Vision_research();


                if (cooldown_Tel == 0 & Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 2 & Plr_pos[0, 0] != -1)
                {
                    Teleport();

                }
                else if (charged_T == 2)
                {
                    Thunderrecoil();


                }
                else if (charged_T == 1)
                {
                    Thundershot();


                }

                else if (charged == true)
                {
                    Beamshot();


                }
                else if (Plr_pos[0, 0] != -1 & Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 6 & cooldown == 0)
                {
                    Thundercharge();
                }
                else if (Plr_pos[0, 0] != -1 & Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 6 & charged == false)
                {
                    Beamcharge();
                }

                else if (route_pos.Count > 0)
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                    this.StaminaChange(-10);
                }
                else //�۶�����
                {
                    this.StaminaChange(5);
                }

                if (cooldown > 0)
                    cooldown--;
                if (cooldown_Tel > 0)
                    cooldown_Tel--;
                this.Turnend();


            }
        }
        private void Teleport()
        {
            while (true)
            {
                int x = random.Next(0, Dungeon.dungeon.currentlevel.levelr.xMax);
                int y = random.Next(0, Dungeon.dungeon.currentlevel.levelr.yMax);
                if (Dungeon.dungeon.currentlevel.map[x, y] == Terrain.GROUND && Dungeon.distance_cal_a(this.transform.position.x, this.transform.position.y,x,y) > 2)
                {
                    
                    this.transform.position = new Vector2(Mathf.Round(x - 1), Mathf.Round(y));
                    cooldown_Tel = 20;
                    route_pos.Clear();
                    break;
                }
            }
            UI.uicanvas.log_add("�� �ּ��簡 �ڷ���Ʈ�߽��ϴ�!");
        }
        private void Beamcharge()//�ϰ���� �� : �÷��̾��� ��ġ�� ���� ���� �Ŀ� �� ���
        {
            
            int count = 0;
            beamtarget = new Vector2(Plr_pos[1, 0], Plr_pos[1, 1]);
            
            warning[count].SetActive(true);
            warning[count].transform.position = new Vector3(Plr_pos[1,0],Plr_pos[1,1], -1);
            charged = true;

        }
        private void Thundercharge()//�ϰ���� �� : �÷��̾��� ��ġ�� ���� ���� �Ŀ� �� ���
        {
            UI.uicanvas.log_add("�� �ּ��簡 ������ ������ �غ��ϰ� �ֽ��ϴ�!");

            beamtarget = new Vector2(Plr_pos[1, 0], Plr_pos[1, 1]);
            
                    charged_T = 1;

        }
        private void Beamshot()
        {

            Thing temp_target = range_attack((int)beamtarget.x, (int)beamtarget.y, false);  //�ڰ��ݷ� 10�� �ӽð��̴�, Floor�� ���� 5/10/15�� �����ϴ� ���ݷ��� ������ ����־ �� ������ ���ݷ� ��ƾ� �Ѵ�
            if (temp_target != null)
                temp_target.be_hit(power * 3);
            UI.uicanvas.range_shot_a(this.transform.position.x, this.transform.position.y, beamtarget.x, beamtarget.y);

            for (int i = 0; i < 10; i++)
                warning[i].SetActive(false);


            charged = false;
            Debug.Log("�׳ɺ�!!!");
            

        }
        private void Thundershot()
        {
            UI.uicanvas.log_add("�� �ּ��簡 ������ ������ �ҷ��Խ��ϴ�!");
            int count = 1;
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {

                    if (i == j && i == 0)
                        Debug.Log("��");
                    else
                    {
                        posattack(beamtarget.x + i, beamtarget.y + j, power);
                        warning[count].SetActive(true);
                        warning[count].transform.position = new Vector3(beamtarget.x + i, beamtarget.y + j, -1);



                        count++;
                    }

                }
            
            charged_T = 2;

            


            Debug.Log("����!!!");

        }
        private void Thunderrecoil()
        {
            for (int i = 0; i < 10; i++)
                warning[i].SetActive(false);
            warning[1].SetActive(true);
            warning[1].transform.position = new Vector3(beamtarget.x, beamtarget.y, -1);

            posattack(beamtarget.x, beamtarget.y, power);
                
            

           
            
            


            Debug.Log("�ļӹ���!!!");
            charged_T = 0;
            cooldown = 9;
        }


    }
}

