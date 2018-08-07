using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core.Model.Calibration;
using UnityEditor;

[CustomEditor(typeof(CalibrationProfileManager))]
public class CalibrationProfileManagerEditor : Editor
{
    // Use this for initialization
    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        CalibrationProfileManager profileManager =
            (CalibrationProfileManager)serializedObject.targetObject;
        if (GUILayout.Button("Add Hand profile slot"))
        {
            Undo.RecordObject(profileManager, "Add slot");
            profileManager.AddHandProfileSlot();
        }
        if (GUILayout.Button("Remove slot"))
        {
            Undo.RecordObject(profileManager, "Remove slot");
            profileManager.RemoveLastHandProfile();
        }
        //CountProp.intValue = EditorGUILayout.IntField("Count: ", CountProp.intValue);
        serializedObject.ApplyModifiedProperties();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
