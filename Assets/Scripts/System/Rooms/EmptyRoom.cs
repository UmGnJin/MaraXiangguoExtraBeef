using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.util;
using ArcanaDungeon.painters;

namespace ArcanaDungeon.rooms
{
    public class EmptyRoom : Room
    {
        public EmptyRoom()
        {
            SizePicker sp = new SizePicker();
            width = sp.Pick(MINROOMSIZE, MAXROOMSIZE);
            height = sp.Pick(MINROOMSIZE, MAXROOMSIZE);
            DefaultSet();
        }
        public override void Paint(Level l)
        {
            EmptyRoomPainter erg = new EmptyRoomPainter();
            erg.Paint(l, this);
        }
    }
}