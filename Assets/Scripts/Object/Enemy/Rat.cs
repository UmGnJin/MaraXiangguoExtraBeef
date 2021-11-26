using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;


namespace ArcanaDungeon.Object
{
    public class Rat : Enemy
    {
        public GameObject taljin;
        
        public void Awake()
        {
            this.maxhp = 130;
            this.maxstamina = 100;
            this.power = 10;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);
            

            this.name = "Rat";
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                HpChange(-70);//���� �׽�Ʈ
        }
        public void FixedUpdate()
        {
            //ü�¹� ����, ���ݹްų� ȸ���� ������ ü�¹ٸ� ������ ���� �ִٸ� ���� ��ġ�� �Űܵ� ��
            this.gameObject.transform.GetChild(1).transform.localScale = new Vector2((this.hp > 0 ? this.hp : 0) / (float)this.maxhp, 0.1f);
            this.gameObject.transform.GetChild(1).localPosition = new Vector3((this.hp > 0 ? this.hp : 0) / this.maxhp, 0.8f, -0.2f);

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
                if (this.exhausted == true)// ���¹̳� ȸ�� ���. �Ϲ������δ� Ư�� ���� ���� �� Ż���� �ɸ���, ���� ��ġ �̻��� ���¹̳����� �޽ĸ� �Ѵ�.
                                              // �׷��� ���� ��ġ���� ȸ���� ����, Ż�� �����̻��� ���ŵǰ�, ������ �ൿ �켱����� �ൿ�� �簳�Ѵ�.
                {
                    GameObject exhau_image = Instantiate(taljin);//Ż�� �� ź�� ����Ʈ �߻�
                    exhau_image.transform.position = this.transform.position;
                    exhau_image.GetComponent<exhaustController>().live = 120;
                    this.StaminaChange(20);
                }
                else
                {
                     if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// ���� �Ÿ� ���� �÷��̾ ���� ��, �⺻ ������ �켱���Ѵ�.
                    {
                        Dungeon.dungeon.Plr.HpChange(-power);
                        this.StaminaChange(-10);
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
                }

                this.isTurn -= 1;

            }
        }
    }
}