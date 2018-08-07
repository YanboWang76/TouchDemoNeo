using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCaseSender : MonoBehaviour {

	public event EventHandler<StringInt> takeinbomb;

	public Transform bomb;
	[SerializeField]
	private ToolCaseStatusManager toolCaseStatusManager;

	private ComponentsManager bombManager;

	private BoxCollider boxCollider;
	private Bounds boxBounds;

	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider> ();
		boxBounds = boxCollider.bounds;

		bombManager = bomb.GetComponent<ComponentsManager> ();
		toolCaseStatusManager.CaseClose += SendMessageIfTakeIn;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SendMessageIfTakeIn(object sender, EventArgs e)
	{
		if (bombManager.putOutFire && boxBounds.Contains(bomb.position))
			takeinbomb (this, new StringInt ("Take in bomb!", 1));
	}
}
