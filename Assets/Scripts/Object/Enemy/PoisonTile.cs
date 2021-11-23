using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon;
using ArcanaDungeon.Object;
public class PoisonTile : MonoBehaviour
{           
    public int life = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Dungeon.dungeon.Plr.transform.position.x == this.transform.position.x && Dungeon.dungeon.Plr.transform.position.y == this.transform.position.y)
        {
            Dungeon.dungeon.Plr.condition_add(Dungeon.dungeon.Plr, 3, 1);
            Destroy(gameObject);
           
        }
    }
}
