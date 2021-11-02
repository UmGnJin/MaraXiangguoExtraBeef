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
            switch(boss)
            {
                case "SlimeColony":
                    p = new SlimeBossRoomPainter();
                    break;
                default:
                    p = new EmptyRoomPainter();
                    break;
            }
            SizePicker sp = new SizePicker();
            width = w;
            height = h;
            DefaultSet();
        }
        public override void Paint(Level l)
        {
            p.Paint(l, this);
        }
    }
}