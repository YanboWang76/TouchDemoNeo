using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CTool.Tween
{
	public class CTweenMoveOnPath :MonoBehaviour {

		public CTweenPath path;
		public float angleRate = 300f;
		public float threshold = 0.5f;
		public bool forward = true,loop = true;
		Transform trans;
		int currNodeIndex;
		Vector3 lastPosForAngle;
		Quaternion targetRotation;
		float lastDis;
		bool canMove = true;
		public delegate void DelegateMove();
		private DelegateMove OnMoveEnd = null;

		void Awake() {
			trans = transform;
			Init();
		}

		internal void Init() {
			if (path) {
				currNodeIndex = -1;
				canMove = true;
			}
			lastPosForAngle = trans.position;
		}

		internal void Move(float speed) {
			if (canMove) {
				if (path) {
					trans.position = Vector3.MoveTowards(trans.position,path.nodes[currNodeIndex + 1].position,speed * Time.deltaTime);
					if (forward) {
						AngleForward();
					}
					if (currNodeIndex < path.nodeSize - 1) {
						float dis = Vector3.Distance(trans.position,path.nodes[currNodeIndex + 1].position);
						if (dis <= threshold || dis > lastDis) {
							currNodeIndex++;
							if (currNodeIndex < path.nodeSize - 1) {
								dis = Vector3.Distance(trans.position,path.nodes[currNodeIndex + 1].position);
							} else {
								if (OnMoveEnd != null) {
									OnMoveEnd();
								}
								if (loop) {
									currNodeIndex = -1;
								} else {
									canMove = false;
								}
							}
						}
						lastDis = dis;
					}
				}
			}
		}
		void AngleForward() {
			if (trans.position != lastPosForAngle) {
				targetRotation = Quaternion.LookRotation(trans.position - lastPosForAngle);
				trans.rotation = Quaternion.RotateTowards(trans.rotation,targetRotation,angleRate * Time.deltaTime);
				lastPosForAngle = trans.position;
			}
		}

		internal void Move2(float speed) {
			if (canMove) {
				if (path) {
					trans.Translate(Vector3.forward * speed * Time.deltaTime);
					if (currNodeIndex < path.nodeSize - 1) {
						Quaternion transRotation = Quaternion.LookRotation(path.nodes[currNodeIndex + 1].position - trans.position);
						trans.rotation = Quaternion.RotateTowards(trans.rotation,transRotation,speed * angleRate * Time.deltaTime);
						float dis = Vector3.Distance(trans.position,path.nodes[currNodeIndex + 1].position);
						if (dis <= threshold || dis > lastDis) {
							currNodeIndex++;
							if (currNodeIndex < path.nodeSize - 1) {
								dis = Vector3.Distance(trans.position,path.nodes[currNodeIndex + 1].position);
							} else {
								if (OnMoveEnd != null) {
									OnMoveEnd();
								}
								if (loop) {
									currNodeIndex = -1;
								} else {
									canMove = false;
								}
							}
						}
						lastDis = dis;
					}
				}
			}
		}

		internal void RegisterMoveEnd(DelegateMove OnMoveEnd) {
			this.OnMoveEnd = OnMoveEnd;
		}

	}
}
