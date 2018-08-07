using UnityEngine;
using System.Collections;
using Libdexmo.Unity.HandController;
using UnityEditor;

namespace Libdexmo.Unity.Editor
{
    [CustomEditor(typeof(DexmoController))]
    public class DexmoControllerEditor : UnityEditor.Editor
    {
        private DexmoController _dexmoController;

        void OnEnable()
        {
            _dexmoController = serializedObject.targetObject as
                DexmoController;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            if (_dexmoController == null)
            {
                return;
            }
            if (GUILayout.Button("Show Hands Rotation Info"))
            {
                ShowHandRotationInfo();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowHandRotationInfo()
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("This function can only be used while running.");
                return;
            }
            _dexmoController.ShowHandsRotationInfo();
        }
    }
}
