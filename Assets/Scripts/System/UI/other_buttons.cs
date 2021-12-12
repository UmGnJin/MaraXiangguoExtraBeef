using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon;

public class other_buttons : MonoBehaviour
{
    public void rest() {
        if (Dungeon.dungeon.Plr.isTurn > 0) {
            Dungeon.dungeon.Plr.isTurn -= 1;
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
        }
    }

    public void exit() {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Qui();    //����Ƽ �����Ϳ����� ����� ����� ���ø����̼ǿ����� ����� ���� �ٸ���
    }
}
