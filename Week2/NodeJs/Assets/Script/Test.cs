using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class Test : MonoBehaviour
{
    public struct SocketEvent
    {
        public string eventName;
        public string data;
        public string token;
        public string answer;

        public SocketEvent(string eventName, string data, string token, string answer)
        {
            this.eventName = eventName;
            this.data = data;
            this.token = token;
            this.answer = answer;
        }
    }

    WebSocket ws;

    public int[] numbers = {10, 20, 30, 40, 50 };
    public InputField studentIDInput;
    public int findAnswer;

    string tempData;
    public string myToken;

    //Start is called before the first frame update
    void Start()
    {
        string url = "ws://gi455-305013.an.r.appspot.com/";

        ws = new WebSocket(url);

        ws.Connect();

        ws.OnMessage += OnMessage;
    }

    // Update is called once per frame
    void Update()
    {
        Notify();
    }

    public void AnswerButton()
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if(i == findAnswer)
            {
                SocketEvent sendAnswerData = new SocketEvent();
                sendAnswerData.eventName = "SendAnswer";
                sendAnswerData.token = myToken;
                sendAnswerData.answer = numbers[i].ToString();

                string stringAns = JsonUtility.ToJson(sendAnswerData);
                ws.Send(stringAns);

                Debug.Log(stringAns);
                break;
            }            
        }
    }

    public void GetExamInfo()
    {
        SocketEvent getStudentData = new SocketEvent();

        getStudentData.eventName = "RequestExamInfo";

        getStudentData.token = myToken;

        string requestDataToJson = JsonUtility.ToJson(getStudentData);

        ws.Send(requestDataToJson);
    }

    public void GetStudentDataButton(string studentID)
    {
        studentID = studentIDInput.text;

        SocketEvent getStudentData = new SocketEvent();

        getStudentData.eventName = "GetStudentData";

        getStudentData.data = studentID;

        string requestDataToJson = JsonUtility.ToJson(getStudentData);

        ws.Send(requestDataToJson);

        //ws.OnMessage += OnMessage;
    }

    public void GetTokenButton(string studentID)
    {
        studentID = studentIDInput.text;

        SocketEvent getStudentData = new SocketEvent();

        getStudentData.eventName = "RequestToken";

        getStudentData.data = studentID;

        string requestDataToJson = JsonUtility.ToJson(getStudentData);

        ws.Send(requestDataToJson);
    }

    public void Notify()
    {
        if(string.IsNullOrEmpty(tempData) == false)
        {
            var receiveTempData = JsonUtility.FromJson<SocketEvent>(tempData);

            switch (receiveTempData.eventName)
            {
                case "RequestToken":
                    {
                        Debug.Log("My Token : " + receiveTempData.token);
                        //myToken = receiveTempData.token;
                        break;
                    }
                case "GetStudentData":
                    {
                        Debug.Log("My Data : " + receiveTempData.data);
                        break;
                    }
                case "RequestExamInfo":
                    {
                        Debug.Log("My Exam : " + receiveTempData.data);
                        break;
                    }
            }
        }

        tempData = "";
    }

    private void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        tempData = messageEventArgs.Data;
        Debug.Log(messageEventArgs.Data);
    }
}
