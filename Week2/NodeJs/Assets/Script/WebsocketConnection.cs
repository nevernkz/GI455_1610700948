using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;


namespace ProgramChat
{
   

    public class WebsocketConnection : MonoBehaviour
    {
        [SerializeField] Text text;
        private WebSocket websocket;
        public InputField inputIP;
        public InputField inputPort;
        public InputField Chat;
       

        public GameObject Userno;
        public GameObject IP, Port, textAddress, TextPort, connectbutton;
        public GameObject ChatUI;
        public GameObject ChatTextShowGameObject;
        public GameObject ChatTextshows;
        public GameObject Scrollview;
        public GameObject ButtonSendMessage;
        public GameObject Chatpanel;
        public GameObject TextGameObject;
        public GameObject ChatShowText;


        public GameObject TextMeshPro;

        


        MessageEventArgs messageEventArgs;

        private string inputip, inputport, chatstr = null;
        public string ChatTextshow;
        public string Textgameobjectother;

        public string UserNumber;
        public TextMeshProUGUI textMesh;
        public int user;
     


        private void Awake()
        {
            ChatUI.SetActive(false);
            ChatTextShowGameObject.SetActive(false);
            Scrollview.SetActive(false);
            ButtonSendMessage.SetActive(false);
            ChatShowText.SetActive(false);
            Userno.SetActive(false);
            TextMeshPro.SetActive(false);
         
            
            
            this.user = 1;



        }
        private void Start()
        {
            
        }


        void Update()
        {
            chatstr = Chat.text;
            Textgameobjectother = Chat.text;
            ChatTextshows.GetComponent<Text>().text = ChatTextshow;

            if (user == 0)
            {
                textMesh.GetComponent<TextMeshProUGUI>().text = "\n<align=right>" + ChatTextshow;
            }
            else
            {
                textMesh.GetComponent<TextMeshProUGUI>().text = "\n<align=left>" + ChatTextshow;
            }
            

        }
        private void OnDestroy()
        {
            if (websocket != null)
            {
                websocket.Close();
            }
        }
        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {

           
            Debug.Log("Message form Server : " + messageEventArgs.Data);
            ChatTextshow += "\n" + messageEventArgs.Data;
            ChatTextshows.GetComponent<Text>().text = ChatTextshow;
            

        }

        public void ClickButtonToConnect()
        {

            //TextFormat
            inputip = inputIP.text;
            inputport = inputPort.text;

            websocket = new WebSocket("ws://" + inputip + ":" + inputport + "/");

            websocket.OnMessage += OnMessage;
            websocket.Connect();
            SetacttiveUI();

        }
        public void SetacttiveUI()
        {
            //SetActive
            ChatUI.SetActive(true);
            ChatTextShowGameObject.SetActive(false);
            IP.SetActive(false);
            Port.SetActive(false);
            Userno.SetActive(false);
            textAddress.SetActive(false);
            TextPort.SetActive(false);
            connectbutton.SetActive(false);
            Scrollview.SetActive(true);
            ButtonSendMessage.SetActive(true);
            ChatShowText.SetActive(false);
            TextMeshPro.SetActive(true);


        }

        public void SendChat()
        {

            Chat.text = null;
            websocket.Send(Textgameobjectother);
        }
        public void SendMessagetochat(string text)
        {
            SendChat();
        }

    }
   
}
    




