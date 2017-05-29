using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour {

	public GameObject player1, player2;
	static bool created = false;

	void Start()
	{
		
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.Instantiate (this.player1.name, new Vector3 (12f, 0.21f, 10f), Quaternion.identity, 0);
			Debug.Log ("Instantiate P1 called by " + PhotonNetwork.player.UserId);
		} else if (created) 
		{
			PhotonNetwork.Instantiate (this.player2.name, new Vector3 (15f, 0.21f, 10f), Quaternion.identity, 0);
			Debug.Log ("Instantiate called by " + PhotonNetwork.player.UserId);
			created = false;
		}
		else if (!created) 
		{ 
			created = true;
		}
	}


	public void OnLeftRoom()
	{
		SceneManager.LoadScene (0);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom ();
	}

	void LoadArena()
	{
		if (!PhotonNetwork.isMasterClient) 
		{
			Debug.Log ("Trying to load but not master client");
		}
		Debug.Log ("Loading level: " + PhotonNetwork.room.PlayerCount);
		PhotonNetwork.LoadLevel ("Room for " + PhotonNetwork.room.PlayerCount);
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer other)
	{
		Debug.Log ("OnPhotonPlayerConnected() " + other.NickName);

		if (PhotonNetwork.isMasterClient) 
		{
			Debug.Log ("Connected as masterClient " + PhotonNetwork.isMasterClient);
			LoadArena ();
		}
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
	{
		Debug.Log ("OnPhotonPlayerDisconnected() " + other.NickName);

		if (PhotonNetwork.isMasterClient) 
		{
			Debug.Log ("Connected as masterClient " + PhotonNetwork.isMasterClient);
			LoadArena ();
		}
	}

}
