using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WebCamera : MonoBehaviour{

	public RawImage image;
	WebCamTexture cameraTexture;
	Texture imageDefaultTexture;
	Color imageDefaultColor = Color.white;

	public void StartCamera(){
		if(!cameraTexture) {
			StartCoroutine(StartCameraIEnumerator());
		}
	}

	public void StopCamera(bool clearPhoto){
		if(cameraTexture) {
			cameraTexture.Stop();
			cameraTexture = null;
			if(clearPhoto) {
				image.texture = imageDefaultTexture;
				image.color = imageDefaultColor;
			}
		}
	}

	public bool IsPlaying {
		get {
			return cameraTexture != null;
		}
	}
	
	IEnumerator StartCameraIEnumerator(){
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
		if(!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
			Debug.LogError("摄影机权限被禁用");
		} else {
			WebCamDevice[] devices = WebCamTexture.devices;
			string cameraName = devices[0].name;
			cameraTexture = new WebCamTexture(cameraName, 1024, 600, 15);
			cameraTexture.Play();
			imageDefaultTexture = image.texture;
			imageDefaultColor = image.color;
			image.color = Color.white;
			image.texture = cameraTexture;
		}
	} 
}
