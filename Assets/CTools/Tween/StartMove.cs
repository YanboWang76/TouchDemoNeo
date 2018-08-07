using UnityEngine;
using System.Collections;
namespace CTool.Tween
{
	public class StartMove : MonoBehaviour{

		public CTween.EndHandler EndEvent;
		CTween cTween;
		Vector3 toPos;
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
			Vector3 vector=useWorldPos?trans.position:trans.localPosition;
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
				trans.position=vector;
			} else{
				trans.localPosition=vector;
			}
			if(t<d){
				t+=Time.deltaTime;
			} else{
				if(useWorldPos){
					if(!xAxis.canGo){
						toPos.x = trans.position.x;
					}
					if(!yAxis.canGo){
						toPos.y = trans.position.y;
					}
					if(!zAxis.canGo){
						toPos.z = trans.position.z;
					}
					trans.position=toPos;
				} else{
					if(!xAxis.canGo){
						toPos.x = trans.localPosition.x;
					}
					if(!yAxis.canGo){
						toPos.y = trans.localPosition.y;
					}
					if(!zAxis.canGo){
						toPos.z = trans.localPosition.z;
					}
					trans.localPosition=toPos;
				}
				this.enabled=false;
				if(EndEvent!=null){
					EndEvent();
				}
			}
		}

		public void Move(Vector3 toPos,float time,bool useWorldPos,CTween.EaseType method,CTween.EndHandler endEvent,bool xEnable,bool yEnable,bool zEnable){
			this.useWorldPos=useWorldPos;
			if(time<=0f){
				if(useWorldPos){
					transform.position=toPos;
				} else{
					if(!xEnable){
						toPos.x = transform.localPosition.x;
					}
					if(!yEnable){
						toPos.y = transform.localPosition.y;
					}
					if(!zEnable){
						toPos.z = transform.localPosition.z;
					}
					transform.localPosition=toPos;
				}
				return;
			}
			t=0f;
			d=time;
			Vector3 transPos=useWorldPos?transform.position:transform.localPosition;
			xAxis.canGo=xEnable?(toPos.x!=transPos.x):xEnable;
			yAxis.canGo=yEnable?(toPos.y!=transPos.y):yEnable;
			zAxis.canGo=zEnable?(toPos.z!=transPos.z):zEnable;
			if(xAxis.canGo){
				xAxis.b=transPos.x;
				xAxis.c=toPos.x-transPos.x;
			}
			if(yAxis.canGo){
				yAxis.b=transPos.y;
				yAxis.c=toPos.y-transPos.y;
			}
			if(zAxis.canGo){
				zAxis.b=transPos.z;
				zAxis.c=toPos.z-transPos.z;
			}
			this.toPos=toPos;
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

		public void StopMove(){
			cTween=null;
			this.enabled=false;
		}
	}

}
