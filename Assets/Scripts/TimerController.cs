using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

	public event EventHandler timeOver;

	/// <summary>
	/// The counting time(Seconds)
	/// </summary>
	[SerializeField]
	private float _time = 180;
	private TextMesh textMesh;
	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMesh> ();
		textMesh.text = "03:00";
		//StartCoroutine (CountDown ());
	}
		
	public void AddTime(float deltaTime)
	{
		_time += deltaTime;
		textMesh.text = string.Format("{0:D2}:{1:D2}", ((int)_time) / 60,  ((int)_time) % 60);
	}

	void Update()
	{
		_time -= Time.deltaTime;
		///Debug.Log ("Time: " + _time);
		if (_time <= 0) 
		{
			textMesh.text = string.Format("{0:D2}:{1:D2}", 0, 0);
			timeOver (this, new EventArgs ());
			enabled = false;
		}
		textMesh.text = string.Format("{0:D2}:{1:D2}", ((int)_time) / 60,  ((int)_time) % 60);
	}
}