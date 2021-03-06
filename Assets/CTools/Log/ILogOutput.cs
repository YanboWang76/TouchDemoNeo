﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILogOutput  
{
	/// <summary>
	/// 输出日志数据
	/// </summary>
	/// <param name="logData">日志数据</param>
	void Log(CLog.LogData logData);
	/// <summary>
	/// 关闭
	/// </summary>
	void Close();
}
