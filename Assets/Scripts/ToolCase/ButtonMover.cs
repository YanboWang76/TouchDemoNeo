using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Triggering;

public class ButtonMover : MonoBehaviour, IBinarySearch
{
    public event EventHandler ButtonPressed; 
    public Transform ButtonOriginalPositionReference
    { get { return _buttonOriginalPositionReference;} }
    public Transform ButtonPressedLimit
    { get { return _buttonPressedLimit; } }

    [SerializeField]
    private Transform _buttonPressedLimit;
    [SerializeField]
    private Transform _buttonOriginalPositionReference;
    [SerializeField]
    private Transform _buttonMoverTriggerTransform;

    private float _buttonPressedLimitTolerance = 0.001f;
    private ButtonStatus _status;
    private ICollisionTriggeredBody _buttonMoverTrigger;
    private ICollisionTriggeringBody _triggeringBody;
    private Dictionary<Collider, OverlapColliderType> _childCollidersToTypeDict;
    private HashSet<Collider> _overlappedColliders;
    private float _curPositionNormalized;
    private Vector3 _movingVector;

    void Awake()
    {
        _status = ButtonStatus.Idle;
        _buttonMoverTrigger = 
            _buttonMoverTriggerTransform.GetComponent<ICollisionTriggeredBody>();
        if (Miscellaneous.CheckNullAndLogError(_buttonMoverTrigger))
        {
            return;
        }
        _buttonMoverTrigger.TriggerStart += OnButtonMoveStart;
        _buttonMoverTrigger.TriggerEnd += OnButtonMoveStop;
        _childCollidersToTypeDict = ColliderUtility.GenerateChildCollidersToTypeDict(transform);
        _overlappedColliders = new HashSet<Collider>();
        _curPositionNormalized = 0f;
    }

    public void Release()
    {
        transform.position = _buttonOriginalPositionReference.position;
        _status = ButtonStatus.Idle;
    }

    private void OnButtonMoveStart(object sender, CollisionTriggerActionEventArgs args)
    {
        if (_triggeringBody != null)
        {
            return;
        }
        //Debug.Log("OnButtonMoveStart.");
        if (_status == ButtonStatus.Idle)
        {
            _status = ButtonStatus.BeingPressed;
            _triggeringBody = args.CollisionTriggeringBody;
        }
    }

    private void OnButtonMoveStop(object sender, CollisionTriggerActionEventArgs args)
    {
        ICollisionTriggeringBody triggeringBody = args.CollisionTriggeringBody;
        if (_triggeringBody != triggeringBody)
        {
            return;
        }
        //Debug.Log("OnButtonMoveStop.");
        if (_status == ButtonStatus.BeingPressed)
        {
            Release();
        }
        _triggeringBody = null;
    }

    private void UpdateOverlappedColliders()
    {
        _overlappedColliders.Clear();
        foreach (var pair in _childCollidersToTypeDict)
        {
            Collider childCollider = pair.Key;
            OverlapColliderType colliderType = pair.Value;
            Collider[] otherColliders = new Collider[0];
            switch (colliderType)
            {
                case OverlapColliderType.BoxCollider:
                    otherColliders = ColliderUtility.CheckOverlapBox((BoxCollider)childCollider);
                    break;

                case OverlapColliderType.SphereCollider:
                    otherColliders = ColliderUtility.CheckOverlapSphere((SphereCollider)childCollider);
                    break;

                default:
                    break;
            }
            int n = otherColliders.Length;
            for (int i = 0; i < n; i++)
            {
                Collider otherCollider = otherColliders[i];
                _overlappedColliders.Add(otherCollider);
            }
        }
    }

    private bool CheckOverlappingWithTriggeringBodyWorker()
    {
        if (_triggeringBody == null)
        {
            throw new InvalidOperationException("_triggeringBody is null.");
        }
        bool correctTriggeringBodyFound = false;
        foreach (Collider overlappedCollider in _overlappedColliders)
        {
            ICollisionTriggeringBody overlappedTriggeringBody =
                overlappedCollider.GetComponentInParent<ICollisionTriggeringBody>();
            if (overlappedTriggeringBody == _triggeringBody)
            {
                correctTriggeringBodyFound = true;
                break;
            }
        }
        return correctTriggeringBodyFound;
    }

    private bool CheckOverlappingWithTriggeringBody()
    {
        UpdateOverlappedColliders();
        return CheckOverlappingWithTriggeringBodyWorker();
    }

    private Vector3 UpdateMovingVector()
    {
        Vector3 buttonMovingVector =
            (_buttonPressedLimit.position - _buttonOriginalPositionReference.position);
        return buttonMovingVector;
    }

    private float GetCurrentButtonPositionNormalized(Vector3 movingVector)
    {
        Vector3 buttonMovingDirection = movingVector.normalized;
        float buttonMovingDistanceMax = movingVector.magnitude;
        float positionNormalized = Vector3.Dot(
            transform.position - _buttonOriginalPositionReference.position,
            buttonMovingDirection) / buttonMovingDistanceMax;
        positionNormalized = Mathf.Clamp01(positionNormalized);
        return positionNormalized;
    }

    private void Move(Vector3 movingVector, float positionNormalized)
    {
        positionNormalized = Mathf.Clamp01(positionNormalized);
        transform.position = _buttonOriginalPositionReference.position +
            positionNormalized * movingVector;
    }

    private void FollowTriggeringBody()
    {
        _curPositionNormalized = 0;
        _movingVector = UpdateMovingVector();
        Miscellaneous.BinarySearchNormalized(this, 20);
        //Move(_curPositionNormalized);
        //while (_curPositionNormalized < 1)
        //{
        //    if (!CheckOverlappingWithTriggeringBody())
        //    {
        //        break;
        //    }
        //    _curPositionNormalized += 0.1f;
        //    Move(_curPositionNormalized);
        //}
    }

    #region IBinarySearch
    public int CompareCurrentPosition()
    {
        if (CheckOverlappingWithTriggeringBody())
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void MoveToPosition(float positionNormalized)
    {
        _curPositionNormalized = positionNormalized;
        Move(_movingVector, positionNormalized);
    }

    #endregion

    private void UpdateButtonStatus()
    {
        switch (_status)
        {
            case ButtonStatus.Idle:
                break;

            case ButtonStatus.BeingPressed:
                FollowTriggeringBody();
                if (_curPositionNormalized > 0.999f)
                {
                    OnButtonPressed();
                    _status = ButtonStatus.Pressed;
                }
                break;

            case ButtonStatus.Pressed:
                break;
        }
    }

    protected void OnButtonPressed()
    {
        Miscellaneous.InvokeEvent(ButtonPressed, this);
    }

    void FixedUpdate()
    {
        UpdateButtonStatus();
    }

    void OnDestroy()
    {
        if (_buttonMoverTrigger != null)
        {
            _buttonMoverTrigger.TriggerStart -= OnButtonMoveStart;
            _buttonMoverTrigger.TriggerEnd -= OnButtonMoveStop;
        }
    }
}
