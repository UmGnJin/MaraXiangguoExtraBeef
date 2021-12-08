using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArcanaDungeon;
using ArcanaDungeon.cards;

public class Cardlist_select : MonoBehaviour
{
    private Cards card_of_this;

    // Start is called before the first frame update
    void Awake()
    {
        this.card_of_this = null;
        this.gameObject.GetComponent<Button>().onClick.AddListener( delegate { UI.uicanvas.SetSelected2(null); } );
    }

    public void SetCOT(Cards c) {
        this.card_of_this = c;
    }
    public Cards GetCOT() {
        return this.card_of_this;
    }
}
