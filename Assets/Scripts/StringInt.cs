using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringInt : EventArgs {

	public string stringarg;
	public int conditionnum{ get; private set;}

	public StringInt(string str, int num)
	{
		stringarg = str;
		conditionnum = num;
	}
}