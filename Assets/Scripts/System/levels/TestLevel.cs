using ArcanaDungeon.util;
using System.Collections.Generic;
using System;
using ArcanaDungeon.rooms;
using ArcanaDungeon.painters;
using ArcanaDungeon.Object;
using UnityEngine;
using Random = System.Random;//����Ƽ ���� �ƴ� c# ���� Ŭ���� �̿�
using Rect = ArcanaDungeon.util.Rect;//����Ƽ ���� �ƴ� util.Rect ���
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
            maxEnemies = 1;//���⿡ �ִ� �� �� �Է�
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

        public override void PlaceRooms()// �켱 �Ա� - ������ - �ⱸ ���·� ������ ����.
        {
            rooms[0].SetPosition(0, 0);
            rooms[0].placed = true;
            int radius = (int)levelsize;

            rooms[1].SetPosition(0, 10 * radius);
            rooms[1].placed = true;

            Rect r = rooms[0].Intersect(rooms[1]);
            //BossRoom br = new BossRoom("SlimeColony", Mathf.Abs(r.Width() * 5), Mathf.Abs(r.Height()));// �ӽ÷� �ϵ��ڵ� ���� �κ�, ���� Ǯ �þ�� �׿� �°� ������ ����.
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