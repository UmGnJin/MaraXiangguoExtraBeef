using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    
    public class Eye : Enemy
    {
        Vector2 beamtarget = new Vector2();
        public GameObject[] warning;
        public GameObject taljin;
        private bool charged = false;
        private int cooldown = 0;
        public void Awake()
        {
            this.maxhp = 90;
            this.maxstamina = 100;
            this.power = 10;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Eye";
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
                    exhau_image.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
                    exhau_image.GetComponent<exhaustController>().live = 120;
                    this.StaminaChange(20);

                }
                else
                {
                    if(charged == true)
                    {
                        Beamshot();
                        cooldown = 25;
                        this.StaminaChange(-50);
                    }

                    else if (Plr_pos[0, 0] != -1 & Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 6 & charged == false & cooldown == 0)
                    {
                        Beamcharge();
                    }
                    else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                    {
                        Dungeon.dungeon.Plr.be_hit(power);
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
                if(cooldown>0)
                cooldown--;
                this.Turnend();

            }
        }
        private void Beamcharge()//�ϰ���� �� : �÷��̾��� ��ġ�� ���� ���� �Ŀ� �� ���
        {
            int count = 0;
            beamtarget = new Vector2(Plr_pos[1, 0], Plr_pos[1, 1]);
            List<float[]> warn = range_check(beamtarget.x, beamtarget.y);
            Debug.Log("Ÿ�� ��: " + warn.Count);
            foreach(float[] w in warn)
            {
                //Debug.Log(count + "�� : x:"+w[0]+" , y: "+w[1]);
                warning[count].SetActive(true);
                warning[count].transform.position = new Vector3(w[0], w[1], -1);
                count++;
                if (count > 9)
                    Debug.LogError("������ʺ��!!!! �����ϴ�!���ǥ�ð�!!!!!");
                
            }
            charged = true;

        }
        private void Beamshot()
        {
            
            Thing temp_target = range_attack((int)beamtarget.x, (int)beamtarget.y, false);  //�ڰ��ݷ� 10�� �ӽð��̴�, Floor�� ���� 5/10/15�� �����ϴ� ���ݷ��� ������ ����־ �� ������ ���ݷ� ��ƾ� �Ѵ�
            if(temp_target!=null)
            temp_target.be_hit(power*3);
            UI.uicanvas.range_shot_a(this.transform.position.x, this.transform.position.y,beamtarget.x,beamtarget.y);
           
            for(int i=0;i<10;i++)    
                warning[i].SetActive(false);
                
            
            charged = false;
        }


    }
}
