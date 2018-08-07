using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Libdexmo.Unity.Touchables.Pickables;

public class ScrewCrossController : MonoBehaviour {

	public event EventHandler loosen;

	public Transform screwcross;
	public enum RotateDirection
	{
		Righthandedhelix = 1,
		Lefthandedhelix = -1
	};
	//Suggest: Let the RotateDir be LeftHanded and
	//the localforward be the direction which the ScrewCross enters in
	//Because Unity use Lefthand
	public RotateDirection rotatedirection = RotateDirection.Lefthandedhelix;
	public float threadnum = 7;
	public float threadlength = 0;
	public Transform screwdriverknob;
	public Vector3 knoblocalrotateaxis = new Vector3(0,0,1);

	private bool istightened = true;
	private float rotateangle = 0;
	private float movedistance = 0;
	private Vector3 originposition;

	private Quaternion? lastknobrotation;
	private Quaternion currentknobrotation;
	private Quaternion deltaknobrotation;
	private Vector3 deltaaxis;
	private float deltaangle;
	private CommonSnappable screwcrosssnappable = null;

	// Use this for initialization
	void Start () {
		screwcrosssnappable = screwcross.gameObject.GetComponentInChildren<CommonSnappable> ();
		Debug.Log ("The Snappable Scripts " + ((screwcrosssnappable == null) ? "is not found" : "is found"));

		originposition = screwcross.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!istightened) 
		{
			Debug.Log ("Not tightened screwcross");
			return;
		}

		if (screwcrosssnappable == null) 
		{
			Debug.Log ("No Snappable");
			return;
		}

		if (screwcrosssnappable.isSnapped == false || screwcrosssnappable.enable == false) 
		{
			Debug.Log ("Not Snapped or not enable");
			return;
		}

		if (lastknobrotation == null) {
			lastknobrotation = screwdriverknob.localRotation;
		}

		//Debug.Log (screwcrosssnappable.ToString());
		//Debug.Log (screwdriverknob.ToString());

		currentknobrotation = screwdriverknob.localRotation;
		deltaknobrotation = Quaternion.Inverse (lastknobrotation ?? currentknobrotation) * currentknobrotation;
		deltaknobrotation.ToAngleAxis (out deltaangle, out deltaaxis);
		if (deltaangle > 180) 
		{
			deltaaxis = -deltaaxis;
			deltaangle = 360 - deltaangle;
		}
		deltaangle *= Mathf.Sign (Vector3.Dot (deltaaxis, knoblocalrotateaxis)) * (int)rotatedirection;

		lastknobrotation = currentknobrotation;
		//Debug.Log ("KnobAngle: " + knobangle);

		rotateangle += deltaangle;
		rotateangle = Mathf.Clamp (rotateangle, 0, threadnum * 360);
		//Debug.Log ("RotateAngle: " + rotateangle);
		movedistance = threadlength * rotateangle / 360;

		screwcross.position = originposition + movedistance * screwcross.forward;
		screwcross.localEulerAngles = new Vector3 (screwcross.localEulerAngles.x, 
			screwcross.localEulerAngles.y, -rotateangle);
		//-rotateangle makes it rotate anti-clockwise

		//Debug.Log ("The local angles' target: " + new Vector3 (screwcross.localEulerAngles.x, 
		//	screwcross.localEulerAngles.y, rotateangle * (int)rotatedirection));

		//float test = screwdriverknob.localEulerAngles.z;
		//Debug.Log ("Test: " + test);
		//Debug.Log ("no trans: " + screwdriverknob.localEulerAngles.z);
			
		//Debug.Log ("The current local angles: " + screwcross.localEulerAngles);

		if (rotateangle == threadnum * 360) 
		{

			istightened = false;
			if (loosen != null) 
			{
				loosen (this, new EventArgs ());
			}
			Rigidbody temp = gameObject.transform.parent.GetComponent<Rigidbody> ();
			temp.useGravity = true;
			temp.isKinematic = false;
			screwcrosssnappable.enable = false;
			this.enabled = false;
		}
	}
}