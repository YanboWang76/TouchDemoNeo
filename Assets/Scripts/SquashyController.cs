using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Libdexmo.Unity.Pickers;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.HandController;
using Libdexmo.Unity.Core.Utility;

public class SquashyController : MonoBehaviour
{
    public float SquashyConstant = 1f;
    public float HeightToWidthRatio = 0.5f;
    public Transform ControlledTransform;
    public SquashyState CurSquashyState { get { return _curSquashyState; } }

    public enum SquashyState
    {
        Idle,
        Pressing,
        Releasing
    }

    private enum ColliderType
    {
        BoxCollider,
        SphereCollider,
        Other
    }
    private Vector3 _controlledTransformOriginalScale;
    private Vector3 _originalScale;
    private float lastAngle;
    private float _startAngle;
    private Collider _colliderInContact;
    private bool _inContact;
    private IPickable _pickableScript;
    private HashSet<Rigidbody> _rbInContactList;
    private Dictionary<Rigidbody, float> _rbToStartAngleDict;
    private bool _lastPickState;
    private Dictionary<Collider, ColliderType> _selfColliderToTypeDict;
    private SquashyState _curSquashyState;
    private float _pressingThreshFromIdle;
    private float _pressingLocalScaleMin;
    private float _releasingLocalScaleMax;
    private Vector3 _controlledTransformTargetLocalScale;
    // Use this for initialization
    void Start()
    {
        _controlledTransformOriginalScale = ControlledTransform.localScale;
        _originalScale = transform.localScale;
        _inContact = false;
        _pickableScript = GetComponent<IPickable>();
        _rbInContactList = new HashSet<Rigidbody>();
        _rbToStartAngleDict = new Dictionary<Rigidbody, float>();
        _lastPickState = false;
        _selfColliderToTypeDict = new Dictionary<Collider, ColliderType>();
        FindChildColliders();
        _curSquashyState = SquashyState.Idle;
        _pressingThreshFromIdle = 0.9f * _controlledTransformOriginalScale.x;
        _pressingLocalScaleMin = _controlledTransformOriginalScale.x;
        _releasingLocalScaleMax = 0;
        _controlledTransformTargetLocalScale = _controlledTransformOriginalScale;
    }

    private void FindChildColliders()
    {
        // Find colliders in this transform
        Collider[] selfColliders = GetComponents<Collider>();
        foreach (Collider c in selfColliders)
        {
            _selfColliderToTypeDict.Add(c, IdentifyColliderType(c));
        }
        // Find colliders in children just one level below
        foreach (Transform child in transform)
        {
            Collider c = child.GetComponent<Collider>();
            if (c != null)
            {
                _selfColliderToTypeDict.Add(c, IdentifyColliderType(c));
            }
        }
    }

    private ColliderType IdentifyColliderType(Collider c)
    {
        ColliderType type;
        if (c is BoxCollider)
        {
            type = ColliderType.BoxCollider;
        }
        else if (c is SphereCollider)
        {
            type = ColliderType.SphereCollider;
        }
        else
        {
            type = ColliderType.Other;
        }
        return type;
    }

    private bool CheckOverlapWithFinger()
    {
        List<Collider> others = new List<Collider>();
        foreach (var pair in _selfColliderToTypeDict)
        {
            Collider c = pair.Key;
            ColliderType type = pair.Value;
            Collider[] overlappedColliders = new Collider[0];
            switch (type)
            {
                case ColliderType.BoxCollider:
                    overlappedColliders = CheckOverlapBox((BoxCollider)c);
                    break;

                case ColliderType.SphereCollider:
                    overlappedColliders = CheckOverlapSphere((SphereCollider)c);
                    break;

                default:
                    break;
            }
            int n = overlappedColliders.Length;
            for (int i = 0; i < n; i++)
            {
                others.Add(overlappedColliders[i]);
            }
        }
        return CheckCollideWithFinger(others);
    }

    private Collider[] CheckOverlapBox(BoxCollider boxCollider)
    {
        Transform attachedTransform = boxCollider.transform;
        Vector3 center = attachedTransform.TransformPoint(boxCollider.center);
        Vector3 extents = Miscellaneous.Vector3Abs(
            Vector3.Scale(boxCollider.size / 2, attachedTransform.lossyScale));
        return Physics.OverlapBox(center, extents, attachedTransform.rotation);
    }

