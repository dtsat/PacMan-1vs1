using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkCharacter : Photon.MonoBehaviour {

	public Text P1, P2, winner;
	public string p1score, p2score, winnertext;

	void Update()
	{
		if (!PhotonNetwork.isMasterClient) 
		{
			P1.text = p1score;
			P2.text = p2score;
			winner.text = winnertext;
		}
	}


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && PhotonNetwork.isMasterClient) {
			stream.SendNext (P1.text);
			stream.SendNext (P2.text);
			stream.SendNext (winner.text);
		} else {
			p1score = (string)stream.ReceiveNext ();
			p2score = (string)stream.ReceiveNext ();
			winnertext = (string)stream.ReceiveNext ();
		}

	}

}
