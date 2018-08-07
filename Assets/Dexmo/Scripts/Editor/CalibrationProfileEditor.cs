using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Model;
using Libdexmo.Unity.Core.Model.Calibration;
using UnityEditor;

[CustomEditor(typeof(HandModelCalibrationProfile))]
public class CalibrationProfileEditor : Editor {
    public enum BendAxis
    {
        XAxis,
        YAxis,
        ZAxis
    }
    private BendAxis _bendAxis;
    private bool _calibrateHandFoldout = false;
    private bool _calibrateFingerFoldout = false;
    private FingerType _fingerTypeSelected;

    void OnEnable()
    {
        HandModelCalibrationProfile handProfile = serializedObject.targetObject as HandModelCalibrationProfile;
        foreach (FingerModelCalibrationProfile fingerProfile in handProfile.Fingers)
        {
            fingerProfile.Init();
        }
    }

    void OnDisable()
    {
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        HandModelCalibrationProfile handProfile = 
            (HandModelCalibrationProfile)serializedObject.targetObject;
        //_bendAxis = (BendAxis)EditorGUILayout.EnumPopup("Bend axis:", _bendAxis);
        _calibrateHandFoldout = EditorGUILayout.Foldout(_calibrateHandFoldout, "Hand Calibration");

        if (_calibrateHandFoldout)
        {
            ShowCalibrateHandOptions(handProfile);
        }

        _calibrateFingerFoldout = EditorGUILayout.Foldout(_calibrateFingerFoldout, "Finger Calibration");

        if (_calibrateFingerFoldout)
        {
            ShowCalibrateFingerOptions(handProfile);
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private delegate void HandProfileMethodForAllFingers(Transform hand);
    private delegate void HandProfileMethodForOneFinger(Transform hand, FingerType fingerType);

    private void AddButtonControlForAllFingers(HandModelCalibrationProfile handProfile, string label, HandProfileMethodForAllFingers handProfileMethodForAllFingers)
    {
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button(label))
        {
            if (EditorGUI.EndChangeCheck())
            {
                if (handProfile.Hand == null)
                {
                    Debug.LogError("Hand transform cannot be null.");
                    throw new NullReferenceException("shit!");
                }
                else
                {
                    Transform[] handAndFingers = handProfile.Hand.GetComponentsInChildren<Transform>();
                    int n = handAndFingers.Length;
                    UnityEngine.Object[] objs = new UnityEngine.Object[n];
                    for (int i = 0; i < n; i++)
                    {
                        objs[i] = handAndFingers[i];
                    }
                    Undo.RecordObjects(objs, "Changed hand and finger transforms.");
                    handProfileMethodForAllFingers(handProfile.Hand);
                }
            }
        }
    }

    private void AddButtonControlForOneFinger(HandModelCalibrationProfile handProfile, string label,
        HandProfileMethodForOneFinger handProfileMethodForOneFinger)
    {
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button(label))
        {
            if (EditorGUI.EndChangeCheck())
            {
                if (handProfile.Hand == null)
                {
                    Debug.LogError("Hand transform cannot be null.");
                    throw new NullReferenceException("shit!");
                }
                else
                {
                    Transform[] handAndFingers = handProfile.Hand.GetComponentsInChildren<Transform>();
                    int n = handAndFingers.Length;
                    UnityEngine.Object[] objs = new UnityEngine.Object[n];
                    for (int i = 0; i < n; i++)
                    {
                        objs[i] = handAndFingers[i];
                    }
                    Undo.RecordObjects(objs, "Changed finger transform.");
                    handProfileMethodForOneFinger(handProfile.Hand, _fingerTypeSelected);
                }
            }
        }
    }

    private void ShowCalibrateHandOptions(HandModelCalibrationProfile handProfile)
    {
        //GUILayout.BeginHorizontal();
        AddButtonControlForAllFingers(handProfile, "Calibrate Hand Initial Config",
            handProfile.CalibrateHandInitialConfigRotation);

        AddButtonControlForAllFingers(handProfile, "Calibrate Hand Split Extreme",
            handProfile.CalibrateHandSplitExtreme);

        AddButtonControlForAllFingers(handProfile, "Calibrate Hand Bend Extreme",
            handProfile.CalibrateHandBendExtreme);

        AddButtonControlForAllFingers(handProfile, "Reset Hand to Initial Config",
            handProfile.ResetHandToInitialConfig);

        AddButtonControlForAllFingers(handProfile, "Reset Hand to Split Extreme",
            handProfile.ResetHandToSplitExtreme);

        AddButtonControlForAllFingers(handProfile, "Reset Hand to Bend Extreme",
            handProfile.ResetHandToBendExtreme);
    }

    private void ShowCalibrateFingerOptions(HandModelCalibrationProfile handProfile)
    {
        _fingerTypeSelected = (FingerType) EditorGUILayout.EnumPopup("Finger Type:", _fingerTypeSelected);
        //GUILayout.BeginHorizontal();

        AddButtonControlForOneFinger(handProfile, "Calibrate Finger Initial Config",
            handProfile.CalibrateFingerInitialConfigRotation);

        AddButtonControlForOneFinger(handProfile, "Calibrate Finger Split Extreme",
            handProfile.CalibrateFingerSplitExtreme);

        AddButtonControlForOneFinger(handProfile, "Calibrate Finger Bend Extreme",
            handProfile.CalibrateFingerBendExtreme);

        AddButtonControlForOneFinger(handProfile, "Reset Finger To Initial Config",
            handProfile.ResetFingerToInitialConfig);

        AddButtonControlForOneFinger(handProfile, "Reset Finger To Split Extreme",
            handProfile.ResetFingerToSplitExtreme);

        AddButtonControlForOneFinger(handProfile, "Reset Finger To Bend Extreme",
            handProfile.ResetFingerToBendExtreme);
        //GUILayout.EndHorizontal();
    }

}

