using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;
using Random = System.Random;


namespace ArcanaDungeon
{
    public class Dungeon : MonoBehaviour
    {
        public static Random random = new Random();

        public GameObject[] Tiles;
        public GameObject[] Mobs;
        public GameObject[] Players;

        public GameObject Player;
        public player Plr;
        
        public List<Level> levels = new List<Level>();
        public List<List<GameObject>> enemies = new List<List<GameObject>>();

        public Level currentlevel;
        public GameObject[] currentMobPool;
        public int changed = 0;
        public int whosTurn = 0;//1 = 플레이어 2 = 몬스터
        public GameObject square;

        public static Dungeon dungeon;

        private void Awake()
        {
            if (dungeon == null)
            {
                dungeon = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (dungeon != this)
                    Destroy(this.gameObject);
            }// 오브젝트 싱글턴화. 시작시 할당되는 자기 자신(씬에 있는 그거)만 유일한 dungeon이다.

            //프리팹 로드
            Tiles = Resources.LoadAll<GameObject>("prefabs/Tiles");
            Mobs = Resources.LoadAll<GameObject>("prefabs/Enemies");
            Players = Resources.LoadAll<GameObject>("prefabs/Player");

            //currentlevel = new AnotherLevel();
            currentlevel = new BossLevel();
            //주석처리 바꿔서 시작레벨 다른거로 테스트 가능.

            currentlevel.Create();
            levels.Add(currentlevel);
            currentlevel.floor = 1;
            PrintLevel();
            SpawnMobs();

            Player = Players[0];
            Player = Instantiate(Player, new Vector2(0, 0), Quaternion.identity) as GameObject;
            Plr = Player.GetComponent<player>();
            Plr.Spawn();
            UI.uicanvas.SetPlr(Player);
            Debug.Log("cur level : " + currentlevel.GetType());

           

            whosTurn = 1;//플레이어 턴

        }
        public void NextLevel()
        {
            //기존에 깔린 판 치우기(어차피 레벨 자체에 맵 정보는 저장되어 있으니 상관없음)
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.name == transform.name)
                    continue;
                Destroy(child.gameObject);
            }
            DeSpawnMobs();
            //내려간 계단 자리 확인
            currentlevel.laststair = new Vector2((int)Plr.transform.position.x, (int)Plr.transform.position.y);

