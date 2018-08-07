using UnityEngine;
using System.Collections;
namespace CTool.Tween
{
	public class StartMoveUIAnchored : MonoBehaviour {

		public CTween.EndHandler EndEvent;
		CTween cTween;
		Vector2 toPos;
		RectTransform trans;
		OneAxis xAxis=new OneAxis();
		OneAxis yAxis=new OneAxis();
		float t,d;
		class OneAxis{
			public float b,c;
			public bool canGo;
		}

		void OnDestroy(){
			EndEvent=null;
		}

		void Awake(){
			trans = GetComponent<RectTransform>();
		}

		void Update(){
			if(cTween==null){
				return;
			}
			Vector3 vector=trans.anchoredPosition;
			if(xAxis.canGo){
				vector.x=(float)cTween.Ease(t,xAxis.b,xAxis.c,d);
			}
			if(yAxis.canGo){
				vector.y=(float)cTween.Ease(t,yAxis.b,yAxis.c,d);
			}
			trans.anchoredPosition = vector;
			if(t<d){
				t+=Time.deltaTime;
			} else{
				if (!xAxis.canGo) {
					toPos.x = trans.anchoredPosition.x;
				}
				if (!yAxis.canGo) {
					toPos.y = trans.anchoredPosition.y;
				}
				trans.anchoredPosition = toPos;
				this.enabled=false;
				if(EndEvent!=null){
					EndEvent();
				}
			}
		}

		public void Move(Vector2 toPos,float time,CTween.EaseType method,CTween.EndHandler endEvent,bool xEnable,bool yEnable){
			if (!trans) {
				trans = GetComponent<RectTransform>();
			}
			if(time<=0f){
				if (!xEnable) {
					toPos.x = trans.anchoredPosition.x;
				}
				if (!yEnable) {
					toPos.y = trans.anchoredPosition.y;
				}
				trans.anchoredPosition = toPos;
				return;
			}
			t=0f;
			d=time;
			Vector2 transPos = trans.anchoredPosition;
			xAxis.canGo=xEnable?(toPos.x!=transPos.x):xEnable;
			yAxis.canGo=yEnable?(toPos.y!=transPos.y):yEnable;
			if(xAxis.canGo){
				xAxis.b=transPos.x;
				xAxis.c=toPos.x-transPos.x;
			}
			if(yAxis.canGo){
				yAxis.b=transPos.y;
				yAxis.c=toPos.y-transPos.y;
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
