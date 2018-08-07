using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	[RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
		Instantiate(Resources.Load("Manager"));
    }

	public static Manager Instance{get;private set;}

	private AudioManager _AudioManager;

	public AudioManager AudioManager{get{return _AudioManager;}}

	private GameSceneManager _GameSceneManager;
	public GameSceneManager GameSceneManager{get{return _GameSceneManager;}}

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		_AudioManager = transform.GetComponent<AudioManager>();
		_GameSceneManager = transform.GetComponent<GameSceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestory(){
		Instance = null;
	}
}
