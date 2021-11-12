using System;
using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.util;
using ArcanaDungeon.painters;


namespace ArcanaDungeon.rooms
{
    public class BranchRoom : Room
    {
        public bool connected = false;
        public bool first = false;
        public bool maze = false;

        public BranchRoom(bool f)
        {
            first = f;
            if (first)
            {
                width = rand.Next(MINROOMSIZE + 2, MAXROOMSIZE);
                height = rand.Next(MINROOMSIZE + 2, MAXROOMSIZE);
            }
            else
            {
                width = rand.Next(MINROOMSIZE, MAXROOMSIZE);
                height = rand.Next(MINROOMSIZE, MAXROOMSIZE);
            }
            if(rand.Next(0, 1) == 0)
                    maze = true;
            DefaultSet();
        }

        public new void Connect(Room r)//이 방과 연결된 방을 연결된 방 리스트에 서로 추가.
        {
            if (connection.ContainsKey(r) || this.connected == true)
                return;
            connection.Add(r, null);
            r.connection.Add(this, null);
            this.connected = true;
        }

        public override void Paint(Level l)
        {
            Painter erg;
            if (!maze)
                erg = new EmptyRoomPainter();
            else
                erg = new MazeRoomPainter();
            erg.Paint(l, this);
        }
    }
}