using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Libdexmo.Unity.Touchables;

public class ShellController : MonoBehaviour {

	public TouchableStatic touchableStatic;
	public List<ScrewCrossController> screwlist;

	private int screwnum = 0;
	// Use this for initialization
	void Start () {
		foreach (ScrewCrossController scr in screwlist) 
		{
			screwnum++;
			scr.loosen += ScrewLoosen;
		}
	}

	void ScrewLoosen(object sender, EventArgs e)
	{
		screwnum--;
		if (screwnum == 0) 
		{
			Destroy (touchableStatic);
		}
	}
}
