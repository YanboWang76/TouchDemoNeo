using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.PyroParticles;

public class FireExtinguish : MonoBehaviour {

	public event EventHandler<StringInt> extinguishsucceed;

	public float range;

	private bool gethit = false;
	private Ray firefindray;
	private RaycastHit hitinfo;
	private LayerMask layermask = 1 << 14;
		
	// Update is called once per frame
	void Update () {
		firefindray = new Ray (transform.position, transform.forward);
		gethit = Physics.Raycast (firefindray,out hitinfo, range, layermask);
		if (gethit) 
		{
			FireConstantBaseScript script = hitinfo.collider.gameObject.GetComponentInParent<FireConstantBaseScript> ();
			script.Duration = 0;
			extinguishsucceed (this, new StringInt ("Put out the fire!", 1));
			enabled = false;
		}
	}
}
