using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Touchables;
using System;
using HackInDexmo.Scripts;

public class FollowingTouchable : TouchableStatic 
{
	public BypassController bypassController;
	public Transform root;
	public Transform reference;
	public float minFollowDistance = 0.01f;
    public float followTimeInterval = 2f;

    private bool enable = true;
	private bool isRight = true;
	private Transform followedPalmCenter = null;

	private Vector3 referencePositionRelativeToRoot;
	private Quaternion referenceRotationRelativeToRoot;
	// Use this for initialization
	void Start () {
		
		bypassController.ReleasePose += Release;
	}

	// Update is called once per frame
	void Update () {
	if (followedPalmCenter != null && enable)
			Follow ();
	}

	private void Follow()
	{
		Debug.Log ("Following");
		if (Vector3.Distance (reference.position, followedPalmCenter.position) > minFollowDistance) {
            referencePositionRelativeToRoot = root.position - reference.position;
            referenceRotationRelativeToRoot = Quaternion.Inverse(reference.rotation) * root.rotation;

            root.rotation = followedPalmCenter.rotation * referenceRotationRelativeToRoot;
			root.position = followedPalmCenter.position + referencePositionRelativeToRoot;
			Debug.Log ("Distace: " + Vector3.Distance (reference.position, followedPalmCenter.position));	
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("On TriggerEnter.");
		if (other.gameObject.layer != LayerMask.NameToLayer ("Hand") || followedPalmCenter != null || !enable)
			return;

	    followedPalmCenter = Utility.GetPalmCenterOfCollider (other);
		Debug.Log ("followedPalmCenter: " + followedPalmCenter);
	    if (followedPalmCenter == null)
			return;

	    if (followedPalmCenter.parent.parent.name.Contains ("Right"))
		    isRight = true;
	    else if (followedPalmCenter.parent.parent.name.Contains ("Left"))
		    isRight = false;
	    else
		    followedPalmCenter = null;
	}

	private void Release(object sender, ReleasePoseEventArgs e)
	{
        BypassController tempController = sender as BypassController;
		if (tempController != bypassController || e.isRight != isRight)
			return;

		followedPalmCenter = null;
        StartCoroutine(Reboot());
	}

    IEnumerator Reboot()
    {
        enable = false;
        yield return new WaitForSeconds(followTimeInterval);
        enable = true;
    }
}
