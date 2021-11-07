using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaDungeon
{
    public class UI : MonoBehaviour //UI ������.
    {
        public static UI uicanvas;

        public Text message;
        public void Awake()
        {
            if (uicanvas == null)
            {
                uicanvas = this;
                //DontDestroyOnLoad(this); //�� ��ȯ�� �����ֵ��� �ϴ� ���.
            }
            else if (uicanvas != this)
                Destroy(this.gameObject);
            //�� �ڵ�� UI �����ڸ� �̱���ȭ�ϴ� �ڵ�. �̱��� = �� Ŭ���� ��ü �� �ϳ��� ����.

            message = this.GetComponent<Text>();// �ΰ��� �α� �ؽ�Ʈ�� �ö� �κ�.
            if (message == null)
                Debug.Log("shdfg");
        }//�̱���ȭ

        public void ShowMessage(string msg)//�Ű������� �ΰ��� ȭ�鿡 �޽����� ���.
        {
            message.text += msg;
            message.text += "\n";
        }
    }
}