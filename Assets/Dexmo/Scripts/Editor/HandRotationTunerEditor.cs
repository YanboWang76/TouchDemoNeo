using UnityEngine;
using System.Collections;
using Libdexmo.Model;
using Libdexmo.Unity.Core.HandController;
using Libdexmo.Unity.HandController;
using UnityEditor;

namespace Libdexmo.Unity.Editor
{
    public class HandRotationTunerEditor : EditorWindow
    {
        private DexmoController _dexmoController;
        private bool _leftExpanded = false;
        private bool _rightExpanded = false;
        private bool[] _leftFingersExpanded = new bool[5];
        private bool[] _rightFingersExpanded = new bool[5];
        private readonly string _leftHandGroupLabel = "Left Hand";
        private readonly string _rightHandGroupLabel = "Right Hand";
        private readonly string[] _fingerNames = {"Thumb", "Index", "Middle", "Ring", "Pinky"};
        private readonly int _handGUIWidth = 350;
        private readonly int _handGUIHeight = 500;

        [MenuItem("Window/LibdexmoUnity/Hand Rotation Tuner")]
        static void Init()
        {
            HandRotationTunerEditor window =
                EditorWindow.GetWindow<HandRotationTunerEditor>();
            window.titleContent = new GUIContent("Rotation Tuner");
            window.Show();
        }

        void OnGUI()
        {
            //_dexmoController = (DexmoController) EditorGUILayout.ObjectField(
            //    "Dexmo LibdexmoClientController", _dexmoController, typeof(DexmoController), true);
            _dexmoController = FindObjectOfType<DexmoController>();
            if (_dexmoController == null)
            {
                EditorGUILayout.LabelField("Cannot find DexmoController.");
                return;
            }
            GUILayout.BeginArea(new Rect(0, 30, _handGUIWidth, _handGUIHeight));
            _leftExpanded = EditorGUILayout.Foldout(_leftExpanded, _leftHandGroupLabel);
            if (_leftExpanded)
            {
                HandFoldoutView(false);
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(_handGUIWidth, 30, _handGUIWidth, _handGUIHeight));
            _rightExpanded = EditorGUILayout.Foldout(_rightExpanded, _rightHandGroupLabel);
            if (_rightExpanded)
            {
                HandFoldoutView(true);
            }
            GUILayout.EndArea();
        }

        private void HandFoldoutView(bool isRight)
        {
            EditorGUI.indentLevel ++;
            HandRotationBounds handRotationBounds = isRight
                ? _dexmoController.HandPool.HandPairs[0].Right.HandRotationBounds
                : _dexmoController.HandPool.HandPairs[0].Left.HandRotationBounds;
            //bool[] fingersExpanded = isRight ? _leftFingersExpanded : _rightFingersExpanded;
            if (handRotationBounds == null)
            {
                EditorGUILayout.LabelField("None");
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    EditorGUILayout.LabelField(_fingerNames[i]);
                    FingerFoldoutView(handRotationBounds.Fingers[i], (FingerType) i);
                    //fingersExpanded[i] = EditorGUILayout.Foldout(
                    //    fingersExpanded[i], _fingerNames[i]);
                    //if (fingersExpanded[i])
                    //{
                    //    FingerFoldoutView(handRotationBounds.Fingers[i], (FingerType) i);
                    //}
                }
            }
            EditorGUI.indentLevel --;
        }

        private void FingerFoldoutView(FingerRotationBounds fingerRotationBounds,
            FingerType fingerType)
        {
            EditorGUI.indentLevel ++;
            if (fingerType == FingerType.Thumb)
            {
                FingerRotationBoundsThumb thumbRotationBounds = 
                    fingerRotationBounds as FingerRotationBoundsThumb;
                if (thumbRotationBounds == null)
                {
                    Debug.LogError("ThumbRotationBounds is null.");
                }
                else
                {
                    InsertBoundaryValueView(thumbRotationBounds.MCPSplitRotationBounds,
                        "MCP Split: ");
                    InsertBoundaryValueView(thumbRotationBounds.MCPBendRotationBounds,
                        "MCP Bend: ");
                    InsertBoundaryValueView(thumbRotationBounds.SplitRotationBounds,
                        "Split: ");
                    InsertBoundaryValueView(thumbRotationBounds.BendRotationBounds,
                        "Bend: ");
                }
            }
            else
            {
                if (fingerRotationBounds == null)
                {
                    Debug.LogError("finerRotationBounds is null.");
                }
                else
                {
                    InsertBoundaryValueView(fingerRotationBounds.SplitRotationBounds,
                        "Split: ");
                    InsertBoundaryValueView(fingerRotationBounds.BendRotationBounds,
                        "Bend: ");
                }
            }
            EditorGUI.indentLevel --;
        }

        private void InsertBoundaryValueView(BoundaryValue bounds, string label)
        {
            EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth((float)_handGUIWidth / 5));
            Undo.RecordObject(_dexmoController, "Change bounds");
            float min = EditorGUILayout.FloatField("Min", bounds.Min,
                GUILayout.MaxWidth((float)2 * _handGUIWidth / 5));
            float max = EditorGUILayout.FloatField("Max", bounds.Max,
                GUILayout.MaxWidth((float)2 * _handGUIWidth / 5));
            min = Mathf.Clamp01(min);
            max = Mathf.Clamp01(max);
            bounds.Min = min;
            bounds.Max = max;
            //EditorGUILayout.LabelField(label, 
            //    GUILayout.MaxWidth((float)_handGUIWidth * 4 / 20));
            //EditorGUILayout.LabelField("Min", 
            //    GUILayout.MaxWidth((float)_handGUIWidth * 4 / 20));
            //bounds.Min = EditorGUILayout.Slider(bounds.Min, 0, 1,
            //    GUILayout.MaxWidth((float)4 * _handGUIWidth / 20));
            //EditorGUILayout.LabelField("Max", 
            //    GUILayout.MaxWidth((float)_handGUIWidth * 4 / 20));
            //bounds.Max = EditorGUILayout.Slider(bounds.Max, 0, 1,
            //    GUILayout.MaxWidth((float)4 * _handGUIWidth / 20));
            EditorGUILayout.EndHorizontal();
        }



        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
