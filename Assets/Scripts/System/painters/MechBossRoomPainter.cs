using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.rooms;
using UnityEngine;

namespace ArcanaDungeon.painters
{
    public class MechBossRoomPainter : Painter
    {
        public override void Paint(Level l, Room r)
        {
            int half = (r.Width() - 1) / 2;
            Debug.Log(half);
            int xcount = 0;
            int ycount = 0;
            for (int i = r.x; i < r.x + r.Width(); i++)
            {
                ycount = 0;
                for (int j = r.y; j < r.y + r.Height(); j++)
                {
                    int tile; 
                    tile = Terrain.GROUND;

                    int xSize = i - r.x;
                    int ySize = j - r.y;

                    if (xcount < half - ycount && half - ycount > 0)
                        tile = Terrain.WALL;
                    else if (half - ycount < 0 && xcount < ycount - half)
                        tile = Terrain.WALL;
                    else if(xcount >= r.Width() - half + ycount && half - ycount > 0)
                    {
                        tile = Terrain.WALL;
                    }
                    else if(half - ycount < 0 && xcount >= r.Width() + half - ycount)
                    {
                        tile = Terrain.WALL;
                    }
                    else if (half - ycount == 0 && (xcount == 0 || xcount == r.Width() - 1))
                        tile = Terrain.GENERATOR;
                    if (xcount == half && ycount == 0)
                        tile = Terrain.STAIRS_UP;
                    else if (xcount == half && ycount == r.Height() - 1)
                        tile = Terrain.STAIRS_DOWN;

                    l.map[i, j] = tile;
                    ycount++;
                }
                xcount++;
            }
        }
    }
}