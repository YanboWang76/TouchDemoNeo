using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSmokeController : MonoBehaviour {

	[SerializeField]
	private ParticleSystem whiteSmoke;
	public float maxParticleNum = 500;
	public float turningParticleNum = 8;
	public float maxAngle = 20;
	public float turningAngle = 2;

	private Vector3 originEulars;
	private float curAngle{get {return CurAngle(); }}
	private float CurAngle()
	{
		float temp = transform.localEulerAngles.x - originEulars.x;
		return temp >= 0 ? temp : temp + 360;
	}
	private float curParticleNum = 0;
	// Use this for initialization
	void Start () {
		originEulars = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (curAngle <= turningAngle) 
		{
			curParticleNum = curAngle / turningAngle * turningParticleNum;
		}

		else if (curAngle <= maxAngle) 
		{
			float diffAngle = maxAngle - turningAngle;
			float coefA = (maxParticleNum - turningParticleNum) / (Mathf.Exp(diffAngle) - 1);
			float coefB = maxParticleNum + coefA;
			curParticleNum = coefB - coefA * Mathf.Exp (maxAngle - curAngle);
		}

		else 
		{
			curParticleNum = maxParticleNum;
		}

		ParticleSystem.EmissionModule emission = whiteSmoke.emission;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(curParticleNum);
		if (whiteSmoke.isStopped) {
			whiteSmoke.Play ();
		}

		if (curParticleNum == 0) 
		{
			whiteSmoke.Stop ();
		}
	}
		
}
