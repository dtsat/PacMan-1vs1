using UnityEngine;
using System.Collections;

public class MsPacManController : MonoBehaviour {

	public GameObject currentTile;
	public GameObject gameController;
	public int playerID, startingTile;
	public Vector3 startingposition;

	private AudioSource[] sounds;
	private GameController guiScript;
	private GameObject[] tiles;
	private GameObject[] orderedTiles = new GameObject[868];
	private bool moving, setup;
	private float speed = 0.25f;

	void Start () 
	{
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
		sounds [1].Play ();
		yield return new WaitForSeconds (5f);
		tiles = GameObject.FindGameObjectsWithTag ("Tile");
		foreach (GameObject t in tiles) 
		{
			orderedTiles [t.GetComponent<Tile> ().nodeID] = t;
		}

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


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Pellet")) {
			Destroy (other.gameObject);
			sounds [0].Play ();
			guiScript.scorePoint (playerID);
		} else if (other.CompareTag ("SuperPellet")) {
			Destroy (other.gameObject);
			StartCoroutine (speedUp ());
			sounds [2].Play ();
			guiScript.scorePoint (playerID);
		}

				
	}

	void FixedUpdate () 
	{
		if (!setup)
			return;
		if (Input.GetKey (KeyCode.UpArrow) && currentTile.GetComponent<Tile> ().north && !PhotonNetwork.isMasterClient) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 90f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.forward), currentTile.GetComponent<Tile> ().nodeID - 28, new Vector3 (0.0f, 90f, 0.0f), false, -1));
		}
		else if (Input.GetKey (KeyCode.DownArrow) && currentTile.GetComponent<Tile> ().south && !PhotonNetwork.isMasterClient) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 270f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.back), currentTile.GetComponent<Tile> ().nodeID + 28, new Vector3 (0.0f, 270f, 0.0f), false, -1));
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && currentTile.GetComponent<Tile> ().teleportLeft && !PhotonNetwork.isMasterClient) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 0f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.left), 419, new Vector3 (0.0f, 0f, 0.0f), true, 0));
		}
		else if (Input.GetKey (KeyCode.RightArrow) && currentTile.GetComponent<Tile> ().teleportRight && !PhotonNetwork.isMasterClient) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 180f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.right), 392, new Vector3 (0.0f, 180f, 0.0f), true, 1));
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && currentTile.GetComponent<Tile> ().west && !PhotonNetwork.isMasterClient) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 0f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.left), currentTile.GetComponent<Tile> ().nodeID - 1, new Vector3 (0.0f, 0f, 0.0f), false, -1));
		}
		else if (Input.GetKey (KeyCode.RightArrow) && currentTile.GetComponent<Tile> ().east && !PhotonNetwork.isMasterClient) 
		{
			//transform.eulerAngles = new Vector3 (0.0f, 180f, 0.0f);
			StartCoroutine (moveFunction (transform.position, (transform.position + Vector3.right), currentTile.GetComponent<Tile> ().nodeID + 1, new Vector3 (0.0f, 180f, 0.0f), false, -1));
		}
	}
}
