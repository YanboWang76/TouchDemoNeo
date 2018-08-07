using UnityEngine;
using System.Collections;
namespace CTool.Tween
{
	public class StartSwing : CMonoBehaviour {

		public CTween.EndHandler EndEvent;
		Vector3 defaultAngle, addAngle;
		float r, speed;
		float swingTime;
		Transform trans;
		float offsetAngle;

		void Awake() {
			trans = transform;
			defaultAngle = trans.localEulerAngles;
		}

		protected override void CUpdate() {
			base.CUpdate();
			if (swingTime > 0f) {
				swingTime -= Time.deltaTime;
				r += speed * Time.deltaTime;
				addAngle.z = Mathf.Sin(r) * offsetAngle * swingTime;
				trans.localEulerAngles = defaultAngle + addAngle;
				if (swingTime <= 0f) {
					trans.localEulerAngles = defaultAngle;
					this.enabled = false;
					if (EndEvent != null) {
						EndEvent();
					}
				}
			}
		}

		public void Swing(float offsetAngle, float speed, float time, CTween.EndHandler endEvent) {
			if (time == 0f) {
				return;
			}
			this.r = Random.Range(0f, Mathf.PI * 2f);
			this.offsetAngle = offsetAngle / time;
			this.speed = speed;
			swingTime = time;
			this.enabled = true;
			this.EndEvent = endEvent;
		}
	}

}
