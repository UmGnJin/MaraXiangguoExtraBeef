using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArcanaDungeon;
using ArcanaDungeon.Object;
using ArcanaDungeon.cards;

namespace ArcanaDungeon
{
    public class UI : MonoBehaviour //UI 관리자.
    {
        public static UI uicanvas;

        public GameObject hp_bar;
        public GameObject st_bar;
        public GameObject[] card_ui = new GameObject[8];   //뽑을 카드 더미, 패 6칸, 버릴 카드 더미
        public GameObject[] stat_ui = new GameObject[6]; //상태이상 및 버프
        public GameObject[] button = new GameObject[4];  //휴식, 셔플, 조사, 메뉴
        public GameObject card_on_cursor;   //마우스 커서를 올린 카드를 나타내주는 이미지 UI
        public GameObject cam;  //카메라, 줌인 & 줌아웃과 마우스 커서 좌표를 스크린좌표에서 월드좌표로 바꿀 때 사용

        public string str = "testing";

        private GameObject Plr; //★플레이어
        public Enemy target; //임시로 만든 적 나중에 던전에서 적 배열을 받아와야 할 수도 있음 jgh
        private Deck deck;   //플레이어에게 들어간 Deck 스트립트, SetPlr에서 위의 Plr와 함께 설정
        
        private int selected = -1;  //손패에서 카드를 클릭하면 이 변수에 그 카드의 Hands에서의 인덱스 번호를 저장

        public Text message;

        private Vector2 mpos;   //마우스 좌표

        private const string card_background = "sprites/Card/카드 배경";
        public Sprite temp; //테스트용 임시 이미지

        public void Awake()
        {
            if (uicanvas == null)
            {
                uicanvas = this;
                //DontDestroyOnLoad(this); //씬 전환시 남아있도록 하는 기능.
            }
            else if (uicanvas != this)
                Destroy(this.gameObject);
            //위 코드는 UI 관리자를 싱글턴화하는 코드. 싱글턴 = 이 클래스 객체 단 하나로 유지.

            message = this.GetComponent<Text>();// 인게임 로그 텍스트가 올라갈 부분.
            if (message == null)
            {
                Debug.Log("shdfg");
            }

            card_on_cursor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(card_background);    //마우스 커서를 따라다닐 UI 배경 저장
        }

        public void ShowMessage(string msg)//매개변수를 인게임 화면에 메시지로 출력.
        {
            message.text += msg;
            message.text += "\n";
        }

        public void SetPlr(GameObject p) {   //이 스크립트의 Plr에 플레이어를 배정해줌, Dungeon에서 단 1번 실행됨
            this.Plr = p;
            deck = Plr.GetComponent<player>().allDeck; // 오류 때문에 이전에 있던 deck을 통합한 덱 클래스로 임시로 바꿈 jgh
            if (deck == null && Plr == null)
            {
                Debug.Log("asdf");
            }
        }

