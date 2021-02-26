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
        public struct LoginEvent
        {

            public string UserID;
            public string Password;
            public string Name;
            public LoginEvent(string UserID, string Password, string Name)
            {

                this.UserID = UserID;
                this.Password = Password;
                this.Name = Name;
            }
        }
        public struct Register
        {
            public string Username;
            public string Password;
            public string Name;

            public Register(string Username, string Password, string Name)
            {

                this.Username = Username;
                this.Password = Password;
                this.Name = Name;
            }
        }
        public struct ChatEvent
        {
            public string UserID;
            public string Name;
            public string Message;
            public ChatEvent(string UserID, string Name, string Message)
            {
                this.UserID = UserID;
                this.Name = Name;
                this.Message = Message;
            }
        } 


        private WebSocket ws;

        private float countDelayPopup= 3.0f;

        public InputField chat;
        public InputField nameRoom;
        public InputField inputNameRoom;
        public InputField joinName;

        public InputField usernameText;
        public InputField passwordText;

        public InputField createUsernameInput;
        public InputField createPasswordInput;
        public InputField createPasswordConfirmInput;
        public InputField createName;


        public GameObject connectbutton;
        public GameObject chatUI;

        public GameObject scrollview;
        public GameObject buttonSendMessage;
        public GameObject chatpanel;
        public GameObject createRooms;
        public GameObject selectJoinRoom, joinRoomInput, joinRoomGameOb;
        public GameObject leaveButton;
        public GameObject popupError, popupErrorCreate;
        public GameObject roomNameTextGameob;
        public GameObject createRoomGameOb, createRoomInputOb;

        public GameObject popUpRegisterFail;
        public GameObject popUpRegisterComplete;

        public GameObject usernameGameobject;
        public GameObject passwordUser;
        public GameObject login;
        public GameObject signUp;

        public GameObject createUsername;
        public GameObject createPassword;
        public GameObject createNameUser;
        public GameObject createPasswordConfirm;

        public GameObject loginInterFace;
        public GameObject signUPInterFace;

        public GameObject createPopUpError;
        public GameObject buttonClosePopUpError;
        public GameObject registerErrorPopup;

        public GameObject usernameNull;
        public GameObject passwordNull;
        public GameObject rePasswordNull;
        public GameObject nameNull;

        public GameObject LoginError;
       
        

        public string chatTextshow;
        public string textgameobjectother;
        public string tempMessageString;

 
        public string username;
        public string message;
        public string password;
        private string ownuserID;
        private string ownName;

        public Text sendChatText;
        public Text receiveText;
        public Text roomNameText;

        public TextMeshProUGUI chatBoxText;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;
        public DelegateHandle OnReceiveMessage;
        public DelegateHandle OnLogin;
        public DelegateHandle OnRegister;
        public DelegateHandle OnAddMoney;
      
        private SocketEvent status;

        
        private void Awake()
        {
            chatUI.SetActive(false);
            createRooms.SetActive(false);
            selectJoinRoom.SetActive(false);
            scrollview.SetActive(false);
            buttonSendMessage.SetActive(false);
            leaveButton.SetActive(false);
            loginInterFace.SetActive(false);
            signUPInterFace.SetActive(false);
            popUpRegisterFail.SetActive(false);
            popUpRegisterComplete.SetActive(false);
            LoginError.SetActive(false);
        }
        void Update()
        {

            textgameobjectother = chat.text;
            UpdateNotifyMessage();
        }
        public void Start()
        {
            OnCreateRoom += CreateRoomDelegate;

        }

        public void CreateUserNameTodatabase()
        {
            if (createUsernameInput.text != "" && createPasswordInput.text != "" && createPasswordConfirmInput.text != "" && createName.text != "")
            {
                if (createPasswordInput.text != createPasswordConfirmInput.text)
                {
                    signUPInterFace.SetActive(false);
                    createPopUpError.SetActive(true);
                    buttonClosePopUpError.SetActive(true);
                    
                }
                else
                {

                    // CreateUser = createUsernameInput.text + "#" + createPasswordInput + "#" + createPasswordConfirmInput + "#" + createName + "#";

                    LoginEvent Register = new LoginEvent(createUsernameInput.text, createPasswordInput.text, createName.text);
                    string RegisterStr = JsonUtility.ToJson(Register);
                    SocketEvent socketEvent = new SocketEvent("Register", RegisterStr);
                    string toJsonStr = JsonUtility.ToJson(socketEvent);

                    Debug.Log("111");
                    //ws.Send(toJsonStr);
                    ws.Send(toJsonStr);


                    loginInterFace.SetActive(true);
                    signUPInterFace.SetActive(false);
                }
            }
            if (createUsernameInput = null)
            {
                usernameNull.SetActive(true);
                countDelayPopup -= Time.deltaTime;
                usernameNull.SetActive(false);
            }

            if (createPasswordInput = null)
            {
                passwordNull.SetActive(true);

                countDelayPopup -= Time.deltaTime;
                passwordNull.SetActive(true);
            }
            if (createPasswordConfirmInput = null)
            {
                rePasswordNull.SetActive(true);
                countDelayPopup -= Time.deltaTime;
            }
            if (createName = null)
            {
                nameNull.SetActive(true);
                countDelayPopup -= Time.deltaTime;
            }
        }
        public void ClosePopUpErrorSignUp()
        {
            buttonClosePopUpError.SetActive(false);
            createPopUpError.SetActive(false);
        }
        public void SignUpInterface()
        {
            signUPInterFace.SetActive(true);
            loginInterFace.SetActive(false);
        }

        public void ButtonBackToCreate()
        {
            signUPInterFace.SetActive(true);
        }
        public void ClickButtonToConnect()
        {
            string url = "ws://127.0.0.1:25500/";
            ws = new WebSocket(url);
            username = usernameText.text;
            loginInterFace.SetActive(true);
            connectbutton.SetActive(false);
            ws.OnMessage += OnMessage;
            ws.Connect();

        }

       // public void 

        public void SetacttiveUI()
        {
            //SetActive
            chatUI.SetActive(true);
            scrollview.SetActive(true);
            buttonSendMessage.SetActive(true);
            createRooms.SetActive(false);
            selectJoinRoom.SetActive(false);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
            chatBoxText.text = "";
        }

        public void RoomUI()
        {
            createRooms.SetActive(true);
            selectJoinRoom.SetActive(true);
            connectbutton.SetActive(false);
            usernameGameobject.SetActive(false);
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
                   

                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                    {
                        OnJoinRoom(receiveMessageData);
                    }


                    if (joinName.text == receiveMessageData.data)
                    {
                        JoinintoChat();
                        leaveButton.SetActive(true);
                        roomNameTextGameob.SetActive(true);
                    }
                    else
                    {
                        roomNameTextGameob.SetActive(false);
                        popupError.SetActive(true);
                        leaveButton.SetActive(true);
                    }

                }
                else if (receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                    
                }
                else if(receiveMessageData.eventName == "Message")
                {
                    Debug.Log("message : " + receiveMessageData.data);
                    if (OnReceiveMessage != null)
                        OnReceiveMessage(receiveMessageData);
                    
                }
                else if(receiveMessageData.eventName == "RequestToken")
                {
                    Debug.Log("message : " + receiveMessageData.data);
                }
                else if(receiveMessageData.eventName =="Login")
                {
                    if (OnLogin != null)
                        OnLogin(receiveMessageData);
                   

                    LoginEvent userstring = JsonUtility.FromJson<LoginEvent>(receiveMessageData.data);
                    
                    ownuserID = userstring.UserID;
                    ownName = userstring.Name;
                   
                    Debug.Log(name);


                    RoomUI();
                    loginInterFace.SetActive(false);
                }
                else if(receiveMessageData.eventName=="Register")
                {
                    if (OnRegister != null)
                        OnRegister(receiveMessageData);

                }
                else if(receiveMessageData.eventName == "SendMessage")
                {
                    Debug.Log("1");
                    ChatEvent ChatMessage = JsonUtility.FromJson<ChatEvent>(receiveMessageData.data);
                 
                    if (ChatMessage.UserID == ownuserID)
                    {

                        chatBoxText.text += "<align=right>" + ChatMessage.Name + ":<align=right>" + ChatMessage.Message + "\n";

                    }
                    else
                    {

                        chatBoxText.text += "\n<align=left>" + ChatMessage.Name + ":<align=left>" + ChatMessage.Message + "\n";
                    }


                }
                //else if(receiveMessageData.eventName =="Addmoney")
                //{
                //    if (OnAddMoney != null)
                //        OnAddMoney(receiveMessageData);
                //}
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

            SocketEvent socketEvent = new SocketEvent("CreateRoom", nameRoom.text);
            //NameRoom.text = GetRoomText;
            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
            roomNameText.text = nameRoom.text;

            Debug.Log(roomNameText.text);
            Debug.Log(nameRoom.text);


        }
       

        public void JoinintoRoom()
        {

            SocketEvent socketEvent = new SocketEvent("JoinRoom", joinName.text);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            //ws.Send(toJsonStr);
            ws.Send(toJsonStr);
            roomNameText.text = joinName.text;
        }
        public void LeaveToActive()
        {

            roomNameText.text = "";
            popupError.SetActive(false);
            chatUI.SetActive(false);
            createRooms.SetActive(true);
            selectJoinRoom.SetActive(true);
            scrollview.SetActive(false);
            buttonSendMessage.SetActive(false);
            usernameGameobject.SetActive(false);
            popupErrorCreate.SetActive(false);
        }
        public void SendChat()
        {

            var json = JsonUtility.ToJson(message);
            //ws.Send(json);
           
            ws.Send(textgameobjectother);
        }

        public void SendMessagetochat()
        {

            SendChat();
        }
        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {

            tempMessageString = messageEventArgs.Data;
            //Debug.Log("Message form Server : " + data.Message);
            Debug.Log(messageEventArgs.Data);

            if (messageEventArgs.Data == "false")
            {
                popupErrorCreate.SetActive(true);
                leaveButton.SetActive(true);
                roomNameTextGameob.SetActive(false);

            }


        }
        public void CreateRoomDelegate(SocketEvent state)
        {
            Debug.Log(state.data);

            if (state.data == "fail")
            {
                popupErrorCreate.SetActive(true);
                leaveButton.SetActive(true);
                roomNameTextGameob.SetActive(false);
                createRoomGameOb.SetActive(false);
                createRoomInputOb.SetActive(false);

            }
            else
            {
                JoinintoChat();
                createRoomGameOb.SetActive(false);
                createRoomInputOb.SetActive(false);
                Debug.Log(state);
            }
        }
       
        public void BackToInterFaceLogin()
        {
            loginInterFace.SetActive(true);
            popUpRegisterComplete.SetActive(false);
        }
        public  void LogginButton()
        {
            if(usernameText.text != ""||passwordText.text !="")
            {
                username = usernameText.text;
                password = passwordText.text;
                LoginEvent LoginUser = new LoginEvent(username,password,"");
                string LoginStr = JsonUtility.ToJson(LoginUser);
                SocketEvent socketEvent = new SocketEvent("Login", LoginStr);
                string toJsonStr = JsonUtility.ToJson(socketEvent);

                ws.Send(toJsonStr);

            }
            else
            {
                LoginError.SetActive(true);
                countDelayPopup -= Time.deltaTime;
                
                loginInterFace.SetActive(true);
                
            }

        }

        public void SendMessageText(InputField Message)
        {

            ChatEvent MessageSend = new ChatEvent(ownuserID, ownName, Message.text);
            Debug.Log(MessageSend);
            string SendInputChat = JsonUtility.ToJson(MessageSend);
            Debug.Log(SendInputChat);
            SocketEvent socketEvent = new SocketEvent("Chat", SendInputChat);
            Debug.Log(socketEvent);
            string toJsonStr = JsonUtility.ToJson(socketEvent);
            Debug.Log(toJsonStr);
            ws.Send(toJsonStr);

           
        }
        
    }
   
}

