
using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class DataRetriever : MonoBehaviour
{
	public GameObject serverPrefab;

	string fileNameRed = "redHosts.xml";
	string urlRed = "129.93.239.169";
	int portRed = 8651;

	string urlViz = "129.93.244.225";
	int portViz = 5679;

	int xCord;
	int zCord;

	void Start ()
	{
		StartCoroutine (CreateServers ());
		StartCoroutine (TCPconnectDrops (urlViz, portViz));
	}

	/*
	void Update(){
		if(Input.GetKeyDown(KeyCode.W)){
			GameObject[] servers = GameObject.FindGameObjectsWithTag ("Server");
			string serverNames = "";
			foreach(GameObject server in servers){
				serverNames += server.name + "\n";
			}
			File.WriteAllText ("serverNames.txt", serverNames);
		}
	}
	*/

	IEnumerator CreateServers(){
		string serverListText = File.ReadAllText ("serverNames.txt");
		string[] serverNameArray = serverListText.Split ('\n');
		foreach(string server in serverNameArray){
			SpawnServer (server);
			yield return null;
		}
	}

	IEnumerator CreateServersFromXML(){
		//File.WriteAllText (fileNameRed, TCPconnectRed());

		XmlTextReader reader = null;
		try{
			reader = new XmlTextReader (fileNameRed);
		}catch(FileNotFoundException fnfe){
			//File.AppendAllText("errorLog.txt", "\n" + fnfe.ToString() + "\n");
			UnityEngine.Debug.Log(fnfe.ToString());
		}
		if (reader != null && reader.ReadToFollowing ("CLUSTER")) {
			if (reader.ReadToDescendant ("HOST")) {
				do {
					string hostName = reader.GetAttribute ("NAME");
					SpawnServer(hostName);
					yield return null;
				} while (reader.ReadToNextSibling("HOST"));
			}
		}
		if(reader != null){
			reader.Close ();
		}
	}

	void UpdateCords(){
		if(xCord < 17){
			xCord++;
		}else{
			zCord++;
			xCord = 0;
		}
	}

	void SpawnServer(string serverName){
		if (GameObject.Find (serverName) == null) {
			GameObject server = Instantiate (serverPrefab, new Vector3 (xCord * 0.6f, 0, zCord * 0.6f), Quaternion.identity) as GameObject;
			server.name = serverName;
			UpdateCords ();
		}
	}

	string TCPconnectRed(){

		TcpClient tcp = new TcpClient (AddressFamily.InterNetwork);
		tcp.Connect (IPAddress.Parse (urlRed), portRed);

		StreamReader data = new StreamReader (tcp.GetStream ());

		string output = data.ReadToEnd ();

		data.Close ();
		tcp.Close ();

		return output;
	}

	IEnumerator TCPconnectDrops (string url, int port){

		TcpClient tcp = new TcpClient (AddressFamily.InterNetwork);
		while (true) {
			int numTimesFailedToConnect = 0;
			while (!tcp.Connected) {
				try {
					tcp.Connect (IPAddress.Parse (url), port);
				} catch (SocketException se) {
					//File.AppendAllText("errorLog.txt", "\n" + se.ToString() + "\n");
					numTimesFailedToConnect++;
					UnityEngine.Debug.Log (se.ToString () + "\n Failed To Connect "+ numTimesFailedToConnect +" times.");
				}
				yield return new WaitForSeconds (10);
			}

			StreamReader data = new StreamReader (tcp.GetStream ());
			while (data.EndOfStream == false) {
				try {
					string line = data.ReadLine ();
					string[] dataIO = ParseData (line);
					GameObject.Find (dataIO [1]).GetComponentInChildren<DropMovement> ().SendDrop (GameObject.Find (dataIO [2]).transform.FindChild ("Drops"));
				}catch(Exception e){
					UnityEngine.Debug.Log (e.ToString ());
				}
				yield return null;
			}

			data.Close ();
			tcp.Close ();
			yield return new WaitForSeconds (5);
		}
	}

	string[] ParseData(string line){
		string[] part = line.Split (new char[]{' '},4);
		//clienttrace red1 red2 12341234
		//UnityEngine.Debug.Log (part[0] + ", " + part[1] + ", " + part[2] + ", " + part[3]);
		if(part[0].Equals("clienttrace")||part[0].Equals("recvblock")){
			SpawnServer (part[1]);
			SpawnServer (part[2]);
		}
		return part;
	}

}
