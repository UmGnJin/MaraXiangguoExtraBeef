using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        private GameObject Plr; //★플레이어
        private CardSlots cs;   //플레이어에게 들어간 CardSlots 스트립트, SetPlr에서 위의 Plr와 함께 설정

        public Text message;
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
                Debug.Log("shdfg");
        }//싱글턴화

        public void ShowMessage(string msg)//매개변수를 인게임 화면에 메시지로 출력.
        {
            message.text += msg;
            message.text += "\n";
        }

        public void SetPlr(GameObject p) {   //이 스크립트의 Plr에 플레이어를 배정해줌, Dungeon에서 단 1번 실행됨
            this.Plr = p;
            cs = Plr.GetComponent<player>().GetCard();
        }

        public void Update() {
            float temp_hp = Plr.GetComponent<player>().GetHp();
            float temp_st = Plr.GetComponent<player>().GetStamina();
            hp_bar.transform.GetChild(2).GetComponent<Text>().text = temp_hp.ToString();
            hp_bar.transform.GetChild(1).localScale = new Vector2 ( temp_hp / 100, 1);
            st_bar.transform.GetChild(2).GetComponent<Text>().text = temp_st.ToString();
            st_bar.transform.GetChild(1).localScale = new Vector2( temp_st / 100, 1);
            for (int i = 1; i <= cs.GetLimit(); i++) {
                card_ui[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            for (int i = cs.GetLimit()+1; i < 7; i++)
            {
                card_ui[i].GetComponent<Image>().color = new Color(0.5f, 0.3f, 0.3f, 1f);
            }
        }
    }
}