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
        public struct SocketEvent
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }
        private WebSocket ws;

        public InputField Chat;
        public InputField UsernameText;
        public InputField NameRoom;
        public InputField InputNameRoom;
        public InputField JoinName;


        public string username;
        public string message;
        public GameObject connectbutton;
        public GameObject ChatUI;

        public GameObject Scrollview;
        public GameObject ButtonSendMessage;
        public GameObject Chatpanel;
        public GameObject UsernameGameobject;
        public GameObject CreateRooms;
        public GameObject SelectJoinRoom,JoinRoomInput,JoinRoomGameOb;
        public GameObject LeaveButton;
        public GameObject PopupError,PopupErrorCreate;
        public GameObject RoomNameTextGameob;
        public GameObject CreateRoomGameOb,CreateRoomInputOb;

        public string ChatTextshow;
        public string Textgameobjectother;
        public string tempMessageString;



        public Text SendChatText;
        public Text ReceiveText;
        public Text RoomNameText;



        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;


       
        private void Awake()
        {
            ChatUI.SetActive(false);
            CreateRooms.SetActive(false);
            SelectJoinRoom.SetActive(false);
            Scrollview.SetActive(false);
            ButtonSendMessage.SetActive(false);
            UsernameGameobject.SetActive(true);
            LeaveButton.SetActive(false);
        }
        void Update()
        {

            Textgameobjectother = Chat.text;
            UpdateNotifyMessage();
        }
        public void Start()
        {
            OnCreateRoom += CreateRoomDelegate;
          
        }
        public void SendChat()
        {

            var json = JsonUtility.ToJson(message);
            ws.Send(json);
            Chat.text = null;
            ws.Send(Textgameobjectother);
        }

        public void SendMessagetochat()
        {

            SendChat();
        }

        public void ClickButtonToConnect()
        {

            string url = "ws://127.0.0.1:25500/";

            ws = new WebSocket(url);
            username = UsernameText.text;
            ws.OnMessage += OnMessage;

            ws.Connect();
            RoomUI();
        }
        public void SetacttiveUI()
        {
            //SetActive
            ChatUI.SetActive(true);
            Scrollview.SetActive(true);
            ButtonSendMessage.SetActive(true);
            CreateRooms.SetActive(false);
            SelectJoinRoom.SetActive(false);
        }
       
        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void RoomUI()
        {
            CreateRooms.SetActive(true);
            SelectJoinRoom.SetActive(true);
            connectbutton.SetActive(false);
            UsernameGameobject.SetActive(false);
        }
        public void JoinintoChat()
        {
            SetacttiveUI();
        }
        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }
        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                    
                        OnCreateRoom(receiveMessageData);
                    Debug.Log(tempMessageString);
                    //if(tempMessageString == "fail")
                    //{
                    //    PopupError.SetActive(true);
                    //    LeaveButton.SetActive(true);
                    //    RoomNameTextGameob.SetActive(false);
                    //}
                    //else
                    //{
                    //    JoinintoChat();
                    //}

                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                    {
                        OnJoinRoom(receiveMessageData);
                    }
                       

                    if (JoinName.text == receiveMessageData.data)
                    {
                        JoinintoChat();
                        LeaveButton.SetActive(true);
                        RoomNameTextGameob.SetActive(true);
                    }
                    else
                    {
                        RoomNameTextGameob.SetActive(false);
                        PopupError.SetActive(true);
                        LeaveButton.SetActive(true);
                    }

                }
                else if (receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                }

                tempMessageString = "";
            }

        }
        private void OnDestroy()
        {
            if (ws != null)
            {
                ws.Close();
            }
        }

       
        public void CreateRoomButton()
        {

            SocketEvent socketEvent = new SocketEvent("CreateRoom", NameRoom.text);
            //NameRoom.text = GetRoomText;
            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
            RoomNameText.text = NameRoom.text;
            
            Debug.Log(RoomNameText.text);
            Debug.Log(NameRoom.text);
            

        }

        public void JoinintoRoom()
        {

            SocketEvent socketEvent = new SocketEvent("JoinRoom", JoinName.text);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            //ws.Send(toJsonStr);
            ws.Send(toJsonStr);
            RoomNameText.text = JoinName.text;

           


        }
        public void LeaveToActive()
        {

            RoomNameText.text = "";
            PopupError.SetActive(false);
            ChatUI.SetActive(false);
            CreateRooms.SetActive(true);
            SelectJoinRoom.SetActive(true);
            Scrollview.SetActive(false);
            ButtonSendMessage.SetActive(false);
            UsernameGameobject.SetActive(false);
            PopupErrorCreate.SetActive(false);
        }
        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {

            tempMessageString = messageEventArgs.Data;
            //Debug.Log("Message form Server : " + data.Message);
            Debug.Log(messageEventArgs.Data);
            if(messageEventArgs.Data== "false")
            {
                PopupErrorCreate.SetActive(true);
                LeaveButton.SetActive(true);
                RoomNameTextGameob.SetActive(false);

            }




        }
        public void CreateRoomDelegate(SocketEvent state)
        {
            Debug.Log(state.data);

            if (state.data == "fail")
            {
                PopupErrorCreate.SetActive(true);
                LeaveButton.SetActive(true);
                RoomNameTextGameob.SetActive(false);
                CreateRoomGameOb.SetActive(false);
                CreateRoomInputOb.SetActive(false);

            }
            else
            {
                JoinintoChat();
                CreateRoomGameOb.SetActive(false);
                CreateRoomInputOb.SetActive(false);
                Debug.Log(state);
            }
        }

      
    }

}

