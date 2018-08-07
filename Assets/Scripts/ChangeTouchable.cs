using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;

public class ChangeTouchable : MonoBehaviour
{
    public List<GameObject> SwitchList = new List<GameObject>();
    public event EventHandler<GenericEventArgs<int>> TouchableChanged;
    public GameObject CurrentTouchable { get { return SwitchList[_curIndex]; } }

    [SerializeField]
    private DummyHandMover _handMover;

    private int _curIndex;
	// Use this for initialization
	void Start () {
        _curIndex = 0;
	}

    //protected void OnChangeTouchable()
    //{
    //    GenericEventArgs<int> args = new GenericEventArgs<int>(_curIndex);
    //    Miscellaneous.InvokeEvent(TouchableChanged, this, args);
    //}

    // Update is called once per frame
    void Update()
    {
	    var switchKey = KeyCode.C;
        int n = SwitchList.Count;
        if (Input.GetKeyDown(switchKey) && n > 0)
        {
            SwitchList[_curIndex].SetActive(false);
            _curIndex = (_curIndex + 1) % n;
            SwitchList[_curIndex].SetActive(true);
            if (_curIndex == 6)
            {
                _handMover.FreeMove = true;
            }
            else
            {
                _handMover.FreeMove = false;
            }
            //OnChangeTouchable();
        }
	}
}