    private Collider[] CheckOverlapSphere(SphereCollider sphereCollider)
    {
        Transform attachedTransform = sphereCollider.transform;
        Vector3 center = attachedTransform.TransformPoint(sphereCollider.center);
        float radius = Mathf.Abs(sphereCollider.radius) *
            Miscellaneous.Vector3MaxComponentAbs(attachedTransform.lossyScale);
        return Physics.OverlapSphere(center, radius);
    }

    private bool CheckCollideWithFinger(List<Collider> colliders)
    {
        bool collideWithFinger = false;
        int n = colliders.Count;
        for (int i = 0; i < n; i++)
        {
            Collider c = colliders[i];
            if (c.gameObject.GetComponent<JointColliderManager>() != null)
            {
                collideWithFinger = true;
                break;
            }
        }
        return collideWithFinger;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScale();
        SquashyStateTransition();
        ModifyControlledTransformLocalScaleWithTransition(_controlledTransformTargetLocalScale);
        _lastPickState = _pickableScript.IsPicked;
    }

    private void UpdateScale()
    {
        if (!_pickableScript.IsPicked)
        {
            if (_lastPickState)
            {
                _controlledTransformTargetLocalScale = _controlledTransformOriginalScale;
            }
            return;
        }
        float scaleFactor = FindScaleFactorThatJustTouchFinger();
        float minScale = 0.001f;
        Vector3 curScale = _controlledTransformOriginalScale;
        curScale.x = curScale.x * scaleFactor;
        curScale.x = Mathf.Clamp(curScale.x, minScale, _controlledTransformOriginalScale.x);
        float xDiff = _controlledTransformOriginalScale.x - curScale.x;
        curScale.y = _controlledTransformOriginalScale.y + HeightToWidthRatio * xDiff;
        _controlledTransformTargetLocalScale = curScale;
        //float angleDiffMax = 0;
        //foreach (var pair in _rbToStartAngleDict)
        //{
        //    Rigidbody rb = pair.Key;
        //    float startAngle = pair.Value;
        //    float angleDiff = rb.transform.localEulerAngles.z - startAngle;
        //    if (angleDiff > angleDiffMax)
        //    {
        //        angleDiffMax = angleDiff;
        //    }
        //}
        //Vector3 curScale = ControlledTransform.localScale;
        ////Debug.LogWarningFormat("Angle diff: {0}", angleDiff);
        //curScale.x = _controlledTransformOriginalScale.x - SquashyConstant * angleDiffMax;
        //float minScale = 0.001f;
        //curScale.x = Mathf.Clamp(modifiedScale.x, minScale, _controlledTransformOriginalScale.x);
        //float xDiff = _controlledTransformOriginalScale.x - curScale.x;
        //curScale.y = _controlledTransformOriginalScale.y + HeightToWidthRatio * xDiff;
        //ControlledTransform.localScale = curScale;
    }

    private void ModifyControlledTransformLocalScaleWithTransition(Vector3 modifiedScale)
    {
        float diffMax = 0.01f;
        Vector3 scaleOld = ControlledTransform.localScale;
        float diff = Vector3.Distance(scaleOld, modifiedScale);
        if (diff > diffMax)
        {
            ControlledTransform.localScale = Vector3.Lerp(scaleOld, modifiedScale,
                0.9f * Time.deltaTime / 0.5f);
        }
        else
        {
            ControlledTransform.localScale = modifiedScale;
        }
    }

    private float FindScaleFactorThatJustTouchFinger()
    {
        float scale = _originalScale.x;
        float scaleStep = scale / 10;
        float upper = scale;
        float lower = scale;
        while (scale > 0 && CheckOverlapWithFinger())
        {
            upper = scale;
            scale -= scaleStep;
            transform.localScale = new Vector3(scale, scale, scale);
            lower = scale;
        }
        if (scale > 0)
        {
            scale = CheckOverlapWithFingerBinarySearch(upper, lower);
        }
        float scaleFactor = scale / _originalScale.x;
        transform.localScale = _originalScale;
        return scaleFactor;
    }

