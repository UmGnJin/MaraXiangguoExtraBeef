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
        //Application.Qui();    //유니티 에디터에서의 종료와 빌드된 어플리케이션에서의 종료는 서로 다르다
    }
}
