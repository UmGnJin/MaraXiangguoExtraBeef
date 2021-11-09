using System;
using System.Collections;
using System.Collections.Generic;
using ArcanaDungeon.util;
using ArcanaDungeon.painters;

namespace ArcanaDungeon.rooms
{
    public class EmptyRoom : Room //다른 방들 모두 방식이 사실상 같으니 여기만 주석 작성함.
                                  //Room의 최소, 최대 사이즈를 받아와서 랜덤(혹은 방에 따라 고정값)으로 크기를 설정하고 그에 맞게 Rect를 설정해준다.
                                  //크기 설정이 끝나면 방에 맞는 Painter 클래스를 불러와서 타일을 채워 넣는다.
    {
        public EmptyRoom()
        {
            width = rand.Next(MINROOMSIZE + 2, MAXROOMSIZE);
            height = width;
            DefaultSet();
        }
        public override void Paint(Level l)
        {
            EmptyRoomPainter erg = new EmptyRoomPainter();
            erg.Paint(l, this);
        }
    }
}