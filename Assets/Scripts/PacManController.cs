using UnityEngine;
using System.Collections;

public class PacManController : Photon.MonoBehaviour {

	public GameObject currentTile;
	public GameObject gameController;
	public int playerID, startingTile;
	public Vector3 startingposition;

	private AudioSource[] sounds;
	private GameController guiScript;
	private GameObject[] tiles, pelletsOrdered = new GameObject[299];
	public static GameObject[] pellets;
	private GameObject[] orderedTiles = new GameObject[868];
	private bool moving, setup;
	private float speed = 0.25f;

	void Start () 
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController");
		startingposition = transform.position;
		sounds = GetComponents<AudioSource> ();
		guiScript = gameController.GetComponent<GameController> ();
		moving = false;
		setup = false;

		StartCoroutine (waitabit ());

	}

	public void resetPosition()
	{
		setup = false;
		GetComponent<BoxCollider> ().enabled = false;
		currentTile = orderedTiles[startingTile];
		transform.position = startingposition;
		StartCoroutine (waitRespawn ());
	}

	IEnumerator waitRespawn()
	{
		yield return new WaitForSeconds (0.5f);
		GetComponent<BoxCollider> ().enabled = true;
		setup = true;
	}
		

	IEnumerator waitabit()
	{
		if(photonView.isMine)
			sounds [1].Play ();
		yield return new WaitForSeconds (7f);
		tiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach (GameObject t in tiles) 
		{
			orderedTiles [t.GetComponent<Tile> ().nodeID] = t;
		}
		currentTile = orderedTiles [startingTile];
		/*currentTile = orderedTiles [startingTile];
		GameObject[] normalPellets, superPellets;
		normalPellets = GameObject.FindGameObjectsWithTag ("Pellet");
		superPellets = GameObject.FindGameObjectsWithTag ("SuperPellet");
		pellets = new GameObject[normalPellets.Length + superPellets.Length];
		normalPellets.CopyTo (PacManController.pellets, 0);
		superPellets.CopyTo (PacManController.pellets, normalPellets.Length);
		foreach (GameObject p in pellets)
			pelletsOrdered [p.GetComponent<Pellet> ().id] = p;*/

		setup = true;
	}




	IEnumerator moveFunction(Vector3 start, Vector3 end, int id, Vector3 rotation, bool teleport, int direction)
	{
		if (!moving) 
		{
			moving = true;
			transform.eulerAngles = rotation;
			float t = 0f;
			while (t < 1f) 
			{
				t += Time.deltaTime / speed;
				transform.position = Vector3.Lerp (start, end, t);
				if (!setup)
					break;
				yield return 0;
			}
			if (teleport) 
			{
				if(direction == 0)
					this.transform.position = new Vector3 (27f, this.transform.position.y, this.transform.position.z);
				else
					this.transform.position = new Vector3 (0f, this.transform.position.y, this.transform.position.z);
			}
			moving = false;
			currentTile = orderedTiles [id];
			if (!setup)
				resetPosition ();
		}
	}

	IEnumerator speedUp()
	{
		speed = 0.15f;
		yield return new WaitForSeconds (5f);
		speed = 0.25f;
	}


	[PunRPC]
	void breakPellet(int p)
	{
		Debug.Log ("breakpellettttt Called by " + PhotonNetwork.player.UserId);
		if(PhotonNetwork.isMasterClient)
			PhotonNetwork.Destroy (pelletsOrdered[p].gameObject);
	}

	[PunRPC]
	void score(int i)
	{
		if (PhotonNetwork.isMasterClient) {
			guiScript.scorePoint (i);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		bool hit = false;
		if (other.CompareTag ("Pellet") && !hit) {
			hit = true;
			//photonView.RPC("breakPellet", PhotonTargets.All, other.GetComponent<Pellet>().id);
			other.GetComponent<Pellet>().eaten = true;
			if (photonView.isMine) {
				sounds [0].Play ();
				photonView.RPC ("score", PhotonTargets.All, playerID);
			}

		} else if (other.CompareTag ("SuperPellet")&& !hit) {
			hit = true;
			//photonView.RPC("breakPellet", PhotonTargets.All, other.GetComponent<Pellet>().id);
			other.GetComponent<Pellet>().eaten = true;
			StartCoroutine (speedUp ());
			if (photonView.isMine) {
				sounds [2].Play ();
				photonView.RPC ("score", PhotonTargets.All, playerID);
			}
		}
	}


	void FixedUpdate () 
	{
		if (!setup)
			return;
		if (photonView.isMine == false)
			return;
		if (Input.GetKey (KeyCode.UpArrow) && currentTile.GetComponent<Tile> ().north) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 90f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.forward), currentTile.GetComponent<Tile> ().nodeID - 28, new Vector3 (0.0f, 90f, 0.0f), false, -1));
		}
		else if (Input.GetKey (KeyCode.DownArrow) && currentTile.GetComponent<Tile> ().south) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 270f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.back), currentTile.GetComponent<Tile> ().nodeID + 28, new Vector3 (0.0f, 270f, 0.0f), false, -1));
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && currentTile.GetComponent<Tile> ().teleportLeft) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 0f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.left), 419, new Vector3 (0.0f, 0f, 0.0f), true, 0));
		}
		else if (Input.GetKey (KeyCode.RightArrow) && currentTile.GetComponent<Tile> ().teleportRight) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 180f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.right), 392, new Vector3 (0.0f, 180f, 0.0f), true, 1));
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && currentTile.GetComponent<Tile> ().west) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 0f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.left), currentTile.GetComponent<Tile> ().nodeID - 1, new Vector3 (0.0f, 0f, 0.0f), false, -1));
		}
		else if (Input.GetKey (KeyCode.RightArrow) && currentTile.GetComponent<Tile> ().east) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 180f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.right), currentTile.GetComponent<Tile> ().nodeID + 1, new Vector3 (0.0f, 180f, 0.0f), false, -1));
		}
	}
}
