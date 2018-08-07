using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTool.Tween;

public class SmoothSwitchScene : MonoBehaviour {

    //private Vector3 _distance;
    //private Vector3 _direction;
    // Use this for initialization
    private float _distance;
    private float _direction;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private GameObject _headset;
    [SerializeField]
    private float _responseDistance;
    [SerializeField]
    private float _responseDirection;

    private bool _onGoing;

	void Start ()
    {
        _onGoing = false;
        
        //Setting default values
        if(_camera == null)
            _camera = GameObject.Find("Camera (head)");
        if(_headset == null)
            _headset = GameObject.Find("vr_headset_prefab");
        if (_responseDirection == 0)
            _responseDirection = 0.92f;
            //about +- 45 degrees
        if (_responseDistance == 0)
            _responseDistance = 0.3f;
    }

    // Update is called once per frame
    void Update () {

        //_distance = Vector3.Distance(_camera.transform.position, _headset.transform.position);
        //_direction = Quaternion.Dot(_camera.transform.rotation, _headset.transform.rotation);
        if(_direction>_responseDirection &&_direction<=1 && _distance <=_responseDistance && !_onGoing)
        {
            Debug.Log("success");
            _headset.transform.DoMove(_camera.transform.position, 1, true);
            _onGoing = true;
            StartCoroutine(WaitAndSwitchScene(1.5F));
            //_headset.transform.DoMove(_camera.transform.position, 1, true, SwitchScene);
        }
        Debug.Log("dis"+_distance);
        Debug.Log("dir"+_direction);
        
    }

    IEnumerator WaitAndSwitchScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Manager.Instance.GameSceneManager.LoadBombExpertScene();
        //等待之后执行的动作  
        //transform.DOMove(pos, 1);
        //transform.DORotate(rot.eulerAngles, 1);
        // transform.position = pos;
        //transform.rotation = rot; 
    }
}
