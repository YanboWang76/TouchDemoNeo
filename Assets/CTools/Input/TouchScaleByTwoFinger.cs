using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace CTool.InputInteraction
{
	public class TouchScaleByTwoFinger : MonoBehaviour {

		public bool canRotate = true, canMove = true;
		public float moveRatio = 1f;
		public float moveLimitL = -500f;
		public float moveLimitR = 500f;
		public float moveLimitT = 500f;
		public float moveLimitB = -500f;
		public float scaleRatio = 0.002f;
		public float scaleMin = 0.1f;
		public float scaleMax = 3f;
		RectTransform rectTrans;
		Transform trans;
		Vector3 previousPosRecord;
		float previousDis, previousAngle;
		float ratio;

		void Awake() {
			rectTrans = GetComponent<RectTransform>();
			trans = transform;
		}

		void Start() {
			CanvasScaler canvasScaler = GameObject.FindObjectOfType<CanvasScaler>();
			ratio = canvasScaler.referenceResolution.y / (float)Screen.height;
		}

		void Update() {
			if (Input.touchCount == 1) {
				if (Input.GetTouch(0).phase == TouchPhase.Ended) {
					previousDis = 0f;
					previousAngle = 0f;
					previousPosRecord = Vector3.zero;
				}
			} else if (Input.touchCount >= 2) {
				//缩放
				if (previousDis == 0f) {
					previousDis = CMath.GetDistanceXY(Input.GetTouch(0).position, Input.GetTouch(1).position);
				} else {
					float currDis = CMath.GetDistanceXY(Input.GetTouch(0).position, Input.GetTouch(1).position);
					float deltaScale = (currDis - previousDis) * scaleRatio * ratio;
					previousDis = currDis;
					trans.localScale += new Vector3(deltaScale, deltaScale, deltaScale);
					if (trans.localScale.x < scaleMin) {
						trans.localScale = Vector3.one * scaleMin;
					} else if (trans.localScale.x > scaleMax) {
						trans.localScale = Vector3.one * scaleMax;
					}
					if (Input.GetTouch(1).phase == TouchPhase.Ended) {
						previousDis = 0f;
					}
				}
				//旋转
				if (canRotate) {
					if (previousAngle == 0) {
						previousAngle = CMath.GetAngle2D(Input.GetTouch(0).position, Input.GetTouch(1).position);
					} else {
						float currAngle = CMath.GetAngle2D(Input.GetTouch(0).position, Input.GetTouch(1).position);
						float deltaAngle = previousAngle - currAngle;
						previousAngle = currAngle;
						Vector3 angle = trans.localEulerAngles;
						angle.z -= deltaAngle;
						trans.localEulerAngles = angle;
						if (Input.GetTouch(1).phase == TouchPhase.Ended) {
							previousAngle = 0f;
						}
					}
				}
				//移动
				if (canMove) {
					if (previousPosRecord == Vector3.zero) {
						previousPosRecord = Input.mousePosition;
					} else {
						Vector3 moveDelta = (Input.mousePosition - previousPosRecord) * moveRatio * ratio;
						previousPosRecord = Input.mousePosition;
						Vector3 pos = trans.localPosition;
						if (rectTrans) {
							pos = rectTrans.anchoredPosition;
						}
						pos.x += moveDelta.x;
						pos.y += moveDelta.y;
						if (pos.x < moveLimitL) {
							pos.x = moveLimitL;
						} else if (pos.x > moveLimitR) {
							pos.x = moveLimitR;
						}
						if (pos.y < moveLimitB) {
							pos.y = moveLimitB;
						} else if (pos.y > moveLimitT) {
							pos.y = moveLimitT;
						}
						if (rectTrans) {
							rectTrans.anchoredPosition = new Vector2(pos.x, pos.y);
						} else {
							trans.localPosition = pos;
						}
						if (Input.GetTouch(1).phase == TouchPhase.Ended) {
							previousPosRecord = Vector3.zero;
						}
					}
				}
			}
		}

	}

}
