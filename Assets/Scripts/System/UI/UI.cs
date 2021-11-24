using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArcanaDungeon.Object;
using ArcanaDungeon.cards;

namespace ArcanaDungeon
{
    public class UI : MonoBehaviour //UI ������.
    {
        public static UI uicanvas;

        public GameObject hp_bar;
        public GameObject st_bar;
        public GameObject[] card_ui = new GameObject[8];   //���� ī�� ����, �� 6ĭ, ���� ī�� ����
        public GameObject[] stat_ui = new GameObject[6]; //�����̻� �� ����
        public GameObject[] button = new GameObject[4];  //�޽�, ����, ����, �޴�
        public GameObject card_on_cursor;   //���콺 Ŀ���� �ø� ī�带 ��Ÿ���ִ� �̹��� UI
        public GameObject cam;  //ī�޶�, ���� & �ܾƿ��� ���콺 Ŀ�� ��ǥ�� ��ũ����ǥ���� ������ǥ�� �ٲ� �� ���

        private GameObject Plr; //���÷��̾�
        public Enemy targeted; //�ӽ÷� ���� �� ���߿� �������� �� �迭�� �޾ƿ;� �� ���� ���� jgh
        private Deck deck;   //�÷��̾�� �� Deck ��Ʈ��Ʈ, SetPlr���� ���� Plr�� �Բ� ����
        private Cards selected;  //���п��� ī�带 Ŭ���ϸ� �� ������ �����Ѵ�

        public Text message;

        private Vector2 mpos;   //���콺 ��ǥ

        private const string card_background = "sprites/Card/ī�� ���";
        public Sprite temp; //�׽�Ʈ�� �ӽ� �̹���

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
            {
                Debug.Log("shdfg");
            }

            card_on_cursor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(card_background);    //���콺 Ŀ���� ����ٴ� UI ��� ����
        }

        public void ShowMessage(string msg)//�Ű������� �ΰ��� ȭ�鿡 �޽����� ���.
        {
            message.text += msg;
            message.text += "\n";
        }

        public void SetPlr(GameObject p) {   //�� ��ũ��Ʈ�� Plr�� �÷��̾ ��������, Dungeon���� �� 1�� �����
            this.Plr = p;
            targeted = Dungeon.dungeon.enemies[0][0].GetComponent<Enemy>();
            deck = Plr.GetComponent<player>().allDeck; // ���� ������ ������ �ִ� deck�� ������ �� Ŭ������ �ӽ÷� �ٲ� jgh
            if (deck == null && Plr == null)
            {
                Debug.Log("asdf");
            }
        }

