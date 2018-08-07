using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace CTool.Tween
{
	public class StartAlpha : MonoBehaviour{
		public CTween.EndHandler EndEvent;
		CTween cTween;
		SpriteRenderer[] spriteRenderers = new SpriteRenderer[0];
		Image[] images = new Image[0];
		RawImage[] rawImages = new RawImage[0];
		Text[] texts = new Text[0];
		float t = 0, d = 0, b = 0, c = 0;
		float toAlpha, time;
		bool isRunAfterAwake, destroySelfWhenFinish;
		CTween.EaseType method;

		void OnDestroy(){
			EndEvent = null;
		}

		void Awake(){
			spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
			images = GetComponentsInChildren<Image>();
			rawImages = GetComponentsInChildren<RawImage>();
			texts = GetComponentsInChildren<Text>();
			if(isRunAfterAwake) {
				Alpha(toAlpha, time, method, EndEvent, destroySelfWhenFinish);
			}
		}

		void Update(){
			if(cTween == null) {
				return;
			}
			float alpha = (float)cTween.Ease(t, b, c, d);
			SetSpriteRendererAlpha(alpha);
			SetImageAlpha(alpha);
			SetRawImageAlpha(alpha);
			SetTextAlpha(alpha);
			if(t < d) {
				t += Time.deltaTime;
			} else {
				SetSpriteRendererAlpha(toAlpha);
				SetImageAlpha(toAlpha);
				SetRawImageAlpha(toAlpha);
				SetTextAlpha(toAlpha);
				this.enabled = false;
				if(EndEvent != null) {
					EndEvent();
				}
				if(destroySelfWhenFinish) {
					Destroy(gameObject);
				}
			}
		}

		void SetSpriteRendererAlpha(float a){
			for(int i = 0; i < spriteRenderers.Length; i++) {
				Color color = spriteRenderers[i].color;
				color.a = a;
				spriteRenderers[i].color = color;
			}
		}

		void SetImageAlpha(float a){
			for(int i = 0; i < images.Length; i++) {
				if(images[i]) {
					Color color = images[i].color;
					color.a = a;
					images[i].color = color;
				}
			}
		}

		void SetRawImageAlpha(float a){
			for(int i = 0; i < rawImages.Length; i++) {
				if(rawImages[i]) {
					Color color = rawImages[i].color;
					color.a = a;
					rawImages[i].color = color;
				}
			}
		}

		void SetTextAlpha(float a){
			for(int i = 0; i < texts.Length; i++) {
				Color color = texts[i].color;
				color.a = a;
				texts[i].color = color;
			}
		}

		public void Alpha(float toAlpha, float time, CTween.EaseType method, CTween.EndHandler endEvent, bool destroySelfWhenFinish){
			this.toAlpha = toAlpha;
			this.time = time;
			this.method = method;
			t = 0f;
			isRunAfterAwake = false;
			if(spriteRenderers.Length > 0) {
				b = spriteRenderers[0].color.a;
			}
			if(images.Length > 0) {
				b = images[0].color.a;
			}
			if(rawImages.Length > 0) {
				b = rawImages[0].color.a;
			}
			if(texts.Length > 0) {
				b = texts[0].color.a;
			}
			if(time == 0) {
				SetSpriteRendererAlpha(toAlpha);
				SetImageAlpha(toAlpha);
				SetTextAlpha(toAlpha);
			} else {
				c = toAlpha - b;
				d = time;
				this.EndEvent = endEvent;
				this.destroySelfWhenFinish = destroySelfWhenFinish;
				switch(method) {
				case CTween.EaseType.Linear:
					cTween = new LinearEase();
					break;

				case CTween.EaseType.ExpoEaseIn:
					cTween = new ExpoEaseIn();
					break;
				case CTween.EaseType.ExpoEaseOut:
					cTween = new ExpoEaseOut();
					break;
				case CTween.EaseType.ExpoEaseInOut:
					cTween = new ExpoEaseInOut();
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
					cTween = new ElasticEaseIn();
					break;
				case CTween.EaseType.ElasticEaseOut:
					cTween = new ElasticEaseOut();
					break;
				case CTween.EaseType.ElasticEaseInOut:
					cTween = new ElasticEaseInOut();
					break;

				case CTween.EaseType.BackEaseIn:
					cTween = new BackEaseIn();
					break;
				case CTween.EaseType.BackEaseOut:
					cTween = new BackEaseOut();
					break;
				case CTween.EaseType.BackEaseInOut:
					cTween = new BackEaseInOut();
					break;

				case CTween.EaseType.BounceEaseIn:
					cTween = new BounceEaseIn();
					break;
				case CTween.EaseType.BounceEaseOut:
					cTween = new BounceEaseOut();
					break;
				case CTween.EaseType.BounceEaseInOut:
					cTween = new BounceEaseInOut();
					break;

				default:
					cTween = new SineEaseOut();
					break;
				}
				this.enabled = true;
			}
			if(spriteRenderers.Length == 0 && images.Length == 0 && rawImages.Length == 0 && texts.Length == 0) {
				isRunAfterAwake = true;
			}
		}
	}
}
