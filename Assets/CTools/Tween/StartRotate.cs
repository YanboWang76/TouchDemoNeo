using UnityEngine;
using System.Collections;
namespace CTool.Tween
{
	public class StartRotate : MonoBehaviour{

		public CTween.EndHandler EndEvent;
		CTween cTween;
		Vector3 toAngle;
		Transform trans;
		OneAxis xAxis=new OneAxis();
		OneAxis yAxis=new OneAxis();
		OneAxis zAxis=new OneAxis();
		bool useWorldPos;
		float t,d;
		class OneAxis{
			public float b,c;
			public bool canGo;
		}

		void OnDestroy(){
			EndEvent=null;
		}

		void Awake(){
			trans=transform;
		}

		void Update(){
			if(cTween==null){
				return;
			}
			Vector3 vector=useWorldPos?trans.eulerAngles:trans.localEulerAngles;
			if(xAxis.canGo){
				vector.x=(float)cTween.Ease(t,xAxis.b,xAxis.c,d);
			}
			if(yAxis.canGo){
				vector.y=(float)cTween.Ease(t,yAxis.b,yAxis.c,d);
			}
			if(zAxis.canGo){
				vector.z=(float)cTween.Ease(t,zAxis.b,zAxis.c,d);
			}
			if(useWorldPos){
				trans.eulerAngles=vector;
			} else{
				trans.localEulerAngles=vector;
			}
			if(t<d){
				t+=Time.deltaTime;
			} else{
				if(useWorldPos){
					if(!xAxis.canGo){
						toAngle.x = trans.eulerAngles.x;
					}
					if(!yAxis.canGo){
						toAngle.y = trans.eulerAngles.y;
					}
					if(!zAxis.canGo){
						toAngle.z = trans.eulerAngles.z;
					}
					trans.eulerAngles=toAngle;
				} else{
					if(!xAxis.canGo){
						toAngle.x = trans.localEulerAngles.x;
					}
					if(!yAxis.canGo){
						toAngle.y = trans.localEulerAngles.y;
					}
					if(!zAxis.canGo){
						toAngle.z = trans.localEulerAngles.z;
					}
					trans.localEulerAngles=toAngle;
				}
				this.enabled=false;
				if(EndEvent!=null){
					EndEvent();
				}
			}
		}

		public void Rotate(Vector3 toAngle,float time,bool useWorldPos,CTween.EaseType method,CTween.EndHandler endEvent,bool xEnable,bool yEnable,bool zEnable){
			this.useWorldPos=useWorldPos;
			if(time<=0f){
				if(useWorldPos){
					transform.eulerAngles=toAngle;
				} else{
					if(!xEnable){
						toAngle.x = transform.localEulerAngles.x;
					}
					if(!yEnable){
						toAngle.y = transform.localEulerAngles.y;
					}
					if(!zEnable){
						toAngle.z = transform.localEulerAngles.z;
					}
					transform.localEulerAngles=toAngle;
				}
				return;
			}
			t=0f;
			d=time;
			Vector3 transPos=useWorldPos?transform.eulerAngles:transform.localEulerAngles;
			xAxis.canGo=xEnable?(toAngle.x!=transPos.x):xEnable;
			yAxis.canGo=yEnable?(toAngle.y!=transPos.y):yEnable;
			zAxis.canGo=zEnable?(toAngle.z!=transPos.z):zEnable;
			if(xAxis.canGo){
				xAxis.b=transPos.x;
				xAxis.c=toAngle.x-transPos.x;
			}
			if(yAxis.canGo){
				yAxis.b=transPos.y;
				yAxis.c=toAngle.y-transPos.y;
			}
			if(zAxis.canGo){
				zAxis.b=transPos.z;
				zAxis.c=toAngle.z-transPos.z;
			}
			this.toAngle=toAngle;
			switch(method){
			case CTween.EaseType.Linear:
				cTween=new LinearEase();
				break;

			case CTween.EaseType.ExpoEaseIn:
				cTween=new ExpoEaseIn();
				break;
			case CTween.EaseType.ExpoEaseOut:
				cTween=new ExpoEaseOut();
				break;
			case CTween.EaseType.ExpoEaseInOut:
				cTween=new ExpoEaseInOut();
				break;

			case CTween.EaseType.SineEaseIn:
				cTween = new SineEaseIn();
				break;
			case CTween.EaseType.SineEaseOut:
				cTween = new SineEaseOut();
				break;
			case CTween.EaseType.SineEaseInOut:
				cTween = new SineEaseInOut();
				break;

			case CTween.EaseType.ElasticEaseIn:
				cTween=new ElasticEaseIn();
				break;
			case CTween.EaseType.ElasticEaseOut:
				cTween=new ElasticEaseOut();
				break;
			case CTween.EaseType.ElasticEaseInOut:
				cTween=new ElasticEaseInOut();
				break;

			case CTween.EaseType.BackEaseIn:
				cTween=new BackEaseIn();
				break;
			case CTween.EaseType.BackEaseOut:
				cTween=new BackEaseOut();
				break;
			case CTween.EaseType.BackEaseInOut:
				cTween=new BackEaseInOut();
				break;

			case CTween.EaseType.BounceEaseIn:
				cTween=new BounceEaseIn();
				break;
			case CTween.EaseType.BounceEaseOut:
				cTween=new BounceEaseOut();
				break;
			case CTween.EaseType.BounceEaseInOut:
				cTween=new BounceEaseInOut();
				break;

			default:
				cTween=new SineEaseOut();
				break;
			}
			this.enabled=true;
			this.EndEvent=endEvent;
		}

		public void StopRotate(){
			cTween=null;
			this.enabled=false;
		}
	}

}
