using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

namespace ProgramChat
{

    public class WebsocketConnection : MonoBehaviour
    {
        private WebSocket websocket;
        public InputField inputIP;
        public InputField inputPort;
        public string inputip, inputport;

        
        /*void Start()
        {
            websocket = new WebSocket("ws://127.0.0.1:25500/");

            websocket.OnMessage += OnMessage;

            websocket.Connect();

        }*/
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                websocket.Send("Number : " + Random.Range(0, 9999));
                

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
        }

        public void ClickButtonToConnect()
        {
            inputip = inputIP.text;
            inputport = inputPort.text;


            websocket = new WebSocket("ws://"+inputip+":"+inputport+"/");
            websocket.OnMessage += OnMessage;
            websocket.Connect();
        }
    }
}