        public void FixedUpdate() {
            //�÷��̾� HP�� ���¹̳� ǥ�� �κ�, ��� ü�°� ���¹̳��� ���� ���� �����ϴ� �� �� ������ �ڵ��� ���Ǹ� ���� �ټ� Ÿ���ߴ�
            float temp_hp = Plr.GetComponent<player>().GetHp();
            float temp_st = Plr.GetComponent<player>().GetStamina();
            hp_bar.transform.GetChild(2).GetComponent<Text>().text = temp_hp.ToString();
            hp_bar.transform.GetChild(1).localScale = new Vector2 ( temp_hp / Plr.GetComponent<player>().maxhp, 1);
            st_bar.transform.GetChild(2).GetComponent<Text>().text = temp_st.ToString();
            st_bar.transform.GetChild(1).localScale = new Vector2( temp_st / Plr.GetComponent<player>().maxstamina, 1);

            /*
            //�� �ִ� ��� ǥ�� �κ�, �ִ� ��� �̳��� ĭ�� ���� ������ / �ִ� ����� �ʰ��ϴ� ĭ�� ��ο� ������ ǥ��
            for (int i = 1; i <= deck.max_Hand; i++) {
                card_ui[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            for (int i = deck.max_Hand+1; i < 7; i++)
            {
                card_ui[i].GetComponent<Image>().color = new Color(0.5f, 0.3f, 0.3f, 1f);
            }
            */

            mpos = Input.mousePosition; //���콺 Ŀ�� ��ġ�� mpos�� ����, ��ǥ�� World ��ǥ�� �ƴ� Screen ��ǥ�� UI ����ϴ� �� ��ǥ�� ����
            GameObject temp_ob;   //�Ʒ����� ���� ���� �۾����� �ӽ÷� ������Ʈ �����ص� �� ��� �� �ӽ� ������Ʈ
            int temp_hand_num = (int)mpos.x / 120;  //���� ���õ� ī�尡 �� ��° ī������ �����صδ� ����, 1��~6���� ��

            //���� ��ǥ ������ ���콺 Ŀ�� ��ǥ�� ������ ���� ���콺�� ����Ű�� ĭ�� ī�尡 ������ ī���� Ȯ��� �̹����� ����ó�� ���
            if (mpos.x>120 & mpos.x<840 & mpos.y>0 & mpos.y<160 & temp_hand_num<=deck.Hands.Count) {
                card_on_cursor.GetComponent<RectTransform>().localPosition = new Vector3(-900 + temp_hand_num*120, -180, 0);
                card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
                card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
                for (int i=1; i<4; i++) {
                    temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                    temp_ob.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
                    temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition * 2;
                    //temp_hand_num�� ���� deck�� Hands���� ī�� �̸�, �Ϸ���Ʈ, ī�� ȿ���� ������ card_on_cursor �ڽĵ����� ������
                    switch (i) {
                        case 1:
                            //ob.GetComponent<Text>.text = deck.Hands[temp_hand_num-1]. (Card�� ����ִ� ī�� �̸�);break;
                            temp_ob.GetComponent<Text>().text = deck.Hands[temp_hand_num - 1].cardName; break; // �̸� ���� �߰� jgh
                        case 2:
                            temp_ob.SetActive(true);
                            temp_ob.GetComponent<Image>().sprite = Resources.Load<Sprite>(deck.Hands[temp_hand_num - 1].illust);break;
                        case 3:
                            //ob.GetComponent<Text>.text = deck.Hands[temp_hand_num-1]. (Card�� ����ִ� ī�� ȿ��);break;
                            temp_ob.GetComponent<Text>().text = deck.Hands[temp_hand_num - 1].cardInfo; break; // �̸� ���� �߰� jgh
                    }
                }
                //���� ��ǥ ���� �ȿ��� Ŭ���ϸ� �� ī�带 selected�� �����Ѵ�
                //selected==null �� ������ �巡�� ���� �ٸ� ī�尡 selected�� ����ȴ�, GetMouseButtonDown�� if���� ���־���� ���� �۵��� �� �Ѵ�
                if (Input.GetMouseButton(0) & selected==null) {
                    selected = deck.Hands[temp_hand_num - 1];
                }
            }

            //���� ��ǥ ���� �ۿ� ���콺 Ŀ�� ��ǥ�� ������ �� �� �ϳ��� : Ȯ��ƴ� ī�� �̹��� ����ϱ�, ī�� �巡���ؼ� ��ǥ �����Ϸ��� ��, ī�� ��ǥ ���� �Ϸ��ϰ� ���
            if (mpos.x < 120 | mpos.x > 840 | mpos.y < 0 | mpos.y > 160) {
                //Ȯ��� ī�� �̹����� ����� ���¶�� �� Ȯ��� �̹����� ����
                if (card_on_cursor.transform.GetChild(0).gameObject.activeSelf)
                {
                    card_on_cursor.transform.GetChild(0).gameObject.SetActive(false);
                    //Text�� ���� �ڽ��� text�� null�� �ϰ�, Image�� ���� �ڽ��� ��Ȱ��ȭ
                    card_on_cursor.transform.GetChild(1).gameObject.GetComponent<Text>().text = null;
                    card_on_cursor.transform.GetChild(2).gameObject.SetActive(false);
                    card_on_cursor.transform.GetChild(3).gameObject.GetComponent<Text>().text = null;
                }
                //���콺 ���� ��ư�� Ŭ���� ���¶�� ���� ������ ī�� �̹����� ���콺 Ŀ���� ����ٴ�
                if (Input.GetMouseButton(0))
                {
                    card_on_cursor.GetComponent<RectTransform>().localPosition = mpos + new Vector2(-860, -440);
                    card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
                    card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    for (int i = 1; i < 4; i++)
                    {
                        temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                        temp_ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                        temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition;
                        //temp_hand_num�� ���� deck�� Hands���� ī�� �̸�, �Ϸ���Ʈ, ī�� ȿ���� ������ card_on_cursor �ڽĵ����� ������
                        switch (i)
                        {
                            case 1:
                                //ob.GetComponent<Text>.text = selected.cardName (Card�� ����ִ� ī�� �̸�);break;
                                temp_ob.GetComponent<Text>().text = "���� ī��"; break;
                            case 2:
                                temp_ob.SetActive(true);
                                temp_ob.GetComponent<Image>().sprite = Resources.Load<Sprite>(selected.illust); break;
                            case 3:
                                //ob.GetComponent<Text>.text = selected.cardInfo (Card�� ����ִ� ī�� ȿ��);break;
                                temp_ob.GetComponent<Text>().text = "�̰��� �����̿�\n�¼� �����̿�"; break;
                        }
                    }
                }else if (selected != null) //���콺 ���� ��ư�� ��� Ŭ���� ��ģ ���¶�� �װ��� ��ǥ�������� ī�带 ����ϱ�� ������ ���̴�, GetMouseButtonUp�� if�� ������ ���� �۵��� �� �ؼ� �̷��� ����
                {
                    Debug.Log(cam.GetComponent<Camera>().ScreenToWorldPoint(mpos) + " / " + Plr.GetComponent<player>().PlayerPos);
                    temp_hand_num = 3; // ���õ� ī�� �ε������� ���콺 Ŭ���� ��ģ ������ ��ġ�� ��ݵ� jgh 
                    //Debug.Log("���õ� ī�� �ε��� �� : " + temp_hand_num);//jgh 
                    Debug.Log("ī�� ���� �ƹ�ư ����� | �� hp : " + targeted.GetHp() + "�÷��̾� hp :" + Plr.GetComponent<player>().GetStamina());//jgh 
                    int i = deck.UsingCard(temp_hand_num-1, Plr.GetComponent<player>(), targeted );//jgh 
                    Debug.Log("ī�� ���� �ƹ�ư ���� "+i+" �� hp : " + targeted.GetHp() + "�÷��̾� hp :" + Plr.GetComponent<player>().GetStamina());//jgh 

                    selected = null;
                }
            }
        }

        public void card_draw() { 
            
        }
    }
}