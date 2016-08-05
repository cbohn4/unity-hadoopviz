using System;
using UnityEngine;
using System.Collections;

public class ServerLoadColor : MonoBehaviour{

	Material mat;
	float incAmt = 0.01f;
	float reducAmt = 0.016f;

	void Start(){
		mat = GetComponent<Renderer>().material;
		mat.color = Color.white;
		InvokeRepeating ("ReduceColor", 1,1);
	}

	void OnParticleCollision(GameObject particalSystem){
		if(mat.color.g - incAmt > 0){
			mat.color = new Color(1,mat.color.g - incAmt,mat.color.b - incAmt,1);
		}
	}

	void ReduceColor(){
		if (mat.color.g + reducAmt < 1) {
			mat.color = new Color(1,mat.color.g + reducAmt,mat.color.b + reducAmt,1);
		}
	}

}