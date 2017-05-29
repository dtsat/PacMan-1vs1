using UnityEngine;
using System.Collections;

public class Launcher : Photon.PunBehaviour {

	public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
	public byte maxPlayers = 2;
	private bool isConnecting;

	public GameObject controlPanel, progressLabel;

	string version = "1";

	void Awake()
	{
		PhotonNetwork.logLevel = Loglevel;
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.automaticallySyncScene = true;
	}

	void Start () 
	{
		progressLabel.SetActive (false);
		controlPanel.SetActive (true);
	}

	public void Connect()
	{
		isConnecting = true;
		progressLabel.SetActive (true);
		controlPanel.SetActive (false);

		if (PhotonNetwork.connected)
			PhotonNetwork.JoinRandomRoom ();
		else
			PhotonNetwork.ConnectUsingSettings (version);
	}
	
	public override void OnConnectedToMaster()
	{
		Debug.Log ("called connected to master");
		if(isConnecting)
			PhotonNetwork.JoinRandomRoom();
	}

	public override void OnDisconnectedFromPhoton()
	{
		progressLabel.SetActive (false);
		controlPanel.SetActive (true);
		Debug.Log ("disconnect was called");
	}

	public override void OnPhotonRandomJoinFailed(object[] code)
	{
		Debug.Log ("failed was called");
		PhotonNetwork.CreateRoom (null, new RoomOptions (){ MaxPlayers = maxPlayers }, null);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log ("called join room");
		if (PhotonNetwork.room.PlayerCount == 1) 
		{
			Debug.Log ("Load room for 1");
			PhotonNetwork.LoadLevel ("Room for 1");
		}
	}

}
