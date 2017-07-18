﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetAndGC : MonoBehaviour {

	void Awake(){
		DataRetriever dr = GameObject.Find ("Data Retriever").GetComponent<DataRetriever> ();
		if(dr.useVR == false){
			Invoke ("ReloadScene", 86400f);
			InvokeRepeating ("GarbageCollect", 3f, 30f);
		}
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.R)) {
			if (IsInvoking ("ReloadScene")) {
				CancelInvoke ("ReloadScene");
			}
			ReloadScene ();
		}
	}

	void ReloadScene(){
		Scene current = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (current.name);
	}

	void GarbageCollect(){
		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
	}


}
