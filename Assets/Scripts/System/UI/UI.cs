using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaDungeon
{
    public class UI : MonoBehaviour
    {
        public static UI uicanvas;

        public Text message;
        public void Awake()
        {
            if (uicanvas == null)
            {
                uicanvas = this;
                //DontDestroyOnLoad(this);
            }
            else if (uicanvas != this)
                Destroy(this.gameObject);

            message = this.GetComponent<Text>();
            if (message == null)
                Debug.Log("shdfg");
        }//╫л╠шеох╜

        public void ShowMessage(string msg)
        {
            message.text += msg;
            message.text += "\n";
        }
    }
}