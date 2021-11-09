using ArcanaDungeon.util;
using System.Collections.Generic;
using System;

namespace ArcanaDungeon.rooms
{
    public abstract class Room : Rect //Room은 방 크기 형태를 제공하는 Rect를 상속받는 클래스.
    {
        protected static Random rand = new Random();//하위 클래스들이 방 크기 설정에 사용할 랜덤 클래스.
        public Dictionary<Room, Door> connection = new Dictionary<Room, Door>();
        public const int MINROOMSIZE = 4;
        public const int MAXROOMSIZE = 9;
        public int width, height;
        public bool placed = false;
        public double angle;//방 배치 과정에서 사용할 각도 저장용.

        public bool IsNeighbour(Room r)//이 방이 이웃이 될 수 있는지를 검사. 
                                       //가로나 세로 중 한 쪽이 서로 붙어 있고(겹친 곳 너비나 높이가 0), 그 칸수가 3칸 이상이어야 한다(0이지 않은쪽은 3 이상)
                                       //당연히 붙어 있어야 문을 넣고, 각자의 모서리벽 1칸씩을 제외하고 문을 넣을 1칸이 필요해 3칸 이상이 겹쳐야 하기 때문.
        {
            Rect rect = Intersect(r);
            if ((rect.Width() != 0 || rect.Height() < 3) && (rect.Height() != 0 || rect.Width() < 3))
                return false;


            return true;
        }

        public void Connect(Room r)//이 방과 연결된 방을 연결된 방 리스트에 서로 추가.
        {
            if (connection.ContainsKey(r))
                return;
            connection.Add(r, null);
            r.connection.Add(this, null);
        }


        public void DefaultSet()//초기화.
        {
            x = 0;
            y = 0;
            xMax = x + width;
            yMax = y + height;
        }
        public String Info()
        {
            return "Position : (" + (x - 1) + ", " + (y + 1) + "), " +"width : " + Width() + ", height : " + Height();
        }

        public Point Center()
        {
            return new Point((x + xMax) / 2, (y + yMax) / 2);
        }
        public abstract void Paint(Level l);//~Painter클래스에서 사용할 함수.

        public class Door : Point // 문 클래스. Room에서만 사용하므로 여기 구현된 것으로 추정.(원본 코드부터 여기 있었음)
        {
            public enum Type//가능하면 숨겨진 문 등 추가예정.
            { 
                EMPTY, REGULAR
            }
            public Type type = Type.REGULAR;

            public Door() { }
            public Door(Point p) : base(p) { }
            public Door(int x, int y) : base(x, y) { }
            //생성자는 Point것을 그대로 사용.
            public void Set(Type t)//문 타입 변경.
            {
                if (t.CompareTo(type) > 0)
                    type = t;
            }
        }

    }
}
