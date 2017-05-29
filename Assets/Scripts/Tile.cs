using UnityEngine;
using System.Collections;

public class Tile : Photon.PunBehaviour {

	public GameObject pellet;
	private Transform pelletSpawn;
	public bool hasPellet;
	public bool walkable;
	public int nodeID;
	public bool north, east, south, west, intersection;
	public bool teleportLeft, teleportRight;

	void Start () 
	{
		Transform[] children = GetComponentsInChildren<Transform> ();
		foreach (Transform g in children)
			if (g.CompareTag ("PelletSpawn"))
				pelletSpawn = g.transform;
		if (hasPellet && PhotonNetwork.isMasterClient) 
		{
			PhotonNetwork.Instantiate (pellet.name, pelletSpawn.position, pellet.transform.rotation, 0);
		}

		if (walkable) 
		{
			if (north)
				Debug.DrawLine (pelletSpawn.position, pelletSpawn.position + Vector3.forward, Color.red, 20f, false);
			if (east)
				Debug.DrawLine (pelletSpawn.position, pelletSpawn.position + Vector3.right, Color.red, 20f, false);
			if (south)
				Debug.DrawLine (pelletSpawn.position, pelletSpawn.position + Vector3.back, Color.red, 20f, false);
			if (west)
				Debug.DrawLine (pelletSpawn.position, pelletSpawn.position + Vector3.left, Color.red, 20f, false);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
