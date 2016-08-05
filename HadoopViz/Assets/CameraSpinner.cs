using System;
using UnityEngine;
using System.Collections;

public class CameraSpinner : MonoBehaviour{

	Camera cam;
	bool autoRotate = true;
	float scaleSpeed = 1f;
	float turnSpeedAuto = 4f;
	float turnSpeedManual = 40f;

	void Awake(){
		cam = GetComponentInChildren<Camera> ();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			autoRotate = !autoRotate;
		}
		if(autoRotate){
			transform.Rotate (new Vector3(0,Time.deltaTime * turnSpeedAuto,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.Rotate (new Vector3(0,Time.deltaTime * turnSpeedManual,0));
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			transform.Rotate (new Vector3(0,-Time.deltaTime * turnSpeedManual,0));
		}


		if(Input.GetKey(KeyCode.DownArrow)){
			cam.orthographicSize += Time.deltaTime * scaleSpeed;
		}
		if(Input.GetKey(KeyCode.UpArrow)){
			cam.orthographicSize -= Time.deltaTime * scaleSpeed;
		}
	}
}

