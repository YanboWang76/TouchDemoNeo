using UnityEngine;
using System.Collections;
namespace CTool.Tween
{
	public class StartScale : MonoBehaviour{

		public CTween.EndHandler EndEvent;
		CTween cTween;
		Vector3 toScale;
		Transform trans;
		OneAxis xAxis=new OneAxis();
		OneAxis yAxis=new OneAxis();
		OneAxis zAxis=new OneAxis();
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
			Vector3 vector=trans.localScale;
			if(xAxis.canGo){
				vector.x=(float)cTween.Ease(t,xAxis.b,xAxis.c,d);
			}
			if(yAxis.canGo){
				vector.y=(float)cTween.Ease(t,yAxis.b,yAxis.c,d);
			}
			if(zAxis.canGo){
				vector.z=(float)cTween.Ease(t,zAxis.b,zAxis.c,d);
			}
			trans.localScale=vector;
			if(t<d){
				t+=Time.deltaTime;
			} else{
				trans.localScale=toScale;
				this.enabled=false;
				if(EndEvent!=null){
					EndEvent();
				}
			}
		}

		public void Scale(Vector3 toScale,float time,CTween.EaseType method,CTween.EndHandler endEvent){
			t=0f;
			d=time;
			xAxis.canGo=(toScale.x!=transform.localScale.x);
			yAxis.canGo=(toScale.y!=transform.localScale.y);
			zAxis.canGo=(toScale.z!=transform.localScale.z);
			if(xAxis.canGo){
				xAxis.b=transform.localScale.x;
				xAxis.c=toScale.x-transform.localScale.x;
			}
			if(yAxis.canGo){
				yAxis.b=transform.localScale.y;
				yAxis.c=toScale.y-transform.localScale.y;
			}
			if(zAxis.canGo){
				zAxis.b=transform.localScale.z;
				zAxis.c=toScale.z-transform.localScale.z;
			}
			this.toScale=toScale;
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

		public void StopScale(){
			cTween=null;
			this.enabled=false;
		}
	}

}
