using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	/// <summary>
	/// The fadein switch.
	/// </summary>
	public bool fadein = true;

	public event EventHandler GameOverTextFadeIn;

	[SerializeField]
	private FireExtinguish fireExtinguish;
	[SerializeField]
	private ComponentsManager componentsManager;
	[SerializeField]
	private ToolCaseSender toolCaseSender;

	private int unfinishedsuccessconditions = 2;

	private bool gameover = false;
	private Image image = null;

	void Awake()
	{
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("game controller initialized");
		if (fadein) 
		{
			FadeIn ();
		}

		fireExtinguish.extinguishsucceed += ConditionTest;
		componentsManager.BoomAndOver += ConditionTest;
		toolCaseSender.takeinbomb += ConditionTest;
	}

	// Update is called once per frame
	void Update () 
	{
	//	if (gameover) 
	//	{
		//	if (Input.GetKeyDown (KeyCode.R)) {
		//		Manager.Instance.GameSceneManager.LoadEntryScene();
	//		}
	}

	private void FadeIn()
	{
		SteamVR_Fade.Start (Color.black, 0);
		SteamVR_Fade.Start (Color.clear, 3f);
	}

	private void FadeOut()
	{
		GameOverTextFadeIn (this, new EventArgs ());
		SteamVR_Fade.Start (Color.black, 3f);
		StartCoroutine (WaitAndSwitchScene (5f));
	}

	IEnumerator WaitAndSwitchScene(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		Manager.Instance.GameSceneManager.LoadEntryScene ();
	}

	private void ConditionTest(object sender, StringInt str)
	{
		//Debug.Log ("run condition test");
		if (str.conditionnum < -50) 
		{
			GameOver ();
			return;
		}
			
		unfinishedsuccessconditions -= str.conditionnum;
		if (unfinishedsuccessconditions == 0)
			Success ();
	}

	private void GameOver()
	{
	//invaild all the events
		//give information and start to wait for restart
		gameover = true;
		Debug.Log ("GameOver!");
		FadeOut ();
	}

	private void Success()
	{
		Debug.Log ("Success!");
	}
}
//Game Over
//Press "R" to restart
//Use the prefabs in the toy demo