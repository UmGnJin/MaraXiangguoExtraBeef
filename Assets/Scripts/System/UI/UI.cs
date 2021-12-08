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
        public GameObject small_tool_tip; //����
        public GameObject blood_par;  //�ӽ� ��ƼŬ ���, ���߿� �� �� �� �߰����� ������ �ʹ�
        public GameObject fire_par;  //�ӽ� ��ƼŬ ���, ���߿� �� �� �� �߰����� ������ �ʹ�
        public GameObject poison_par;  //�ӽ� ��ƼŬ ���, ���߿� �� �� �� �߰����� ������ �ʹ�
        public GameObject range;    //��Ÿ� ǥ�� ���
        public GameObject Cardlist_scroll;  //Cardlist_Panel���� ī�带 ����/������ �� ���Ǵ� ��ũ�Ѻ�
        public GameObject Cardlist_panel;   //Cardlist_Panel���� ī�带 ����/������ �� ���Ǵ� �г�
        public GameObject Cardlist_card;    //Cardlist_Panel���� ī�带 ����/������ �� ���Ǵ� ī�� �̹���, ��ҿ� ������ ī�带 ȹ���� ������ Instantiate�� �� ī�� ����� ������ �� �̹����� �Ҵ��� ����
        public GameObject Cardlist_cancel;  //Cardlist_Panel���� ī�带 ����/������ �� ���Ǵ� ��� ��ư, �г��� ��ũ�� �� �� �����̸� �� �ż� ���� ����Ǿ� ����

        private GameObject Plr; //���÷��̾�
        private LineRenderer line;
        private Deck deck;   //�÷��̾�� �� Deck ��Ʈ��Ʈ, SetPlr���� ���� Plr�� �Բ� ����
        public Text log;  //�α�

        private int selected = -1;  //���п��� ī�带 Ŭ���ϸ� �� ������ �� ī���� Hands������ �ε��� ��ȣ�� ����
        public Cards selected2; //Cardlist_Panel���� ī�带 ������ �� ���õ� ī�带 ��Ÿ��
        private bool selecting; //Cardlist_Panel���� ī�带 ������ �� ī�带 ���� ���� ���¸� ��Ÿ��
        private int wii; //ui���� ��� �κ��� ����Ǿ�� �ϴ��� ��Ÿ����
        //-1=�⺻��, ����� �� ���� / 0 : ��� ui ��� �ʱ�ȭ / 1 : ���� ī�� Ȯ�� / 2 : ������ ī�� ��Ÿ� ǥ�� (ī�尡 Ŀ�� ����ٴϴ� �� update�� ����) / 3 : �����̻� ���� ǥ��

        private Vector2 mpos;   //���콺 ��ǥ

        private const string card_background = "sprites/Card/ī�� ���";

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

            card_on_cursor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(card_background);    //���콺 Ŀ���� ����ٴ� UI ��� ����
            wii = 0;
            selected = -1;
            selected2 = null;
            selecting = false;
            line = this.gameObject.GetComponent<LineRenderer>();
        }
        public void Start() {
            //Cardlist_card ��ư�� ���� ���� �� ���� ī�� ���̸�ŭ ����, �����ڿ����� ��ũ���� API�� ������ �� ���ٸ� ������� �ʴ´�� ����
            for (int i = 0; i < deck.showDeckList().Count; i++)
            {
                Create_Cardlist_card();
            }
        }

        public void SetPlr(GameObject p) {   //�� ��ũ��Ʈ�� Plr�� �÷��̾ ��������, Dungeon���� �� 1�� �����
            this.Plr = p;
            deck = Plr.GetComponent<player>().allDeck; // ���� ������ ������ �ִ� deck�� ������ �� Ŭ������ �ӽ÷� �ٲ� jgh
            if (deck == null && Plr == null)
            {
                Debug.Log("asdf");
            }
        }

        public void Update() {

            mpos = Input.mousePosition; //���콺 Ŀ�� ��ġ�� mpos�� ����, ��ǥ�� World ��ǥ�� �ƴ� Screen ��ǥ�� UI ����ϴ� �� ��ǥ�� ����
            int temp_hand_num = (int)mpos.x / 120;  //���� ���õ� ī�尡 �� ��° ī������ �����صδ� ����, 1��~6���� ��
            GameObject temp_ob;   //�Ʒ����� ���� ���� �۾����� �ӽ÷� ������Ʈ �����ص� �� ��� �� �ӽ� ������Ʈ

            //wii �����ϴ� �κ�
            if (mpos.x > 120 & mpos.x < 840 & mpos.y > 0 & mpos.y < 160 & temp_hand_num <= deck.Hands.Count)
            {
                wii = 1;    //������ ī�带 Ȯ���϶�� wii��
                //���� ��ǥ �ȿ��� Ŭ���ϸ� �� ī�� ��ȣ�� selected�� �����Ѵ�, selected==-1 �� ������ �巡�� ���� �ٸ� ī�尡 selected�� ����ȴ�, GetMouseButtonDown�� if���� ���־���� ���� �۵��� �� �Ѵ�
                if (Input.GetMouseButton(0) & selected == -1) { selected = temp_hand_num - 1; }
                if (!Input.GetMouseButton(0)) { selected = -1; }
            }
            //���� ��ǥ �ۿ� ���콺 Ŀ���� ���� ���� ��Ÿ����, �� �� ó���� �� �ִ� ���� Ȯ��� ī�� �̹��� ����, ������ ī�� �̹����� Ŀ���� ����ٴ�, ������ ī�� ��� �̷��� 3������. 
            if (mpos.x < 120 | mpos.x > 840 | mpos.y < 0 | mpos.y > 160)
            {
                //selected�� ���� ������ �������� ������ ī�带 �������� ���� ä ���� ��ǥ�� ��� ���̴�
                if ((selected > -1) & (selected < deck.max_Hand))
                {
                    //���콺 ���� ��ư�� Ŭ���� ���̸� ������ ī�� �̹����� ���콺 Ŀ���� ����ٴ�, ī�� ��Ÿ� ǥ�ô� wii ó���� �ñ��
                    if (Input.GetMouseButton(0))
                    {
                        card_on_cursor.GetComponent<RectTransform>().localPosition = mpos + new Vector2(-860, -440);
                        card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
                        card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                        wii = 2;    //ī�� ��Ÿ��� ǥ���϶�� wii��
                        for (int i = 1; i < 4; i++)
                        {
                            temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                            temp_ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                            temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition;
                            //selected�� ���� deck�� Hands���� ī�� �̸�, �Ϸ���Ʈ, ī�� ȿ���� ������ card_on_cursor �ڽĵ����� ������
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
                    //���콺 ���� ��ư�� Ŭ������ ������  Ŭ���� ���� ��ǥ�� ī�带 ����ϱ�� ������ ���̴�, ���� ī���� ��Ÿ��� �����ϴ��� Ȯ�� �� ���ó���Ѵ�
                    else
                    {
                        //���콺 Ŀ�� ��ǥ�� ����Ƽ ���� world ��ǥ�� ������ �� ��ǥ ���� enemy ã��
                        Vector3 mpos_world = cam.GetComponent<Camera>().ScreenToWorldPoint(mpos);
                        mpos_world = new Vector3(Mathf.Round(mpos_world.x), Mathf.Round(mpos_world.y), 0);
                        //�ش� ��ǥ�� ����� ī���� ��Ÿ� �̳��̸�, ��ü �� ������ ��ǥ�̸� ���
                        if (mpos_world.x >= Plr.transform.position.x - deck.Hands[selected].getRange() & mpos_world.x <= Plr.transform.position.x + deck.Hands[selected].getRange() &
                            mpos_world.x >= 0 & mpos_world.x <= Dungeon.dungeon.currentlevel.width & mpos_world.y >= 0 & mpos_world.x <= Dungeon.dungeon.currentlevel.height)
                        {
                            //���� �� ��° �ε����� ���Ǿ�����, �÷��̾� ��ũ��Ʈ, enemy ��ũ��Ʈ�� �Ķ���ͷ� �����ϵ� ���� ���� ���� ���� �巡���ߴٸ� enemy ��ũ��Ʈ �ڸ��� null�� ���޵�
                            deck.UsingCard(selected, Plr.GetComponent<player>(), Dungeon.dungeon.find_enemy(mpos_world.x, mpos_world.y));
                            Plr.GetComponent<player>().condition_process(); //���÷��̾� ��ũ��Ʈ���� ó���ϰ� �ű� ��
                            //���� ���� �������� ī����� 1ĭ�� �������� �̵���Ű��
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
                        wii = 0;    //ui �ʱ�ȭ�ϴ� wii��
                    }
                }
                //���� ��ǥ �ۿ� �����鼭 ī�尡 ���õ��� �ʾҰ� card_on_cursor�� Ȱ��ȭ�� ���¶�� ui�� �ʱ�ȭ�Ѵ�
                else if (card_on_cursor.transform.GetChild(0).gameObject.activeSelf){ wii = 0; }
            }
            //�����̻� Ȯ�� �������� Ŀ�� �ø�
            if (mpos.x > 1440 & mpos.x < 1680 & mpos.y > 0 & mpos.y < 160) { wii = 3; }

            //wii�� ���� ui ó�� �κ�
            if (wii == 0) { AllReset(); wii = -1; }   //ui ��� �α׸� ��Ȱ��ȭ��Ű�� �κ�
            if (wii == 1) { CardZoom(temp_hand_num); wii = -1; } //�п� Ŀ���� �ø� ī�� Ȯ��
            if (wii == 2) { RangeDisplay(deck.Hands[selected].getRange()); wii = -1; }  //���� ������ ī���� ��Ÿ� ǥ��
            if (wii == 3) { ConditionTooltip((int)(Mathf.Floor((mpos.x - 1440) / 80) + 3 * Mathf.Floor((160 - mpos.y) / 80))); wii = -1; }   //�����̻� ���� ǥ��

            //���Ÿ� ������ ǥ���� �� ���Ǵ� linerenderer �����ϰ� �����
            if (line.startColor[3] > 0f)
            {
                line.SetColors(new Color(1f, 1f, 1f, line.startColor[3] - 0.03f), new Color(1f, 1f, 1f, line.endColor[3] - 0.03f));
            }

            if (Input.GetKey(KeyCode.Space) & deck.Hands.Count < deck.max_Hand)//������ ������ ��
            {
                deck.DrawCards(); card_draw(deck.Hands[deck.Hands.Count - 1]);
            }
        }

        public void SetWii(int a) {
            wii = a;
        }

        private void AllReset() {
            //�� �ִ� ��� ǥ�� �κ�, �ִ� ����� �ʰ��ϴ� ĭ�� ��ο� ���� ������ ǥ��
            for (int i = 1; i <= deck.max_Hand; i++) { card_ui[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f); }
            for (int i = deck.max_Hand + 1; i < 7; i++) { card_ui[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.3f, 0.3f, 1f); }
            //card_on_cursor �ʱ�ȭ, ī�� Ȯ�� �� Ŀ�� ����ٴϴ� �̹����� ����, Text�� ���� �ڽ��� text�� null�� �ϰ�, Image�� ���� �ڽ��� ��Ȱ��ȭ
            card_on_cursor.transform.GetChild(0).gameObject.SetActive(false);
            card_on_cursor.transform.GetChild(1).gameObject.GetComponent<Text>().text = null;
            card_on_cursor.transform.GetChild(2).gameObject.SetActive(false);
            card_on_cursor.transform.GetChild(3).gameObject.GetComponent<Text>().text = null;
            //small_tool_tip �ʱ�ȭ, �����̻� ������ ����
            small_tool_tip.SetActive(false);
            small_tool_tip.transform.GetChild(0).gameObject.GetComponent<Text>().text = null;
            //range �ʱ�ȭ, ��Ÿ� ǥ�ÿ� ���ehla
            range.SetActive(false);
        }
        private void CardZoom(int th) { //Ŀ���� �ø� ���� ī�� �̹��� Ȯ��
            GameObject temp_ob = null;
            card_on_cursor.GetComponent<RectTransform>().localPosition = new Vector3(-900 + th * 120, -180, 0);
            card_on_cursor.transform.GetChild(0).gameObject.SetActive(true);
            card_on_cursor.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
            for (int i = 1; i < 4; i++)
            {
                temp_ob = card_on_cursor.transform.GetChild(i).gameObject;
                temp_ob.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
                temp_ob.GetComponent<RectTransform>().localPosition = card_ui[1].transform.GetChild(i).GetComponent<RectTransform>().localPosition * 2;
                //th�� ���� deck�� Hands���� ī�� �̸�, �Ϸ���Ʈ, ī�� ȿ���� ������ card_on_cursor �ڽĵ����� ������
                switch (i)
                {
                    case 1:
                        //ob.GetComponent<Text>.text = deck.Hands[th-1]. (Card�� ����ִ� ī�� �̸�);break;
                        temp_ob.GetComponent<Text>().text = deck.Hands[th - 1].cardName; break; // �̸� ���� �߰� jgh
                    case 2:
                        temp_ob.SetActive(true);
                        temp_ob.GetComponent<Image>().sprite = Resources.Load<Sprite>(deck.Hands[th - 1].illust); break;
                    case 3:
                        //ob.GetComponent<Text>.text = deck.Hands[th-1]. (Card�� ����ִ� ī�� ȿ��);break;
                        temp_ob.GetComponent<Text>().text = deck.Hands[th - 1].cardInfo; break; // �̸� ���� �߰� jgh
                }
            }
        }
        private void RangeDisplay(int r) {  //������ ī���� ��Ÿ� ǥ��
            range.GetComponent<RectTransform>().sizeDelta = new Vector2((2 * r + 1) * 55, (2 * r + 1) * 55);
            range.SetActive(true);
        }
        private void ConditionTooltip(int sn) { //�÷��̾��� �����̻� ���� ǥ��
            if (Plr.GetComponent<player>().GetCondition().Count >= sn + 1)
            {
                small_tool_tip.transform.localPosition = new Vector3(520 + sn % 3 * 80, -240, 0);
                small_tool_tip.SetActive(true);
                small_tool_tip.transform.GetChild(0).gameObject.GetComponent<Text>().text = Resources.Load<TextAsset>("condition_tooltip").text + Plr.GetComponent<player>().GetCondition()[sn];    //��
            }
        }

        public void GaugeChange()
        {    //�÷��̾��� HP/���¹̳�/�� ����
            if (this.Plr == null) { return; }   //���� ó���� player�� �����ϰ� maxhp�� ������ �� ȣ��Ǹ鼭 Plr�� null�� ä�� ������ �� ���� �ִ�
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
            //which�� � ī�� ���� ����� �������� ��Ÿ��, 0=�÷��̾��� ��� ī�� / 1=���� ī�� ���� / 2=���� ī�� ����

            int temp1 = 0;
            List<GameObject> temp2 = new List<GameObject>();
            Transform temp3 = null;
            //���� ī�� ���̸� temp2�� ����
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

            //temp2�� ī�� �̸��� ���� ����, �̰� �� �ϸ� ���� ī�� ���̿��� ������ ���� ī�尡 ���� �� ���δ�
            temp2 = temp2.OrderBy(x => x.GetComponent<Cardlist_select>().GetCOT().cardName).ToList();

            Debug.Log("temp1 : " + temp1);
            //Cardlist_panel�� height�� temp2�� �ε��� ������ ���� �����ϰ� Ȱ��ȭ, temp2�� �ε����� ��ġ�ϰ� Ȱ��ȭ�� ���ó�� ���̰� �����
            temp1 = Mathf.Max(420 * (temp1 / 5 + 1) + 20, 1080);    //�ӽ÷� �Ʊ� ���� temp ���� ��
            Cardlist_panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, temp1); //Scroll View���� Content�� ũ�Ⱑ Viewport���� ũ�� ������ ��ũ���� �̷����� ����
            Cardlist_panel.GetComponent<RectTransform>().localPosition = new Vector2(0, -temp1 * 0.5f + 540);
            Cardlist_scroll.SetActive(true);
            Cardlist_cancel.SetActive(true);   //�ݱ� ��ư
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
            //�÷��̾ �Է��� ������ ���� ����
            selecting = true;

            //����Ե� Time.timeScale=0f�� �Ǹ� ���� ���̴� �Լ��� update�� �����Ǿ� ���� ������ ������ �����, ���п� �Ʒ� �κ��� ������ �� �ִ�
            Debug.Log("ī�弿��Ʈ ����!");
        }
        public void SetSelected2(Cards c) {
            selected2 = c;
            //Card_select���� ���� ��� ������Ʈ ��Ȱ��ȭ�ϰ� �����
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