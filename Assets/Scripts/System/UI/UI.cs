using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaDungeon
{
    public class UI : MonoBehaviour //UI 관리자.
    {
        public static UI uicanvas;

        public Text message;
        public void Awake()
        {
            if (uicanvas == null)
            {
                uicanvas = this;
                //DontDestroyOnLoad(this); //씬 전환시 남아있도록 하는 기능.
            }
            else if (uicanvas != this)
                Destroy(this.gameObject);
            //위 코드는 UI 관리자를 싱글턴화하는 코드. 싱글턴 = 이 클래스 객체 단 하나로 유지.

            message = this.GetComponent<Text>();// 인게임 로그 텍스트가 올라갈 부분.
            if (message == null)
                Debug.Log("shdfg");
        }//싱글턴화

        public void ShowMessage(string msg)//매개변수를 인게임 화면에 메시지로 출력.
        {
            message.text += msg;
            message.text += "\n";
        }
    }
}