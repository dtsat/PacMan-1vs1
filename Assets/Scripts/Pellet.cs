using UnityEngine;
using System.Collections;

public class Pellet : MonoBehaviour {

	public int id = 0;
	public bool eaten = false;

	void Update()
	{
		if (PhotonNetwork.isMasterClient && eaten)
			PhotonNetwork.Destroy (this.gameObject);
	}
}
