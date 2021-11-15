using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class temp_tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    public GameObject tooltip_txt;
    float t;
    bool on;

    // Start is called before the first frame update
    void Awake()
    {
        on = false;
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (on) {
            t += Time.deltaTime;
        }
        tooltip.GetComponent<Image>().color = new Color(1f, 1f, 1f, t);
        tooltip_txt.GetComponent<Text>().color = new Color(0f, 0f, 0f, t);
    }

    public void OnPointerEnter(PointerEventData e) {
        t = -0.1f;
        on = true;
        tooltip.SetActive(true);
        tooltip_txt.SetActive(true);
    }
    public void OnPointerExit(PointerEventData e) {
        on = false;
        Debug.Log("testing");
        tooltip.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        tooltip.SetActive(false);
        tooltip_txt.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0f);
        tooltip_txt.SetActive(false);
    }
}
