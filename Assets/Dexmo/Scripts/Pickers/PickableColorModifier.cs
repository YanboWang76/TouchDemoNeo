/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using UnityEngine;

namespace Libdexmo.Unity.Pickers
{
    /// <summary>
    /// This is a utility class used by all pickers that modifies the color of the corresponding
    /// pickers' touched objects if needed.
    /// <remarks>
    /// It maintains a set of previously touched objects. From the latest set of touched objects,
    /// it will figure out which objects are still in touch and which ones are no longer in 
    /// touch and modify color accordingly.
    /// </remarks>
    /// </summary>
    public class PickableColorModifier
    {
        private HashSet<ITouchable> _lastTouchableSet;
        private HashSet<ITouchable> _touchableEnterSetTemp;
        private HashSet<ITouchable> _touchableExitSetTemp;
        private static Dictionary<Renderer, Color> _rendererToOriginalColorDict;
        private static Dictionary<Renderer, int> _rendererToRefCountDict;

        public PickableColorModifier()
        {
            if (_rendererToOriginalColorDict == null)
            {
                _rendererToOriginalColorDict = new Dictionary<Renderer, Color>();
            }
            if (_rendererToRefCountDict == null)
            {
                _rendererToRefCountDict = new Dictionary<Renderer, int>();
            }
            _lastTouchableSet = new HashSet<ITouchable>();
            _touchableEnterSetTemp = new HashSet<ITouchable>();
            _touchableExitSetTemp = new HashSet<ITouchable>();
        }

        /// <summary>
        /// Update the color of the touched objects. From the latest set of touched objects,
        /// it finds which objects just come into contact. The color of these objects will
        /// be modified if needed. Then it finds which objects are no longer in contact. The
        /// color of these objects will be restored to original.
        /// </summary>
        /// <param name="touchableSet">Latest set of touched objects.</param>
        public void UpdateTouchedObjectColor(HashSet<ITouchable> touchableSet)
        {
            // Find touchable objects that comes in and the ones left our hand
            Miscellaneous.FindHashSetDifference(touchableSet, _lastTouchableSet,
                _touchableEnterSetTemp, _touchableExitSetTemp);
            // Colorize touchable objects that just come into contact with the hand
            foreach (ITouchable touchable in _touchableEnterSetTemp)
            {
                if (touchable.IndicateColorOnTouching)
                {
                    ChangeTouchableObjectColorUponEnter(touchable);
                }
            }
            // Restore color of the touchable objects that leave the hand
            foreach (ITouchable touchable in _touchableExitSetTemp)
            {
                if (touchable.IndicateColorOnTouching)
                {
                    RestoreTouchableObjectColorUponExit(touchable);
                }
            }
            _lastTouchableSet.Clear();
            _lastTouchableSet.CopyFrom(touchableSet);
        }

        /// <summary>
        /// Change the color of the first material of the renderer to a given color.
        /// </summary>
        /// <param name="renderer">Renderer whose color will be changed.</param>
        /// <param name="indicatedColor">The color that will be change to.</param>
        private void ChangeRendererColor(Renderer renderer, Color indicatedColor)
        {
            if (renderer == null)
            {
                return;
            }
            if (renderer.Equals(null))
            {
                // The game object that this renderer belongs to has been destroyed.
                _rendererToOriginalColorDict.Remove(renderer);
                return;
            }

            if (!_rendererToOriginalColorDict.ContainsKey(renderer))
            {
                // The mapping between the renderer and its original color does not
                // exist. Create a mapping for it.
                Color originalColor = renderer.material.color;
                _rendererToOriginalColorDict.Add(renderer, originalColor);
            }
            // Mapping between renderer and refcount makes it easier in the future to extend it
            // to modifying multiple renderers associated with a single touchable object.
            if (!_rendererToRefCountDict.ContainsKey(renderer))
            {
                _rendererToRefCountDict.Add(renderer, 1);
            }
            else
            {
                _rendererToRefCountDict[renderer]++;
            }
            renderer.material.color = indicatedColor;
        }

        /// <summary>
        /// Change the color of the touchable object that first comes into contact with
        /// the picker.
        /// </summary>
        /// <param name="touchable">The touchable that first comes into contact.</param>
        private void ChangeTouchableObjectColorUponEnter(ITouchable touchable)
        {
            if (touchable == null || touchable.Equals(null))
            {
                // Touchable may have been destroyed
                return;
            }
            Transform touchableTransform = touchable.BindedGameObject.transform;
            Renderer renderer = touchableTransform.GetComponent<Renderer>();
            // Change color for all renderers of the object and its children.
            if (renderer != null)
            {
                ChangeRendererColor(renderer, touchable.IndicatedColorOnTouching);
            }
            foreach (Transform child in touchableTransform)
            {
                renderer = child.GetComponent<Renderer>();
                if (renderer == null)
                {
                    continue;
                }
                ChangeRendererColor(renderer, touchable.IndicatedColorOnTouching);
            }
        }

        /// <summary>
        /// Restore the color of the renderer.
        /// </summary>
        /// <param name="renderer">The renderer whose color will be restored.</param>
        private void RestoreRendererColor(Renderer renderer)
        {
            if (renderer == null)
            {
                return;
            }
            if (renderer.Equals(null))
            {
                // Renderer component may have been destroyed.
                _rendererToOriginalColorDict.Remove(renderer);
                return;
            }

            _rendererToRefCountDict[renderer]--;
            if (_rendererToRefCountDict[renderer] < 0)
            {
                Debug.LogError("Renderer ref count is less than 0.");
            }
            if (_rendererToRefCountDict[renderer] == 0)
            {
                // This object has completely left our hands. Restore its color.
                if (_rendererToOriginalColorDict.ContainsKey(renderer))
                {
                    Color originalColor = _rendererToOriginalColorDict[renderer];
                    renderer.material.color = originalColor;
                }
                else
                {
                    Debug.LogError("Unknown original color of the material.");
                }
            }
        }

        /// <summary>
        /// Restore the color of the touchable object that just leaves the picker.
        /// </summary>
        /// <param name="touchable">Touchable object whose color will be restored.</param>
        private void RestoreTouchableObjectColorUponExit(ITouchable touchable)
        {
            // Equals is overridden by MonoBehaviour, so can be used to check if the game object
            // has been destroyed.
            if (touchable == null || touchable.Equals(null))
            {
                return;
            }
            Transform touchableTransform = touchable.BindedGameObject.transform;
            Renderer renderer = touchableTransform.GetComponent<Renderer>();
            if (renderer != null && _rendererToRefCountDict.ContainsKey(renderer))
            {
                RestoreRendererColor(renderer);
            }
            // Restore color for the renderers of the touchable and its children.
            foreach (Transform child in touchableTransform)
            {
                renderer = child.GetComponent<Renderer>();
                if (renderer == null)
                {
                    continue;
                }
                if (!_rendererToRefCountDict.ContainsKey(renderer))
                {
                    continue;
                }
                RestoreRendererColor(renderer);
            }
        }
    }
}
