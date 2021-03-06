using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ExamTest : MonoBehaviour
{
    public struct SocketEvent
    {
        public string eventName;
        public string data;
        public string studentID;
        public string token;
        public string answer;

        public SocketEvent(string eventName, string data, string studentID, string token, string answer)
        {
            this.eventName = eventName;
            this.data = data;
            this.studentID = studentID;
            this.token = token;
            this.answer = answer;
        }
    }

    WebSocket ws;
    public GameObject token;
    public GameObject buttonConnect;
    public InputField studenIDandTokenInputF;
    public InputField answerInputF;
    private string tempMessageString;



    // Start is called before the first frame update
    void Start()
    {
        token.SetActive(false);
        buttonConnect.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNotify();
    }

    public void ButtonConnect()
    {
        token.SetActive(true);
        buttonConnect.SetActive(false);
        string url = "ws://gi455-305013.an.r.appspot.com/";
        ws = new WebSocket(url);
        ws.Connect();
        ws.OnMessage += OnMessage;

        Debug.Log("Connect To server");

    }

    public void GetStudenDataButton(string studentID)
    {
        studentID = studenIDandTokenInputF.text;
        SocketEvent getStudentData = new SocketEvent();
        getStudentData.eventName = "GetStudentData";
        getStudentData.studentID = studentID;
        string requestDataToJson = JsonUtility.ToJson(getStudentData);
        ws.Send(requestDataToJson);
    }

    public void GetTokenButton(string studentID)
    {
        studentID = studenIDandTokenInputF.text;
        SocketEvent getStudentData = new SocketEvent();
        getStudentData.eventName = "RequestToken";
        getStudentData.studentID = studentID;
        string requestdataToJson = JsonUtility.ToJson(getStudentData);
        ws.Send(requestdataToJson);
    }
    public void GetExamButton()
    {
        SocketEvent getStudentData = new SocketEvent();
        getStudentData.eventName = "RequestExamInfo";
        getStudentData.token = studenIDandTokenInputF.text;
        string requestDataToJson = JsonUtility.ToJson(getStudentData);
        ws.Send(requestDataToJson);
    }

    public void AnswerButton()
    {
        if(answerInputF.text !="")
        {
            SocketEvent sendAnswerData = new SocketEvent();
            sendAnswerData.eventName = "SendAnswer";
            sendAnswerData.token = studenIDandTokenInputF.text;
            sendAnswerData.answer = answerInputF.text;

            string stringAnswer = JsonUtility.ToJson(sendAnswerData);
            ws.Send(stringAnswer);
            Debug.LogError(stringAnswer);
        }
        else
        {
            Debug.Log("Nothing Text");
        }
    }


    public void UpdateNotify()
    {
        if (string.IsNullOrEmpty(tempMessageString) == false)
        {
            SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

            switch (receiveMessageData.eventName)
            {
                case "RequestToken":
                    {

                        Debug.Log("Your Token : " + receiveMessageData.token);

                        break;
                    }
                case "GetStudentData":
                    {
                        Debug.Log("Your Data : " + receiveMessageData.data);
                        break;
                    }
                case "RequestExamInfo":
                    {
                        Debug.Log("My Exam " + receiveMessageData.data) ;
                            break;
                    }
                case "SendAnwser":
                    {
                        Debug.Log("My Answer " + receiveMessageData.answer);
                        break;
                    }
            }
        }
        tempMessageString = "";
    }


    private void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        tempMessageString = messageEventArgs.Data;
        Debug.Log(messageEventArgs.Data);
    }

}