    private float CheckOverlapWithFingerBinarySearch(float upper, float lower,
        float tol = 0.001f, int maxIter = 1000)
    {
        int iter = 0;
        float error = Mathf.Abs(upper - lower);
        while (iter < maxIter && error > tol)
        {
            float scale = (upper + lower) / 2;
            transform.localScale = new Vector3(scale, scale, scale);
            if (CheckOverlapWithFinger())
            {
                upper = scale;
            }
            else
            {
                lower = scale;
            }
            error = Mathf.Abs(upper - lower);
            iter++;
        }
        return lower;
    }

    private void SquashyStateTransition()
    {
        switch (_curSquashyState)
        {
            case SquashyState.Idle:
                if (ControlledTransform.localScale.x < _pressingThreshFromIdle)
                {
                    _curSquashyState = SquashyState.Pressing;
                }
                break;

            case SquashyState.Pressing:
                if (ControlledTransform.localScale.x >=
                    _pressingLocalScaleMin + 0.1f * _controlledTransformOriginalScale.x)
                {
                    _curSquashyState = SquashyState.Releasing;
                    _pressingLocalScaleMin = _controlledTransformOriginalScale.x;
                }
                else
                {
                    _pressingLocalScaleMin = Mathf.Min(ControlledTransform.localScale.x, _pressingLocalScaleMin);
                }
                break;

            case SquashyState.Releasing:
                if (ControlledTransform.localScale.x <=
                    _releasingLocalScaleMax - 0.1f * _controlledTransformOriginalScale.x)
                {
                    _curSquashyState = SquashyState.Pressing;
                    _releasingLocalScaleMax = 0;
                }
                else if (ControlledTransform.localScale.x >
                    _controlledTransformOriginalScale.x - 0.00001f)
                {
                    _curSquashyState = SquashyState.Idle;
                    _releasingLocalScaleMax = 0;
                }
                else
                {
                    _releasingLocalScaleMax = Mathf.Max(ControlledTransform.localScale.x, _releasingLocalScaleMax);
                }
                break;
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (enabled)
        {
            if (CheckColliderIsPicker(other))
            {
                Rigidbody rb = other.attachedRigidbody;
                if (!_rbToStartAngleDict.ContainsKey(rb))
                {
                    float startAngle = rb.transform.localEulerAngles.z;
                    _rbToStartAngleDict.Add(rb, startAngle);
                }
            }
            //if (CheckColliderIsPicker(other) && !_inContact)
            //{
            //    _startAngle = other.attachedRigidbody.transform.localEulerAngles.z;
            //    _colliderInContact = other;
            //    _inContact = true;
            //}
        }
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (enabled && _pickableScript.IsPicked)
    //    {
    //        if (CheckColliderIsPicker(other))
    //        {

    //        }
    //        //if (CheckColliderIsPicker(other) && other == _colliderInContact)
    //        //{
    //        //    float angle = other.attachedRigidbody.transform.localEulerAngles.z;
    //        //    if (angle > _startAngle)
    //        //    {
    //        //        float angleDiff = angle - _startAngle;
    //        //        Vector3 curScale = ControlledTransform.localScale;
    //        //        //Debug.LogWarningFormat("Angle diff: {0}", angleDiff);
    //        //        curScale.x = _controlledTransformOriginalScale.x - SquashyConstant * angleDiff;
    //        //        float minScale = 0.001f;
    //        //        curScale.x = Mathf.Clamp(modifiedScale.x, minScale, _controlledTransformOriginalScale.x);
    //        //        float xDiff = _controlledTransformOriginalScale.x - curScale.x;
    //        //        curScale.y = _controlledTransformOriginalScale.y + HeightToWidthRatio * xDiff;
    //        //        ControlledTransform.localScale = curScale;
    //        //    }
    //        //}
    //    }
    //}

    void OnTriggerExit(Collider other)
    {
        if (enabled)
        {
            if (CheckColliderIsPicker(other))
            {
                Rigidbody rb = other.attachedRigidbody;
                if (_rbToStartAngleDict.ContainsKey(rb))
                {
                    _rbToStartAngleDict.Remove(rb);
                }
            }
            //if (CheckColliderIsPicker(other) && other == _colliderInContact)
            //{
            //    ControlledTransform.localScale = _controlledTransformOriginalScale;
            //    _colliderInContact = null;
            //    _inContact = false;
            //}
        }
    }

    private bool CheckColliderIsPicker(Collider c)
    {
        return c.gameObject.GetComponentInParent<Picker>() != null;
    }

}
