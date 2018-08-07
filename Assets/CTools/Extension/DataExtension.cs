using CTool.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataExtension  {

	/// <summary>
	/// 增序遍历List
	/// </summary>
	public static void ForEach<T>(this List<T> list,Action<int> action){
		if (list.Count <= 0)
			return;
		MForEach (0, list.Count-1, 1, action);
	}
	/// <summary>
	/// 增序遍历数组
	/// </summary>
	public static void ForEach<T>(this T[] list,Action<int> action){
		if (list.Length <= 0)
			return;
		MForEach (0, list.Length-1, 1, action);
	}

	/// <summary>
	/// 根据fisrtIndex以及lastIndex遍历list
	/// </summary>
	public static void ForEach<T>(this T[] list,int firstIndex,int lastIndex,Action<int> action,int stepLength =1){
		if (list.Length <= 0)
			return;
		firstIndex = firstIndex.GetClampRange (0, list.Length-1);
		lastIndex = lastIndex.GetClampRange (0, list.Length-1);
		MForEach (firstIndex, lastIndex, stepLength, action);
	}

	/// <summary>
	/// 根据fisrtIndex以及lastIndex遍历数组
	/// </summary>
	public static void ForEach<T>(this List<T> list,int firstIndex,int lastIndex,Action<int> action,int stepLength =1){
		if (list.Count <= 0)
			return;
		firstIndex = firstIndex.GetClampRange (0, list.Count-1);
		lastIndex = lastIndex.GetClampRange (0, list.Count-1);
		MForEach (firstIndex, lastIndex, stepLength, action);
	}

	private static void MForEach(int firstIndex,int lastIndex,int stepLength,Action<int> action){
		if (firstIndex > lastIndex) {
			for (int i = firstIndex; i >= lastIndex; i -= stepLength) {
				action (i);
			}
		} else {
			for (int i = firstIndex; i <= lastIndex; i += stepLength) {
				action (i);
			}
		}
	}


}
