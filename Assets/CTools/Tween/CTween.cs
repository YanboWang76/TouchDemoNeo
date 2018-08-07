using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CTool.Tween
{
	public static class CTweener
	{
		//开始移动物体
		public static void DoMove(this Transform trans, Vector3 toPos, bool useWorldPos) {
			DoMove(trans, toPos, 0f, useWorldPos, CTween.EaseType.Linear, null);
		}

		public static void DoMove(this Transform trans, Vector3 toPos, float time, bool useWorldPos) {
			DoMove(trans, toPos, time, useWorldPos, CTween.EaseType.ExpoEaseOut, null);
		}

		public static void DoMove(this Transform trans, Vector3 toPos, float time, bool useWorldPos, CTween.EaseType method) {
			DoMove(trans, toPos, time, useWorldPos, method, null);
		}

		public static void DoMove(this Transform trans, Vector3 toPos, float time, bool useWorldPos, CTween.EndHandler endEvent) {
			DoMove(trans, toPos, time, useWorldPos, CTween.EaseType.ExpoEaseOut, endEvent);
		}

		public static void DoMove(this Transform trans, Vector3 toPos, float time, bool useWorldPos, CTween.EaseType method, CTween.EndHandler endEvent) {
			DoMove(trans, toPos, time, useWorldPos, method, endEvent, true, true, true);
		}

		public static void DoMove(this Transform trans, Vector3 toPos, float time, bool useWorldPos, CTween.EaseType method, CTween.EndHandler endEvent, bool xEnable, bool yEnable, bool zEnable) {
			StartMove move = trans.GetComponent<StartMove>();
			if (!move) {
				move = trans.gameObject.AddComponent<StartMove>();
			}
			move.Move(toPos, time, useWorldPos, method, endEvent, xEnable, yEnable, zEnable);
		}

		public static void StopMove(this Transform trans) {
			StartMove move = trans.GetComponent<StartMove>();
			if (!move) {
				move = trans.gameObject.AddComponent<StartMove>();
			}
			move.StopMove();
		}

		public static void DoMoveUIAnchored(this Transform trans, Vector2 toPos) {
			DoMoveUIAnchored(trans, toPos, 0f, CTween.EaseType.ExpoEaseOut, null);
		}

		public static void DoMoveUIAnchored(this Transform trans, Vector2 toPos, float time) {
			DoMoveUIAnchored(trans, toPos, time, CTween.EaseType.ExpoEaseOut, null);
		}

		public static void DoMoveUIAnchored(this Transform trans, Vector2 toPos, float time, CTween.EaseType method) {
			DoMoveUIAnchored(trans, toPos, time, method, null);
		}

		public static void DoMoveUIAnchored(Transform trans, Vector2 toPos, float time, CTween.EndHandler endEvent) {
			DoMoveUIAnchored(trans, toPos, time, CTween.EaseType.ExpoEaseOut, endEvent);
		}

		public static void DoMoveUIAnchored(this Transform trans, Vector2 toPos, float time, CTween.EaseType method, CTween.EndHandler endEvent) {
			DoMoveUIAnchored(trans, toPos, time, method, endEvent, true, true);
		}

		public static void DoMoveUIAnchored(this Transform trans, Vector2 toPos, float time, CTween.EaseType method, CTween.EndHandler endEvent, bool xEnable, bool yEnable) {
			StartMoveUIAnchored move = trans.GetComponent<StartMoveUIAnchored>();
			if (!move) {
				move = trans.gameObject.AddComponent<StartMoveUIAnchored>();
			}
			move.Move(toPos, time, method, endEvent, xEnable, yEnable);
		}

		public static void StopMoveUIAnchored(this Transform trans) {
			StartMoveUIAnchored move = trans.GetComponent<StartMoveUIAnchored>();
			if (!move) {
				move = trans.gameObject.AddComponent<StartMoveUIAnchored>();
			}
			move.StopMove();
		}

		//抛物线运动
		public static void DoParabola(this Transform trans, Vector3 toPos, float height, float delta) {
			DoParabola(trans, toPos, height, delta, false, false, 0.3f, null, null);
		}
		public static void DoParabola(this Transform trans, Vector3 toPos, float height, float delta, bool useWorldPos) {
			DoParabola(trans, toPos, height, delta, useWorldPos, false, 0.3f, null, null);
		}
		public static void DoParabola(this Transform trans, Vector3 toPos, float height, float delta, bool useWorldPos, bool isBounce) {
			DoParabola(trans, toPos, height, delta, useWorldPos, isBounce, 0.3f, null, null);
		}
		public static void DoParabola(this Transform trans, Vector3 toPos, float height, float delta, bool useWorldPos, bool isBounce, float elasticity) {
			DoParabola(trans, toPos, height, delta, useWorldPos, isBounce, elasticity, null, null);
		}
		public static void DoParabola(this Transform trans, Vector3 toPos, float height, float delta, bool useWorldPos, bool isBounce, float elasticity, CTween.EndHandler CollisionEvent) {
			DoParabola(trans, toPos, height, delta, useWorldPos, isBounce, elasticity, CollisionEvent, null);
		}
		public static void DoParabola(this Transform trans, Vector3 toPos, float height, float delta, bool useWorldPos, bool isBounce, float elasticity, CTween.EndHandler CollisionEvent, CTween.EndHandler EndEvent) {
			StartParabola parabola = trans.GetComponent<StartParabola>();
			if (!parabola) {
				parabola = trans.gameObject.AddComponent<StartParabola>();
			}
			parabola.Go(toPos, height, delta, useWorldPos, isBounce, elasticity, CollisionEvent, EndEvent);
		}

		//开始缩放到指定大小
		public static void DoScale(this Transform trans, Vector3 toScale) {
			trans.localScale = toScale;
		}

		public static void DoScale(this Transform trans, Vector3 toScale, float time) {
			DoScale(trans, toScale, time, CTween.EaseType.Linear, null);
		}

		public static void DoScale(this Transform trans, Vector3 toScale, float time, CTween.EaseType method) {
			DoScale(trans, toScale, time, method, null);
		}

		public static void DoScale(this Transform trans, Vector3 toScale, float time, CTween.EndHandler endEvent) {
			DoScale(trans, toScale, time, CTween.EaseType.Linear, endEvent);
		}

		public static void DoScale(this Transform trans, Vector3 toScale, float time, CTween.EaseType method, CTween.EndHandler endEvent) {
			StartScale scale = trans.GetComponent<StartScale>();
			if (!scale) {
				scale = trans.gameObject.AddComponent<StartScale>();
			}
			scale.Scale(toScale, time, method, endEvent);
		}

		public static void StopScale(this Transform trans) {
			StartScale scale = trans.GetComponent<StartScale>();
			if (!scale) {
				scale = trans.gameObject.AddComponent<StartScale>();
			}
			scale.StopScale();
		}

		//开始就近旋转物体
		public static void DoRotate(this Transform trans, Vector3 toPos, bool useWorldAngle) {
			DoRotate(trans, toPos, 0f, useWorldAngle, CTween.EaseType.Linear, null);
		}

		public static void DoRotate(this Transform trans, Vector3 toPos, float time, bool useWorldAngle) {
			DoRotate(trans, toPos, time, useWorldAngle, CTween.EaseType.Linear, null);
		}

		public static void DoRotate(this Transform trans, Vector3 toPos, float time, bool useWorldAngle, CTween.EaseType method) {
			DoRotate(trans, toPos, time, useWorldAngle, method, null);
		}

		public static void DoRotate(this Transform trans, Vector3 toPos, float time, bool useWorldAngle, CTween.EndHandler endEvent) {
			DoRotate(trans, toPos, time, useWorldAngle, CTween.EaseType.Linear, endEvent);
		}

		public static void DoRotate(this Transform trans, Vector3 toPos, float time, bool useWorldAngle, CTween.EaseType method, CTween.EndHandler endEvent) {
			DoRotate(trans, toPos, time, useWorldAngle, method, endEvent, true, true, true);
		}

		public static void DoRotate(this Transform trans, Vector3 toAngle, float time, bool useWorldAngle, CTween.EaseType method, CTween.EndHandler endEvent, bool xEnable, bool yEnable, bool zEnable) {
			StartRotate rotate = trans.GetComponent<StartRotate>();
			if (!rotate) {
				rotate = trans.gameObject.AddComponent<StartRotate>();
			}
			rotate.Rotate(toAngle, time, useWorldAngle, method, endEvent, xEnable, yEnable, zEnable);
		}

		//开始渐变到指定透明度
		public static void DoAlpha(this Transform trans, float toAlpha) {
			DoAlpha(trans, toAlpha, 0f, CTween.EaseType.Linear, null, false);
		}

		public static void DoAlpha(this Transform trans, float toAlpha, float time) {
			DoAlpha(trans, toAlpha, time, CTween.EaseType.Linear, null, false);
		}

		public static void DoAlpha(this Transform trans, float toAlpha, float time, CTween.EaseType method) {
			DoAlpha(trans, toAlpha, time, method, null, false);
		}

		public static void DoAlpha(this Transform trans, float toAlpha, float time, CTween.EndHandler endEvent) {
			DoAlpha(trans, toAlpha, time, CTween.EaseType.Linear, endEvent, false);
		}

		public static void DoAlpha(this Transform trans, float toAlpha, float time, CTween.EaseType method, CTween.EndHandler endEvent) {
			DoAlpha(trans, toAlpha, time, method, endEvent, false);
		}

		public static void DoAlpha(this Transform trans, float toAlpha, float time, CTween.EaseType method, CTween.EndHandler endEvent, bool destroySelfWhenFinish) {
			StartAlpha alpha = trans.GetComponent<StartAlpha>();
			if (!alpha) {
				alpha = trans.gameObject.AddComponent<StartAlpha>();
			}
			alpha.Alpha(toAlpha, time, method, endEvent, destroySelfWhenFinish);
		}

		//开始抖动物体
		public static void DoShake(this Transform trans, float offset, float time) {
			DoShake(trans, offset, time, true, null);
		}

		public static void DoShake(this Transform trans, float offset, float time, CTween.EndHandler endEvent) {
			DoShake(trans, offset, time, true, endEvent);
		}

		public static void DoShake(this Transform trans, float offset, float time, bool useScale) {
			DoShake(trans, offset, time, useScale, null);
		}

		public static void DoShake(this Transform trans, float offset, float time, bool useScale, CTween.EndHandler endEvent) {
			StartShake shake = trans.GetComponent<StartShake>();
			if (!shake) {
				shake = trans.gameObject.AddComponent<StartShake>();
			}
			shake.Shake(offset, time, useScale, endEvent);
		}

		//开始摇摆物体
		public static void DoSwing(this Transform trans, float offsetAngle, float speed, float time) {
			DoSwing(trans, offsetAngle, speed, time, null);
		}

		public static void DoSwing(this Transform trans, float offsetAngle, float speed, float time, CTween.EndHandler endEvent) {
			StartSwing swing = trans.GetComponent<StartSwing>();
			if (!swing) {
				swing = trans.gameObject.AddComponent<StartSwing>();
			}
			swing.Swing(offsetAngle, speed, time, endEvent);
		}
	}

	public class CTween  {
		public delegate void EndHandler();

		public enum EaseType {
			Linear,
			ExpoEaseIn,
			ExpoEaseOut,
			ExpoEaseInOut,
			SineEaseIn,
			SineEaseOut,
			SineEaseInOut,
			ElasticEaseIn,
			ElasticEaseOut,
			ElasticEaseInOut,
			BackEaseIn,
			BackEaseOut,
			BackEaseInOut,
			BounceEaseIn,
			BounceEaseOut,
			BounceEaseInOut
		}



		public double Linear(double t, double b, double c, double d) {
			return c * t / d + b;
		}

		class Quad {
			public static double EaseIn(double t, double b, double c, double d) {
				return c * (t /= d) * t + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				return -c * (t /= d) * (t - 2) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if ((t /= d / 2) < 1) {
					return c / 2 * t * t + b;
				}
				return -c / 2 * ((--t) * (t - 2) - 1) + b;
			}
		}

		public class Cubic {
			public static double EaseIn(double t, double b, double c, double d) {
				return c * (t /= d) * t * t + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				return c * ((t = t / d - 1) * t * t + 1) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if ((t /= d / 2) < 1) {
					return c / 2 * t * t * t + b;
				}
				return c / 2 * ((t -= 2) * t * t + 2) + b;
			}
		}

		public class Quart {
			public static double EaseIn(double t, double b, double c, double d) {
				return c * (t /= d) * t * t * t + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				return -c * ((t = t / d - 1) * t * t * t - 1) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if ((t /= d / 2) < 1) {
					return c / 2 * t * t * t * t + b;
				}
				return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
			}
		}

		public class Quint {
			public static double EaseIn(double t, double b, double c, double d) {
				return c * (t /= d) * t * t * t * t + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if ((t /= d / 2) < 1) {
					return c / 2 * t * t * t * t * t + b;
				}
				return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
			}
		}

		public class Sine {
			public static double EaseIn(double t, double b, double c, double d) {
				return -c * Math.Cos(t / d * (Math.PI / 2)) + c + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				return c * Math.Sin(t / d * (Math.PI / 2)) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				return -c / 2 * (Math.Cos(Math.PI * t / d) - 1) + b;
			}
		}

		public class Expo {
			public static double EaseIn(double t, double b, double c, double d) {
				return (t == 0) ? b : c * Math.Pow(2, 10 * (t / d - 1)) + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				return (t == d) ? b + c : c * (-Math.Pow(2, -10 * t / d) + 1) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if (t == 0) {
					return b;
				}
				if (t == d) {
					return b + c;
				}
				if ((t /= d / 2) < 1) {
					return c / 2 * Math.Pow(2, 10 * (t - 1)) + b;
				}
				return c / 2 * (-Math.Pow(2, -10 * --t) + 2) + b;
			}
		}

		public class Circ {
			public static double EaseIn(double t, double b, double c, double d) {
				return -c * (Math.Sqrt(1 - (t /= d) * t) - 1) + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				return c * Math.Sqrt(1 - (t = t / d - 1) * t) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if ((t /= d / 2) < 1) {
					return -c / 2 * (Math.Sqrt(1 - t * t) - 1) + b;
				}
				return c / 2 * (Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
			}
		}

		public class Elastic {
			public static double EaseIn(double t, double b, double c, double d) {
				if (t == 0) {
					return b;
				}
				if ((t /= d) == 1) {
					return b + c;
				}
				double p = d * .3;
				return -(c * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - p / 4) * (2 * Math.PI) / p)) + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				if (t == 0) {
					return b;
				}
				if ((t /= d) == 1) {
					return b + c;
				}
				double p = d * .3;
				return (c * Math.Pow(2, -10 * t) * Math.Sin((t * d - p / 4) * (2 * Math.PI) / p) + c + b);
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if (t == 0) {
					return b;
				}
				if ((t /= d / 2) == 2) {
					return b + c;
				}
				double p = d * (.3 * 1.5);
				if (t < 1) {
					return -.5 * (c * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - p / 4) * (2 * Math.PI) / p)) + b;
				}
				return c * Math.Pow(2, -10 * (t -= 1)) * Math.Sin((t * d - p / 4) * (2 * Math.PI) / p) * .5 + c + b;
			}
		}

		public class Back {
			public static double EaseIn(double t, double b, double c, double d) {
				double s = 1.2;
				return c * (t /= d) * t * ((s + 1) * t - s) + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				double s = 1.2;
				return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				double s = 1.2;
				if ((t /= d / 2) < 1) {
					return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
				}
				return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
			}
		}

		public class Bounce {
			public static double EaseIn(double t, double b, double c, double d) {
				return c - EaseOut(d - t, 0, c, d) + b;
			}

			public static double EaseOut(double t, double b, double c, double d) {
				if ((t /= d) < (1 / 2.75)) {
					return c * (7.5625 * t * t) + b;
				} else if (t < (2 / 2.75)) {
					return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
				} else if (t < (2.5 / 2.75)) {
					return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
				} else {
					return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
				}
			}

			public static double EaseInOut(double t, double b, double c, double d) {
				if (t < d / 2) {
					return EaseIn(t * 2, 0, c, d) * .5 + b;
				} else {
					return EaseOut(t * 2 - d, 0, c, d) * .5 + c * .5 + b;
				}
			}
		}

		public virtual double Ease(double t, double b, double c, double d) {
			return 0;
		}
	}

	//Linear
	public class LinearEase : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Linear(t, b, c, d);
		}
	}
	//Expo
	public class ExpoEaseIn : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Expo.EaseIn(t, b, c, d);
		}
	}

	public class ExpoEaseOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Expo.EaseOut(t, b, c, d);
		}
	}

	public class ExpoEaseInOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Expo.EaseInOut(t, b, c, d);
		}
	}
	//Sine
	public class SineEaseIn : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Sine.EaseIn(t, b, c, d);
		}
	}

	public class SineEaseOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Sine.EaseOut(t, b, c, d);
		}
	}

	public class SineEaseInOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Sine.EaseInOut(t, b, c, d);
		}
	}
	//Elastic
	public class ElasticEaseIn : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Elastic.EaseIn(t, b, c, d);
		}
	}

	public class ElasticEaseOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Elastic.EaseOut(t, b, c, d);
		}
	}

	public class ElasticEaseInOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Elastic.EaseInOut(t, b, c, d);
		}
	}
	//Back
	public class BackEaseIn : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Back.EaseIn(t, b, c, d);
		}
	}

	public class BackEaseOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Back.EaseOut(t, b, c, d);
		}
	}

	public class BackEaseInOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Back.EaseInOut(t, b, c, d);
		}
	}
	//Bounce
	public class BounceEaseIn : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Bounce.EaseIn(t, b, c, d);
		}
	}

	public class BounceEaseOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Bounce.EaseOut(t, b, c, d);
		}
	}

	public class BounceEaseInOut : CTween {
		public override double Ease(double t, double b, double c, double d) {
			return Bounce.EaseInOut(t, b, c, d);
		}
	}


}
