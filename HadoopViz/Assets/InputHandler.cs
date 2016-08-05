using System;
using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour{
	int height = 768;
	int width = 1024;

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit ();
		}

		if(Input.GetKeyDown(KeyCode.F11)){
			if(!Screen.fullScreen){
				height = Screen.height;
				width = Screen.width;
				Screen.SetResolution (Screen.resolutions[Screen.resolutions.Length-1].width,Screen.resolutions[Screen.resolutions.Length-1].height,true);
			}
			else{
				Screen.SetResolution (width,height,false);
			}
		}
	}
}

