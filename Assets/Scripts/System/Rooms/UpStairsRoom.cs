using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.util;
using ArcanaDungeon.painters;

namespace ArcanaDungeon.rooms
{
    public class UpStairsRoom : Room
    {

        public UpStairsRoom()
        {
            SizePicker sp = new SizePicker();
            width = 6; // sp.Pick(MINROOMSIZE + 1, MAXROOMSIZE - 2);
            height = 6; // sp.Pick(MINROOMSIZE + 1, MAXROOMSIZE - 2);
            DefaultSet();
        }
        public override void Paint(Level l)
        {
            UpStairRoomPainter erg = new UpStairRoomPainter();
            erg.Paint(l, this);
        }
    }
}