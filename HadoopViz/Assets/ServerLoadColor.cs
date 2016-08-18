using System;
using UnityEngine;
using System.Collections;

public class ServerLoadColor : MonoBehaviour{

	Material mat;
	float incAmt = 0.05f;
	float reducAmt = 0.08f;
	float heightLimit = 10f;
	float alpha = 1.0f;
	//int mode = 1;

	float heightCount = 0;

	void Start(){
		mat = GetComponent<Renderer>().material;
		mat.color = new Color(0,1,0,alpha);
		InvokeRepeating ("ReduceColor", 1,1);
	}
	/*
	void Update(){
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			mode = 1;
			mat.color = Color.green;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			mode = 2;
			mat.color = Color.white;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			mode = 3;
			mat.color = Color.white;
		}
	}
*/
	void OnParticleCollision(GameObject particalSystem){
		IncColor ();
	}

	public void IncColor(){
		//if(mode == 1){
		if (mat.color.r + incAmt < 1) {
			mat.color = new Color (mat.color.r + incAmt, mat.color.g, 0, alpha);
		} else if (mat.color.g - incAmt > 0) {
			mat.color = new Color (1, mat.color.g - incAmt, 0, alpha);
		} else {
			mat.color = new Color(1,0,0,alpha);
		}
		ChangeHeight (incAmt);
		/*}else if(mode == 2){
			if(mat.color.g - incAmt > 0){
				mat.color = new Color (1,mat.color.g - incAmt,mat.color.g - incAmt,1);
			}
		}else if(mode == 3){
			if (mat.color.b - incAmt > 0) {
				mat.color = new Color (1, mat.color.g, mat.color.b - incAmt, 1);
			} else if (mat.color.g - incAmt > 0) {
				mat.color = new Color (1, mat.color.g - incAmt, 0, 1);
			} else {
				mat.color = Color.red;
			}
		}*/
	}

	void ReduceColor(){

		//if(mode == 1){
			if (mat.color.r == 1) {
				if (mat.color.g + reducAmt < 1) {
				mat.color = new Color (1, mat.color.g + reducAmt, 0, alpha);
				} else {
				mat.color = new Color (mat.color.r - reducAmt, 1, 0, alpha);
				}
			} else if (mat.color.r - reducAmt > 0) {
				mat.color = new Color (mat.color.r - reducAmt, 1, 0, alpha);
			} else {
			mat.color = new Color(0,1,0,alpha);
			}
		/*}else if(mode == 2){
			if(mat.color.g + incAmt < 1){
				mat.color = new Color (1,mat.color.g + incAmt,mat.color.g + incAmt,1);
			}
		}else if(mode ==3){
			if (mat.color.b == 0) {
				if (mat.color.g + reducAmt < 1) {
					mat.color = new Color (1, mat.color.g + reducAmt, 0, 1);
				} else {
					mat.color = new Color (1, 1, mat.color.b + reducAmt, 1);
				}
			} else if (mat.color.b + reducAmt < 1) {
				mat.color = new Color (1, 1, mat.color.b + reducAmt, 1);
			} else {
				mat.color = Color.white;
			}
		}*/

		ChangeHeight (-reducAmt);
	}

	void ChangeHeight (float amt){
		if(amt + heightCount >= 0 && amt + heightCount <= 2){
			heightCount += amt;
			transform.position = new Vector3 (transform.position.x,heightCount/heightLimit,transform.position.z);
		}
	}

}