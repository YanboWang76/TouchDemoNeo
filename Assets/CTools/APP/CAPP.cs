using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 单例类,管理APP的运行状态
/// </summary>
public class CAPP : CMonoSingleton<CAPP> {
	/// <summary>
	/// App OnGUI 更新GUI
	/// </summary>
	public Action OnAppGUI;
	/// <summary>
	/// App OnDestory 销毁
	/// </summary>
	public Action OnAppDestory;
	/// <summary>
	/// App OnApplicationFocus 应用是否在前端运行
	/// </summary>
	public Action<bool> OnAppApplicationFocus;
	/// <summary>
	/// App OnApplicationPause 应用是否暂停
	/// </summary>
	public Action<bool> OnAppApplicationPause;
	/// <summary>
	/// App OnApplicationQuit 退出应用
	/// </summary>
	public Action OnAppApplicationQuit;
	/// <summary>
	/// App OnUpdate 更新
	/// </summary>
	public Action OnAppUpdate;
	/// <summary>
	/// App OnlateUpdate 更新
	/// </summary>
	public Action OnAppLateUpdate;
	/// <summary>
	/// App OnFixedUpdate 更新
	/// </summary>
	public Action OnAppFixedUpdate;

	private CAPP()
	{
		#if !Debug
			Debug.unityLogger.logEnabled = false;
		#endif
	}

	protected override void CUpdate ()
	{
		if (null != OnAppUpdate)
			OnAppUpdate ();
	}
	protected override void CFixedUpdate ()
	{
		if (null != OnAppFixedUpdate)
			OnAppFixedUpdate ();
	}
	protected override void CLateUpdate ()
	{
		if (null != OnAppLateUpdate)
			OnAppLateUpdate ();
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		if (null != OnAppDestory)
			OnAppDestory ();
	}

	protected override void OnApplicationFocus (bool hasFocus)
	{
		base.OnApplicationFocus (hasFocus);
		if (null != OnAppApplicationFocus)
			OnAppApplicationFocus (hasFocus);
	}

	protected override void OnApplicationPause (bool pauseStatus)
	{
		base.OnApplicationPause (pauseStatus);
		if (null != OnAppApplicationPause)
			OnAppApplicationPause (pauseStatus);
	}
	protected override void OnApplicationQuit ()
	{
		base.OnApplicationQuit ();
		if (null != OnAppApplicationQuit)
			OnAppApplicationQuit ();
	}
	protected override void OnGUI ()
	{
		base.OnGUI ();
		if (null != OnAppGUI)
			OnAppGUI ();
	}
}
