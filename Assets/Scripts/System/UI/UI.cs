using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public GameObject small_tool_tip; //툴팁
        public GameObject blood_par;  //임시 파티클 담당, 나중에 몇 개 더 추가되지 않을까 싶다
        public GameObject fire_par;  //임시 파티클 담당, 나중에 몇 개 더 추가되지 않을까 싶다
        public GameObject poison_par;  //임시 파티클 담당, 나중에 몇 개 더 추가되지 않을까 싶다
        public GameObject range;    //사거리 표시 담당
        public GameObject Cardlist_scroll;  //Cardlist_Panel에서 카드를 선택/나열할 때 사용되는 스크롤뷰
        public GameObject Cardlist_panel;   //Cardlist_Panel에서 카드를 선택/나열할 때 사용되는 패널
        public GameObject Cardlist_card;    //Cardlist_Panel에서 카드를 선택/나열할 때 사용되는 카드 이미지, 평소엔 없지만 카드를 획득할 때마다 Instantiate된 후 카드 목록을 열람할 때 이미지를 할당해 사용됨
        public GameObject Cardlist_cancel;  //Cardlist_Panel에서 카드를 선택/나열할 때 사용되는 취소 버튼, 패널이 스크롤 될 때 움직이면 안 돼서 따로 저장되어 있음

        private GameObject Plr; //★플레이어
        private LineRenderer line;
        private Deck deck;   //플레이어에게 들어간 Deck 스트립트, SetPlr에서 위의 Plr와 함께 설정
        public Text log;  //로그

        private int selected = -1;  //손패에서 카드를 클릭하면 이 변수에 그 카드의 Hands에서의 인덱스 번호를 저장
        public Cards selected2; //Cardlist_Panel에서 카드를 선택할 때 선택된 카드를 나타냄
        private bool selecting; //Cardlist_Panel에서 카드를 선택할 때 카드를 선택 중인 상태를 나타냄
        private int wii; //ui에서 어느 부분이 변경되어야 하는지 나타낸다
        //-1=기본값, 변경될 것 없음 / 0 : 모든 ui 요소 초기화 / 1 : 패의 카드 확대 / 2 : 선택한 카드 사거리 표시 (카드가 커서 따라다니는 건 update에 구현) / 3 : 상태이상 툴팁 표시

        private Vector2 mpos;   //마우스 좌표

        private const string card_background = "sprites/Card/카드 배경";

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

            card_on_cursor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(card_background);    //마우스 커서를 따라다닐 UI 배경 저장
            wii = 0;
            selected = -1;
            selected2 = null;
            selecting = false;
            line = this.gameObject.GetComponent<LineRenderer>();
        }
        public void Start() {
            //Cardlist_card 버튼을 게임 시작 시 뽑을 카드 더미만큼 생성, 생성자에서는 스크립팅 API에 접근할 수 없다며 실행되지 않는댄다 나참
            for (int i = 0; i < deck.showDeckList().Count; i++)
            {
                Create_Cardlist_card();
            }
        }

        public void SetPlr(GameObject p) {   //이 스크립트의 Plr에 플레이어를 배정해줌, Dungeon에서 단 1번 실행됨
            this.Plr = p;
            deck = Plr.GetComponent<player>().allDeck; // 오류 때문에 이전에 있던 deck을 통합한 덱 클래스로 임시로 바꿈 jgh
            if (deck == null && Plr == null)
            {
                Debug.Log("asdf");
            }
        }

        public void Update() {

            mpos = Input.mousePosition; //마우스 커서 위치를 mpos에 저장, 좌표는 World 좌표가 아닌 Screen 좌표로 UI 사용하는 그 좌표일 것임
            int temp_hand_num = (int)mpos.x / 120;  //현재 선택된 카드가 몇 번째 카드인지 저장해두는 변수, 1번~6번이 패
            GameObject temp_ob;   //아래에서 나올 각종 작업에서 임시로 오브젝트 저장해둘 때 계속 쓸 임시 오브젝트

            //wii 조정하는 부분
            if (mpos.x > 120 & mpos.x < 840 & mpos.y > 0 & mpos.y < 160 & temp_hand_num <= deck.Hands.Count)
            {
                wii = 1;    //손패의 카드를 확대하라는 wii값
                //손패 좌표 안에서 클릭하면 그 카드 번호를 selected에 저장한다, selected==-1 이 없으면 드래그 도중 다른 카드가 selected에 저장된다, GetMouseButtonDown은 if문에 들어가있어서인지 가끔 작동을 안 한다
                if (Input.GetMouseButton(0) & selected == -1) { selected = temp_hand_num - 1; }
                if (!Input.GetMouseButton(0)) { selected = -1; }
            }
            //손패 좌표 밖에 마우스 커서가 있을 때를 나타낸다, 이 때 처리될 수 있는 것은 확대된 카드 이미지 없앰, 선택한 카드 이미지가 커서를 따라다님, 선택한 카드 사용 이렇게 3가지다. 
            if (mpos.x < 120 | mpos.x > 840 | mpos.y < 0 | mpos.y > 160)
            {
                //selected가 다음 조건을 만족하지 않으면 카드를 선택하지 않은 채 손패 좌표를 벗어난 것이다
                if ((selected > -1) & (selected < deck.max_Hand))
                {
                    //마우스 왼쪽 버튼이 클릭된 중이며 선택한 카드 이미지가 마우스 커서를 따라다님, 카드 사거리 표시는 wii 처리에 맡긴다
                    if (Input.GetMouseButton(0))
                    {
                        card_on_cursor.GetComponent<RectTransform>().localPosition = mpos + new Vector2(-860, -440);
                        card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
                        card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                        wii = 2;    //카드 사거리를 표시하라는 wii값
                        for (int i = 1; i < 4; i++)
                        {
                            temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                            temp_ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                            temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition;
                            //selected에 따라 deck의 Hands에서 카드 이름, 일러스트, 카드 효과를 가져와 card_on_cursor 자식들한테 저장함
                            switch (i)
                            {
                                case 1:
                                    temp_ob.GetComponent<Text>().text = deck.Hands[selected].cardName; break;
                                case 2:
                                    temp_ob.SetActive(true);
                                    temp_ob.GetComponent<Image>().sprite = Resources.Load<Sprite>(deck.Hands[selected].illust); break;
                                case 3:
                                    temp_ob.GetComponent<Text>().text = deck.Hands[selected].cardInfo; break;
                            }
                        }
                    }
                    //마우스 왼쪽 버튼이 클릭되지 않으면  클릭을 멈춘 좌표에 카드를 사용하기로 결정한 것이다, 선택 카드의 사거리를 충족하는지 확인 후 사용처리한다
                    else
                    {
                        //마우스 커서 좌표를 유니티 내부 world 좌표로 변경해 그 좌표 위의 enemy 찾기
                        Vector3 mpos_world = cam.GetComponent<Camera>().ScreenToWorldPoint(mpos);
                        mpos_world = new Vector3(Mathf.Round(mpos_world.x), Mathf.Round(mpos_world.y), 0);
                        //해당 좌표가 사용할 카드의 사거리 이내이며, 전체 맵 내부의 좌표이면 사용
                        if (mpos_world.x >= Plr.transform.position.x - deck.Hands[selected].getRange() & mpos_world.x <= Plr.transform.position.x + deck.Hands[selected].getRange() &
                            mpos_world.x >= 0 & mpos_world.x <= Dungeon.dungeon.currentlevel.width & mpos_world.y >= 0 & mpos_world.x <= Dungeon.dungeon.currentlevel.height)
                        {
                            //패의 몇 번째 인덱스가 사용되었는지, 플레이어 스크립트, enemy 스크립트를 파라미터로 전달하되 만약 적이 없는 곳에 드래그했다면 enemy 스크립트 자리에 null이 전달됨
                            deck.UsingCard(selected, Plr.GetComponent<player>(), Dungeon.dungeon.find_enemy(mpos_world.x, mpos_world.y));
                            Plr.GetComponent<player>().condition_process(); //★플레이어 스크립트에서 처리하게 옮길 것
                            //사용된 손패 오른쪽의 카드들을 1칸씩 왼쪽으로 이동시키기
                            for (int i = selected + 1; i < deck.max_Hand; i++)
                            {
                                card_ui[i].transform.GetChild(1).GetComponent<Text>().text = card_ui[i + 1].transform.GetChild(1).GetComponent<Text>().text;
                                card_ui[i].transform.GetChild(2).GetComponent<Image>().sprite = card_ui[i + 1].transform.GetChild(2).GetComponent<Image>().sprite;
                                card_ui[i].transform.GetChild(3).GetComponent<Text>().text = card_ui[i + 1].transform.GetChild(3).GetComponent<Text>().text;
                            }
                            card_ui[deck.Hands.Count + 1].transform.GetChild(1).GetComponent<Text>().text = null;
                            card_ui[deck.Hands.Count + 1].transform.GetChild(2).gameObject.SetActive(false);
                            card_ui[deck.Hands.Count + 1].transform.GetChild(3).GetComponent<Text>().text = null;
                        }
                        selected = -1;
                        wii = 0;    //ui 초기화하는 wii값
                    }
                }
                //손패 좌표 밖에 있으면서 카드가 선택되지 않았고 card_on_cursor가 활성화된 상태라면 ui를 초기화한다
                else if (card_on_cursor.transform.GetChild(0).gameObject.activeSelf){ wii = 0; }
            }
            //상태이상 확인 구역으로 커서 올림
            if (mpos.x > 1440 & mpos.x < 1680 & mpos.y > 0 & mpos.y < 160) { wii = 3; }

            //wii에 따른 ui 처리 부분
            if (wii == 0) { AllReset(); wii = -1; }   //ui 요소 싸그리 비활성화시키는 부분
            if (wii == 1) { CardZoom(temp_hand_num); wii = -1; } //패에 커서를 올린 카드 확대
            if (wii == 2) { RangeDisplay(deck.Hands[selected].getRange()); wii = -1; }  //현재 선택한 카드의 사거리 표시
            if (wii == 3) { ConditionTooltip((int)(Mathf.Floor((mpos.x - 1440) / 80) + 3 * Mathf.Floor((160 - mpos.y) / 80))); wii = -1; }   //상태이상 툴팁 표시

            //원거리 공격을 표시할 때 사용되는 linerenderer 투명하게 만들기
            if (line.startColor[3] > 0f)
            {
                line.SetColors(new Color(1f, 1f, 1f, line.startColor[3] - 0.03f), new Color(1f, 1f, 1f, line.endColor[3] - 0.03f));
            }

            if (Input.GetKey(KeyCode.Space) & deck.Hands.Count < deck.max_Hand)//★추후 삭제할 것
            {
                deck.DrawCards(); card_draw(deck.Hands[deck.Hands.Count - 1]);
            }
        }

        public void SetWii(int a) {
            wii = a;
        }

        private void AllReset() {
            //패 최대 장수 표시 부분, 최대 장수를 초과하는 칸은 어두운 붉은 색으로 표시
            for (int i = 1; i <= deck.max_Hand; i++) { card_ui[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f); }
            for (int i = deck.max_Hand + 1; i < 7; i++) { card_ui[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.3f, 0.3f, 1f); }
            //card_on_cursor 초기화, 카드 확대 및 커서 따라다니는 이미지에 사용됨, Text를 가진 자식은 text를 null로 하고, Image를 가진 자식은 비활성화
            card_on_cursor.transform.GetChild(0).gameObject.SetActive(false);
            card_on_cursor.transform.GetChild(1).gameObject.GetComponent<Text>().text = null;
            card_on_cursor.transform.GetChild(2).gameObject.SetActive(false);
            card_on_cursor.transform.GetChild(3).gameObject.GetComponent<Text>().text = null;
            //small_tool_tip 초기화, 상태이상 툴팁에 사용됨
            small_tool_tip.SetActive(false);
            small_tool_tip.transform.GetChild(0).gameObject.GetComponent<Text>().text = null;
            //range 초기화, 사거리 표시에 사용ehla
            range.SetActive(false);
        }
        private void CardZoom(int th) { //커서를 올린 패의 카드 이미지 확대
            GameObject temp_ob = null;
            card_on_cursor.GetComponent<RectTransform>().localPosition = new Vector3(-900 + th * 120, -180, 0);
            card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
            card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
            for (int i = 1; i < 4; i++)
            {
                temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                temp_ob.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
                temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition * 2;
                //th에 따라 deck의 Hands에서 카드 이름, 일러스트, 카드 효과를 가져와 card_on_cursor 자식들한테 저장함
                switch (i)
                {
                    case 1:
                        //ob.GetComponent<Text>.text = deck.Hands[th-1]. (Card에 들어있는 카드 이름);break;
                        temp_ob.GetComponent<Text>().text = deck.Hands[th - 1].cardName; break; // 이름 설명 추가 jgh
                    case 2:
                        temp_ob.SetActive(true);
                        temp_ob.GetComponent<Image>().sprite = Resources.Load<Sprite>(deck.Hands[th - 1].illust); break;
                    case 3:
                        //ob.GetComponent<Text>.text = deck.Hands[th-1]. (Card에 들어있는 카드 효과);break;
                        temp_ob.GetComponent<Text>().text = deck.Hands[th - 1].cardInfo; break; // 이름 설명 추가 jgh
                }
            }
        }
        private void RangeDisplay(int r) {  //선택한 카드의 사거리 표시
            range.GetComponent<RectTransform>().sizeDelta = new Vector2((2 * r + 1) * 55, (2 * r + 1) * 55);
            range.SetActive(true);
        }
        private void ConditionTooltip(int sn) { //플레이어의 상태이상 툴팁 표시
            if (Plr.GetComponent<player>().GetCondition().Count >= sn + 1)
            {
                small_tool_tip.transform.localPosition = new Vector3(520 + sn % 3 * 80, -240, 0);
                small_tool_tip.SetActive(true);
                small_tool_tip.transform.GetChild(0).gameObject.GetComponent<Text>().text = Resources.Load<TextAsset>("condition_tooltip").text + Plr.GetComponent<player>().GetCondition()[sn];    //★
            }
        }

        public void GaugeChange()
        {    //플레이어의 HP/스태미나/방어도 갱신
            if (this.Plr == null) { return; }   //가장 처음에 player를 생성하고 maxhp를 설정할 때 호출되면서 Plr가 null인 채로 오류가 날 때가 있다
            float temp_hp = Plr.GetComponent<player>().GetHp();
            float temp_st = Plr.GetComponent<player>().GetStamina();
            if (temp_hp < 0) { temp_hp = 0f; }
            if (temp_st < 0) { temp_st = 0f; }
            hp_bar.transform.GetChild(2).GetComponent<Text>().text = temp_hp.ToString();
            hp_bar.transform.GetChild(1).localScale = new Vector2(temp_hp / Plr.GetComponent<player>().maxhp, 1);
            st_bar.transform.GetChild(2).GetComponent<Text>().text = temp_st.ToString();
            st_bar.transform.GetChild(1).localScale = new Vector2(temp_st / Plr.GetComponent<player>().maxstamina, 1);
            if (Plr.GetComponent<player>().GetBlock() > 0)
            {
                hp_bar.transform.GetChild(3).GetComponent<Text>().text = Plr.GetComponent<player>().GetBlock().ToString();
            }
            else
            {
                hp_bar.transform.GetChild(3).GetComponent<Text>().text = null;
            }
        }

        public void log_add(string str) {
            log.text = log.text.Remove(0, log.text.IndexOf('\n')+1) + "\n"+ str;
        }

        public void card_draw(Cards c) {
            int temp = deck.Hands.Count;
            card_ui[temp].transform.GetChild(1).GetComponent<Text>().text = c.cardName;
            card_ui[temp].transform.GetChild(2).gameObject.SetActive(true);
            card_ui[temp].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(c.illust);
            card_ui[temp].transform.GetChild(3).GetComponent<Text>().text = c.cardInfo;
        }

        public void Card_Select(int which) {
            //which는 어떤 카드 더미 목록을 보여줄지 나타냄, 0=플레이어의 모든 카드 / 1=뽑을 카드 더미 / 2=버린 카드 더미

            int temp1 = 0;
            List<GameObject> temp2 = new List<GameObject>();
            Transform temp3 = null;
            //뽑을 카드 더미를 temp2에 저장
            if (which == 0 | which == 1) {
                foreach (Cards c in deck.showDeckList())
                {
                    temp3 = Cardlist_panel.transform.GetChild(temp1);
                    temp3.GetChild(0).gameObject.GetComponent<Text>().text = c.cardName;
                    temp3.GetChild(1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(c.illust);
                    temp3.GetChild(2).gameObject.GetComponent<Text>().text = c.cardInfo;
                    temp3.gameObject.GetComponent<Cardlist_select>().SetCOT(c);
                    temp2.Add(temp3.gameObject);
                    temp1++;
                }
            }
            if (which == 0 | which == 2)
            {
                foreach (Cards c in deck.showUsedList())
                {
                    temp3 = Cardlist_panel.transform.GetChild(temp1);
                    temp3.GetChild(0).gameObject.GetComponent<Text>().text = c.cardName;
                    temp3.GetChild(1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(c.illust);
                    temp3.GetChild(2).gameObject.GetComponent<Text>().text = c.cardInfo;
                    temp3.gameObject.GetComponent<Cardlist_select>().SetCOT(c);
                    temp2.Add(temp3.gameObject);
                    temp1++;
                }
            }

            //temp2를 카드 이름에 따라 정렬, 이거 안 하면 뽑을 카드 더미에서 다음에 뽑을 카드가 뭔지 다 보인다
            temp2 = temp2.OrderBy(x => x.GetComponent<Cardlist_select>().GetCOT().cardName).ToList();

            Debug.Log("temp1 : " + temp1);
            //Cardlist_panel의 height를 temp2의 인덱스 개수에 따라 조절하고 활성화, temp2의 인덱스를 배치하고 활성화해 목록처럼 보이게 만들기
            temp1 = Mathf.Max(420 * (temp1 / 5 + 1) + 20, 1080);    //임시로 아까 쓰던 temp 돌려 씀
            Cardlist_panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, temp1); //Scroll View에서 Content의 크기가 Viewport보다 크지 않으면 스크롤이 이뤄지지 않음
            Cardlist_panel.GetComponent<RectTransform>().localPosition = new Vector2(0, -temp1 * 0.5f + 540);
            Cardlist_scroll.SetActive(true);
            Cardlist_cancel.SetActive(true);   //닫기 버튼
            temp1 = 0;
            foreach (GameObject g in temp2)
            {
                g.GetComponent<RectTransform>().localPosition = new Vector2(- 760 + 320 * (temp1 % 5), Cardlist_panel.GetComponent<RectTransform>().rect.height*0.5f - 220 - 420 * (temp1 / 5));
                g.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    g.transform.GetChild(i).gameObject.SetActive(true);
                }
                temp1++;
            }
            //플레이어가 입력할 때까지 실행 보류
            selecting = true;

            //놀랍게도 Time.timeScale=0f가 되면 실행 중이던 함수라도 update와 연관되어 있지 않으면 모조리 멈춘다, 덕분에 아래 부분이 보류될 수 있다
            Debug.Log("카드셀렉트 종료!");
        }
        public void SetSelected2(Cards c) {
            selected2 = c;
            //Card_select에서 사용된 모든 오브젝트 비활성화하고 숨기기
            Cardlist_scroll.SetActive(false);
            Cardlist_cancel.SetActive(false);
            foreach (Transform child in Cardlist_panel.transform) {
                child.gameObject.SetActive(false);
            }
            selecting = false;
        }
        public Cards GetSelected2()
        {
            return selected2;
        }
        public bool GetSelecting() {
            return selecting;
        }
        public void Create_Cardlist_card() {
            GameObject temp = Instantiate(this.Cardlist_card) as GameObject;
            temp.SetActive(false);
            temp.transform.SetParent(Cardlist_panel.transform, false);
        }

        public void blood(Vector3 pos) {
            Vector2 temp_pos = cam.GetComponent<Camera>().WorldToScreenPoint(pos);
            blood_par.transform.localPosition = new Vector2(temp_pos.x - 960, temp_pos.y - 540);
            blood_par.GetComponent<ParticleSystem>().Play();
        }
        public void fire(Vector3 pos)
        {
            Vector2 temp_pos = cam.GetComponent<Camera>().WorldToScreenPoint(pos);
            fire_par.transform.localPosition = new Vector2(temp_pos.x - 960, temp_pos.y - 540);
            fire_par.GetComponent<ParticleSystem>().Play();
        }
        public void poison(Vector3 pos)
        {
            poison_par.transform.position = pos;
            poison_par.GetComponent<ParticleSystem>().Play();
        }
        public void range_shot(GameObject a, GameObject b) {
            line.SetPosition(0, new Vector3(a.transform.position.x, a.transform.position.y, -1));
            line.SetPosition(1, new Vector3(b.transform.position.x, b.transform.position.y, -1));
            line.SetColors(new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 1f));
        }
    }
}