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
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Gnoll";
        }

        public void FixedUpdate()
        {
            if (isTurn > 0)
            {
                Vision_research();

                if (Plr_pos[0, 0] != -1)
                {
                    this.range_attack(Plr_pos[0, 0], Plr_pos[0, 1], 10, true, false);  //★공격력 10은 임시값이다, Floor에 따라 변경되는 공격력을 변수에 집어넣어서 그 변수만큼만 깎아야 한다
                }
                else if (route_pos.Count > 0)
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                }
                this.isTurn -= 1;
            }
        }
    }
}
