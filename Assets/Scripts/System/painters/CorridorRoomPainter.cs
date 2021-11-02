using ArcanaDungeon.rooms;
using System;
using System.Collections.Generic;
using ArcanaDungeon.util;

namespace ArcanaDungeon.painters
{
    public class CorridorRoomPainter : Painter
    {
        public override void Paint(Level l, Room r)
        {
           

        }//문과 문 사이를 잇는 길 말고는 전부 벽으로 채운다.
        //1.문이 2개 : 문 2개의 좌표를 구한다. x,y 로 몇칸 가야 하는지 구하고 적당히 랜덤으로 길을 꺾는다.
        //2. 문이 3개 이상 : 문들의 좌표를 구한다. 가장 먼 2개의 방을 1번 방식으로 연결하고, 나머지는 해당 복도와 가장 가까운 지점 / 복도의 아무 지점에 연결시킨다.
    }
}