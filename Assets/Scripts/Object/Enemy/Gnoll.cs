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
                HpChange(-70);//���� �׽�Ʈ
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
                
                if (this.exhausted == true)//Ż�� ���¿��� ���¹̳� ȸ��, �Ϲ������δ� Ư�� ���� ���� �� Ż���� �ɸ���, ���� ��ġ �̻��� ���¹̳����� �޽ĸ� �Ѵ�.
                                           //�׷��� ���� ��ġ���� ȸ���� ����, Ż�� �����̻��� ���ŵǰ�, ������ �ൿ �켱����� �ൿ�� �簳�Ѵ�.
                {
                    GameObject exhau_image = Instantiate(taljin);//Ż�� �� ź�� ����Ʈ �߻�
                    exhau_image.transform.position = new Vector3( this.transform.position.x,this.transform.position.y,-1);
                    exhau_image.GetComponent<exhaustController>().live = 120;
                    this.StaminaChange(20);

                }
                else
                {
                    Debug.Log("Plr_pos : "+Plr_pos[0,0]+ " / "+Plr_pos[0,1]);
                    if (Plr_pos[0, 0] != -1)
                    {
                        Thing temp_target = range_attack(Plr_pos[0, 0], Plr_pos[0, 1], false);  //�ڰ��ݷ� 10�� �ӽð��̴�, Floor�� ���� 5/10/15�� �����ϴ� ���ݷ��� ������ ����־ �� ������ ���ݷ� ��ƾ� �Ѵ�
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
                    else //�۶�����
                    {
                        this.StaminaChange(5);
                    }
                }

                this.Turnend();

            }
        }
    }
}
