using System.Collections;  
using UnityEngine;

public class ServerSocketDemo : MonoBehaviour  
{  
	private CSocket ldysocket;  
	string serverContent;  


	void Awake ()  
	{  
		ldysocket = new CSocket ();  
		//执行初始化服务器方法，传入委托函数  
		ldysocket.InitServer (ShowMsg);  
	}  

	void ShowMsg (string msg)  
	{  
		serverContent = msg;  
	}  

	void OnGUI ()  
	{  
		GUILayout.Label (serverContent);  
	}  
}  