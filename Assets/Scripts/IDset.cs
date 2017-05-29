using UnityEngine;
using System.Collections;

public class IDset : MonoBehaviour {

	public int initial;
	private Transform[] tiles;

	void Start () 
	{
		tiles = GetComponentsInChildren<Transform> ();
		foreach (Transform g in tiles) 
		{
			if (g.CompareTag ("Tile")) 
			{
				g.GetComponent<Tile> ().nodeID = initial++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
