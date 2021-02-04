using UnityEngine;

using System;
using System.Text;
using System.Collections.Generic;

#if !UNITY_WEBPLAYER
using WebSocketSharp;
#endif

public sealed class WebSocketSystem : MonoBehaviour
{
	readonly List<string> messages	= new List<string>();
	void OnOpen(string none)
	{
		text	= string.Empty;
		messages.Add("Opened");
		StartCoroutine(Ping(10));
	}
	void OnClose(string none)
	{
		messages.Add("Closed");
	}
	void OnError(string data)
	{
		messages.Add("Error : " + data);
	}
	void OnMessage(string data)
	{
		messages.Add(data);
	}

	string text	= null;
	void OnGUI()
	{
		while(messages.Count > 32)
			messages.RemoveAt(0);

		var builder = new StringBuilder();
		foreach(var message in messages)
			builder.AppendLine(message);

		GUI.skin.box.wordWrap	= true;
		GUI.skin.box.alignment	= TextAnchor.LowerLeft;
		GUI.Box(new Rect(10,10,500,300),builder.ToString());
		if(text == null)
			return;

		text	= GUI.TextField(new Rect(10,315,500,25),text);
		var e	= Event.current;
		if(e.isKey && !string.IsNullOrEmpty(text))
		{
			if(e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
			{
				Send(text);
				text	= string.Empty;
			}
		}
	}

	/* Real WebSocket Code */
	void Send(string message)
	{
		#if UNITY_WEBPLAYER
		gameObject.name	= Convert.ToBase64String(BitConverter.GetBytes(gameObject.GetInstanceID()));
		Application.ExternalCall("window['" + gameObject.name + "'].send",message);
		#else
		webSocket.Send(message);
		#endif
	}

	IEnumerator<object> Ping(int second)
	{
		if(second < 1)
			second	= 1;

		while(this.enabled)
		{
			yield return new WaitForSeconds(10);
			Send("");
		}
	}

	[SerializeField]
	string URL	= "websockettest-1.apphb.com";
	void Start()
	{
#if UNITY_WEBPLAYER
		string scheme	= new Uri(Application.absoluteURL).Scheme.Replace("file","http").Replace("http","ws");
		gameObject.name	= Convert.ToBase64String(BitConverter.GetBytes(gameObject.GetInstanceID()));
		messages.Add("Connect to " + scheme + "://" + URL);

		var code	= new StringBuilder();
		code.AppendLine("var unity = UnityObject2.instances[0];");
		code.AppendLine("var ws = new WebSocket('" + scheme + "://" + URL + "/');");
		code.AppendLine("ws.onopen = function() { unity.getUnity().SendMessage('" + gameObject.name + "','OnOpen',''); };");
		code.AppendLine("ws.onclose = function() { unity.getUnity().SendMessage('" + gameObject.name + "','OnClose',''); };");
		code.AppendLine("ws.onerror = function(e) { unity.getUnity().SendMessage('" + gameObject.name + "','OnError',e.message); };");
		code.AppendLine("ws.onmessage = function(e) { unity.getUnity().SendMessage('" + gameObject.name + "','OnMessage',e.data); };");
		code.AppendLine("window['" + gameObject.name + "'] = ws;");

		Application.ExternalEval(code.ToString());
#else
		try
		{
			webSocket	= new WebSocket((IsSecure ? "wss" : "ws") + "://" + URL);
			webSocket.OnOpen	+= (sender,ea) => OnOpen(null);
			webSocket.OnClose	+= (sender,ea) => OnClose(null);
			webSocket.OnMessage	+= (sender,mea) => OnMessage(mea.Data);
			webSocket.OnError	+= (sender,eea) => OnError(eea.Message);
			webSocket.Connect();
		}
		catch(Exception e)
		{
			OnError(e.Message);
		}
#endif
	}

#if !UNITY_WEBPLAYER
	[SerializeField]
	bool IsSecure	= false;
	WebSocket webSocket;
#endif
}
