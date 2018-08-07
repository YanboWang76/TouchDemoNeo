using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CTool.InputInteraction
{
	public class TouchRotateByOneFinger : MonoBehaviour,ITouchRayEvent
	{
		public bool clickTrigger = false;
		public Transform parent, target;
		Vector3 previousPos;
		bool canDrag;
		float ratio;

		void Start ()
		{
			TouchRayEvent.GetInstance().AddListen (target.gameObject, true, false, false);
			CanvasScaler canvasScaler = GameObject.FindObjectOfType<CanvasScaler> ();
			ratio = canvasScaler.referenceResolution.y / (float)Screen.height;
			if (!target) {
				target = transform;
			}
			if (!parent) {
				parent = new GameObject ("parent").transform;
				parent.SetParent (target.parent);
				parent.localPosition = Vector3.zero;
				parent.localEulerAngles = Vector3.zero;
				parent.localScale = Vector3.one;
				target.SetParent (parent);
			}
		}
		void OnDestory()
		{
			TouchRayEvent.GetInstance().RemoveListen (target.gameObject);
		}

		void Update ()
		{
			#if UNITY_EDITOR
			if (UnityEngine.Input.GetMouseButtonDown(0)) {
				OnDown();
			}
			if (UnityEngine.Input.GetMouseButton(0)) {
				OnMove();
			}
			if (UnityEngine.Input.GetMouseButtonUp(0)) {
				OnUp();
			}

			#else
			if (Input.touchCount == 1) {
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					OnDown ();
				}
				if (Input.GetTouch (0).phase == TouchPhase.Moved) {
					OnMove ();
				}
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					OnUp ();
				}
			}
			#endif
		}

		void OnDown ()
		{
			
			canDrag = true;
			previousPos = Input.mousePosition;
		}

		void OnMove ()
		{
			if (canDrag) {
				Vector3 moveDelta = (Input.mousePosition - previousPos) * ratio;
				previousPos = Input.mousePosition;
				if (parent) {
					Vector3 angle = parent.localEulerAngles;
					angle.x -= moveDelta.y;
					parent.localEulerAngles = angle;
				}
				if (target) {
					Vector3 angle = target.localEulerAngles;
					angle.y -= moveDelta.x;
					target.localEulerAngles = angle;
				}
			}
		}

		void OnUp ()
		{
			canDrag = false;
		}

		public void OnTouchDown(Vector3[] vectors)
		{
			
		}
		public void OnTouchMove (Vector3[] vector)
		{
		}

		public void OnTouchUp (Vector3[] vector)
		{
		}
	}

}
