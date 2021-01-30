using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace ProgramChat
{
    public class WebsocketConnection : MonoBehaviour
    {
        private WebSocket websocket;
        // Start is called before the first frame update
        void Start()
        {
            websocket = new WebSocket("ws://127.0.0.1:49151/");

            websocket.OnMessage += OnMessage;

            websocket.Connect();

            //websocket.Send("I'm cuming");

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(websocket.ReadyState == WebSocketState.Open)
                {
                    websocket.Send("Random number :"+Random.Range(0,9999));

                }
            }
        }
        private void OnDestroy()
        {
            if(websocket != null)
            { 
                websocket.Close(); 
            }
            
        }
        public void OnMessage(object sender , MessageEventArgs messageEventArgs)
        {
            Debug.Log("Receive msg" + messageEventArgs);
        }
    }
}


