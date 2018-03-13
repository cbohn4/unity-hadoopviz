
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class DataRetriever : MonoBehaviour
{
	public GameObject serverPrefab;
	public GameObject gridPrefab;

	string fileNameRed = "redHosts.xml";
	string urlRed = "129.93.239.169";
	int portRed = 8651;

	string urlViz = "129.93.244.225";
	int portViz = 5679;

	int xCord;
	int zCord;

	float mbps;

	public bool useVR;

	public Text textMBPS;

	void Start ()
	{
		StartCoroutine (CreateServers ());
		StartCoroutine (TCPconnectDrops (urlViz, portViz));
		InvokeRepeating ("ResetMBPS",1,5);
	}

	//updates the data transfer rate to be displayed
	void ResetMBPS(){
		mbps = mbps/100000;
		mbps = mbps/5;
		textMBPS.text = Mathf.FloorToInt(mbps).ToString();
		//UnityEngine.Debug.Log (mbps + " mb/s");
		mbps = 0;
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

	//uses the list of known servers to create the base population of server representation
	IEnumerator CreateServers(){
		string serverListText = File.ReadAllText ("serverNames.txt");
		string[] serverNameArray = serverListText.Split ('\n');
		foreach(string server in serverNameArray){
			SpawnServer (server);
			yield return null;
		}
	}

	//creates servers based off of the XML file from the fish visulization
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

	//function used to handle how long the rows and collums should be
	void UpdateCords(){
		if(xCord < 17){
			xCord++;
		}else{
			zCord++;
			xCord = 0;
		}
		if (useVR && (xCord <= 11 && xCord >= 6)&&(zCord <= 10 && zCord >= 7)) {
			UpdateCords ();
		}
	}

	int xGrid = 0;
	//creates a server from prefab
	void SpawnServer(string serverName){
		if (GameObject.Find (serverName) == null) {
			if (serverName.Contains ("red-gridftp")) {
				GameObject server = Instantiate (gridPrefab, new Vector3 (xGrid * 0.6f, 0, -1.2f), Quaternion.identity) as GameObject;
				server.name = serverName;
				xGrid++;
			} else {
				GameObject server = Instantiate (serverPrefab, new Vector3 (xCord * 0.6f, 0, zCord * 0.6f), Quaternion.identity) as GameObject;
				server.name = serverName;
				UpdateCords ();
			}
		}
	}

	//if you need the current RedXML file, this will return it as a string
	string TCPconnectRed(){

		TcpClient tcpR = new TcpClient (AddressFamily.InterNetwork);
		tcpR.Connect (IPAddress.Parse (urlRed), portRed);

		StreamReader dataR = new StreamReader (tcpR.GetStream ());

		string output = dataR.ReadToEnd ();

		dataR.Close ();
		tcpR.Close ();

		return output;
	}

	//gets the tcp for data transfers, and spawns corresponding drops
	TcpClient tcp;
	StreamReader data;
	private static readonly Queue<string> queueOfInputs = new Queue<string>();

	IEnumerator TCPconnectDrops (string url, int port){

		tcp = new TcpClient (AddressFamily.InterNetwork);
		while (!tcp.Connected) {
			try{
				tcp.Connect (IPAddress.Parse (url), port);
			}catch{
				print("Failed to Connect to Hadoop.");
			}
			yield return new WaitForSeconds (10);
		}
		data = new StreamReader (tcp.GetStream ());

		var inputThread = new Thread (new ThreadStart(ReadIncomingData));
		inputThread.Start ();
	}

	void Update(){
		lock (queueOfInputs) {
			while (queueOfInputs.Count > 0) {
				string line = queueOfInputs.Dequeue();
				StartCoroutine (UseData (line));
			}
		}	
	}

	public void Enqueue(string line) {
		lock (queueOfInputs) {
			queueOfInputs.Enqueue (line);
		}
	}

	void ReadIncomingData(){
		while (true) {
			string line = data.ReadLine ();
			Enqueue (line);
		}
	}

	IEnumerator UseData(string line){
		string[] dataIO = line.Split (new char[]{' '},4);
		if (GameObject.Find (dataIO [1]) != null && GameObject.Find (dataIO [2]) != null ) {
			int loadSize = Int32.Parse(dataIO[3]);
			mbps += Int32.Parse(dataIO[3]);
			do{
				GameObject.Find (dataIO [1]).GetComponentInChildren<DropMovement> ().SendDrop (GameObject.Find (dataIO [2]).transform.Find ("Drops"));
				GameObject.Find (dataIO [1]).GetComponent<ServerLoadColor>().IncColor();
				loadSize = loadSize - 10000000;
				yield return null;//new WaitForSeconds(0.01f);
			}while(loadSize > 10000000);
		}
	}

	void OnDisable() {
		data.Close();
		tcp.GetStream().Close();
		tcp.Close();
	}

}
