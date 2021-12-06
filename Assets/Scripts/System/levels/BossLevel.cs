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
    public class BossLevel : Level
    {
        
        public override void InitRooms()//
        {
            levelsize = LevelSize.SMALL;
            rooms = new List<Room>();
            //biome = (Biome)rand.Next(10, 15);
            biome = Biome.BOSS_MIMIC;
            if (biome != Biome.BOSS_MECH)
            {
                rooms.Add(new UpStairsRoom());
                exitnum = 1;
                rooms.Add(new DownStairsRoom());
            }
            maxEnemies = 1;

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
            int radius = (int)levelsize;
            BossRoom br;
            Rect r = new Rect();
            if (biome != Biome.BOSS_MECH)
            {
                rooms[0].SetPosition(0, 0);
                rooms[0].placed = true;

                rooms[1].SetPosition(0, 5 * radius);
                rooms[1].placed = true;

                r = rooms[0].Intersect(rooms[1]);
            }
            switch (biome)
            {
                case Biome.BOSS_MECH:
                    br = new BossRoom("Mech", 17, 17);  //�� �κ��� ��� default�� �ű�� ������ �׽�Ʈ ����, �ű��� �ʾƵ� 20% Ȯ���� �׽�Ʈ ����
                    break;
                default:
                    br = new BossRoom("asdf", Mathf.Abs(r.Width() * 10), Mathf.Abs(r.Height()));
                    br.SetPosition(-(br.width / 2), Mathf.Abs(rooms[0].Height()));
                    break;
            }
            
            rooms.Add(br);
            br.placed = true;
        }

        public new Rect LevelRect()//�ֻ�/��/��/������ �������� ���� ���簢��ȭ�� �����Ѵ�.
                               //�� ���� ä��⳪ �� ��ü�̵� � ���.
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