using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Libdexmo.Unity.Core.Utility;

[RequireComponent(typeof(Collider))]
public class CommonSnapper : MonoBehaviour {

	/// <summary>
	/// The root transform for the whole object。Parent is the real obj, gameObject is used for scripts and collider
	/// </summary>
	public Transform root;

	/// <summary>
	/// The reference transform of snapper.
	/// Note: the reference must not be a child object of gameobject
	/// </summary>
	public Transform reference;

	/// <summary>
	/// The address.Only objects with the same address will be snapped.
	/// </summary>
	public string address;

	/// <summary>
	/// The min distance while snapping.If the distance is less than it, the reference transform
	/// will be the target right away.
	/// </summary>
	public float snappingDistanceMin = 0.01f;

	/// <summary>
	/// snaptimeinterval(seconds).After Snapping end, the snapper is disabled for snaptimeinterval.
	/// </summary>
	public float snapTimeInterval;

	[HideInInspector]
	public bool isSnapped = false;
	[HideInInspector]
	public bool isSnapping = false;
	[HideInInspector]
	private bool enable = true;

	private Transform target;
	private CommonSnappable snappable;

	private Vector3 snapperLocalPosition;
	private Quaternion snapperLocalRotation;

	private Vector3 referencePositionRelativeToRoot;
	private Quaternion referenceRotationRelativeToRoot;

	void Start () {
		snapperLocalPosition = transform.position - root.position;
		snapperLocalRotation = Quaternion.Inverse (root.rotation) * transform.rotation;

		//referencePositionRelativeToRoot = reference.InverseTransformPoint(root.position);
		referencePositionRelativeToRoot = root.position - reference.position;
		referenceRotationRelativeToRoot = Quaternion.Inverse(reference.rotation) * root.rotation;
    }

	void Update()
	{
		//Debug.Log (root.gameObject + " Root:" + root.position.ToString("f4"));
		//Debug.Log (root.gameObject + " Ref:" + reference.position.ToString("f4"));
		//if (snappable != null) {
		//	Debug.Log (root.gameObject + " Target:" + target.position.ToString("f4"));	
		//}
	}

	void OnTriggerEnter(Collider snappablecoll)
	{
		//Debug.Log(root.gameObject + "CommonSnapper OnTriggerEnter");
		Debug.Log("CommonSnapper OnTriggerEnter");
        if (snappable == null) 
		{
			if (snappablecoll.tag == "CommonSnappable") {
				snappable = snappablecoll.gameObject.GetComponent<CommonSnappable> ();
				if (snappable == null)
					return;
				if (snappable.enable == false || address != snappable.address) 
				{
					snappable = null;
					return;
				}
					
				target = snappable.target;
				isSnapping = true;
				//Debug.Log (snappable.gameObject.transform.parent);
				snappable.isSnapping = true;
				snappable.snapper = this;
			} else
				return;
        }
	}

	//Run with FixedUpdate, use Time.FixedDeltaTime
	void OnTriggerStay(Collider snappablecoll)
	{
		Debug.Log (root.gameObject + "CommonSnapper OnTriggerStay");
		if (snappable == null)
		{
			if (snappablecoll.tag == "CommonSnappable") {
				snappable = snappablecoll.gameObject.GetComponent<CommonSnappable> ();
				if (snappable == null)
					return;
				if (snappable.enable == false || address != snappable.address) 
				{
					snappable = null;
					return;
				}

				target = snappable.target;
				isSnapping = true;
				snappable.isSnapping = true;
				snappable.snapper = this;
			} else
				return;
		}

		if (enable == false)
			return;

		//Debug.Log ("isSnapped: " + isSnapped);
		//Debug.Log ("isSnapping: " + isSnapping);

		if (isSnapping && !isSnapped)
		{
			if (target == null)
				return;

			root.rotation = target.rotation * referenceRotationRelativeToRoot;
			root.position = target.position + referencePositionRelativeToRoot;
			Debug.Log ("Distace: " + Vector3.Distance (reference.position, target.position));
			if (Vector3.Distance (reference.position, target.position) < snappingDistanceMin) 
			{
				Debug.Log ("Distace: " + Vector3.Distance (reference.position, target.position));
				isSnapping = false;
				snappable.isSnapping = false;
				isSnapped = true;
				snappable.isSnapped = true;
			} 

			//Debug.Log ("equal rot: " + (reference.rotation == target.rotation));
			//Debug.Log ("equal pos: " + (reference.position == target.position));
          }
				
		//The parent stay but the reference move with collider
		if (!isSnapping && isSnapped) 
		{
			//Debug.Log (root.gameObject + "CommonSnapper Enter Keeping mode");
			Debug.Log ("CommonSnapper Enter Keeping mode");
			Vector3 temppos = transform.position;
			Quaternion temprot = transform.rotation;

			if (Vector3.Distance (reference.position, target.position) > snappingDistanceMin) 
			{
			   root.rotation = target.rotation * referenceRotationRelativeToRoot;
			   root.position = target.position + referencePositionRelativeToRoot;
				Debug.Log ("Move to keep");
			}
			transform.position = temppos;
			transform.rotation = temprot;
		}
	}

	void OnTriggerExit(Collider snappablecoll)
	{			
		if (snappable == null) {
			return;
		}
		else {
			CommonSnappable temp = snappablecoll.GetComponent<CommonSnappable> ();
			if (temp == snappable) {
				//Debug.Log ("snappable.gameObject: " + snappable.gameObject);
				//Debug.Log ("snappablecoll.gameObject: " + snappablecoll.gameObject);
				//Debug.Log (root.gameObject + "CommonSnapper OnTriggerExit");
				Debug.Log ("CommonSnapper OnTriggerExit");
				enable = false;
				isSnapped = false;
				isSnapping = false;
				if (snappable != null) 
				{
					snappable.isSnapped = false;
					snappable.isSnapping = false;
					snappable.snapper = null;
					snappable = null;
				}
				transform.rotation = root.rotation * snapperLocalRotation;
				transform.position = root.position + snapperLocalPosition;
				StartCoroutine (Reable (snapTimeInterval));
			} else {
				return;
			}
		}
	}

	IEnumerator Reable(float time)
	{
		yield return new WaitForSeconds (time);
		enable = true;
	}

}