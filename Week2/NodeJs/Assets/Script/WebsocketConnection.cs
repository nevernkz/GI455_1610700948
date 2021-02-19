using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;


namespace ProgramChat
{

    public class Messagedata
    {
        public string Username;
        public string Message;

        public Messagedata(string username, string message)
        {
            Username = username;
            Message = message;
        }
    }
    public class WebsocketConnection : MonoBehaviour
    {
        [SerializeField] Text text;
        private WebSocket websocket;

        public InputField Chat;
        public InputField UsernameText;


        public string username;
        public string message;
        public GameObject connectbutton;
        public GameObject ChatUI;
        public GameObject ChatTextShowGameObject;
        public GameObject ChatTextshows;
        public GameObject Scrollview;
        public GameObject ButtonSendMessage;
        public GameObject Chatpanel;
        public GameObject TextGameObject;
        public GameObject ChatShowText;
        public GameObject UsernameGameobject;

        public GameObject TextMeshPro;




        MessageEventArgs messageEventArgs;

        private string inputip, inputport, chatstr = null;
        public string ChatTextshow;
        public string Textgameobjectother;

        public string UserNumber;
        public TextMeshProUGUI textMesh;




        private void Awake()
        {
            ChatUI.SetActive(false);
            ChatTextShowGameObject.SetActive(false);
            Scrollview.SetActive(false);
            ButtonSendMessage.SetActive(false);
            ChatShowText.SetActive(false);
            UsernameGameobject.SetActive(true);
            TextMeshPro.SetActive(false);

        }
        private void Start()
        {

        }


        void Update()
        {
            chatstr = Chat.text;
            Textgameobjectother = Chat.text;
            ChatTextshows.GetComponent<Text>().text = ChatTextshow;

           

        }
        private void OnDestroy()
        {
            if (websocket != null)
            {
                websocket.Close();
            }
        }
        public void SendChat()
        {
            var message = new Messagedata(username, Chat.text);
            var json = JsonUtility.ToJson(message);
            websocket.Send(json);
            Chat.text = null;
            websocket.Send(Textgameobjectother);
        }
        public void SendMessagetochat(string text)
        {

            SendChat();
        }


        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            var data = JsonUtility.FromJson<Messagedata>(messageEventArgs.Data);
            if (data.Username == username)
            {
                Debug.Log("Same Account ");
                textMesh.text += "\n<align=right>" + data.Username + ": <align=right>" + data.Message;
            }
            else
            {
                Debug.Log("None Same Account ");
                textMesh.text += "\n<align=left>" + data.Username + ": <align=left>" + data.Message;
            }

            Debug.Log("Message form Server : " + data.Message);
            /*ChatTextshow += "\n" + messageEventArgs.Data;
            //ChatTextshow += "\n" + messageEventArgs.Data;
            textMesh.GetComponent<TextMeshProUGUI>().text = "\n<align=right>" + ChatTextshow;
            textMesh.GetComponent<TextMeshProUGUI>().text = "\n<align=left>" + ChatTextshow;
            ChatTextshows.GetComponent<Text>().text = ChatTextshow; */


        }

        public void ClickButtonToConnect()
        {

            //TextFormat


            websocket = new WebSocket("ws://127.0.0.1:25500/");
            username = UsernameText.text;
            websocket.OnMessage += OnMessage;
            websocket.Connect();
            SetacttiveUI();

        }
        public void SetacttiveUI()
        {
            //SetActive
            ChatUI.SetActive(true);
            ChatTextShowGameObject.SetActive(false);

            connectbutton.SetActive(false);
            Scrollview.SetActive(true);
            ButtonSendMessage.SetActive(true);
            ChatShowText.SetActive(false);
            TextMeshPro.SetActive(true);

            UsernameGameobject.SetActive(false);
        }

    }
   
}
    




