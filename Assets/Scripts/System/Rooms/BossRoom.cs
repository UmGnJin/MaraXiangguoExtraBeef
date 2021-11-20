using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.util;
using ArcanaDungeon.painters;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.rooms
{
    public class BossRoom : Room
    {
        Painter p;
        public BossRoom(string boss, int w, int h)
        {
            width = w;
            height = h;
            switch (boss)
            {
                case "Mech":
                    p = new MechBossRoomPainter();

                    break;
                case "SlimeColony":
                    p = new SlimeBossRoomPainter();
                    break;
                default:
                    p = new EmptyRoomPainter();
                    break;
            }
            DefaultSet();
        }
        
        public override void Paint(Level l)
        {
            p.Paint(l, this);
        }
    }
}