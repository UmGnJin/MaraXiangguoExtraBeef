using System.Collections.Generic;
using ArcanaDungeon.Object;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class ThiefAttackCard : Cards
    {
        private int cardDamage = 20; // 기본 데미지.

        public ThiefAttackCard(int typ)
        {
            this.costChange(20);
            this.setRange(2);
            settingCard(typ);
            /*
            this.setCardSprite("임시 공격", "공격카드", "사거리 1칸, " + cardDamage + "만큼 피해를 줍니다.");*/
        }
        private void settingCard(int typ)
        {
            int t = typ;
            cardDamage = t / 100; t %= 100;
            this.setWeakness(t / 10);
            this.cardTape = t % 10;
            switch (this.cardTape)
            {
                case 0:
                    {
                        this.setCardSprite("임시 공격", "급소 찌르기", "사거리 2칸, " + cardDamage + "만큼 피해를 주고, 약화 " + this.getWeaknessSt() + "을 부여합니다.");
                        break;
                    }
                case 1:
                    {
                        this.setCardSprite("임시 강타", "수 읽기", cardDamage + "만큼 피해를 주고, 카드를 한 장 뽑습니다.");
                        break;
                    }
                case 2:
                    {
                        this.setCardSprite("화살 쏘기", "연속 공격", "사거리 2칸, " + cardDamage + "만큼 피해를 두번 줍니다.");
                        break;
                    }
                case 3:
                    {
                        this.setCardSprite("화살 쏘기", "은밀한 공격", "사거리 2칸, " + cardDamage + "만큼 피해를 주고, 약화 " + this.getWeaknessSt() + "을 부여합니다. 턴을 넘기지 않습니다.");
                        break;
                    }
                case 4:
                    {
                        this.setCardSprite("화살 쏘기", "다리걸기", "사거리 2칸, " + cardDamage + "만큼 피해를 줍니다." + "카드를 한 장 뽑고 다음 공격 카드에 약화 +3을 부여합니다..");
                        break;
                    }
                case 5:
                    {
                        this.setCardSprite("화살 쏘기", "상처 찌르기", "사거리 2칸, " + " 적의 약화 중첩만큼 피해를 줍니다. 대상에게 부여된 약점을 모두 없앱니다.  4도약");
                        break;
                    }

            }

        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    switch (this.cardTape)
                    {
                        case 0:
                            {
                                enemy.be_hit(cardDamage);
                                enemy.condition_add(5, this.getWeaknessSt() * 10 + 2);
                                Debug.Log("약화 공격");
                                //this.setCardSprite("임시 공격", "급소 찌르기", "사거리 2칸, " + cardDamage + "만큼 피해를 주고, 약화 " + this.getWeaknessSt() + "을 부여합니다.");
                                break;
                            }
                        case 1:
                            {
                                Plr.allDeck.DrawCards();
                                Debug.Log("카드 뽑기 함수 구현1");
                                //this.setCardSprite("임시 강타", "수 읽기", "카드를 한 장 뽑습니다.");
                                break;
                            }
                        case 2:
                            {
                                enemy.be_hit(cardDamage);
                                enemy.be_hit(cardDamage);
                                //this.setCardSprite("화살 쏘기", "연속 공격", "사거리 2칸, " + cardDamage + "만큼 피해를 두번 줍니다.");
                                break;
                            }
                        case 3:
                            {
                                enemy.be_hit(cardDamage);
                                enemy.condition_add(5, this.getWeaknessSt() * 10 + 2);
                                Plr.allDeck.passTurn();
                                //this.setCardSprite("화살 쏘기", "은밀한 공격", "사거리 2칸, " + cardDamage + "만큼 피해를 주고, 약화 " + this.getWeaknessSt() + "을 부여합니다. 턴을 넘기지 않습니다.");
                                break;
                            }
                        case 4:
                            {
                                enemy.be_hit(cardDamage);
                                Plr.allDeck.DrawCards();
                                Debug.Log("카드 뽑기 함수 구현4");
                                Plr.allDeck.addweak(3);
                                //this.setCardSprite("화살 쏘기", "다리걸기", "사거리 2칸, " + cardDamage + "만큼 피해를 줍니다." + "카드를 한 장 뽑고 다음 공격 카드에 약화 +3을 부여합니다..");
                                break;
                            }
                        case 5:
                            {
                                Dictionary<int, int> temp = enemy.GetCondition();
                                if (temp.ContainsKey(5))
                                {
                                    enemy.be_hit(temp[5] / 10);
                                    Debug.Log("5번타입 카드 딜 : " + temp[5] / 10 + " 초기화 값" + temp[5] / 10 + 9);
                                    enemy.condition_add(5,(temp[5] / 10 * 10) + 9 - temp[5] % 10);
                                }
                                Debug.Log("도약 미구현");
                                //this.setCardSprite("화살 쏘기", "상처 찌르기", "사거리 2칸, " + " 적의 약화 중첩만큼 피해를 줍니다. 대상에게 부여된 약점을 모두 없앱니다.  4도약");
                                break;
                            }
                    }
                    //상태이상 추가 할 부분
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("적을 찾을 수 없습니다.");
            }
            else
                Debug.Log("플레이어를 찾을 수없습니다.");

        }
    }
}

