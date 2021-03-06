using System;
using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour{
	int height = 768;
	int width = 1024;

	void Update(){
		//close the app
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit ();
		}

		//toggle full screen
		if(Input.GetKeyDown(KeyCode.F11)){
			if(!Screen.fullScreen){
				height = Screen.height;
				width = Screen.width;
				Screen.SetResolution (1920,1080,true);
			}
			else{
				Screen.SetResolution (width,height,false);
			}
		}
	}
}