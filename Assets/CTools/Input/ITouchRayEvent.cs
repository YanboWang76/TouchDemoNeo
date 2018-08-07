using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchRayEvent  {
	void OnTouchDown (Vector3[] vector);
	void OnTouchMove (Vector3[] vector);
	void OnTouchUp   (Vector3[] vector);
}
