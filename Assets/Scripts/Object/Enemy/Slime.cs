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

            this.name = "������";
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
                if (this.GetStamina() < 20 && this.exhausted == false)
                    this.exhausted = true;
                else if (this.GetStamina() >= 60 && this.exhausted == true)
                    this.exhausted = false;

                Vision_research();
                if(this.exhausted == true)// ���¹̳� ȸ�� ���. �Ϲ������δ� Ư�� ���� ���� �� Ż���� �ɸ���, ���� ��ġ �̻��� ���¹̳����� �޽ĸ� �Ѵ�.
                                          // �׷��� ���� ��ġ���� ȸ���� ����, Ż�� �����̻��� ���ŵǰ�, ������ �ൿ �켱����� �ൿ�� �簳�Ѵ�.
                {
                    GameObject exhau_image = Instantiate(taljin);//Ż�� �� ź�� ����Ʈ �߻�
                    exhau_image.transform.position = this.transform.position;
                    exhau_image.GetComponent<exhaustController>().live = 120;
                    this.StaminaChange(20);
                }
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// ���� �Ÿ� ���� �÷��̾ ���� ��, �⺻ ������ �켱���Ѵ�.
                                                                                                                   // �������� �⺻ ������ ���� �ο��Ѵ�.
                {
                    //Debug.Log(this.name+"��(��) ����� �����մϴ�.");
                    Dungeon.dungeon.Plr.be_hit(this.power);
                    Dungeon.dungeon.Plr.condition_add(3, 2);    //�ߵ� 2 �ο�
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

        public override void die() // ������ ��� ȿ�� - ���� �����ְ�
                                   // 1. ���� - �����鼭 �ֺ��� ���� ���ؿ� �� �ο�. ���� �迭 ī������.
                                   // 2. �ٴ� - ���� ��ġ�� ���� �ϵ��� �����Ǵ� / �� �� ���� ������ �����Ǵ� �� ���� ����. ���� �ö󰡸� �� 1 �ο�. �̹� ���� �ִٸ� ��ø�� ���ҵ��� �ʰ� ���ŵ�.
        {
            Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1].Remove(this.gameObject);
            GameObject PoisonTile = Instantiate(poisontile);
            PoisonTile.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);

            Destroy(this.gameObject);


        }
    }
}