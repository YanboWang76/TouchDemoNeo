using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CTool.InputInteraction
{	
	public class TouchRayEvent : MonoBehaviour {
		public enum TouchType {
			OnDown,
			OnMove,
			OnUp
		}
		public delegate void TouchDelegate(Vector3 pos);
		private List<Target> targets = new List<Target>();
		private class Target {
			public GameObject go;
			public ITouchRayEvent touchEvent;
			public bool downOnScope, moveOnScope, upOnScope;
		}
		private static TouchRayEvent instance;
		internal static TouchRayEvent GetInstance()
		{
			if (instance == null) 
			{
				GameObject go = new GameObject ("TouchRayEvent");
				instance = go.AddComponent<TouchRayEvent> ();
			}
			return instance;
		}
		void OnDestory()
		{
			instance = null;
		}
		void Update() {
			if (Application.isEditor) {
				if (Input.GetMouseButtonDown(0)) {
					OnDown(Input.mousePosition);
				}
				if (Input.GetMouseButton(0)) {
					OnMove(Input.mousePosition);
				}
				if (Input.GetMouseButtonUp(0)) {
					OnUp(Input.mousePosition);
				}
			} else {
				if (Input.touchCount > 0) {
					if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began) {
						OnDown(Input.GetTouch(0).position);
					}
					if (Input.GetTouch(0).phase == TouchPhase.Moved) {
						OnMove(Input.GetTouch(0).position);
					}
					if (Input.GetTouch(0).phase == TouchPhase.Ended) {
						OnUp(Input.GetTouch(0).position);
					}
				}
			}
		}

		void OnDown(Vector3 pos) {
			if (!Camera.main) {
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(pos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				for (int i = 0; i < targets.Count; i++) {
					Target target = targets[i];
					if (target.downOnScope && target.go == hit.collider.gameObject) {
						if (target.go.activeInHierarchy) {
							target.touchEvent.OnTouchDown (new Vector3[] { pos, hit.point });
//							target.go.SendMessage("OnTouchDown", new Vector3[] { pos, hit.point });
						}
					}
				}
			}
			for (int i = 0; i < targets.Count; i++) {
				Target target = targets[i];
				if (!target.downOnScope) {
					if (target.go.activeInHierarchy) {
						target.touchEvent.OnTouchDown (new Vector3[] { pos, hit.point });
//						target.go.SendMessage("OnTouchDown", new Vector3[] { pos, hit.point });
					}
				}
			}
		}

		void OnMove(Vector3 pos) {
			if (!Camera.main) {
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(pos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				for (int i = 0; i < targets.Count; i++) {
					Target target = targets[i];
					if (target.moveOnScope && target.go == hit.collider.gameObject) {
						if (target.go.activeInHierarchy) {
							target.touchEvent.OnTouchMove (new Vector3[] { pos, hit.point });
//							target.go.SendMessage("OnTouchMove", new Vector3[] { pos, hit.point });
						}
					}
				}
			}
			for (int i = 0; i < targets.Count; i++) {
				Target target = targets[i];
				if (!target.moveOnScope) {
					if (target.go) {
						if (target.go.activeInHierarchy) {
							target.touchEvent.OnTouchMove (new Vector3[] { pos, hit.point });
//							target.go.SendMessage("OnTouchMove", new Vector3[] { pos, hit.point });
						}
					}
				}
			}
		}

		void OnUp(Vector3 pos) {
			if (!Camera.main) {
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(pos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				for (int i = 0; i < targets.Count; i++) {
					Target target = targets[i];
					if (target.upOnScope && target.go == hit.collider.gameObject) {
						if (target.go.activeInHierarchy) {
							target.touchEvent.OnTouchUp (new Vector3[] { pos, hit.point });
//							target.go.SendMessage("OnTouchUp", new Vector3[] { pos, hit.point });
						}
					}
				}
			}
			for (int i = 0; i < targets.Count; i++) {
				Target target = targets[i];
				if (!target.upOnScope) {
					if (target.go) {
						if (target.go.activeInHierarchy) {
							target.touchEvent.OnTouchUp (new Vector3[] { pos, hit.point });
//							target.go.SendMessage("OnTouchUp", new Vector3[] { pos, hit.point });
						}
					}
				}
			}
		}

		//Add
		public void AddListen(GameObject go, bool downOnScope, bool moveOnScope, bool upOnScope) {
			if (go) {
				int count = targets.Count;
				for (int i = 0; i < count; i++) {
					Target target = targets[i];
					if (target.go == go) {
						return;
					}
				}
				Target newTarget = new Target();
				newTarget.go = go;
				newTarget.touchEvent = go.GetComponent<ITouchRayEvent> ();
				newTarget.downOnScope = downOnScope;
				newTarget.moveOnScope = moveOnScope;
				newTarget.upOnScope = upOnScope;
				targets.Add(newTarget);
			}
		}


		//Remove
		public void RemoveListen(GameObject go) {
			if (go) {
				int count = targets.Count;
				for (int i = 0; i < count; i++) {
					Target target = targets[i];
					if (target.go == go) {
						targets.RemoveAt(i);
						return;
					}
				}
			}
		}

		//Clear
		public void ClearEventListener(TouchType TouchEventType) {
			targets.Clear();
		}

	}

}
