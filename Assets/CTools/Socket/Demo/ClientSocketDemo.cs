using System.Collections;  
using UnityEngine;

public class ClientSocketDemo : MonoBehaviour  
{  

	private CSocket ldysocket;  
	private string clientContent;  
	private string needSendText = "";  

	void Awake ()  
	{  
		ldysocket = new CSocket ();  
		ldysocket.InitClient ("127.0.0.1", 23456, (string msg) => {  
			clientContent = msg;  
		});  
	}  

	void OnGUI ()  
	{  
		needSendText = GUILayout.TextField (needSendText);  
		if (GUILayout.Button ("点击发送消息")) {  
			if (needSendText != "") {  
				ldysocket.ClientSendMessage (needSendText);  
			}  
		}  
	}  
}