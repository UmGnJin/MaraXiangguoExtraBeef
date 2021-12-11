using System.Collections.Generic;
using ArcanaDungeon.Object;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class ThiefAttackCard : Cards
    {
        private int cardDamage = 20; // �⺻ ������.

        public ThiefAttackCard(int typ)
        {
            this.costChange(20);
            this.setRange(2);
            settingCard(typ);
            /*
            this.setCardSprite("�ӽ� ����", "����ī��", "��Ÿ� 1ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");*/
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
                        this.setCardSprite("�ӽ� ����", "�޼� ���", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ְ�, ��ȭ " + this.getWeaknessSt() + "�� �ο��մϴ�.");
                        break;
                    }
                case 1:
                    {
                        this.setCardSprite("�ӽ� ��Ÿ", "�� �б�", cardDamage + "��ŭ ���ظ� �ְ�, ī�带 �� �� �̽��ϴ�.");
                        break;
                    }
                case 2:
                    {
                        this.setCardSprite("ȭ�� ���", "���� ����", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ι� �ݴϴ�.");
                        break;
                    }
                case 3:
                    {
                        this.setCardSprite("ȭ�� ���", "������ ����", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ְ�, ��ȭ " + this.getWeaknessSt() + "�� �ο��մϴ�. ���� �ѱ��� �ʽ��ϴ�.");
                        break;
                    }
                case 4:
                    {
                        this.setCardSprite("ȭ�� ���", "�ٸ��ɱ�", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�." + "ī�带 �� �� �̰� ���� ���� ī�忡 ��ȭ +3�� �ο��մϴ�..");
                        break;
                    }
                case 5:
                    {
                        this.setCardSprite("ȭ�� ���", "��ó ���", "��Ÿ� 2ĭ, " + " ���� ��ȭ ��ø��ŭ ���ظ� �ݴϴ�. ��󿡰� �ο��� ������ ��� ���۴ϴ�.  4����");
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
                                Debug.Log("��ȭ ����");
                                //this.setCardSprite("�ӽ� ����", "�޼� ���", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ְ�, ��ȭ " + this.getWeaknessSt() + "�� �ο��մϴ�.");
                                break;
                            }
                        case 1:
                            {
                                Plr.allDeck.DrawCards();
                                Debug.Log("ī�� �̱� �Լ� ����1");
                                //this.setCardSprite("�ӽ� ��Ÿ", "�� �б�", "ī�带 �� �� �̽��ϴ�.");
                                break;
                            }
                        case 2:
                            {
                                enemy.be_hit(cardDamage);
                                enemy.be_hit(cardDamage);
                                //this.setCardSprite("ȭ�� ���", "���� ����", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ι� �ݴϴ�.");
                                break;
                            }
                        case 3:
                            {
                                enemy.be_hit(cardDamage);
                                enemy.condition_add(5, this.getWeaknessSt() * 10 + 2);
                                Plr.allDeck.passTurn();
                                //this.setCardSprite("ȭ�� ���", "������ ����", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ְ�, ��ȭ " + this.getWeaknessSt() + "�� �ο��մϴ�. ���� �ѱ��� �ʽ��ϴ�.");
                                break;
                            }
                        case 4:
                            {
                                enemy.be_hit(cardDamage);
                                Plr.allDeck.DrawCards();
                                Debug.Log("ī�� �̱� �Լ� ����4");
                                Plr.allDeck.addweak(3);
                                //this.setCardSprite("ȭ�� ���", "�ٸ��ɱ�", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�." + "ī�带 �� �� �̰� ���� ���� ī�忡 ��ȭ +3�� �ο��մϴ�..");
                                break;
                            }
                        case 5:
                            {
                                Dictionary<int, int> temp = enemy.GetCondition();
                                if (temp.ContainsKey(5))
                                {
                                    enemy.be_hit(temp[5] / 10);
                                    Debug.Log("5��Ÿ�� ī�� �� : " + temp[5] / 10 + " �ʱ�ȭ ��" + temp[5] / 10 + 9);
                                    enemy.condition_add(5,(temp[5] / 10 * 10) + 9 - temp[5] % 10);
                                }
                                Debug.Log("���� �̱���");
                                //this.setCardSprite("ȭ�� ���", "��ó ���", "��Ÿ� 2ĭ, " + " ���� ��ȭ ��ø��ŭ ���ظ� �ݴϴ�. ��󿡰� �ο��� ������ ��� ���۴ϴ�.  4����");
                                break;
                            }
                    }
                    //�����̻� �߰� �� �κ�
                    Plr.StaminaChange(-this.getCost());
                }
                else
                    Debug.Log("���� ã�� �� �����ϴ�.");
            }
            else
                Debug.Log("�÷��̾ ã�� �������ϴ�.");

        }
    }
}

