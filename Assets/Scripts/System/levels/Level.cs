using System.Collections.Generic;
using System;
using ArcanaDungeon.rooms;
using ArcanaDungeon.painters;
using ArcanaDungeon.Object;
using ArcanaDungeon.util;
using UnityEngine;
using Random = System.Random;
using Rect = ArcanaDungeon.util.Rect;
using System.Linq;

namespace ArcanaDungeon
{
    public enum LevelSize
    {
        SMALL = 4,
        NORMAL = 10,
        LARGE = 16
        //SMALL(4~8), NORMAL(10~14), LARGE(16~20)
    }

    public enum Biome
    {
        NORMAL = 0,
        FIRE = 1,

        BOSS_SLIME = 10
    }


    public abstract class Level 
    {
        public GameObject[,] temp_gameobjects;//★시야를 표현하기 위한 임시 게임오브젝트 배열, 나중에 그래픽 표현이나 좌표 체계를 정리할 필요가 있다
        public int width, height, length;
        public int[,] map;

        public List<Room> rooms;

        public LevelSize levelsize;
        public Biome biome;

        public int floor;
        public Rect levelr;
        public int exitnum;
        public static Random rand = new Random();

        public bool[,] vision_blockings;
        public Vector2 laststair;

        public List<Enemy> enemies;
        public int maxEnemies;

        public void Create()
        {
            Random random = new Random();
            biome = (Biome)random.Next(0, 2);
            Debug.Log(biome);
            InitRooms();

            foreach (Room r1 in rooms)
            {
                foreach (Room r2 in rooms)
                {
                    if (r1.GetHashCode() == r2.GetHashCode())
                        continue;
                    if (r1.IsNeighbour(r2))
                    {
                        r1.Connect(r2);
                        r2.Connect(r1);
                    }
                }
            }

            maxEnemies = rooms.Count() / 3;

            PaintRooms();
            SpawnMobs();
            //foreach (Room r in rooms)
             //   Debug.Log(r.Info());


            //map[]에 있는 타일 중 시야를 가리는 친구들을 true로 나타내는 배열 vision_blocking을 완성한다, Visionchecker에서 사용한다



            temp_gameobjects = new GameObject[width, height];
            vision_blockings = new bool[width,height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++)
                {
                    vision_blockings[i,j] = (Terrain.thing_tag[map[i,j]] & Terrain.vision_blocking) != 0;
                }
            }

        }
        public abstract void InitRooms();
        public abstract void SpawnMobs();
        public void PlaceDoors(Room r)
        {
            foreach(Room room in r.connection.Keys.ToList())
            {
                var door = r.connection[room];
                if (door != null)
                    continue;
                var i = r.Intersect(r, room);
                if (i.Width() == 0)
                    door = new Room.Door(i.x - 1, rand.Next(i.y + 1, i.yMax - 1));
                else
                    door = new Room.Door(rand.Next(i.x + 1, i.xMax - 1), i.y - 1);

                if (r.connection.ContainsKey(room))
                    r.connection[room] = door;
                else
                    r.connection.Add(room, door);
                room.connection[r] = door;
            }
        }
        protected internal virtual void PaintDoors(Room r)
        {
            foreach (var n in r.connection.Keys)
            {
               
                var d = r.connection[n];

                switch (d.type)
                {
                    case Room.Door.Type.EMPTY:
                        map[d.x, d.y] = Terrain.GROUND;
                        break;
                    case Room.Door.Type.REGULAR:
                        map[d.x, d.y] = Terrain.DOOR;
                        break;
                }
            }
        }
        public abstract void PlaceRooms();
        
        public bool MoveRoom(Room r, int xDir, int yDir, out int index)
        {
            r.MovePosition(xDir, yDir);
            int i = -1;
            if(CheckOverlap(r, out i))
            {
                r.MovePosition(-xDir, -yDir);
                index = i;
                return false;
            }
            index = -1;
            return true;
        }
        public bool CheckOverlap(Room r1, Room r2) // 매개변수인 두 방의 겹침 여부 확인.
        {
            Rect rect = r1.Intersect(r1, r2);
            if (rect.Width() > 0 && rect.Height() > 0)
                return true;
            return false;
        }
        public bool CheckOverlap(Room r) // 이 방과 겹치는 방이 있는지 모든 방을 검사함.
        {
            foreach (Room r1 in rooms)
            {
                if ((r.GetHashCode() == r1.GetHashCode() || !r1.placed) && r1.GetType() != typeof(DownStairsRoom))
                    continue;
                Rect rect = r.Intersect(r, r1);
                if (rect.Width() > 0 && rect.Height() > 0)
                {
                    //Debug.Log("Intersects with Room number " + rooms.IndexOf(r1) + ", Intersect Range : " + rect.Width() + ", " + rect.Height());
                    return true;
                }
            }
            return false;
        }
        public bool CheckOverlap(Room r, out int index) // 겹치는 방 발견 시, 해당 방 번호를 같이 제공(최초 발견한 방만)
        {
            foreach (Room r1 in rooms)
            {
                if (r.GetHashCode() == r1.GetHashCode() || !r1.placed)
                    continue;
                Rect rect = r.Intersect(r, r1);
                if (rect.Width() > 0 && rect.Height() > 0)
                {
                    //Debug.Log("Intersects with Room number " + rooms.IndexOf(r1) + ", Intersect Range : " + rect.Width() + ", " + rect.Height());
                    index = rooms.IndexOf(r1);
                    return true;
                }
            }
            index = -1;
            return false;
        }
        public void PaintRooms()
        {
            foreach(Room r in rooms)
            {
                PlaceDoors(r);
                r.Paint(this);
                PaintDoors(r);
            }
        }
        public Rect LevelRect()
        {
            Rect rect = new Rect();
            foreach(Room r in rooms)
            {
                if(r.x < rect.x)
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
            width = rect.Width();
            height = rect.Height();
            length = width * height;
            return rect;
        }
        public void MoveRooms()
        {
            int xDir = -levelr.x;
            int yDir = -levelr.y;

            levelr.SetPosition(-1, -1);
            foreach(Room r in rooms)
            {
                r.MovePosition(xDir, yDir);
            }
        }

        public abstract Vector2 SpawnPoint();
        
    }
   
}