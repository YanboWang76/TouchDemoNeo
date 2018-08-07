using UnityEngine;
using System.Collections;

//关于网络
using System.Net;

//关于套接字
using System.Net.Sockets;

//关于文本
using System.Text;

//声明一个委托
public delegate void ldyReceiveCallBack (string content);

public class CSocket
{
	#region 服务器端

	//声明一个服务器端的套接字
	Socket serverSocket;
	//声明一个委托对象
	ldyReceiveCallBack serverCallBake;
	//生成一个比特缓存
	byte[] serverBuffer = new byte[1024];
	//初始化服务器
	public void InitServer (ldyReceiveCallBack rcb)
	{  
		//传入委托对象  
		serverCallBake = rcb;  
		//初始化服务器端的套接字  
		serverSocket = new Socket (AddressFamily.InterNetwork/*IPV4*/, SocketType.Stream/*双向读写流（服务端可以发给客户 客户也可以发服务）*/,  
			ProtocolType.Tcp/*TCP协议*/);  
		//实例一个网络端点  传入地址和端口  
		IPEndPoint serverEP = new IPEndPoint (IPAddress.Any, 23456);  
		//绑定网络端点  
		serverSocket.Bind (serverEP);  
		//设置最大监听数量  
		serverSocket.Listen (2);  
		//异步接受客户端的连接(CallBack)  
		serverSocket.BeginAccept (new System.AsyncCallback (ServerAccept), serverSocket);  
		//发送一个消息 表示服务器已经创建  
		serverCallBake ("Server Has Init");  
	}
	//服务器接受
	void ServerAccept (System.IAsyncResult ar)
	{  
		//接受结果状态  
		serverSocket = ar.AsyncState  as Socket;  
		//接收结果  
		Socket workingSocket = serverSocket.EndAccept (ar);  

		workingSocket.BeginReceive (serverBuffer/*消息缓存*/,   
			0/*接受消息的偏移量 就是从第几个开始*/,   
			this.serverBuffer.Length/*设置接受字节数*/,  
			SocketFlags.None/*Socket标志位*/,   
			new System.AsyncCallback (ServerReceive)/*接受回调*/,   
			workingSocket/*最后的状态*/);  

		//继续接受客户端的请求  
		workingSocket.BeginAccept (new System.AsyncCallback (ServerAccept), workingSocket);  

	}

	void ServerReceive (System.IAsyncResult ar)
	{  
		//获取正在工作的Socket对象（用来接受数据的 ）  
		Socket workingSocket = ar.AsyncState as Socket;  
		//接受到得数据字节   
		int byteCount = 0;  
		//接收到的数据字符串  
		string content = "";  
		try {  
			byteCount = workingSocket.EndReceive (ar);  

		} catch (SocketException ex) {  
			//如果接受失败 返回详细异常  
			serverCallBake (ex.ToString ());  
		}  
		if (byteCount > 0) {  
			//转换byte数组为字符串（支持中文）  
			content = UTF8Encoding.UTF8.GetString (serverBuffer);  
		}  
		//发送接收到的消息  
		serverCallBake (content);  
		//继续接受消息  
		workingSocket.BeginReceive (serverBuffer/*消息缓存*/,   
			0/*接受消息的偏移量 就是从第几个开始*/,   
			this.serverBuffer.Length/*设置接受字节数*/,  
			SocketFlags.None/*Socket标志位*/,   
			new System.AsyncCallback (ServerReceive)/*接受回调*/,   
			workingSocket/*最后的状态*/);  
	}

	#endregion

	#region  客户端

	//声明客户端的套接字
	Socket clientSocket;
	//声明客户端的委托对象
	ldyReceiveCallBack clientReceiveCallBack;
	//声明客户端的缓存1KB
	byte[] clientBuffer = new byte[1024];
	//1.ip地址 2.端口3.委托对象
	public void InitClient (string ip, int port, ldyReceiveCallBack rcb)
	{  
		//接受委托对象  
		clientReceiveCallBack = rcb;  
		//实例客户端的Socket 参数（IPV4 ，双向读写流，TCP协议）  
		clientSocket = new Socket (AddressFamily.InterNetwork,  
			SocketType.Stream, ProtocolType.Tcp);  
		//实例化一个客户端的网络端点        IPAddress.Parse (ip)：将IP地址字符串转换为Ip地址实例  
		IPEndPoint clientEP = new IPEndPoint (IPAddress.Parse (ip), port);  
		//连接服务器  
		clientSocket.Connect (clientEP);  
		//第一个是缓存  第二个 是从第几个开始接受 第三个 接受多少个字节  第四个 需不需要特殊的服务 第五个回调函数 第六个当前对象  
		clientSocket.BeginReceive (clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None,  
			new System.AsyncCallback (clientReceive), this.clientSocket);  
	}

	void clientReceive (System.IAsyncResult ar)
	{  
		//获取一个客户端正在接受数据的对象  
		Socket workingSocket = ar.AsyncState as Socket;  
		int byteCount = 0;  
		string content = "";  
		try {  
			//结束接受数据 完成储存  
			byteCount = workingSocket.EndReceive (ar);  

		} catch (SocketException ex) {  
			//如果接受消息失败  
			clientReceiveCallBack (ex.ToString ());  
		}  
		if (byteCount > 0) {  
			//转换已经接受到得Byte数据为字符串  
			content = UTF8Encoding.UTF8.GetString (clientBuffer);  
		}  
		//发送数据  
		clientReceiveCallBack (content);  
		//接受下一波数据  
		clientSocket.BeginReceive (clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None,  
			new System.AsyncCallback (clientReceive), this.clientSocket);  

	}

	public void ClientSendMessage (string msg)
	{  
		if (msg != "") {  
			//将要发送的字符串消息转换成BYTE数组  
			clientBuffer = UTF8Encoding.UTF8.GetBytes (msg);  
		}  
		clientSocket.BeginSend (clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None,  
			new System.AsyncCallback (SendMsg),  
			this.clientSocket);  
	}

	void SendMsg (System.IAsyncResult ar)
	{  
		Socket workingSocket = ar.AsyncState as Socket;  
		workingSocket.EndSend (ar);  
	}

	#endregion
}  