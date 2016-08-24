/*
 * Click to raycast onto server and desplay its data.
*/

using UnityEngine;
using System.Collections;

public class ServerSelector : MonoBehaviour {

	Ray ray;
	RaycastHit hit;
	bool showGUI = false;
	GameObject selectedServer;
	Vector3 serverPosistion;
	float windowSizeX;
	float windowSizeY;
	int screenSizeX;
	int screenSizeY;
	int lineSize = 20;

	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(selectedServer != null){
				selectedServer.GetComponent<ServerLoadColor> ().ToggleSelected (0);
			}
			screenSizeX = Camera.main.pixelWidth;
			windowSizeX = 140f;
			screenSizeY = Camera.main.pixelHeight;
			windowSizeY = lineSize;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			//Debug.DrawRay (ray.origin, ray.direction*300, Color.yellow);

			if(Physics.Raycast(ray.origin, ray.direction, out hit) && hit.collider.tag.Equals("Server")){
				selectedServer = hit.collider.gameObject;
				selectedServer.GetComponent<ServerLoadColor> ().ToggleSelected (1f);
				showGUI = true;
			}else{
				showGUI = false;
			}
		}
		if(Input.GetMouseButtonDown(1)){
			showGUI = false;
		}

	}

	void OnGUI(){
		if(showGUI){
			//make gui follow server
			serverPosistion = Camera.main.WorldToScreenPoint(selectedServer.transform.position);

			float x1 = serverPosistion.x;
			float y1 = screenSizeY-serverPosistion.y;

			if(serverPosistion.x + windowSizeX >= screenSizeX){
				x1 = screenSizeX - windowSizeX;
			}
			if(serverPosistion.y - windowSizeY <= 0){
				y1 = screenSizeY - windowSizeY;
			}

			GUI.Window(0,new Rect(x1,y1,windowSizeX,windowSizeY), GUIdata, "");
		}
	}

	void GUIdata(int windowID){
		//data to display
		GUI.contentColor = selectedServer.GetComponent<ServerLoadColor> ().GetMatColor();
		GUI.Label (new Rect(5,0,windowSizeX-10,lineSize), selectedServer.name);
	}

}
