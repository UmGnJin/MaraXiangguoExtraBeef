using ArcanaDungeon.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // 안쓴 카드와 쓴 카드를 놓을 공간
        private List<Cards> CardsDeck = new List<Cards>(); // 덱 리스트
        private List<Cards> UsedDeck = new List<Cards>(); // 사용한 카드 덱 리스트
        /*  카드 타입,2 |카드 코스트,2 | 카드 사거리,1 | 카드 수치(ex공격력 ,2 |
         *  1,30,5,10,1,4 13051014*/
        private int nextCdweak = 0;
        private bool ispass = false;
        public bool isnext = false;
        private int CardCount; // 덱의 카드 수

        public List<Cards> Hands = new List<Cards>();

        public const int MAX_HAND = 6;
        public int max_Hand = 3;
        public Deck()
        {
            SettingFstDeck();
            ChangDeck();
            //DrawCards(); //UI.uicanvas.card_draw(Hands[Hands.Count - 1]);
            //DrawCards(); //UI.uicanvas.card_draw(Hands[Hands.Count - 1]); //★테스트를 위해 임시로 패를 만듬, 카드 드로우 기능 구현이 완료되면 삭제해도 됨, 가능하면 UI의 card_draw를 이렇게 따로 부르지 않아도 DrawCards에서 자동으로 처리하면 좋을 듯
        }

        public void SettingFstDeck()// 만약 플레이어 직업 생기면 직업별 초기 카드 세팅 
        {
            // 공격력 | 약화 수치 | 추가효과 타입
            /*  14         2             0
             *  6          0             1드로우
             *  9          0             2공격 두번 발동
             *  1          1             3턴 턴소모 x
             *  2          0             4다음 카드 약화 +3 1드로우
             *  - 20 / 1칸 / 공격 (대상이 가진 약점) / 대상에게 부여된 약점을 모두 없앱니다 / 카드를 사용한 방향의 반대 방향으로 도약 4칸 5
                - 30 / 3칸 / 도약 / 즉시 덱을 셔플 도약 6
                - 20 / 다음 번 체력을 잃는 것을 막습니다.7
                - 40 / 패를 모두 버림 / 패가 가득 찰 때까지 드로우 / 이 카드는 턴을 소모하지 않음 9
                - 20 / 체력 +(연속으로 공격 카드를 사용한 턴 수 *3)10
             */
            CardsDeck.Add(new ThiefAttackCard(1420));
            //CardsDeck.Add(new ThiefAttackCard(601));
            //CardsDeck.Add(new ThiefAttackCard(902));
            CardsDeck.Add(new ThiefAttackCard(113));
            CardsDeck.Add(new ThiefAttackCard(204));
            CardsDeck.Add(new ThiefAttackCard(005));
            //CardsDeck.Add(new BasicConsCard(3030025)); // - 30 / 급류 5턴 8


            CardCount = CardsDeck.Count;
        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // 덱 리스트의 맨 위 카드
            Cards Tempcard = CardsDeck[TopOfDeck];
            CardsDeck.RemoveAt(TopOfDeck); // 덱 리스트의 맨 위 카드 제거

            return Tempcard;
        }

        public void ChangDeck() //덱 셔플, CardsDeck에 UsedDeck을 이어붙인 뒤 카드 개수만큼 무작위로 위치를 바꿔서 셔플한다
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
            CardCount = CardsDeck.Count;
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

        public void DrawCards() // 덱에 있는 맨 위부터 카드 정해진 수 만큼 가져오기
        {
            UI.uicanvas.log_add("덱을 셔플했습니다.");
            if (Hands.Count < max_Hand & CardCount > 0)
            {
                CardCount--;
                Hands.Add(CardsDeck[CardCount]);
                CardsDeck.RemoveAt(CardCount);
                UI.uicanvas.card_draw(Hands[Hands.Count - 1]);
            }
            //Debug.Log("핸드에 있는 카드 수 " + Hands.Count);
        }

        public void UsingCard(int SlotNum, player PLR, Enemy EMY)
        {
            if (Hands[SlotNum].cardTape == 5 & isnext)
            {
                if (EMY != null)
                {
                    EMY.condition_add(5, this.nextCdweak);
                    this.nextCdweak = 0;
                }
            }
            Hands[SlotNum].UseCard(PLR, EMY);
            if (Hands[SlotNum].cardTape < 5 & isnext)
            {
                if (EMY != null)
                {
                    EMY.condition_add(5, this.nextCdweak);
                    this.nextCdweak = 0;
                }
            }

            //CardSlot[SlotNum].UseCard(smthing);
            //Debug.Log("카드 사용됨" + cost);
            UsedDeck.Add(Hands[SlotNum]);
            //Debug.Log(UsedDeck[0].cardTape + "사용된 카드타입");
            Hands.RemoveAt(SlotNum);
            if (!ispass)
            {
                PLR.drawCountting();
                Dungeon.dungeon.Plr.Turnend();
                ispass = false;
                isnext = true;
            }
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