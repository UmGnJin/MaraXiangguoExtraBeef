using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    public class Gnoll : Enemy
    {

        public void Awake() {
            this.maxhp = 90;
            this.maxstamina = 100;
            this.power = 10;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Gnoll";
        }

        public void FixedUpdate()
        {
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
                    
                    this.StaminaChange(20);

                }
                else
                {
                    Debug.Log(Plr_pos);
                    if (Plr_pos[0, 0] != -1)
                    {
                        this.range_attack(Plr_pos[0, 0], Plr_pos[0, 1], power, true, false);  //�ڰ��ݷ� 10�� �ӽð��̴�, Floor�� ���� 5/10/15�� �����ϴ� ���ݷ��� ������ ����־ �� ������ ���ݷ� ��ƾ� �Ѵ�
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
                
                this.isTurn -= 1;
                
            }
        }
    }
}
