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
            this.name = "�̹�";
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                HpChange(-70);//���� �׽�Ʈ
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
                        if (this.exhausted == true)// ���¹̳� ȸ�� ���. �Ϲ������δ� Ư�� ���� ���� �� Ż���� �ɸ���, ���� ��ġ �̻��� ���¹̳����� �޽ĸ� �Ѵ�.
                                                   // �׷��� ���� ��ġ���� ȸ���� ����, Ż�� �����̻��� ���ŵǰ�, ������ �ൿ �켱����� �ൿ�� �簳�Ѵ�.
                        {
                            GameObject exhau_image = Instantiate(taljin);//Ż�� �� ź�� ����Ʈ �߻�
                            exhau_image.transform.position = this.transform.position;
                            exhau_image.GetComponent<exhaustController>().live = 120;
                            this.StaminaChange(20);
                        }
                        else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)                  {
                        //Debug.Log(this.name+"��(��) ����� �����մϴ�.");

                        Dungeon.dungeon.Plr.condition_add(0, 2);    //��ȭ 2 �ο�
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
                Dungeon.dungeon.currentlevel.locked = false;//���� ��� ���������� ���� ��
            }
               
            Destroy(this.gameObject);


        }
            public void loot()
            {
                switch (bosang)
                {
                    case Bosang.KEY:
                        Debug.Log("��� ���� ī��!");
                        break;
                    case Bosang.GGWANG:
                        Debug.Log("�Ϲ� �� ī��.");
                        break;
                    case Bosang.BOMUL:
                        Debug.Log("Ȳ�� ���� ī��!!!!!!");
                        break;

                }
            }
    }
}

