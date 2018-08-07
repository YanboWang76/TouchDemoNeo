using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Libdexmo.Unity.Touchables;

public class ComponentsManager : MonoBehaviour {

	public event EventHandler<StringInt> BoomAndOver;

	[HideInInspector]
	public bool putOutFire = false;

	[SerializeField]
	private SnappableWithEvent RedSlot;
	[SerializeField]
	private SnappableWithEvent GreenSlot;
	[SerializeField]
	private SnappableWithEvent BlueSlot;

	[SerializeField]
	private GameObject RedComponent;
	[SerializeField]
	private GameObject GreenComponent;
	[SerializeField]
	private GameObject BlueComponent;

	[SerializeField]
	private GameObject campFire;
	[SerializeField]
	private GameObject fireExplosion;
	[SerializeField]
	private TimerController timerController;
	[SerializeField]
	private FireExtinguish fireExtinguish;

	private bool enable = false;
    private bool fireStart = false;
	private bool explosionStart = false;

    // Use this for initialization
    void Start () 
	{
        RedSlot.SlotIn += CheckIfAllInPosition;
        GreenSlot.SlotIn += CheckIfAllInPosition;
        BlueSlot.SlotIn += CheckIfAllInPosition;

		timerController.timeOver += Boom;
		fireExtinguish.extinguishsucceed += PutOutFire;

		//Manager.Instance.AudioManager.PlayTiktokAudio ();

		StartCoroutine (DelayStart ());
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

	//If a snapper is snapped on a slot, this function will be called and check if all the components are on position.
	private void CheckIfAllInPosition(object sender, EventArgs e)
    {
		if (RedSlot.snapper == null || GreenSlot.snapper == null || BlueSlot.snapper == null || !enable)
			return;

		if (RedSlot.snapper.root.name.Equals (RedComponent.gameObject.name) &&
		    GreenSlot.snapper.root.name.Equals (GreenComponent.gameObject.name) &&
		    BlueSlot.snapper.root.name.Equals (BlueComponent.gameObject.name)) 
		{
			if (!fireStart) {
                Debug.Log("ComponentsInPosition");
				timerController.enabled = false;
				StartCoroutine (WaitAndLightFire (5.0f));
				fireStart = true;
				//Maybe destory some components in slots and colorcomponents
			}
		} 
		else 
		{
			if (!explosionStart) {
				Boom (this, new EventArgs ());
				explosionStart = true;
			}
		}
    }
	
	private IEnumerator WaitAndLightFire(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Instantiate(campFire, transform.position, transform.rotation);
		StartCoroutine (FireToBoom(60));
	}

	private IEnumerator FireToBoom(float fireTime)
	{
		yield return new WaitForSeconds(fireTime);
		if (!putOutFire)
			Boom (this, new EventArgs ());
		
		Debug.Log ("explosionStartInFireToBoom");
	}

	private void Boom(object sender, EventArgs e)
	{
		Instantiate (fireExplosion, transform.position, transform.rotation);
		BoomAndOver (this, new StringInt ("Boom!", -100));
	}

	private IEnumerator DelayStart()
	{
		yield return new WaitForSeconds (1);
		enable = true;
	}

	private void PutOutFire(object sender, StringInt str)
	{
		putOutFire = true;
		Destroy (GetComponent<TouchableStatic> ());
	}

}