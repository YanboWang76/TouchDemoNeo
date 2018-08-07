using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CTool.InputInteraction;

public class CameraLookAround : MonoBehaviour, ITouchRayEvent {

    public Transform target;
    public float distane = 30;
    public bool canCtrlHeightAngle = true;
    public float currHeightAngle = 15;
    public bool heightAngleLimit;
    public float heightAngleMin = 0;
    public float heightAngleMax = 90;
    public bool autoRotate;
    public float autoRotateSpeed = 0.2f;
    Transform trans;
    Vector3 previousPos;
    float r, speedXScale = 0.01f, speedYScale;
    bool isDraging;

    void Awake() {
        trans = transform;
        r = -Mathf.PI * 0.5f;
        CanvasScaler canvasScaler = GameObject.FindObjectOfType<CanvasScaler>();
        if (canvasScaler) {
            speedXScale = 0.006f * canvasScaler.referenceResolution.y / (float)Screen.height;
        }
        speedYScale = speedXScale * 50f;
    }

    void Start() {
        StartRun();
    }

    void OnDestroy() {
		TouchRayEvent.GetInstance().RemoveListen(gameObject);
    }

    internal void StartRun() {
		TouchRayEvent.GetInstance().AddListen(gameObject, false, false, false);
    }

    void Update() {
        if (target) {
            if (autoRotate) {
                r += autoRotateSpeed * Time.deltaTime;
            }
            Vector3 pos = new Vector3(target.position.x + Mathf.Cos(r) * distane, 10f, target.position.z + Mathf.Sin(r) * distane);
            trans.position = pos;
            Vector3 axis = Vector3.Cross(Vector3.up, target.position - trans.position);
            if (heightAngleLimit) {
                if (currHeightAngle < heightAngleMin) {
                    currHeightAngle = heightAngleMin;
                } else if (currHeightAngle > heightAngleMax) {
                    currHeightAngle = heightAngleMax;
                }
            }
            trans.RotateAround(target.position, axis, currHeightAngle);
            trans.LookAt(target);
        }
    }

	public void OnTouchDown(Vector3[] pos) {
        if (target) {
            isDraging = true;
            previousPos = pos[0];
        }
    }

	public void OnTouchMove(Vector3[] pos) {
        if (target && isDraging) {
            float deltaX = (pos[0].x - previousPos.x) * speedXScale;
            r -= deltaX;
            if (canCtrlHeightAngle) {
                float deltaY = (pos[0].y - previousPos.y) * speedYScale;
                currHeightAngle -= deltaY;
            }
            previousPos = pos[0];
        }
    }

	public void OnTouchUp(Vector3[] pos)
	{
        isDraging = false;
    }
}
