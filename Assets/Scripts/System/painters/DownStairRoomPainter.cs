using ArcanaDungeon.rooms;
using System;
using System.Collections.Generic;
using ArcanaDungeon.util;

namespace ArcanaDungeon.painters
{
    public class DownStairRoomPainter : Painter
    {
        public static Random rand = new Random();
        public override void Paint(Level l, Room r)
        {
            int door_x = rand.Next(r.x + 1, r.xMax - 2);
            int door_y = rand.Next(r.y + 1, r.yMax - 2);
            bool stair = false;
            for (int i = r.x; i < r.x + r.Width(); i++)
            {
                for (int j = r.y; j < r.y + r.Height(); j++)
                {
                    int tile;
                    tile = Terrain.GROUND;
                    if (i == r.x + r.Height() - 1 || j == r.y + r.Width() - 1)
                        tile = Terrain.WALL;
                    if (!stair & i == door_x & j == door_y)
                    {
                        stair = true;
                        tile = Terrain.STAIRS_DOWN;
                    }
                    l.map[i, j] = tile;
                }
            }
        }
    }
}