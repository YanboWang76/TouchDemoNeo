using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonoBehaviour : MonoBehaviour
{
	protected virtual void OnApplicationFocus (bool hasFocus)
	{
	}

	protected virtual void OnApplicationPause (bool pauseStatus)
	{
	}

	protected virtual void OnApplicationQuit ()
	{
	}

	protected virtual void OnBecameInvisible ()
	{
	}

	protected virtual void OnBecameVisible ()
	{
	}

	protected virtual void OnCollisionEnter (Collision collisiion)
	{
	}

	protected virtual void OnCollisionEnter2D (Collision2D coll)
	{
	}

	protected virtual void OnCollisionExit (Collision collisionInfo)
	{
	}

	protected virtual void OnCollisionExit2D (Collision2D coll)
	{
	}

	protected virtual void OnCollisionStay (Collision collisionInfo)
	{
	}

	protected virtual void OnCollisionStay2D (Collision2D coll)
	{
	}

	protected virtual  void OnConnectedToServer ()
	{
	}

	protected virtual void OnDisconnectedFromServer (NetworkDisconnection info)
	{
	
	}

	protected virtual void OnPreRender ()
	{
	}

	protected virtual void OnPostRender ()
	{
	}

	protected virtual void OnPreCull ()
	{
	}


	protected virtual void OnRenderImage (RenderTexture src, RenderTexture dest)
	{
	}

	protected virtual void OnRenderObject ()
	{
	}

	protected virtual void OnTransformChildrenChanged ()
	{
	}

	protected virtual void OnTransformParentChanged ()
	{
	}

	protected virtual void OnWillRenderObject ()
	{
	}
	public static bool OpenUpdate = true;

	protected virtual void CUpdate (){}
	protected virtual void CLateUpdate (){}
	protected virtual void CFixedUpdate (){}

	protected virtual void OnGUI(){
		
	}
	void Update(){
		if (OpenUpdate) {
			CUpdate ();
		}
	}
	void LateUpdate()
	{
		if (OpenUpdate) {
			CLateUpdate ();
		}
	}
	void FixedUpdate()
	{
		if (OpenUpdate) {
			CFixedUpdate();
		}
	}
}
