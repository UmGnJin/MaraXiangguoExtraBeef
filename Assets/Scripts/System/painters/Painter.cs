using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.rooms;

namespace ArcanaDungeon.painters
{
    public abstract class Painter //방의 타일을 채워넣는 기능을 하는 클래스. 여기는 상속용 본체.
    {

        public abstract void Paint(Level l, Room r);
    }
}