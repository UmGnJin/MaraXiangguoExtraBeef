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
    public enum LevelSize//레벨 크기 프리셋. 방의 개수 결정에 이용.
    {
        SMALL = 5,
        NORMAL = 7,
        LARGE = 9
        //메인 루트에 들어갈 계단방을 제외한 방 수. 따라서 실제 메인 루트는 해당값 +2가 된다.
    }

    public enum Biome//레벨이 가질 지형 예시. 
    {
        NORMAL = 0,
        FIRE = 1,

        BOSS_SLIME = 10
    }


    public abstract class Level 
    {
        public GameObject[,] temp_gameobjects;//★시야를 표현하기 위한 임시 게임오브젝트 배열, 나중에 그래픽 표현이나 좌표 체계를 정리할 필요가 있다
        public int width, height, length;
        public int[,] map;//텍스트로 구현된 맵을 저장할 장소.

        public List<Room> rooms;//레벨 내의 방 저장.

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

        public void Create()//레벨 생성 시 최초로 실행되는 부분. 무작위로 바이옴을 결정하고, 방을 생성한다.(이는 레벨 타입마다 다르므로 하위 레벨 클래스에서 오버라이딩 하도록 되어 있음.)
                            //방 배치가 완료되면, 배치된 방들 중 조건에 맞는 방들을 이웃으로 묶는다. 그 다음, 이웃 방들끼리 연결할 문을 만들어 준다.
                            //문이 완성되면 텍스트 형태의 1차 맵이 완성된다. 이후 맵에 몹과 오브젝트를 소환하고 시야 관련 작업을 한 뒤 종료된다.
                            //그래픽 형태의 2차 맵은 Dungeon 클래스에서 만든다.
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
        public abstract void PlaceRooms();
        public void PlaceDoors(Room r)//이웃 방끼리 문을 만들어 준다. 붙어 있는 부분을 찾아 그 사이의 무작위 위치로 문을 만든다.
        {
            foreach(Room room in r.connection.Keys.ToList())
            {
                var door = r.connection[room];
                if (door != null)
                    continue;
                var i = r.Intersect(room);
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
        protected internal virtual void PaintDoors(Room r)//PlaceDoors에서 만든 문을 실제 지형의 형태로 맵에 추가한다.
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
        public bool MoveRoom(Room r, int xDir, int yDir, out int index)//방을 옮기면서 다른 방과 겹치는지를 확인한다. 겹치면 false가 나오고, index를 통해 겹친 방의 번호가 나온다.
        {
            r.MovePosition(xDir, yDir);
            int i = -1;
            if(CheckOverlap(r, out i))
            {
                r.MovePosition(-xDir, -yDir);
                index = i;
                //Debug.Log(i);
                return false;
            }
            index = -1;
            return true;
        }
        public bool CheckOverlap(Room r1, Room r2) // 매개변수인 두 방의 겹침 여부 확인.
        {
            Rect rect = r1.Intersect(r2);
            if (rect.Width() > 0 && rect.Height() > 0)
                return true;
            return false;
        }
        public bool CheckOverlap(Room r) // 이 방과 겹치는 방이 있는지 모든 방을 검사함.
        {
            foreach (Room r1 in rooms)
            {
                //if ((r.GetHashCode() == r1.GetHashCode() || !r1.placed) && r1.GetType() != typeof(DownStairsRoom))
                if(r.GetHashCode() == r1.GetHashCode() || !r1.placed)
                    continue;
                Rect rect = r.Intersect(r1);
                if (rect.Width() > 0 && rect.Height() > 0)
                {
                    //Debug.Log("room " + rooms.IndexOf(r) + " Intersects with Room number " + rooms.IndexOf(r1) + ", Intersect Range : " + rect.Width() + ", " + rect.Height());
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
                Rect rect = r.Intersect(r1);
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
        public void PaintRooms()//일단 문은 위치만 잡아놓고 방에 타일 배치를 한다. 이후, 문 부분만 타일을 바꿔 준다.
                                //Painter 클래스에서 문타일 감지하는거보다 이게 코드 사용량이 적음.
        {
            foreach(Room r in rooms)
            {
                PlaceDoors(r);
                r.Paint(this);
                PaintDoors(r);
            }
        }
        public Rect LevelRect()//최상/하/좌/우측을 기준으로 맵을 직사각형화해 저장한다.
                               //빈 공간 채우기나 맵 전체이동 등에 사용.
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
        public void MoveRooms()//고정 위치로 전체 레벨 타일을 이동.
        {
            int xDir = -levelr.x;
            int yDir = -levelr.y;

            levelr.SetPosition(-1, -1);
            foreach(Room r in rooms)
            {
                r.MovePosition(xDir, yDir);
            }
        }

        public abstract Vector2 SpawnPoint();//특정 상황에서 지정 위치에 몹을 소환해야 할 경우 여기에 명시.ex)문을 막고 있는 보스
        
    }
   
}