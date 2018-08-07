using UnityEngine;
using System.Collections;
using Libdexmo.Model;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.HandController;
using Libdexmo.Unity.HandController;
using UnityEditor;

namespace Libdexmo.Unity.Editor
{
    public class HandRotationDisplayEditor: EditorWindow
    {
        private IDexmoController _dexmoController;
        private readonly int _handGUIWidth = 200;
        private readonly int _handGUIHeight = 500;

        [MenuItem("Window/LibdexmoUnity/Hand Rotation Display")]
        static void Init()
        {
            HandRotationDisplayEditor window =
                EditorWindow.GetWindow<HandRotationDisplayEditor>();
            window.titleContent = new GUIContent("Rotation Disp");
            window.Show();
        }

        void OnGUI()
        {
            _dexmoController = DexmoController.Instance;
            if (_dexmoController == null)
            {
                EditorGUILayout.LabelField("DexmoController is not initialized.");
                return;
            }
            GUILayout.BeginArea(new Rect(0, 30, _handGUIWidth, _handGUIHeight));
            DisplayHandRotationNormalized(false);
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(_handGUIWidth, 30, _handGUIWidth, _handGUIHeight));
            DisplayHandRotationNormalized(true);
            GUILayout.EndArea();
        }

        private void DisplayHandRotationNormalized(bool isRight)
        {
            string handLabel = isRight ? "Right Hand" : "Left Hand";
            EditorGUILayout.LabelField(handLabel);
            EditorGUI.indentLevel++;
            if (_dexmoController.HandControllerPairs == null ||
                _dexmoController.HandControllerPairs.Count == 0)
            {
                EditorGUILayout.LabelField(handLabel + " is not initialized.");
            }
            else
            {
                UnityHandController handController = isRight
                    ? _dexmoController.HandControllerPairs[0].Right
                    : _dexmoController.HandControllerPairs[0].Left;
                //if (!handController.Active)
                //{
                //    EditorGUILayout.LabelField(handLabel + " is not active.");
                //}
                //else
                {
                    IHandRotationNormalized handRotation =
                        handController.GetCurrentFingerRotationInfo();
                    int n = handRotation.Fingers.Length;
                    for (int i = 0; i < n; i++)
                    {
                        IFingerRotationNormalized fingerRotation =
                            handRotation.Fingers[i];
                        FingerType fingerType = (FingerType)i;
                        EditorGUILayout.LabelField(fingerType.ToString());
                        DisplayFingerRotationNormalized(fingerRotation);
                    }
                }
            }
            EditorGUI.indentLevel--;
        }

        private void DisplayFingerRotationNormalized(IFingerRotationNormalized fingerRotation)
        {
            EditorGUI.indentLevel++;
            IFingerRotationNormalizedThumb thumbRotation = fingerRotation as
                IFingerRotationNormalizedThumb;
            if (thumbRotation != null)
            {
                DisplayRotationNormalized(thumbRotation.Rotate, "Rotate");
                DisplayRotationNormalized(thumbRotation.Split, "Split");
                DisplayRotationNormalized(thumbRotation.Bend, "Bend");
            }
            else
            {
                DisplayRotationNormalized(fingerRotation.Split, "Split");
                DisplayRotationNormalized(fingerRotation.Bend, "Bend");
            }
            EditorGUI.indentLevel--;
        }

        private void DisplayRotationNormalized(RotationNormalizedInfo rotation, string label)
        {
            //EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth((float)_handGUIWidth / 2));
            EditorGUILayout.LabelField(rotation.Value.ToString(),
                GUILayout.MaxWidth((float)_handGUIWidth / 2));
            EditorGUILayout.EndHorizontal();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
