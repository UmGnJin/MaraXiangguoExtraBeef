using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArcanaDungeon;
using ArcanaDungeon.cards;

public class Show_deck : MonoBehaviour
{
    public void show_CardsDeck() {
        StartCoroutine(corout(1));
    }
    public void show_UsedDeck() {
        StartCoroutine(corout(2));
    }
    private IEnumerator corout(int para) {
        UI.uicanvas.Card_Select(para);
        yield return new WaitWhile(() => UI.uicanvas.GetSelecting());
        UI.uicanvas.GetSelected2();
    }
}
