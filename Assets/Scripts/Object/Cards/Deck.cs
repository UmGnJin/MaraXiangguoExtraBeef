using ArcanaDungeon.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // �Ⱦ� ī��� �� ī�带 ���� ����
        private List<Cards> CardsDeck = new List<Cards>(); // �� ����Ʈ
        private List<Cards> UsedDeck = new List<Cards>(); // ����� ī�� �� ����Ʈ
        /*  ī�� Ÿ��,2 |ī�� �ڽ�Ʈ,2 | ī�� ��Ÿ�,1 | ī�� ��ġ(ex���ݷ� ,2 |
         *  1,30,5,10,1,4 13051014*/
        private int nextCdweak = 0;
        private bool ispass = false;
        public bool isnext = false;
        private int[] cardtypes = new int[] { 48300, 33001, 20002, 25203 , 20004 , 20005 , 0009 };

        public List<Cards> Hands = new List<Cards>();

        public const int MAX_HAND = 6;
        public int max_Hand = 6;
        public Deck()
        {
            SettingFstDeck();
            //ChangDeck();
            //DrawCards(); //UI.uicanvas.card_draw(Hands[Hands.Count - 1]);
            //DrawCards(); //UI.uicanvas.card_draw(Hands[Hands.Count - 1]); //���׽�Ʈ�� ���� �ӽ÷� �и� ����, ī�� ��ο� ��� ������ �Ϸ�Ǹ� �����ص� ��, �����ϸ� UI�� card_draw�� �̷��� ���� �θ��� �ʾƵ� DrawCards���� �ڵ����� ó���ϸ� ���� ��
        }

        public void SettingFstDeck()// ���� �÷��̾� ���� ����� ������ �ʱ� ī�� ���� 
        {
            // ���ݷ� | ��ȭ ��ġ | �߰�ȿ�� Ÿ��
            /*  14         2             0
             *  6          0             1��ο�
             *  9          0             2���� �ι� �ߵ�
             *  1          1             3�� �ϼҸ� x
             *  2          0             4���� ī�� ��ȭ +3 1��ο�
             *  - 20 / 1ĭ / ���� (����� ���� ����) / ��󿡰� �ο��� ������ ��� ���۴ϴ� / ī�带 ����� ������ �ݴ� �������� ���� 4ĭ 5
                - 30 / 3ĭ / ���� / ��� ���� ���� ���� 6
                - 20 / ���� �� ü���� �Ҵ� ���� �����ϴ�.7
                - 40 / �и� ��� ���� / �а� ���� �� ������ ��ο� / �� ī��� ���� �Ҹ����� ���� 9
                - 20 / ü�� +(�������� ���� ī�带 ����� �� �� *3)10 -���� 
             */
            /*CardsDeck.Add(new ThiefAttackCard(2880));
            CardsDeck.Add(new ThiefAttackCard(1201));
            CardsDeck.Add(new ThiefAttackCard(1802));
            CardsDeck.Add(new ThiefAttackCard(443));
            CardsDeck.Add(new ThiefAttackCard(804));
            CardsDeck.Add(new ThiefAttackCard(805));
            CardsDeck.Add(new ThiefAttackCard(009));
            CardsDeck.Add(new BasicConsCard(3030025)); // - 30 / �޷� 5�� 8*/

            for (int i = 0; i < 5; i++)
            {
                CardsDeck.Add(new ThiefAttackCard(48300));
                CardsDeck.Add(new ThiefAttackCard(33001));
                CardsDeck.Add(new ThiefAttackCard(20002));
                CardsDeck.Add(new ThiefAttackCard(25203));
                CardsDeck.Add(new ThiefAttackCard(20004));
            }
            for (int i = 0; i < 3; i++)
            {
                CardsDeck.Add(new ThiefAttackCard(48300));
                CardsDeck.Add(new ThiefAttackCard(20005));
                CardsDeck.Add(new ThiefAttackCard(0009));
                CardsDeck.Add(new BasicConsCard(3030025)); // - 30 / �޷� 5�� 8*/
            }


        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // �� ����Ʈ�� �� �� ī��
            Cards Tempcard = CardsDeck[TopOfDeck];
            CardsDeck.RemoveAt(TopOfDeck); // �� ����Ʈ�� �� �� ī�� ����

            return Tempcard;
        }

        public void ChangDeck() //�� ����, CardsDeck�� UsedDeck�� �̾���� �� ī�� ������ŭ �������� ��ġ�� �ٲ㼭 �����Ѵ�
        {
            CardsDeck.AddRange(UsedDeck);
            UsedDeck.Clear();
            for (int i = 0; i < CardsDeck.Count(); i++)
            {
                int ra1 = Dungeon.random.Next(0, CardsDeck.Count());
                int ra2 = Dungeon.random.Next(0, CardsDeck.Count());
                Cards temp = CardsDeck[ra1];
                CardsDeck[ra1] = CardsDeck[ra2];
                CardsDeck[ra2] = temp;
            }
        }

        public Cards getRandomCardcode()
        {
            int index = this.cardtypes[Dungeon.random.Next(0, cardtypes.Length)];
            return new ThiefAttackCard(index);
        }

        public List<Cards> showDeckList()
        {
            return CardsDeck;
        }
        public List<Cards> showUsedList()
        {
            return UsedDeck;
        }
        public List<Cards> showCardSlot()
        {
            return Hands;
        }

        public void resetHands()
        {

        }
        public void DrawCards() // ���� �ִ� �� ������ ī�� ������ �� ��ŭ ��������
        {
            //UI.uicanvas.log_add("���� �����߽��ϴ�.");
            if (CardsDeck.Count == 0)//�켱 �׽�Ʈ�� ���� ���� ī�尡 ���� ��Ȳ�� �ڵ� ���� ����� �־���.
                ChangDeck();
            if (Hands.Count < max_Hand & CardsDeck.Count > 0)
            {
                Hands.Add(CardsDeck[CardsDeck.Count - 1]);
                CardsDeck.RemoveAt(CardsDeck.Count - 1);
                UI.uicanvas.card_draw(Hands[Hands.Count - 1]);
            }
        }

        public void wasteHandCard(int index)
        {
            UsedDeck.Add(Hands[index]);
            Hands.RemoveAt(index);
        }
        
        public void UsingCard(int SlotNum, player PLR, Enemy EMY)
        {
            Cards temp = Hands[SlotNum];

            UsedDeck.Add(Hands[SlotNum]);
            Hands.RemoveAt(SlotNum);
            UI.uicanvas.updateUseCard();
            if (temp.cardTape == 5 & isnext)
            {
                if (EMY != null)
                {
                    EMY.condition_add(5, this.nextCdweak);
                    this.nextCdweak = 0;
                }
            }
            temp.UseCard(PLR, EMY);
            if (temp.cardTape < 5 & isnext)
            {
                if (EMY != null)
                {
                    EMY.condition_add(5, this.nextCdweak);
                    this.nextCdweak = 0;
                }
            }

            //CardSlot[SlotNum].UseCard(smthing);
            //Debug.Log("ī�� ����" + cost);
            //Debug.Log(UsedDeck[0].cardTape + "���� ī��Ÿ��");
            if (ispass == false)
            {
                UI.uicanvas.log_add("ī�� ���.");
                Dungeon.dungeon.Plr.drawCountting();
                //Dungeon.dungeon.Plr.Turnend();
                Dungeon.dungeon.Plr.Turnend();
                ispass = false;
                isnext = true;
            }else
                ispass = false;
        }
        public void passTurn()
        {
            this.ispass = true;
        }
        public void addweak(int we)
        {
            isnext = false;
            this.nextCdweak = we * 10  + 2;
        }


    }
}