        public void FixedUpdate() {
            //플레이어 HP와 스태미나 표시 부분, 사실 체력과 스태미나가 변할 때만 수정하는 게 더 좋지만 코딩의 편의를 위해 다소 타협했다
            float temp_hp = Plr.GetComponent<player>().GetHp();
            float temp_st = Plr.GetComponent<player>().GetStamina();
            if (temp_hp < 0) { temp_hp = 0f; }
            if (temp_st < 0) { temp_st = 0f; }
            hp_bar.transform.GetChild(2).GetComponent<Text>().text = temp_hp.ToString();
            hp_bar.transform.GetChild(1).localScale = new Vector2 ( temp_hp / Plr.GetComponent<player>().maxhp, 1);
            st_bar.transform.GetChild(2).GetComponent<Text>().text = temp_st.ToString();
            st_bar.transform.GetChild(1).localScale = new Vector2( temp_st / Plr.GetComponent<player>().maxstamina, 1);

            /*
            //패 최대 장수 표시 부분, 최대 장수 이내의 칸은 밝은 색으로 / 최대 장수를 초과하는 칸은 어두운 색으로 표시
            for (int i = 1; i <= deck.max_Hand; i++) {
                card_ui[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            for (int i = deck.max_Hand+1; i < 7; i++)
            {
                card_ui[i].GetComponent<Image>().color = new Color(0.5f, 0.3f, 0.3f, 1f);
            }
            */

            mpos = Input.mousePosition; //마우스 커서 위치를 mpos에 저장, 좌표는 World 좌표가 아닌 Screen 좌표로 UI 사용하는 그 좌표일 것임
            GameObject temp_ob;   //아래에서 나올 각종 작업에서 임시로 오브젝트 저장해둘 때 계속 쓸 임시 오브젝트
            int temp_hand_num = (int)mpos.x / 120;  //현재 선택된 카드가 몇 번째 카드인지 저장해두는 변수, 1번~6번이 패

            //손패 좌표 범위에 마우스 커서 좌표가 있으며 현재 마우스가 가리키는 칸에 카드가 있으면 카드의 확대된 이미지를 툴팁처럼 띄움
            if (mpos.x>120 & mpos.x<840 & mpos.y>0 & mpos.y<160 & temp_hand_num<=deck.Hands.Count) {
                card_on_cursor.GetComponent<RectTransform>().localPosition = new Vector3(-900 + temp_hand_num*120, -180, 0);
                card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
                card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
                for (int i=1; i<4; i++) {
                    temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                    temp_ob.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
                    temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition * 2;
                    //temp_hand_num에 따라 deck의 Hands에서 카드 이름, 일러스트, 카드 효과를 가져와 card_on_cursor 자식들한테 저장함
                    switch (i) {
                        case 1:
                            //ob.GetComponent<Text>.text = deck.Hands[temp_hand_num-1]. (Card에 들어있는 카드 이름);break;
                            temp_ob.GetComponent<Text>().text = deck.Hands[temp_hand_num - 1].cardName; break; // 이름 설명 추가 jgh
                        case 2:
                            temp_ob.SetActive(true);
                            temp_ob.GetComponent<Image>().sprite = Resources.Load<Sprite>(deck.Hands[temp_hand_num - 1].illust);break;
                        case 3:
                            //ob.GetComponent<Text>.text = deck.Hands[temp_hand_num-1]. (Card에 들어있는 카드 효과);break;
                            temp_ob.GetComponent<Text>().text = deck.Hands[temp_hand_num - 1].cardInfo; break; // 이름 설명 추가 jgh
                    }
                }
                //손패 좌표 범위 안에서 클릭하면 그 카드를 selected에 저장한다
                //selected==-1 이 없으면 드래그 도중 다른 카드가 selected에 저장된다, GetMouseButtonDown은 if문에 들어가있어서인지 가끔 작동을 안 한다
                if (Input.GetMouseButton(0) & selected==-1) {
                    selected = temp_hand_num - 1;
                }
            }

            //손패 좌표 범위 밖에 마우스 커서 좌표가 있으면 셋 중 하나다 : 확대됐던 카드 이미지 축소하기, 카드 드래그해서 목표 지정하려고 함, 카드 목표 지정 완료하고 사용
            if (mpos.x < 120 | mpos.x > 840 | mpos.y < 0 | mpos.y > 160) {
                //확대된 카드 이미지가 띄워졌고 선택한 카드도 없는 상태라면 그 확대된 이미지를 없앰
                if (card_on_cursor.transform.GetChild(0).gameObject.activeSelf & (selected == -1))
                {
                    card_on_cursor.transform.GetChild(0).gameObject.SetActive(false);
                    //Text를 가진 자식은 text를 null로 하고, Image를 가진 자식은 비활성화
                    card_on_cursor.transform.GetChild(1).gameObject.GetComponent<Text>().text = null;
                    card_on_cursor.transform.GetChild(2).gameObject.SetActive(false);
                    card_on_cursor.transform.GetChild(3).gameObject.GetComponent<Text>().text = null;
                }
                //마우스 왼쪽 버튼이 클릭된 중이며 선택된 카드가 있다면 현재 선택한 카드 이미지가 마우스 커서를 따라다님
                if (Input.GetMouseButton(0) & (selected != -1))
                {
                    card_on_cursor.GetComponent<RectTransform>().localPosition = mpos + new Vector2(-860, -440);
                    card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
                    card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    for (int i = 1; i < 4; i++)
                    {
                        temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                        temp_ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                        temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition;
                        //temp_hand_num에 따라 deck의 Hands에서 카드 이름, 일러스트, 카드 효과를 가져와 card_on_cursor 자식들한테 저장함
                        switch (i)
                        {
                            case 1:
                                temp_ob.GetComponent<Text>().text = deck.Hands[selected].cardName;break;
                            case 2:
                                temp_ob.SetActive(true);
                                temp_ob.GetComponent<Image>().sprite = Resources.Load<Sprite>(deck.Hands[selected].illust); break;
                            case 3:
                                temp_ob.GetComponent<Text>().text = deck.Hands[selected].cardInfo;break;
                        }
                    }
                }
                //마우스 왼쪽 버튼이 클릭되지 않은 상태이며 선택된 카드가 있다면 클릭을 멈춘 좌표에 카드를 사용하기로 결정한 것이다, 사실상 마우스 버튼이 떼질 때 작동한다
                if (!Input.GetMouseButton(0) & (selected != -1))
                {
                    //마우스 커서 좌표를 유니티 내부 world 좌표로 변경해 그 좌표 위의 enemy 찾기
                    Vector3 mpos_world = cam.GetComponent<Camera>().ScreenToWorldPoint(mpos);
                    mpos_world = new Vector3(Mathf.Round(mpos_world.x), Mathf.Round(mpos_world.y), 0);
                    target = null;
                    foreach (GameObject enem in Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor-1]) {
                        if (enem.transform.position == mpos_world) {
                            target = enem.GetComponent<Enemy>();
                        }
                    }
                    //카드 사용 함수 실행
                    //Debug.Log("카드 사용, UsingCard 직전 / 적 체력 : " + target.GetHp() + " / 플레이어 스태미나 :" + Plr.GetComponent<player>().GetStamina());   //enemy를 대상으로 카드를 사용하지 않으면 계속 게임이 멈춰서 잠시 주석처리함
                    int used = deck.UsingCard(selected, Plr.GetComponent<player>(), (Enemy)target );   //패의 몇 번째 인덱스가 사용되었는지, 플레이어 스크립트, enemy 스크립트를 파라미터로 전달하되 만약 적이 없는 곳에 드래그했다면 enemy 스크립트 자리에 null이 전달됨 
                    //Debug.Log("카드 사용 완료됨 / 반환값 : "+used+" / 적 체력 : " + target.GetHp() + "플레이어 스태미나 :" + Plr.GetComponent<player>().GetStamina());  //enemy를 대상으로 카드를 사용하지 않으면 계속 게임이 멈춰서 잠시 주석처리함
                    //사용된 손패 오른쪽의 카드들을 1칸씩 왼쪽으로 이동시키기
                    for (int i = selected + 1; i < deck.max_Hand; i++) {
                        card_ui[i].transform.GetChild(1).GetComponent<Text>().text = card_ui[i + 1].transform.GetChild(1).GetComponent<Text>().text;
                        card_ui[i].transform.GetChild(2).GetComponent<Image>().sprite = card_ui[i + 1].transform.GetChild(2).GetComponent<Image>().sprite;
                        card_ui[i].transform.GetChild(3).GetComponent<Text>().text = card_ui[i + 1].transform.GetChild(3).GetComponent<Text>().text;
                    }
                    card_ui[deck.Hands.Count + 1].transform.GetChild(1).GetComponent<Text>().text = null;
                    card_ui[deck.Hands.Count + 1].transform.GetChild(2).gameObject.SetActive(false);
                    card_ui[deck.Hands.Count + 1].transform.GetChild(3).GetComponent<Text>().text = null;
                    selected = -1;

                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("draw testing2");
                deck.DrawCards(); card_draw(deck.Hands[deck.Hands.Count - 1]);
            }
        }

        public void card_draw(Cards c) {
            int temp = deck.Hands.Count;
            card_ui[temp].transform.GetChild(1).GetComponent<Text>().text = c.cardName;
            card_ui[temp].transform.GetChild(2).gameObject.SetActive(true);
            card_ui[temp].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(c.illust);
            card_ui[temp].transform.GetChild(3).GetComponent<Text>().text = c.cardInfo;
        }
    }
}