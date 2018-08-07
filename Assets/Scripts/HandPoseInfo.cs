using System.Collections;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.HandController;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.HandController;
using UnityEngine;

public class HandPoseInfo : MonoBehaviour {

    public static HandPoseInfo left = null;
    public static HandPoseInfo right = null;

    public static UnityHandPoseManager leftHandPoseManager = null;
    public static UnityHandPoseManager rightHandPoseManager = null;

    void Awake()
    {
        if (gameObject.name.Contains("Left"))
        {
            if (left == null)
            {
                left = this;
                leftHandPoseManager = gameObject.GetComponentInChildren<UnityHandPoseManager>();
            }
            
            else if (left != this)
            {
                Destroy(this);
            }
            return;
        }

        if (gameObject.name.Contains("Right"))
        {
            if (right == null)
            {
                right = this;
                rightHandPoseManager = gameObject.GetComponentInChildren<UnityHandPoseManager>();
            }
            
            else if (right != this)
            {
                Destroy(this);
            }
            return;
        }
    }

    public static bool GetLeftHandInfo(out float[][] Fingers)
    {
        if (left == null)
        {
            Debug.Log("No leftHandPoseManager! Please attached HandPoseInfo script to the left hand root object.");
            Fingers = new float[0][];
            return false;
        }
        else
        {
            Fingers = new float[5][];
            Fingers[0] = new float[3];
            Fingers[0][0] = leftHandPoseManager.CurHandRotationNormalized.Thumb.Split.Value;
            Fingers[0][1] = leftHandPoseManager.CurHandRotationNormalized.Thumb.Bend.Value;
            Fingers[0][2] = leftHandPoseManager.CurHandRotationNormalized.Thumb.Rotate.Value;

            Fingers[1] = new float[2];
            Fingers[1][0] = leftHandPoseManager.CurHandRotationNormalized.Index.Split.Value;
            Fingers[1][1] = leftHandPoseManager.CurHandRotationNormalized.Index.Bend.Value;

            Fingers[2] = new float[2];
            Fingers[2][0] = leftHandPoseManager.CurHandRotationNormalized.Middle.Split.Value;
            Fingers[2][1] = leftHandPoseManager.CurHandRotationNormalized.Middle.Bend.Value;

            Fingers[3] = new float[2];
            Fingers[3][0] = leftHandPoseManager.CurHandRotationNormalized.Ring.Split.Value;
            Fingers[3][1] = leftHandPoseManager.CurHandRotationNormalized.Ring.Bend.Value;

            Fingers[4] = new float[2];
            Fingers[4][0] = leftHandPoseManager.CurHandRotationNormalized.Pinky.Split.Value;
            Fingers[4][1] = leftHandPoseManager.CurHandRotationNormalized.Pinky.Bend.Value;

            return true;
        }
    }

    public static bool GetRightHandInfo(out float[][] Fingers)
    {
        if (right == null)
        {
            Debug.Log("No rightHandPoseManager! Please attached HandPoseInfo script to the right hand root object.");
            Fingers = new float[0][];
            return false;
        }
        else
        {
            Fingers = new float[5][];
            Fingers[0] = new float[3];
            Fingers[0][0] = rightHandPoseManager.CurHandRotationNormalized.Thumb.Split.Value;
            Fingers[0][1] = rightHandPoseManager.CurHandRotationNormalized.Thumb.Bend.Value;
            Fingers[0][2] = rightHandPoseManager.CurHandRotationNormalized.Thumb.Rotate.Value;

            Fingers[1] = new float[2];
            Fingers[1][0] = rightHandPoseManager.CurHandRotationNormalized.Index.Split.Value;
            Fingers[1][1] = rightHandPoseManager.CurHandRotationNormalized.Index.Bend.Value;

            Fingers[2] = new float[2];
            Fingers[2][0] = rightHandPoseManager.CurHandRotationNormalized.Middle.Split.Value;
            Fingers[2][1] = rightHandPoseManager.CurHandRotationNormalized.Middle.Bend.Value;

            Fingers[3] = new float[2];
            Fingers[3][0] = rightHandPoseManager.CurHandRotationNormalized.Ring.Split.Value;
            Fingers[3][1] = rightHandPoseManager.CurHandRotationNormalized.Ring.Bend.Value;

            Fingers[4] = new float[2];
            Fingers[4][0] = rightHandPoseManager.CurHandRotationNormalized.Pinky.Split.Value;
            Fingers[4][1] = rightHandPoseManager.CurHandRotationNormalized.Pinky.Bend.Value;

            return true;
        }
    }
}
