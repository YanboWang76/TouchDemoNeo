using UnityEngine;
using System.IO;
using System;
using System.Text;


/// <summary>
/// 各种文件的读写复制操作,主要是对System.IO的一些封装
/// </summary>
public static class CIO  {
	private const string TAG = "CLF CIO";
	#region 文件夹操作
	/// <summary>
	/// 创建文件夹
	/// </summary>
	/// <returns><c>true</c>创建成功<c>false</c>创建失败</returns>
	/// <param name="dirName">文件夹路径</param>
	public static bool CreatDir(this string dirName)
	{
		try
		{
			dirName = dirName.Trim();
			if(string.IsNullOrEmpty(dirName))
			{
				Debug.unityLogger.LogError(TAG,"Creat Dir Error :"+"dir Name error "+dirName);
				return false;
			}
			if(!Directory.Exists(dirName))
				Directory.CreateDirectory(dirName);
			return true;
		}
		catch(IOException e)
		{
			Debug.unityLogger.LogError(TAG,"Creat Dir Error :"+ e.Message);
			return false;
		}	
	}

	#endregion

	#region 文件操作

	/// <summary>
	/// 判断文件是否存在
	/// </summary>
	/// <returns><c>true</c>,文件存在, <c>false</c> 文件不存在 </returns>
	/// <param name="fileName">文件路径</param>
	public static bool ExistsFile(this string fileName)
	{
		fileName = fileName.Trim ();
		if(string.IsNullOrEmpty(fileName))
			return false;
		bool fileExisted = File.Exists (fileName);
		if (!fileExisted)
			Debug.unityLogger.LogError (TAG, "file is not exist :" + fileName);
		return fileExisted;
	}

	/// <summary>
	/// 创建文件 
	/// </summary>
	/// <returns><c>true</c>文件创建成功<c>false</c>文件已经存在，创建失败</returns>
	/// <param name="fileName">File name.</param>
	public static bool CreateFile(this string fileName){
		try
		{
			if(fileName.ExistsFile()) return false;
			var fs = File.Create(fileName.Trim());
			fs.Close();
			fs.Dispose();
		}
		catch(IOException e)
		{
			Debug.unityLogger.LogError (TAG, "Creat File Error :" + e.Message);
		}
		return true;
	}

