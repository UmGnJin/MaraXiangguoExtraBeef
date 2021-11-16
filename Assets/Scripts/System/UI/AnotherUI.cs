using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArcanaDungeon.Object;
using ArcanaDungeon.cards;

namespace ArcanaDungeon
{
    public class AnotherUI : MonoBehaviour //UI ������.
    {
        public static AnotherUI uicanvas;

        public GameObject hp_bar;
        public GameObject st_bar;
        public GameObject[] card_ui = new GameObject[8];   //���� ī�� ����, �� 6ĭ, ���� ī�� ����
        public GameObject[] stat_ui = new GameObject[6]; //�����̻� �� ����
        public GameObject[] button = new GameObject[4];  //�޽�, ����, ����, �޴�

        private GameObject Plr; //���÷��̾�
        private AnotherDeck deck;   //�÷��̾�� �� CardSlots ��Ʈ��Ʈ, SetPlr���� ���� Plr�� �Բ� ����

        public Text message;
        public void Awake()
        {
            if (uicanvas == null)
            {
                uicanvas = this;
                //DontDestroyOnLoad(this); //�� ��ȯ�� �����ֵ��� �ϴ� ���.
            }
            else if (uicanvas != this)
                Destroy(this.gameObject);
            //�� �ڵ�� UI �����ڸ� �̱���ȭ�ϴ� �ڵ�. �̱��� = �� Ŭ���� ��ü �� �ϳ��� ����.

            message = this.GetComponent<Text>();// �ΰ��� �α� �ؽ�Ʈ�� �ö� �κ�.
            if (message == null)
                Debug.Log("shdfg");
        }//�̱���ȭ

        public void ShowMessage(string msg)//�Ű������� �ΰ��� ȭ�鿡 �޽����� ���.
        {
            message.text += msg;
            message.text += "\n";
        }

        public void SetPlr(GameObject p)
        {   //�� ��ũ��Ʈ�� Plr�� �÷��̾ ��������, Dungeon���� �� 1�� �����
            this.Plr = p;
            deck = Plr.GetComponent<player>().deck;
            if(deck == null && Plr == null)
                Debug.Log("asdf");
        }

        public void Update()
        {
            float temp_hp = Plr.GetComponent<player>().GetHp();
            float temp_st = Plr.GetComponent<player>().GetStamina();
            hp_bar.transform.GetChild(2).GetComponent<Text>().text = temp_hp.ToString();
            hp_bar.transform.GetChild(1).localScale = new Vector2(temp_hp / 100, 1);
            st_bar.transform.GetChild(2).GetComponent<Text>().text = temp_st.ToString();
            st_bar.transform.GetChild(1).localScale = new Vector2(temp_st / 100, 1);
            for (int i = 1; i < deck.max_Hand + 1; i++)
            {
                card_ui[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            for (int i = deck.max_Hand + 1; i < AnotherDeck.MAX_HAND + 1; i++)
            {
                card_ui[i].GetComponent<Image>().color = new Color(0.5f, 0.3f, 0.3f, 1f);
            }
        }
    }
}