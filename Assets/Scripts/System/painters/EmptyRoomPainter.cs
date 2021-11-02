using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.rooms;

namespace ArcanaDungeon.painters
{
    public class EmptyRoomPainter : Painter
    {
        public override void Paint(Level l, Room r)
        {
            for (int i = r.x; i < r.x + r.Width(); i++)
            {
                for (int j = r.y; j < r.y + r.Height(); j++)
                {
                    int tile;
                    tile = Terrain.GROUND;
                    if (i == r.x + r.Width() - 1 || j == r.y + r.Height() - 1)
                        tile = Terrain.WALL;
                    l.map[i,j] = tile;
                }
            }
        }
    }
}