using ArcanaDungeon.util;
using System.Collections.Generic;
using System;
using ArcanaDungeon.rooms;
using ArcanaDungeon.painters;
using ArcanaDungeon.Object;
using UnityEngine;
using Rect = ArcanaDungeon.util.Rect;
using System.Linq;
using Random = System.Random;

namespace ArcanaDungeon
{
    public class AnotherLevel : Level
    {
        public override void InitRooms()
        {
            levelsize = LevelSize.SMALL;
            rooms = new List<Room>();
            rooms.Add(new UpStairsRoom());
            rooms.Add(new EmptyRoom());
            

            PlaceRooms();
            AddBranches();
            AddAnotherBranches();
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
        public override void PlaceRooms()//일직선으로 
        {
            Room ent = rooms[0];
            ent.SetPosition(0, 0);
            ent.placed = true;
            double startangle = rand.Next(0, 3) * (Math.PI / 2);
            ent.angle = startangle;
            //Debug.Log(Math.Sin(startangle) + " " + Math.Cos(startangle));
            int n = 0;
            
            while(n < rooms.Count)
            {
                Room r2 = rooms[n];
                if (r2.GetHashCode() == ent.GetHashCode() || r2.placed == true)
                {
                    n++;
                    continue;
                }

                r2.SetPosition(ent.x, ent.y);
                int xOrigin = r2.x;
                int yOrigin = r2.y;
                int count = 1;

                while(CheckOverlap(r2))
                {
                    r2.SetPosition(xOrigin + (int)(count * Math.Sin(ent.angle)), yOrigin + (int)(count * Math.Cos(ent.angle)));
                    //Debug.Log(r2.Info());
                    count++;
                    if (count > 200)//만약에 방 겹침이나 무한루프 이슈 발생시, count 값을 늘려볼 것.
                    {
                        Debug.Log("ERR");
                        break;
                    }
                }

                count = 0;
                int max = 0;
                //if (r2.IsNeighbour(ent))
                    //Debug.Log("main root placed.");
                while (!r2.IsNeighbour(ent))
                {
                    Rect rect = r2.Intersect(ent);
                    int xDir = 0, yDir = 0;

                    if (rect.Width() < 0)
                        xDir = -rect.Width();
                    else if (rect.Width() > 0 && rect.Width() < 3)
                        xDir = 3 - rect.Width();

                    if (rect.Height() < 0)
                        yDir = -rect.Height();
                    else if (rect.Height() > 0 && rect.Height() < 3)
                        yDir = 3 - rect.Height();

                    if (rect.Width() == 0 && rect.Height() == 0)
                    {
                        //Debug.Log(".");
                        bool hor = rand.Next(2) == 0 ? true : false;
                        if (hor)
                            xDir = 3;
                        else
                            yDir = 3;
                    }
                    if (r2.y > ent.y)
                        yDir *= -1;
                    if (r2.x > ent.x)
                        xDir *= -1;

                    int index = -1;
                    if (MoveRoom(r2, xDir, yDir, out index) && r2.IsNeighbour(ent))
                    {
                        //Debug.Log("main root placed.");
                        //rect = r.Intersect(r2, ent);
                        break;
                    }
                    if (max++ > 2)
                    {
                        if (count++ > rooms.Count)
                        {
                            Debug.Log("failed to attach room " + rooms.IndexOf(r2) + ", " + rect.Width() + " " + rect.Height());
                            break;
                        }
                        if (index != -1)
                            ent = rooms[index];
                        max = 0;
                        continue;
                    }
                }

                r2.placed = true;
                if (rooms.Count - 1 < (int)levelsize)
                {
                    Room rm = new EmptyRoom();
                    rooms.Add(rm);                   
                }
                else if(rooms.Count - 1 == (int)levelsize && r2.GetType() != typeof(DownStairsRoom))
                {
                    Room d = new DownStairsRoom();
                    rooms.Add(d);
                }
                r2.angle = ent.angle;
                ent = r2;
                n++;
            }

        }
        public void AddBranches()
        {
            int n = 0;
            int inverse = 1;
            while (n < rooms.Count)
            {
                Room r = rooms[n];
                if(r.GetType() != typeof(EmptyRoom))
                {
                    n++;                 
                    continue;
                }

                double bangle = r.angle + inverse * (Math.PI / 2);
                inverse *= -1;
                Room room = new BranchRoom(true);
                if (r.angle == Math.PI /2 || r.angle == Math.PI * 3 / 2)
                {
                    if(room.width > room.height)
                    {
                        room.Set(room.y, room.x, room.yMax, room.xMax);
                    }
                }
                else if (r.angle == Math.PI || r.angle == 0)
                {
                    if (room.width < room.height)
                    {
                        room.Set(room.y, room.x, room.yMax, room.xMax);
                    }
                }
                rooms.Add(room);
                room.SetPosition(r.x, r.y);
                int xOrigin = room.x;
                int yOrigin = room.y;
                int count = 1;

                while (CheckOverlap(room))
                {
                    if (r.angle == Math.PI / 2 || r.angle == Math.PI * 3 / 2)
                        room.SetPosition(xOrigin, yOrigin + (int)(count * Math.Cos(bangle)));
                    else if (r.angle == Math.PI || r.angle == 0)
                        room.SetPosition(xOrigin + (int)(count * Math.Sin(bangle)), yOrigin);
                    //Debug.Log(room.Info());
                    count++;
                    if (count > 200)//만약에 방 겹침이나 무한루프 이슈 발생시, count 값을 늘려볼 것.
                    {
                        Debug.Log("ERR");
                        rooms.Remove(room);
                        break;
                    }
                }

                count = 0;
                int max = 0;
                if (room.IsNeighbour(r))
                {
                    //Debug.Log("1st branch placed.");
                    room.placed = true;
                    room.angle = bangle;
                }
                while(!room.IsNeighbour(r))
                {
                    Rect rect = room.Intersect(r);
                    int xDir = 0, yDir = 0;

                    if (rect.Width() < 0)
                        xDir = -rect.Width();
                    else if (rect.Width() > 0 && rect.Width() < 3)
                        xDir = 3 - rect.Width();

                    if (rect.Height() < 0)
                        yDir = -rect.Height();
                    else if (rect.Height() > 0 && rect.Height() < 3)
                        yDir = 3 - rect.Height();

                    if (rect.Width() == 0 && rect.Height() == 0)
                    {
                        //Debug.Log(".");
                        bool hor = rand.Next(2) == 0 ? true : false;
                        if (hor)
                            xDir = 3;
                        else
                            yDir = 3;
                    }
                    if (room.y > r.y)
                        yDir *= -1;
                    if (room.x > r.x)
                        xDir *= -1;

                    int index = -1;
                    if (MoveRoom(room, xDir, yDir, out index) && room.IsNeighbour(r))
                    {
                        //rect = r.Intersect(room, r);
                        //Debug.Log("1st branch placed.");
                        room.placed = true;
                        room.angle = bangle;
                        break;
                    }
                    if (max++ > 2)
                    {
                        if (count++ > rooms.Count)
                        {
                            Debug.Log("failed to attach room " + rooms.IndexOf(room) + ", " + rect.Width() + " " + rect.Height() + ", " + room.Info() + " at First branch.");
                            rooms.Remove(room);
                            break;
                        }
                        if (index != -1)
                            r = rooms[index];
                        max = 0;
                        continue;
                    }
                }
                n++;
            }
        }
        public void AddAnotherBranches()
        {
            int n = 0;
            int max_branches = (int)levelsize / 2;
            int branches = 0;
            while (n < rooms.Count && branches <= max_branches)
            {
                BranchRoom r;
                if (rooms[n].GetType() == typeof(BranchRoom))
                    r = (BranchRoom)rooms[n];
                else
                {
                    n++;
                    continue;
                }
                if (r.first == false)
                {
                    n++;
                    continue;
                }
                //Debug.Log(r.angle);
                BranchRoom room = new BranchRoom(false);
                rooms.Add(room);
                //Debug.Log("Add start.");
                room.SetPosition(r.x, r.y);
                int xOrigin = room.x;
                int yOrigin = room.y;
                int count = 1;

                while (CheckOverlap(room))
                {
                    if (r.angle == Math.PI / 2 || r.angle == Math.PI * 3 / 2)
                        room.SetPosition(xOrigin + (int)(count * Math.Sin(r.angle)), yOrigin);
                    else if (r.angle == Math.PI || r.angle == 0)
                        room.SetPosition(xOrigin, yOrigin + (int)(count * Math.Cos(r.angle)));
                    //Debug.Log(room.Info());
                    count++;
                    if (count > 200)//만약에 방 겹침이나 무한루프 이슈 발생시, count 값을 늘려볼 것.
                    {
                        Debug.Log("ERR");
                        rooms.Remove(room);
                        break;
                    }
                }

                count = 0;
                int max = 0;
                if (room.IsNeighbour(r))
                {
                    //Debug.Log("2nd branch placed.");
                    room.Connect(r);
                    room.placed = true;
                    branches++;
                }
                while (!room.IsNeighbour(r))
                {
                    Rect rect = room.Intersect(r);
                    int xDir = 0, yDir = 0;

                    if (rect.Width() < 0)
                        xDir = -rect.Width();
                    else if (rect.Width() > 0 && rect.Width() < 3)
                        xDir = 3 - rect.Width();

                    if (rect.Height() < 0)
                        yDir = -rect.Height();
                    else if (rect.Height() > 0 && rect.Height() < 3)
                        yDir = 3 - rect.Height();

                    if (rect.Width() == 0 && rect.Height() == 0)
                    {
                        bool hor = rand.Next(2) == 0 ? true : false;
                        if (hor)
                            xDir = 3;
                        else
                            yDir = 3;
                    }
                    if (room.y > r.y)
                        yDir *= -1;
                    if (room.x > r.x)
                        xDir *= -1;

                    int index = -1;
                    if (MoveRoom(room, xDir, yDir, out index) && room.IsNeighbour(r))
                    {
                        //Debug.Log("2nd Branch added.");
                        room.Connect(r);
                        room.placed = true;
                        branches++;
                        break;
                    }
                    else if (max++ > 2)
                    {
                        if (count++ > rooms.Count)
                        {
                            Debug.Log("failed to attach room " + rooms.IndexOf(room) + "from " + rooms.IndexOf(r) + ". " + rect.Width() + " " + rect.Height() + ", " + room.Info());
                            rooms.Remove(room);
                            break;
                        }
                        max = 0;
                        continue;
                    }
                    
                }
                n++;
            }
        }
        public override void SpawnMobs()
        {
            
        }
        public override Vector2 SpawnPoint()
        {
            return new Vector2();
        }
    }

}