            //새로 판 깔기
            if (levels.IndexOf(currentlevel) == levels.Count - 1)//현재가 마지막층이면, 새 층을 만들어 깐다.
            {
                Level l;
                /*if (currentlevel.GetType() == typeof(RegularLevel))
                {
                    l = new BossLevel();
                }*/
                if (currentlevel.GetType() == typeof(AnotherLevel))
                {
                    l = new BossLevel();
                    //l = new AnotherLevel();
                }
                else if (currentlevel.GetType() == typeof(TestLevel))
                {
                    l = new TestLevel();
                    //l = new AnotherLevel();
                }
                else if (currentlevel.GetType() == typeof(BossLevel))
                {
                    //l = new RegularLevel();
                    l = new AnotherLevel();
                }
                else
                {
                    //l = new RegularLevel();
                    l = new AnotherLevel();
                }

                l.Create();
                levels.Add(l);
                if(l.GetType() == typeof(AnotherLevel))
                    l.floor = currentlevel.floor + 1;
                currentlevel = l;
            }
            else//마지막층이 아니면, 이미 있는 다음층을 깐다.
            {
                currentlevel = levels[currentlevel.floor];
            }
            PrintLevel();
            Plr.Spawn();
            Player.transform.position = Plr.PlayerPos;
            SpawnMobs();
        }//여기 판 갈아주세요 (판 치우고 새로 깔아야 한다.)

        public void PrevLevel()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.name == transform.name)
                    continue;
                Destroy(child.gameObject);
            }
            DeSpawnMobs();

            currentlevel = levels[currentlevel.floor -2];
            PrintLevel();
            Plr.Spawn(new Vector2(currentlevel.laststair.x + 1, currentlevel.laststair.y));
            Player.transform.position = Plr.PlayerPos;
            SpawnMobs();
        }//이전 층으로 갈 때는, 내려갈 때 저장된 계단 위치로 이동한다.
        public void PrintLevel()
        {
            for (int i = 0; i < currentlevel.width; i++)
            {
                for (int j = 0; j < currentlevel.height; j++)
                {
                    GameObject tileObject;
                    int tile = currentlevel.map[i, j];
                    switch (tile)
                    {
                        case Terrain.EMPTY:
                            continue;
                        case Terrain.GROUND:
                            switch (currentlevel.biome)
                            {
                                
                                default:
                                    tileObject = Tiles[Array.FindIndex(Tiles, t => t.name == "FloorTile")];
                                    break;
                            }
                            break;
                        case Terrain.WALL:
                            switch (currentlevel.biome)
                            {
                                
                                default:
                                    tileObject = Tiles[Array.FindIndex(Tiles, t => t.name == "WallTile")];
                                    break;
                            }
                            break;
                        case Terrain.STAIRS_UP:
                            switch (currentlevel.biome)
                            {
                                
                                default:
                                    tileObject = Tiles[Array.FindIndex(Tiles, t => t.name == "Upstairs")];
                                    break;
                            }
                            break;
                        case Terrain.STAIRS_DOWN:
                            switch (currentlevel.biome)
                            {
                                
                                default:
                                    tileObject = Tiles[Array.FindIndex(Tiles, t => t.name == "Downstairs")];
                                    break;
                            }
                            break;
                        case Terrain.DOOR:
                            switch (currentlevel.biome)
                            {
                                
                                default:
                                    tileObject = Tiles[Array.FindIndex(Tiles, t => t.name == "DoorTile")];
                                    break;
                            }
                            break;
                        case Terrain.GENERATOR:
                            tileObject = Tiles[Array.FindIndex(Tiles, t => t.name == "Generator")];
                            break;
                        default:
                            continue;
                    }

                    GameObject newTile = Instantiate(tileObject, new Vector2(i, j), Quaternion.identity) as GameObject;
                    newTile.transform.SetParent(this.transform, false);
                    try
                    {
                        currentlevel.temp_gameobjects[i, j] = newTile;//★나중에 그래픽 표현 방법이랑 좌표 체계 정리해야 한다
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }//현재 레벨의 맵 화면상에 출력
        }
        private void Update()
        {

            if (currentlevel.map[(int)Plr.transform.position.x, (int)Plr.transform.position.y] == Terrain.STAIRS_DOWN && currentlevel.floor < 3 && Plr.MoveTimer <= 0 && currentlevel.locked == false)
            {
                NextLevel();
            }
            if (currentlevel.map[(int)Plr.transform.position.x, (int)Plr.transform.position.y] == Terrain.STAIRS_UP && currentlevel.floor > 1 && Plr.MoveTimer <= 0 && currentlevel.locked == false)
            {
                PrevLevel();
            }
            else if(currentlevel.map[(int)Plr.transform.position.x, (int)Plr.transform.position.y] == Terrain.STAIRS_DOWN && currentlevel.floor == 3 && Plr.MoveTimer <= 0 && currentlevel.locked == false)
            {
                UI.uicanvas.log_add("Clear!!!");
            }
            //턴
            if (Plr.isTurn <= 0)
            {
                
                foreach(GameObject mob in enemies[currentlevel.floor - 1])
                {
                    mob.GetComponent<Enemy>().isTurn += 1;
                }
                Plr.isTurn += 1;
            }
            //if (Ene.isEnemyturn == false)
        }

        public static int distance_cal(Transform a, Transform b)
        {
            //물체의 x좌표값 차이와 y좌표값 차이를 구해서 절댓값을 씌운다.
            int x_gap = Math.Abs((int)(a.position.x - b.position.x));
            int y_gap = Math.Abs((int)(a.position.y - b.position.y));

            //둘을 비교해 더 큰 값을 반환한다, 대각선으로 이동하는 게임 특성 상 그냥 더 큰 쪽이 거리가 된다
            return (x_gap > y_gap ? x_gap : y_gap);
        }
        public static int distance_cal_a(float x1, float y1, float x2, float y2)
        {
            //물체의 x좌표값 차이와 y좌표값 차이를 구해서 절댓값을 씌운다.
            int x_gap = Math.Abs((int)(x1 - x2));
            int y_gap = Math.Abs((int)(y1 - y2));

            //둘을 비교해 더 큰 값을 반환한다, 대각선으로 이동하는 게임 특성 상 그냥 더 큰 쪽이 거리가 된다
            return (x_gap > y_gap ? x_gap : y_gap);
        }

        public void SpawnMobs()//레벨 지형이 깔린 뒤 실행. 일단은 무작위 좌표를 선정하기 때문에 몹 밀도가 불균일하게 나올 수 있다. 시간이 남는다면 방 선정 - 스폰할 몹 수 선정 - 위치 선정의 과정으로 바꿀 것.
        {
            //Debug.Log(currentlevel.floor);
            if (enemies.Count == 0 || enemies.Count == levels.Count -1)//현재 레벨이 처음이면, 몬스터를 조건에 맞게 스폰한다.
            {
                 List<GameObject>enemylist = new  List<GameObject>();
                int count = 0;
                while (count<currentlevel.maxEnemies)
                {
                    GameObject mob;
                    GameObject[] mobs = new GameObject[4];


                    Vector2[] pos = new Vector2[4];
                    switch (currentlevel.biome)
                    {
                        case Biome.BOSS_CRAB:
                            mobs[0] = Mobs[Array.FindIndex(Mobs, m => m.name == "Crabig")];
                            break;
                        case Biome.BOSS_MIMIC:
                            mobs[0] = Mobs[Array.FindIndex(Mobs, m => m.name == "Mimic")];
                            currentlevel.maxEnemies = 3;
                            break;
                        case Biome.BOSS_GNOLL:
                            mobs[0] = Mobs[Array.FindIndex(Mobs, m => m.name == "Ali")];
                            mobs[1] = Mobs[Array.FindIndex(Mobs, m => m.name == "Zera")];
                            currentlevel.maxEnemies = 2; 
                            break;
                        default:
                            mobs[0] = Mobs[Array.FindIndex(Mobs, m => m.name == "Rat")];
                            mobs[1] = Mobs[Array.FindIndex(Mobs, m => m.name == "Gnoll")];
                            mobs[2] = Mobs[Array.FindIndex(Mobs, m => m.name == "Eye")];
                            mobs[3] = Mobs[Array.FindIndex(Mobs, m => m.name == "Slime")];
                            break;
                    }

                    int index = 0;
                    while (true)
                    {
                        int x = random.Next(0, currentlevel.levelr.xMax);
                        int y = random.Next(0, currentlevel.levelr.yMax);
                        if (currentlevel.map[x, y] == Terrain.GROUND)
                        {
                            pos[index] = new Vector2(Mathf.Round(x), Mathf.Round(y));
                            index++;
                            if (index >= mobs.Length)
                                break;
                            if(mobs[index] == null)
                                break;
                        }
                    }
                    for (int u = 0; u < mobs.Length; u++)
                    {
                        if (count < currentlevel.maxEnemies && mobs[u] != null)
                        {
                            enemylist.Add(Instantiate(mobs[u], pos[u], Quaternion.identity));
                            enemylist[enemylist.Count - 1].GetComponent<Enemy>().Initiate();
                            count++;
                        }
                        else
                        {
                            count = currentlevel.maxEnemies;
                            break;
                        }

                    }
                    
                }

                enemies.Add(enemylist);
            }
            else
            {
                foreach(GameObject mob in enemies[currentlevel.floor - 1])
                {
                    mob.SetActive(true);
                }
            }//새 층이 아니라서 존재하는 몬스터 풀이 있을 경우, 디스폰(비활성화)한걸 다시 활성화만 한다.
        }
        public void DeSpawnMobs()
        {
            Debug.Log("Despawned.");
            foreach (GameObject mob in enemies[currentlevel.floor - 1])
            {
               
                mob.SetActive(false); 

            }
        }

        public Enemy find_enemy(float x, float y) {
            foreach (GameObject e in enemies[currentlevel.floor - 1]) {
                if (e.transform.position.x == x & e.transform.position.y == y) {
                    return e.GetComponent<Enemy>();
                }
            }
            return null;
        }

        
    }
}