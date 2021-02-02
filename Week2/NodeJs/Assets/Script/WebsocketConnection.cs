using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ProgramChat
{

    public class WebsocketConnection : MonoBehaviour
    {
        private WebSocket websocket;
        public InputField inputIP;
        public InputField inputPort;
        public string inputip, inputport,chatstr = null;
        private SceneManager sceneIP;
        public InputField Chat;
        public GameObject ChatUI;
        public GameObject ChatTextShow;
        public GameObject ChatTextshows;
        public string ChatTextshow = null;
        public string[] ListChat;
        int i = 0;
        
        
        
        private void Awake()
        {
            ChatUI.SetActive(false);
            ChatTextShow.SetActive(false);
            
            
            
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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                i++;
                ListChat = new string[i];
                

               
                Chat.text = "";
                ChatTextshows.GetComponent<Text>().text = ChatTextshow;
                
                
                //chatstr = Chat.text;
                websocket.Send(chatstr);
                

                //if enter clear inputfield


                //websocket.Send("Number : " + Random.Range(0, 9999));
                
                

            }
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
            ChatUI.SetActive(true);
            ChatTextShow.SetActive(true);
            inputip = inputIP.text;
            inputport = inputPort.text;

            websocket = new WebSocket("ws://"+inputip+":"+inputport+"/");
            websocket.OnMessage += OnMessage;
            websocket.Connect();
            
        }
    }
}


