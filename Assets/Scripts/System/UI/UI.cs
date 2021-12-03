using System.Collections;
using System.Collections.Generic;
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

        private GameObject Plr; //���÷��̾�
        private Deck deck;   //�÷��̾�� �� Deck ��Ʈ��Ʈ, SetPlr���� ���� Plr�� �Բ� ����
        public Text log;  //�α�

        private int selected = -1;  //���п��� ī�带 Ŭ���ϸ� �� ������ �� ī���� Hands������ �ε��� ��ȣ�� ����
        private int wii; //ui���� ��� �κ��� ����Ǿ�� �ϴ��� ��Ÿ����
        //-1=�⺻��, ����� �� ���� / 0 : ��� ui ��� �ʱ�ȭ / 1 : hp�� ���¹̳� ���� / 2 : ���� ī�� Ȯ�� / 3 : ������ ī�� ��Ÿ� ǥ�� (ī�尡 Ŀ�� ����ٴϴ� �� update�� ����) / 4 : �����̻� ���� ǥ��

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

            card_on_cursor.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(card_background);    //���콺 Ŀ���� ����ٴ� UI ��� ����
            wii = 0;
        }

        public void SetPlr(GameObject p) {   //�� ��ũ��Ʈ�� Plr�� �÷��̾ ��������, Dungeon���� �� 1�� �����
            this.Plr = p;
            deck = Plr.GetComponent<player>().allDeck; // ���� ������ ������ �ִ� deck�� ������ �� Ŭ������ �ӽ÷� �ٲ� jgh
            if (deck == null && Plr == null)
            {
                Debug.Log("asdf");
            }
        }

        public void FixedUpdate() {

            mpos = Input.mousePosition; //���콺 Ŀ�� ��ġ�� mpos�� ����, ��ǥ�� World ��ǥ�� �ƴ� Screen ��ǥ�� UI ����ϴ� �� ��ǥ�� ����
            int temp_hand_num = (int)mpos.x / 120;  //���� ���õ� ī�尡 �� ��° ī������ �����صδ� ����, 1��~6���� ��
            GameObject temp_ob;   //�Ʒ����� ���� ���� �۾����� �ӽ÷� ������Ʈ �����ص� �� ��� �� �ӽ� ������Ʈ

            //wii �����ϴ� �κ�
            //������ ī�� Ȯ�� �ʿ�
            if (mpos.x > 120 & mpos.x < 840 & mpos.y > 0 & mpos.y < 160 & temp_hand_num <= deck.Hands.Count)
            {
                wii = 2;
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
                        wii = 3;    //ī�� ��Ÿ��� ǥ���϶�� wii��
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
                            int used = deck.UsingCard(selected, Plr.GetComponent<player>(), Dungeon.dungeon.find_enemy(mpos_world.x, mpos_world.y));
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
            if (mpos.x > 1440 & mpos.x < 1680 & mpos.y > 0 & mpos.y < 160) { wii = 4; }

            //wii�� ���� ui ó�� �κ�
            if (wii == 0) { AllReset(); wii = -1; }   //ui ��� �α׸� ��Ȱ��ȭ��Ű�� �κ�
            if (wii == 1) { GaugeChange(); wii = -1; } //�÷��̾� HP�� ���¹̳� ����
            if (wii == 2) { CardZoom(temp_hand_num);  wii = -1; }  //�п� Ŀ���� �ø� ī�� Ȯ��
            if (wii == 3) { RangeDisplay(deck.Hands[selected].getRange()); wii = -1; }   //���� ������ ī���� ��Ÿ� ǥ��
            if (wii == 4) { ConditionTooltip((int)(Mathf.Floor((mpos.x - 1440) / 80) + 3 * Mathf.Floor((160 - mpos.y) / 80))); wii = -1; }   //�����̻� ���� ǥ��

            if (Input.GetKey(KeyCode.Space))//��
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
        private void GaugeChange() {    //�÷��̾��� HP �� ���¹̳� ������ ����
            float temp_hp = Plr.GetComponent<player>().GetHp();
            float temp_st = Plr.GetComponent<player>().GetStamina();
            if (temp_hp < 0) { temp_hp = 0f; }
            if (temp_st < 0) { temp_st = 0f; }
            hp_bar.transform.GetChild(2).GetComponent<Text>().text = temp_hp.ToString();
            hp_bar.transform.GetChild(1).localScale = new Vector2(temp_hp / Plr.GetComponent<player>().maxhp, 1);
            st_bar.transform.GetChild(2).GetComponent<Text>().text = temp_st.ToString();
            st_bar.transform.GetChild(1).localScale = new Vector2(temp_st / Plr.GetComponent<player>().maxstamina, 1);
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
    }
}