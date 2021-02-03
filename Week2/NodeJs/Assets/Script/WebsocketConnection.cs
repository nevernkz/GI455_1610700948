using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;




namespace ProgramChat
{
   
    public class WebsocketConnection : MonoBehaviour
    {
        [SerializeField] Text text;
        private WebSocket websocket;
        public InputField inputIP;
        public InputField inputPort;
        public InputField Chat;
      
        public GameObject IP, Port, textAddress , TextPort, connectbutton;
        public GameObject ChatUI;
        public GameObject ChatTextShowGameObject;
        public GameObject ChatTextshows;
        public GameObject Scrollview;
        public GameObject ButtonSendMessage;
        public GameObject Chatpanel;
        public GameObject TextGameObject;
       

        MessageEventArgs messageEventArgs;

        private string inputip, inputport, chatstr = null;
        public string ChatTextshow = null;
        public string Textgameobjectother;

        [SerializeField]
        List<Message> messagesList = new List<Message>();
            private void Awake()
        {
            ChatUI.SetActive(false);
            ChatTextShowGameObject.SetActive(false);
            Scrollview.SetActive(false);
            ButtonSendMessage.SetActive(false);
            


        }
        public void ChatMaija(GameObject ChatAdobe)
        {
            //chatAdobe.Add(chatstr);
        }

        /*
        void Start()
        {
            
              websocket = new WebSocket("ws://127.0.0.1:25500/");

            websocket.OnMessage += OnMessage;

            websocket.Connect();
        }
            */

    
        void Update()
        {
            chatstr = Chat.text;
            
            ChatTextshows.GetComponent<Text>().text = ChatTextshow;
            //if (Input.GetKeyDown(KeyCode.Return))
            //{
            //    Chat.text = null;
            //    chatstr = Chat.text;
            //    websocket.Send(chatstr);
                



            //    //websocket.Send("Number : " + Random.Range(0, 9999));

            //}

        }
        private void OnDestroy()
        {
            if(websocket != null)
            { 
                websocket.Close(); 
            }
        }
        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            
            Debug.Log("Message form Server : " + messageEventArgs.Data);
            ChatTextshow = messageEventArgs.Data;

            ChatTextshows.GetComponent<Text>().text = ChatTextshow;
            

        }

        public void ClickButtonToConnect()
        {
           
            //TextFormat
            inputip = inputIP.text;
            inputport = inputPort.text;
            
            websocket = new WebSocket("ws://"+inputip+":"+inputport+"/");
            
            websocket.OnMessage += OnMessage;
            websocket.Connect();
            
            

            SetacttiveUI();

        }
        public void SetacttiveUI()
        {
            //SetActive
            ChatUI.SetActive(true);
            ChatTextShowGameObject.SetActive(true);
            IP.SetActive(false);
            Port.SetActive(false);
            textAddress.SetActive(false);
            TextPort.SetActive(false);
            connectbutton.SetActive(false);
            Scrollview.SetActive(true);
            ButtonSendMessage.SetActive(true);
        }
        
        public void SendChat()
        {
            //clearInputfield chat
            Chat.text = null;
            //chatstr = Chat.text;
            websocket.Send(chatstr);
            SendMessagetochat();


        }
        public void SendMessagetochat()
        {
            Message newChat = new Message();
            //text = Chat.text;
            Debug.Log(chatstr);
            //newChat.text = text;
            GameObject newText = Instantiate(TextGameObject, Chatpanel.transform);
            newChat.textObject = newText.GetComponent<Text>();
            newChat.textObject.text = newChat.text;
            messagesList.Add(newChat);
            //SendMessagetochat(Chat.text);
        }
        
    }
    
}
[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}


