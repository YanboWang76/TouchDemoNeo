using UnityEngine;
using System.Collections;
namespace CTool.Tween
{
	public class StartParabola : CMonoBehaviour {

		public CTween.EndHandler CollisionEvent, EndEvent;
		Vector3 fromPos, heightPos, toPos;
		Transform trans;
		float height, delta, t, elasticity;
		bool useWorldPos, canBounce;

		void OnDestroy() {
			EndEvent = null;
		}

		void Awake() {
			trans = transform;
		}

		protected override void CUpdate() {
			base.CUpdate();
			Vector3 pos = useWorldPos ? trans.position : trans.localPosition;
			t += delta * Time.deltaTime;
			if (t > 1f) {
				t = 1f;
			}
			pos = CMath.GetBezierPoint(fromPos, heightPos, toPos, t);
			if (useWorldPos) {
				trans.position = pos;
			} else {
				trans.localPosition = pos;
			}
			if (t == 1f) {
				if (CollisionEvent != null) {
					CollisionEvent();
				}
				if (canBounce) {
					Vector3 nextPos = toPos + (toPos - fromPos) * elasticity;
					float nextHeight = height * elasticity;
					float nextDelta = delta / elasticity;
					Go(nextPos, nextHeight, nextDelta, useWorldPos, canBounce, elasticity, null, EndEvent);
					if (height < 0.1f) {
						if (EndEvent != null) {
							EndEvent();
						}
						this.enabled = false;
					}
				} else {
					if (EndEvent != null) {
						EndEvent();
					}
					this.enabled = false;
				}
			}
		}

		public void Go(Vector3 toPos, float height, float delta, bool useWorldPos, bool canBounce, float elasticity, CTween.EndHandler CollisionEvent, CTween.EndHandler EndEvent) {
			this.useWorldPos = useWorldPos;
			this.canBounce = canBounce;
			this.fromPos = useWorldPos ? transform.position : transform.localPosition;
			this.toPos = toPos;
			this.height = height;
			this.heightPos = new Vector3((fromPos.x + toPos.x) * 0.5f, fromPos.y + height, (fromPos.z + toPos.z) * 0.5f);
			this.delta = delta;
			this.elasticity = elasticity;
			this.enabled = true;
			this.CollisionEvent = CollisionEvent;
			this.EndEvent = EndEvent;
			t = 0f;
		}

		public void StopMove() {
			this.enabled = false;
		}
	}

}
