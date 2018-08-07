using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjElement : MonoBehaviour,IGameObjElement {
    public GameState.GameLogicState LogicState
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    public void GameLogicFail()
    {
        throw new System.NotImplementedException();
    }

    public void GameLogicSuccess()
    {
        throw new System.NotImplementedException();
    }


}
