using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.rooms;

namespace ArcanaDungeon.painters
{
    public class EmptyRoomPainter : Painter //Painter 클래스들도 대체로 비슷하므로 여기서만 설명.
                                            //방마다 채워넣는 방식만 다르고, 기본적으로 우측과 하단에만 벽을 생성한다.
                                            //좌측 상단은 빈 공간 벽, 혹은 연결된 방의 우측 하단으로 채워넣을 수 있기 때문.
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