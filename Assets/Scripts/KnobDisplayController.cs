using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnobDisplayController : MonoBehaviour {

    // Use this for initialization

    private float _knobTurningAngle;
    private int _previousDisplay;

    private GameObject _knobController;
    private GameObject _knobDisplayText;

	void Start () {
        _knobTurningAngle = 0;
        _previousDisplay = 0;
        _knobController = GameObject.Find("KnobController");
        _knobDisplayText = GameObject.Find("KnobDisplayText");

    }
	
	// Update is called once per frame
	void Update () {
        AngleDisplayUpdate();
    }

    private void AngleDisplayUpdate()
    {
        _knobTurningAngle = _knobController.GetComponent<Transform>().localEulerAngles.y;
        //Debug.Log("y:" + _knobTurningAngle);
        PlayOnTurning();
        _knobDisplayText.GetComponent<Text>().text = Mathf.Round(_knobTurningAngle).ToString();
    }

    private void PlayOnTurning()
    {
        if((int)Mathf.Round(_knobTurningAngle)!=_previousDisplay)
        {
            Manager.Instance.AudioManager.PlayKnobAudio();
            _previousDisplay = (int)Mathf.Round(_knobTurningAngle);
        }
    }

    public int GetKnobDisplay()
    {
        return (int)Mathf.Round(_knobTurningAngle);
    }
}
