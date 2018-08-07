using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherController : MonoBehaviour {

    [SerializeField]
	private ParticleSystem whiteSmoke;
	[SerializeField]
	private FireExtinguish fireextinguish;

	// Use this for initialization
	void Start () {
		whiteSmoke.Stop();
		fireextinguish.enabled = false;
	}
	        
    private void OnTriggerStay(Collider other)
    {
		if (other.gameObject.layer.Equals(12))
        {
			whiteSmoke.Play();
			fireextinguish.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.layer.Equals(12) && whiteSmoke.isPlaying)
        {
			whiteSmoke.Stop();
			fireextinguish.enabled = false;
        }
    }
}