	/// <summary>
	/// 读文件内容，转换为字符串类型
	/// </summary>
	/// <param name="fileName">文件路径</param>
	public static string ReadStr(this string fileName)
	{
		if (!fileName.ExistsFile ())
			return string.Empty;
		else {
			using(var fs = new FileStream(fileName.Trim(),FileMode.Open))
			{
				return new StreamReader (fs).ReadToEnd ();
			}
		}
	}
	/// <summary>
	/// 读取文件内容转换为Char数组
	/// </summary>
	/// <returns>Char数组</returns>
	/// <param name="fileName">文件路径</param>
	public static Char[] ReadChar(this string fileName){
		if (!fileName.ExistsFile ())
			return null;
		var byData = new byte[1024];
		var charData = new char[1024];
		FileStream mFileStream = null;
		try
		{
			mFileStream = new FileStream(fileName, FileMode.Open);
			mFileStream.Seek(135, SeekOrigin.Begin);
			mFileStream.Read(byData, 0, 1024);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		finally{
			if (null != mFileStream)
				mFileStream.Close ();
		}
		var decoder = Encoding.UTF8.GetDecoder();
		decoder.GetChars(byData, 0, byData.Length, charData, 0);
		return charData;
	}

	/// <summary>
	/// 读取文件内容转换为byte[]
	/// </summary>
	/// <returns>字节数组</returns>
	/// <param name="fileName">文件路径</param>
	public static byte[] ReadByte(this string fileName)
	{
		if (!fileName.ExistsFile ())
			return null;
		FileStream pFileStream = null;
		try{
			pFileStream = new FileStream(fileName,FileMode.Open,FileAccess.Read);
			var r = new BinaryReader(pFileStream);
			r.BaseStream.Seek(0,SeekOrigin.Begin);
			var pReadByte = r.ReadBytes((int)r.BaseStream.Length);
			return pReadByte;
		}catch(Exception e){
			Debug.unityLogger.LogError (TAG,"ReadByte Error: "+ e.Message);
		}finally{
			if (null != pFileStream)
				pFileStream.Close ();
		}
		return null;
	}
	/// <summary>
	/// 向fileName中写入pReadByte字节流
	/// </summary>
	/// <returns><c>true</c>写入成功<c>false</c>写入失败</returns>
	/// <param name="fileName">文件路径</param>
	/// <param name="pReadByte">要写入的字节流</param>
	public static bool WriteFile(this string fileName,byte[] pReadByte)
	{
		if (null == pReadByte|| !fileName.ExistsFile ())
			return false;
		try
		{
			using(var fs = new FileStream(fileName,FileMode.OpenOrCreate))
			{
				lock(fs)
				{
					if(!fs.CanWrite)
					{
						Debug.unityLogger.LogError(TAG,"WriteFile Error "+ fileName+" can not be writed!");
						return false;
					}
					fs.Write(pReadByte,0,pReadByte.Length);
				}
			}	
		}
		catch(IOException ex){
			Debug.unityLogger.LogError (TAG,"WriteFile Error " + ex.Message);
			return false;
		}
		return true;
	}
	/// <summary>
	/// 写入字符串
	/// </summary>
	/// <returns><c>true</c>写入成功 <c>false</c>写入失败</returns>
	/// <param name="fileName">文件路径</param>
	/// <param name="context">要写入的文本字符串</param>
	public static bool WriteFile(this string fileName ,string context){
		if (string.IsNullOrEmpty (context) || !fileName.ExistsFile ())
			return false;
		byte[] buffer = Encoding.Default.GetBytes (context);
		return fileName.WriteFile (buffer);
	}
	/// <summary>
	/// 向文件中写入一行文本
	/// </summary>
	/// <returns><c>true</c>写入成功<c>false</c>写入失败</returns>
	/// <param name="fileName">文件路径</param>
	/// <param name="context">文本内容</param>
	public static bool WriteFileLine(this string fileName,string context)
	{
		if(string.IsNullOrEmpty(fileName)||string.IsNullOrEmpty(context))
		{
			Debug.unityLogger.LogError(TAG,"WriteFile Line Error : "+fileName + " or "+ context + " is invalid!");
			return false;
		}
		using(var fs = new FileStream(fileName,FileMode.OpenOrCreate))
		{
			lock(fs)
			{
				if(!fs.CanWrite)
				{
					Debug.unityLogger.LogError(TAG,"WriteFile Line Error : "+ fileName+" can not be writed");
					return false;
				}
				var sw = new StreamWriter(fs);
				sw.WriteLine(context);
				sw.Close();
				sw.Dispose();
			}
		}
		return true;
	}
	/// <summary>
	/// 向文件中写入一行字节
	/// </summary>
	/// <returns><c>true</c>写入成功<c>false</c>写入失败</returns>
	/// <param name="fileName">文件路径</param>
	/// <param name="context">字节流</param>
	public static bool WriteFileLine(this string fileName,byte[] pWriteByte)
	{
		if(string.IsNullOrEmpty(fileName)||null==pWriteByte)
		{
			Debug.unityLogger.LogError(TAG,"WriteFile Line Error : "+fileName + " or "+ pWriteByte + " is invalid!");
			return false;
		}
		string str = Encoding.Default.GetString (pWriteByte);
		return fileName.WriteFileLine (str);
	}
	/// <summary>
	/// 删除文件
	/// </summary>
	/// <returns><c>true</c>删除文件成功<c>false</c>删除文件失败</returns>
	/// <param name="fileName">文件路径</param>
	public static bool DeleteFile(this string fileName){
		if (!fileName.ExistsFile ())
			return false;
		try
		{
			File.Delete(fileName);
		}
		catch (IOException ioe)
		{
			Debug.unityLogger.LogError (TAG,"DeleteFile Error: "+ioe.Message);
		}
		return true;
	}

	#endregion
}
