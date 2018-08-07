using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CTool.Timer
{
	public class CTimer :MonoBehaviour
	{
		private CTimer(){}

		private bool beginTimer = false;
		private float maxTime = 0;
		private float firstTime = 0;
		private event Action fulFillTimer = null;

		public float CurTime{ get; private set;}
		internal static CTimer Init(GameObject go)
		{
			CTimer timer = go.GetComponent<CTimer> ();
			if(timer == null)
				timer = go.AddComponent<CTimer> ();
			return timer;
		}
		internal void StartTimer(float _MaxTime,Action _FulFillTimer){
			maxTime = _MaxTime;
			beginTimer = true;
			fulFillTimer += _FulFillTimer;
		}
		internal void CancelTimer(){
			beginTimer = false;
			fulFillTimer = null;
		}
		
		void Update()
		{
			if (beginTimer) 
			{
				CurTime = Time.time - firstTime;
				if (CurTime > maxTime) {
					beginTimer = false;
					if (fulFillTimer != null)
						fulFillTimer ();
				}
			}	
		}
	}
}