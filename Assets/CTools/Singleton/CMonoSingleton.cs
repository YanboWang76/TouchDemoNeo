using UnityEngine;

/// <summary>
/// 需要使用Unity生命周期的单例模式
/// </summary>
public abstract class CMonoSingleton<T> : CMonoBehaviour where T : CMonoSingleton<T>
{
	protected static T instance = null;

	public static T GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<T>();
			if (FindObjectsOfType<T>().Length > 1)
			{
				return instance;
			}
			if (instance == null)
			{
				string instanceName = typeof(T).Name;
				GameObject instanceGO = GameObject.Find(instanceName);
				if (instanceGO == null)
					instanceGO = new GameObject(instanceName);
				instance = instanceGO.AddComponent<T>();
				DontDestroyOnLoad(instanceGO);  //保证实例不会被释放
			}
			else
			{
			}
		}

		return instance;
	}


	protected virtual void OnDestroy()
	{
		instance = null;
	}
}
