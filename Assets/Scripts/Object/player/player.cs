using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon;
using ArcanaDungeon.cards;
using ArcanaDungeon.rooms;
using Terrain = ArcanaDungeon.Terrain;



namespace ArcanaDungeon.Object
{
    public class player : Thing
    {
        /*★Card 클래스 제작 후 주석 제거할 것
        Card[] deck;
        Card[] discarded;
        Card[] hand;
         */
        AttackCard atcd = new AttackCard();

        public player me = null;

        private int hand_limit; //패 장수 제한

        public float MovePower = 0.2f;
        public int MoveTimerLimit = 5;
        public SpriteRenderer spriteRenderer;
        public Animator anim;
        public Rigidbody2D rigid;
        public Transform tr;
        public Vector3 movement;

        public Vector2 PlayerPos = new Vector2(0, 0);
        public Vector2 MoveVector = new Vector2(0, 0);
        public Vector2 MousePos = new Vector2(0, 0);
        Camera cam;

        public int MoveTimer = 0;
        int Mou_x = 0;
        int Mou_y = 0;
        bool isMouseMove = false;

        public bool[,] FOV;
        // Start is called before the first frame update
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            tr = GetComponent<Transform>();
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
            if (me == null)
                me = this;

            maxhp = 100;
            maxstamina = 100;
            HpChange(maxhp);
            StaminaChange(maxstamina);
        }
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            tr = GetComponent<Transform>();
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();

