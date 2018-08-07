using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommonSnappable : MonoBehaviour {

	public event EventHandler SnappableEnter;
	public event EventHandler SnappableExit;

	public string address;
	public Transform target;

	[HideInInspector]
	public bool enable = true;
	[HideInInspector]
	public bool isSnapping = false;
	[HideInInspector]
	public bool isSnapped = false;
	[HideInInspector]
	public CommonSnapper snapper = null;

	//void OnTriggerEnter()
	//{
		
	//}

	//void OnTriggerExit()
	//{
		
	//}

}