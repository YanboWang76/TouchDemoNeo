using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libdexmo.Unity.Core.Utility;

public class UpperLeverController : MonoBehaviour {

	public Transform controlledObj;
	public float AngleLimit = 30f;
	public float angleDiff = 0.01f;
	public int handLayerNum = 12;

	private enum State
	{
		Idle,
		Pressing,
		Releasing,
	}
	private State state = State.Idle;
	private bool isTouched {get {return !(_colliderHashSet.Count == 0); }}
	private Vector3 OriginAngle;

	private Collider[] _selfCollider;
	private HashSet<Collider> _colliderHashSet;

	private float angleX = 0;
	private float maxPressingAngleX;
	private float minReleasingAngleX;
	private float curAngleX {get {return CurAngleX ();}}
	private float CurAngleX()
	{
		float temp = controlledObj.localEulerAngles.x - OriginAngle.x;
		return temp >= 0 ? temp : temp + 360;
	}
	// Use this for initialization
	void Start () {
		OriginAngle = controlledObj.localEulerAngles;

		maxPressingAngleX = 0;
		minReleasingAngleX = AngleLimit;

		_selfCollider = GetComponents<Collider> ();
		_colliderHashSet = new HashSet<Collider> ();
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log ("UpperLeverTriggerEnter");
		if (other.gameObject.layer != handLayerNum)
			return;

		_colliderHashSet.Add (other);
	}

	void OnTriggerExit(Collider other)
	{
		//Debug.Log ("UpperLeverTriggerExit");
		if (other.gameObject.layer != handLayerNum)
			return;

		if (_colliderHashSet.Contains (other)) {
			_colliderHashSet.Remove (other);
		}
	}

	void Update()
	{
		Debug.Log ("State: " + state);
		Debug.Log ("IsTouched: " + isTouched);
		UpdateAngleX ();
		StateTransition ();
		ModifyAngleX (angleDiff);
		//Debug.Log ("AngleX: " + angleX);
		//Debug.Log ("CurAndleX: " + curAngleX);
	}

	private void UpdateAngleX()
	{
		if (!(state == State.Pressing)) 
		{
			if (state == State.Releasing) 
			{
				angleX = 0;
			}
			return;
		}
		angleX = FindEulerAngleXThatJustTouchHand ();
		angleX = Mathf.Clamp (angleX, 0, AngleLimit);
	}

	private float FindEulerAngleXThatJustTouchHand()
	{
		float angleX = AngleLimit;
		float angleStep = AngleLimit / 10;
		float upper = angleX;
		float lower = angleX;
		controlledObj.localEulerAngles = (OriginAngle + new Vector3(angleX, 0, 0)) ;

		while (angleX > 0 && !CheckOverlapWithHand())
		{
			upper = angleX;
			angleX -= angleStep;
			controlledObj.localEulerAngles = (OriginAngle + new Vector3(angleX, 0, 0)) ;
			lower = angleX;
		}

		angleX = CheckOverlapWithHandBinarySearch(upper, lower);
		Debug.Log ("AngleX: " + angleX);
		return angleX;
	}

	private float CheckOverlapWithHandBinarySearch(float upper, float lower,
		float tol = 0.001f, int maxIter = 1000)
	{
		int iter = 0;
		float error = Mathf.Abs(upper - lower);

		while (iter < maxIter && error > tol)
		{
			angleX = (upper + lower) / 2;
			controlledObj.localEulerAngles = (OriginAngle + new Vector3(angleX, 0, 0)) ;
			if (CheckOverlapWithHand())
			{
				lower = angleX;
			}
			else
			{
				upper = angleX;
			}
			//Debug.Log ("Upper: " + upper + " Lower: " + lower);
			error = Mathf.Abs(upper - lower);
			iter++;
		}
		return lower;
	}

	private bool CheckOverlapWithHand()
	{
		bool collideWithHand = false;
		Collider[] overlappedColliders;
		HashSet<Collider> others = new HashSet<Collider> ();
		foreach (Collider collider in _selfCollider) 
		{
			overlappedColliders = CheckOverlap (collider);

			int n = overlappedColliders.Length;
			for (int i = 0; i < n; i++)
			{
				others.Add(overlappedColliders[i]);
			}
		}
		foreach (Collider collider in others)
		{
			if (collider.gameObject.layer == handLayerNum) {
				collideWithHand = true;
				break;
			}
		}	

		return collideWithHand;
	}
		
	private Collider[] CheckOverlap(Collider collider)
	{
		if (collider is BoxCollider) 
		{
			BoxCollider boxCollider = collider as BoxCollider;
			Transform attachedTransform = boxCollider.transform;
			Vector3 center = attachedTransform.TransformPoint (boxCollider.center);
			Vector3 extents = Miscellaneous.Vector3Abs (
				                  Vector3.Scale (boxCollider.size / 2, attachedTransform.lossyScale));
			return Physics.OverlapBox (center, extents, attachedTransform.rotation);
		} 
		else if (collider is SphereCollider) 
		{
			SphereCollider sphereCollider = collider as SphereCollider;
			Transform attachedTransform = collider.transform;
			Vector3 center = attachedTransform.TransformPoint (sphereCollider.center);
			float radius = Mathf.Abs (sphereCollider.radius) *
			               Miscellaneous.Vector3MaxComponentAbs (attachedTransform.lossyScale);
			return Physics.OverlapSphere (center, radius);
		} 
		else 
		{
			return new Collider[0];
		}
	}

	private void StateTransition()
	{
		switch (state) 
		{
		case State.Idle:
			if (curAngleX > 0.02 * AngleLimit || isTouched) 
			{
				state = State.Pressing;
			}
			maxPressingAngleX = 0;
			break;

		case State.Pressing:
			if (curAngleX <= maxPressingAngleX - 0.01f * AngleLimit || !isTouched) 
			{
				state = State.Releasing;
				maxPressingAngleX = 0;
			} else 
			{
				maxPressingAngleX = Mathf.Max (maxPressingAngleX, curAngleX);
			}
			break;

		case State.Releasing:
			if(curAngleX < 0.02f * AngleLimit)
			{
				state = State.Idle;
				minReleasingAngleX = AngleLimit;
			}
			else if(curAngleX >= minReleasingAngleX + 0.01 * AngleLimit || isTouched)
			{
				state=State.Pressing;
				minReleasingAngleX = AngleLimit;
			}
			else{
				minReleasingAngleX = Mathf.Min(minReleasingAngleX, curAngleX);
			}
			break;

		}
	}

	private void ModifyAngleX(float diffMax)
	{		
		float diff = Mathf.Abs (curAngleX - angleX);
		if (diff > diffMax) 
		{
			float lerpAngle = Mathf.Lerp (curAngleX, angleX, 5 * Time.deltaTime);
			controlledObj.localEulerAngles = (OriginAngle + new Vector3 (lerpAngle, 0, 0));
		} 
		else 
		{
			controlledObj.localEulerAngles = (OriginAngle + new Vector3 (angleX, 0, 0));
		}

	}                               
}