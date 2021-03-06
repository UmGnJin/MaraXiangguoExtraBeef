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
    public class BossLevel : Level
    {
        public static bool[] availableBossList = new bool[]{true, true, true};
        public override void InitRooms()//
        {
            locked = true;
            levelsize = LevelSize.SMALL;
            rooms = new List<Room>();
            //biome = Biome.NORMAL;
            for(int i = 0; i < availableBossList.Length; i++)
            {
                if (availableBossList[i] == true)
                {
                    biome = (Biome)(i + 1);
                    break;
                }
            }
            //biome = Biome.BOSS_CRAB;

            availableBossList[(int)biome - 1] = false;
            maxEnemies = 1;

            rooms.Add(new UpStairsRoom());
            exitnum = 1;
            rooms.Add(new DownStairsRoom());

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
            int radius = (int)levelsize;
            BossRoom br;
            Rect r = new Rect();

            rooms[0].SetPosition(0, 0);
            rooms[0].placed = true;

            rooms[1].SetPosition(0, 5 * radius);
            rooms[1].placed = true;

            r = rooms[0].Intersect(rooms[1]);

            br = new BossRoom("asdf", Mathf.Abs(r.Width() * 10), Mathf.Abs(r.Height()));
            br.SetPosition(-(br.width / 2), Mathf.Abs(rooms[0].Height()));
                    
            
            rooms.Add(br);
            br.placed = true;
        }

        public new Rect LevelRect()//최상/하/좌/우측을 기준으로 맵을 직사각형화해 저장한다.
                               //빈 공간 채우기나 맵 전체이동 등에 사용.
        {
            Rect rect = new Rect();
            foreach (Room r in rooms)
            {
                if (r.x < rect.x)
                    rect.x = r.x;
                if (r.y < rect.y)
                    rect.y = r.y;
                if (r.xMax > rect.xMax)
                    rect.xMax = r.xMax;
                if (r.yMax > rect.yMax)
                    rect.yMax = r.yMax;
            }
            rect.x -= 1;
            rect.y -= 1;
            rect.xMax += 1;
            rect.yMax += 1;
            width = rect.Width();
            height = rect.Height();
            length = width * height;
            return rect;
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