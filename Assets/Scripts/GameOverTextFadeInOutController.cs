using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameOverTextFadeInOutController : MonoBehaviour {
    
	[SerializeField]
	private GameController gameController;
    // Use this for initialization
    void Start () {        
		gameController.GameOverTextFadeIn += TextFadeIn;
        gameObject.GetComponent<TextMesh>().color = Color.white;
		gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () 
	{
        gameObject.GetComponent<TextMesh>().color = Color.Lerp(Color.clear, Color.red, Mathf.PingPong(Time.time, 2));
    }

    public void TextFadeIn(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
