using ArcanaDungeon.util;
using System.Collections.Generic;
using System;
using ArcanaDungeon.rooms;
using ArcanaDungeon.painters;
using ArcanaDungeon.Object;
using UnityEngine;
using Random = System.Random;//유니티 것이 아닌 c# 랜덤 클래스 이용
using Rect = ArcanaDungeon.util.Rect;//유니티 것이 아닌 util.Rect 사용
using System.Linq;

namespace ArcanaDungeon
{
    public class TestLevel : Level
    {

        public override void InitRooms()//
        {
            levelsize = LevelSize.SMALL;
            rooms = new List<Room>();
            rooms.Add(new UpStairsRoom());
            exitnum = 1;
            rooms.Add(new DownStairsRoom());
            maxEnemies = 1;//여기에 최대 적 수 입력
            biome = Biome.NORMAL;

            PlaceRooms();
            levelr = LevelRect();
            MoveRooms();
            map = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = Terrain.WALL;
                }
            }
        }

        public override void PlaceRooms()// 우선 입구 - 보스방 - 출구 형태로 제작할 예정.
        {
            rooms[0].SetPosition(0, 0);
            rooms[0].placed = true;
            int radius = (int)levelsize;

            rooms[1].SetPosition(0, 10 * radius);
            rooms[1].placed = true;

            Rect r = rooms[0].Intersect(rooms[1]);
            //BossRoom br = new BossRoom("SlimeColony", Mathf.Abs(r.Width() * 5), Mathf.Abs(r.Height()));// 임시로 하드코딩 넣은 부분, 보스 풀 늘어나면 그에 맞게 조정할 예정.
            EmptyRoom er = new EmptyRoom(Mathf.Abs(r.Width() * 10), Mathf.Abs(r.Height()));
            er.SetPosition(-(er.Width() / 2), rooms[0].Height());

            rooms.Add(er);
            er.placed = true;
            //if (br.IsNeighbour(rooms[0]) && br.IsNeighbour(rooms[1]))
            //  Debug.Log("Bossroom Spawned.");
        }



        public override Vector2 SpawnPoint()
        {
            Vector2 point = new Vector2();
            /*
            foreach(Room r in rooms)
            {
                if (r.GetType() == typeof(DownStairsRoom))
                {
                    Room.Door door = r.connection.Values.ToList()[0];
                    point = new Vector2(door.x, door.y);

                }
                else
                    continue;
            }
            */
            return new Vector2();
        }

        public override void SpawnMobs()
        {

        }
    }
}