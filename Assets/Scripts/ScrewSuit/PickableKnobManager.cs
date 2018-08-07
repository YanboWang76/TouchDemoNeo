using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableKnobManager : MonoBehaviour {

	private bool ispickable = true;
	private bool isknob = false;

	public Collider knobcollider;
    public Collider pickablecollider;

	public Transform palmcenter;
	public Transform snappablereference;
	// Use this for initialization
	void Start () {
		knobcollider.enabled = false;
        pickablecollider.enabled = true;
	}

	void Update()
	{
		if (palmcenter.position == snappablereference.position)
		{
			if (ispickable && !isknob)
			{
				PickableToKnob();
			}
		}

		else
		{
			if (!ispickable && isknob)
			{
				KnobToPickable();
			}
		}
	}

	void PickableToKnob()
	{
        pickablecollider.enabled = false;
        knobcollider.enabled = true;
		ispickable = false;
		isknob = true;
	}

	void KnobToPickable()
	{
        pickablecollider.enabled = true;
        knobcollider.enabled = false;
		ispickable = true;
		isknob = false;
	}
}
