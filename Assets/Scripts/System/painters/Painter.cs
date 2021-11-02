using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.rooms;

namespace ArcanaDungeon.painters
{
    public abstract class Painter
    {

        public abstract void Paint(Level l, Room r);
    }
}