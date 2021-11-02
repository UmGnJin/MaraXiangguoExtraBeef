using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;


namespace ArcanaDungeon.Object
{
    public class Rat : Enemy
    {
        public void Awake()
        {
            this.maxhp = 115;
            this.maxstamina = 100;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Rat";
        }
    }
}