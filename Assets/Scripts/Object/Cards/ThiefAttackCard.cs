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
            cardDamage = t / 100;t %= 100;
            this.setWeakness(t / 10);
            this.cardTape = t % 10;
            switch (this.cardTape)
            {
                case 0:
                    {
                        this.setCardSprite("�ӽ� ����", "�޼� ���", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ְ�, ��ȭ " + this.getWeaknessSt() +  "�� �ο��մϴ�.");
                        break;
                    }
                case 1:
                    {
                        this.setCardSprite("�ӽ� ��Ÿ", "�� �б�", "ī�带 �� �� �̽��ϴ�.");
                        break;
                    }
                case 2:
                    {
                        this.setCardSprite("ȭ�� ���", "ȭ�� ���", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ι� �ݴϴ�.");
                        break;
                    }
                case 3:
                    {
                        this.setCardSprite("ȭ�� ���", "ȭ�� ���", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
                        break;
                    }
                case 4:
                    {
                        this.setCardSprite("ȭ�� ���", "ȭ�� ���", "��Ÿ� 2ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
                        break;
                    }

            }

        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            this.cardDamage += DmgUp;
            setRange(1);
            this.setCardSprite("�ӽ� ��Ÿ", this.cardName + "+", "��Ÿ� 1ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
        }
        public void BasicRange()
        {  //���Ÿ� ���� �⺻��
            this.cardTape = 1;
            setRange(5);
            this.setCardSprite("ȭ�� ���", "ȭ�� ���", "��Ÿ� 5ĭ, " + cardDamage + "��ŭ ���ظ� �ݴϴ�.");
        }

        public override void UseCard(player Plr, Enemy enemy)
        {
            if (Plr != null)
            {
                if (enemy != null)
                {
                    enemy.be_hit(cardDamage);
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

