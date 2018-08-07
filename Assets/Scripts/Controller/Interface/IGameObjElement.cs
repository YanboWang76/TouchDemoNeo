using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IGameObjElement
{
	GameState.GameLogicState LogicState{get;}
	void GameLogicSuccess();
	void GameLogicFail();
}
