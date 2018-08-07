using UnityEngine;
using System.Collections;
namespace CTool.Tween
{
	public class StartShake : CMonoBehaviour{

		public CTween.EndHandler EndEvent;
		Vector3 defaultPos,defaultScale;
		float shakerTimer;
		Transform trans;
		Vector3 shakerOffset;
		float offset;
		bool useScale;

		void Awake(){
			trans=transform;
			defaultPos=trans.localPosition;
		}

		protected override void CUpdate(){
			base.CUpdate();
			if(shakerTimer>0f){
				shakerTimer-=Time.deltaTime;
				shakerOffset=new Vector3(Random.Range(-offset,offset),Random.Range(-offset,offset),Random.Range(-offset,offset));
				trans.localPosition=defaultPos+shakerOffset;
				if(shakerTimer<=0f){
					trans.localPosition=defaultPos;
					this.enabled=false;
					if(EndEvent!=null){
						EndEvent();
					}
					if(useScale){
						transform.DoScale (defaultScale, 0.2f);
					}
				}
			}
		}

		public void Shake(float offset,float time,bool useScale,CTween.EndHandler endEvent){
			this.offset=offset*Screen.height*0.002f;
			shakerTimer=time;
			this.useScale=useScale;
			this.enabled=true;
			this.EndEvent=endEvent;
			if(useScale){
				if(defaultScale==Vector3.zero){
					defaultScale=transform.localScale;
				}
				transform.DoScale (defaultScale * 1.2f, 0.1f);
			}
		}


		public void StopShake(){
			shakerTimer=0f;
			trans.localPosition=defaultPos;
			this.enabled=false;
			if(useScale){
				transform.DoScale (defaultScale);
			}
		}
	}

}
