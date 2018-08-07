using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuggageController : MonoBehaviour,IGameObjElement{

	[SerializeField]
	private GameState.GameLogicState _LogicState;
    public GameState.GameLogicState LogicState
    {
        get
        {
			return _LogicState;
        }
    }

    public void GameLogicFail()
    {
		//
    }

    public void GameLogicSuccess()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