            if (me == null)
                me = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTurn > 0)
            {
                if (MoveTimer <= 0)
                    Get_MouseInput(); //마우스 입력

                if (Input.GetButton("Horizontal"))
                    spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

                StaminaChange(5);//★아무 행동도 하지 않으면 회복량이 3배가 되도록 해야 함

            }
        }

        private void FixedUpdate()
        {//입력받는곳
            

            if (Input.GetKey(KeyCode.Q))
            {
                UI.uicanvas.ShowMessage("나는 공격을 했다!!!");

                //atcd.UseCard(Dungeon.dungeon.Ene);
                //Debug.Log("플레이어측 체력" + Dungeon.dungeon.Ene.GetHp());
            }// 임시 키 입력 
            /*if (isTurn == true && isMouseMove == false)
            {

                if (MoveTimer == 0)
                {

                    //left
                    if (Input.GetAxisRaw("Horizontal") == -1 && !anim.GetBool("iswalking"))
                    {

                        PlayerPos = new Vector2(Mathf.Round(transform.position.x - 1), Mathf.Round(transform.position.y));
                        //Debug.Log(PlayerPos);
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x - 1), (int)Mathf.Round(transform.position.y)]] & Terrain.passable) != 0)
                        {

                            anim.SetBool("iswalking", true);
                            MoveTimer = MoveTimerLimit;

                            dir = 1;
                        }
                    }
                    //right
                    else if (Input.GetAxisRaw("Horizontal") == 1 && !anim.GetBool("iswalking"))
                    {



                        PlayerPos = new Vector2(Mathf.Round(transform.position.x + 1), Mathf.Round(transform.position.y));
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x + 1), (int)Mathf.Round(transform.position.y)]] & Terrain.passable) != 0)
                        {

                            anim.SetBool("iswalking", true);
                            MoveTimer = MoveTimerLimit;

                            dir = 2;
                        }

                    }
                    //up
                    else if (Input.GetAxisRaw("Vertical") == 1 && !anim.GetBool("iswalking"))
                    {



                        PlayerPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + 1));
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y + 1)]] & Terrain.passable) != 0)
                        {

                            anim.SetBool("iswalking", true);
                            MoveTimer = MoveTimerLimit;

                            dir = 3;
                        }

                    }
                    //down
                    else if (Input.GetAxisRaw("Vertical") == -1 && !anim.GetBool("iswalking"))
                    {



                        PlayerPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 1));
                        Debug.Log(Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y - 1)] + " , " + Terrain.passable);
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y - 1)]] & Terrain.passable) != 0)
                        {

                            anim.SetBool("iswalking", true);
                            MoveTimer = MoveTimerLimit;

                            dir = 4;
                        }


                    }
                }

            }


            //입력받는곳 끝
            
            //playermove
            switch (dir)
            {
                case 1:
                    MoveVector = new Vector2(transform.position.x - MovePower, transform.position.y);
                    transform.position = MoveVector;
                    if (transform.position.x <= PlayerPos.x)
                    {
                        dir = 0;
                        anim.SetBool("iswalking", false);
                        transform.position = PlayerPos;
                        vision_marker();//★나중에 플레이어의 Turn() 함수가 완성되면 그쪽에서 1번만 실행되게 옮길 것, 아래의 같은 함수들도 동일
                        isTurn = false;            //Debug.Log("2");
                    }
                    break;
                case 2:
                    MoveVector = new Vector2(transform.position.x + MovePower, transform.position.y);
                    transform.position = MoveVector;
                    if (transform.position.x >= PlayerPos.x)
                    {
                        transform.position = PlayerPos;
                        vision_marker();//★
                        dir = 0;
                        anim.SetBool("iswalking", false);
                        isTurn = false;
                    }
                    break;
                case 3:
                    MoveVector = new Vector2(transform.position.x, transform.position.y + MovePower);
                    transform.position = MoveVector;
                    if (transform.position.y >= PlayerPos.y)
                    {
                        transform.position = PlayerPos;
                        vision_marker();//★
                        dir = 0;
                        anim.SetBool("iswalking", false);
                        isTurn = false;
                    }
                    break;
                case 4:
                    MoveVector = new Vector2(transform.position.x, transform.position.y - MovePower);
                    transform.position = MoveVector;
                    if (transform.position.y <= PlayerPos.y)
                    {
                        transform.position = PlayerPos;
                        vision_marker();//★
                        dir = 0;
                        anim.SetBool("iswalking", false);
                        isTurn = false;
                    }
                    break;
                default:
                    break;
            }*/
            if (MoveTimer > 0)
                MoveTimer--;
            if (transform.position.x == Mou_x && transform.position.y == Mou_y )
            {
                isMouseMove = false;
                
            }
          
            else if(MoveTimer <= 0 && isMouseMove == true)
            {
                try //다음 층으로 이동하면 배열 인덱스 범위를 벗어났다는 오류가 뜬다, 아무래도 층을 이동하면서 route_pos에 문제가 생기는 것으로 보인다
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                    MoveTimer = MoveTimerLimit;
                    isTurn -= 1;
                }
                catch (Exception e) { }
            }
           

        }
        private void Get_MouseInput()
        {
            

            if (Input.GetMouseButtonDown(0))
            {
                
                MousePos = Input.mousePosition;
                MousePos = cam.ScreenToWorldPoint(MousePos);
                isMouseMove = true;




                Mou_x = Mathf.RoundToInt(MousePos.x);
                Mou_y = Mathf.RoundToInt(MousePos.y);
                route_BFS(Mou_x, Mou_y);
                
                Debug.Log("x = " + Mou_x +"("+ MousePos.x + ") y = " + Mou_y +"(" + MousePos.y + ")");
            }
        }
        public override void Spawn()
        {
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    if (Dungeon.dungeon.currentlevel.map[i, j] == Terrain.STAIRS_UP)
                    {
                        PlayerPos = new Vector2(i + 1, j);
                        transform.position = new Vector2(i + 1, j);
                        vision_marker();
                        return;
                    }
                    else
                        continue;
                }
            }
            Debug.Log("Cannot find Upstairs.");
        }//맵에 입장 시, 계단 자리에 스폰
        public void Spawn(Vector2 pos)
        {
            PlayerPos = pos;
            transform.position = pos;
            vision_marker();
        }// 특정 좌표로 소환
         //★visionchecker을 먼저 실행해 시야에 보이는 부분을 표시하고, Level에 있는 몬스터 배열을 가져와서 좌표를 비교해 몬스터의 위치도 표시하는 함수
        public override void turn()
        {
            
        }

        private void vision_marker()
        {
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            util.Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);


            //★나중에 단순히 그림자를 씌우고 벗기는 것 이외에 몬스터의 모습을 지우고 다시 나타나게 하는 것까지 넣어줘야 한다, 아니면 그건 몬스터의 vision_searcher에 넣던가
            //프리팹의 RGB값은 0~1 범위로 나타내는 게 기본값같다
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    if (FOV[i, j])
                    {
                        Dungeon.dungeon.currentlevel.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        Dungeon.dungeon.currentlevel.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                    }
                }
            }
        }


        public override void die()
        {

        }
    }
}