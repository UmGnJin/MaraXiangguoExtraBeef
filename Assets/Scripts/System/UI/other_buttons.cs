using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon;

public class other_buttons : MonoBehaviour
{
    public void rest() {
        if (Dungeon.dungeon.Plr.isTurn > 0) {
            Dungeon.dungeon.Plr.StaminaChange(10);
            Dungeon.dungeon.Plr.drawCountting();
            Dungeon.dungeon.Plr.Turnend();//�ϳѱ��
        }
    }

    public void research_cancel() {
        UI.uicanvas.SetWii(0);
    }

    public void shuffle() {
        if (Dungeon.dungeon.Plr.isTurn > 0)
        {
            Dungeon.dungeon.Plr.isTurn -= 1;
            Dungeon.dungeon.Plr.allDeck.ChangDeck();
            Dungeon.dungeon.Plr.StaminaChange(10);
            Dungeon.dungeon.Plr.drawCountting();
            Dungeon.dungeon.Plr.Turnend();//�ϳѱ��
        }
    }

    public void exit() {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Qui();    //����Ƽ �����Ϳ����� ����� ����� ���ø����̼ǿ����� ����� ���� �ٸ���
    }

    public void treasure_yes() {
        //�ڿ��⿡�� ī�带 ���� �߰����ָ� ��
        UI.uicanvas.treasure_panel.SetActive(false);
    }

    public void treasure_no() {
        UI.uicanvas.treasure_panel.SetActive(false);
    }
}
