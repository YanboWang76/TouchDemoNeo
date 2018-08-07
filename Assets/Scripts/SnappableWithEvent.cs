using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SnappableWithEvent : CommonSnappable {

    public event EventHandler SlotIn;

	private bool isSnappedLastTime = false;
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (transform.parent.gameObject + "before: " + (!isSnappedLastTime && isSnapped));
		if (!isSnappedLastTime && isSnapped)
        {
			Debug.Log (snapper.name);
			SlotIn(this, new EventArgs());
			Debug.Log (gameObject + ": SlotIn");
        }
		isSnappedLastTime = isSnapped;
		//Debug.Log (transform.parent.gameObject + "after: " + (!isSnappedLastTime && isSnapped));
		//Debug.Log(transform.parent.gameObject + ": " + (snapper==null));
		//Debug.Log(transform.parent.gameObject + ": " + snapper.gameObject.transform.parent.gameObject);
		//Debug.Log (gameObject + ": isSnappedLastTime: " + isSnappedLastTime);
		//Debug.Log (gameObject + ": isSnapped: " + isSnapped);
	}
}
