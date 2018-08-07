using UnityEngine;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;

public class DummyHandMover : MonoBehaviour
{
    public bool FreeMove { get { return _freeMove; } set { _freeMove = value; } }

    [SerializeField]
    private Transform _handMovementDefault;
    [SerializeField]
    private List<Transform> _handMovementList;
    //[SerializeField]
   // private Transform _handRotationOnTouchingButtons; //A special transform made to let the hand fit the buttons scene

    private int _movement;
    private bool _freeMove = false;
    private bool _freeMoveLastTime = false;

    void Start()
    {
        if (Miscellaneous.CheckNullAndLogError(_handMovementDefault) ||
            Miscellaneous.CheckNullAndLogError(_handMovementList))
        {
            return;
        }
    }

    private void UpdatePosition()
    {
        if (_freeMoveLastTime && !_freeMove)
        {
            // Free move switch was just switched off
            transform.position = _handMovementDefault.position;
            transform.rotation = _handMovementDefault.rotation;
        }

        if (_freeMove)
        {
            int n = _handMovementList.Count;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _movement++;
                if (_movement > n - 1)
                {
                    _movement = n - 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _movement--;
                if (_movement < 0)
                {
                    _movement = 0;
                }
            }
            transform.position = _handMovementList[_movement].position;
            transform.rotation = _handMovementList[_movement].rotation;
        }
        _freeMoveLastTime = _freeMove;
    }

    void Update()
    {
        UpdatePosition();
    }

}