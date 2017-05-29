using UnityEngine;
using System.Collections;

public class GhostMovement : MonoBehaviour {

	public Transform player1, player2;
	public GameObject deathAnimation;
	private NavMeshAgent agent;
	private bool ready = false;
	private AudioSource sounds;
	private bool hit = false;
	private bool twoPlayers = false;
	private Vector3 target;

	void Start () 
	{

		agent = GetComponent<NavMeshAgent> ();
		sounds = GetComponent<AudioSource> ();
		StartCoroutine (waitabit ());
	}

	IEnumerator waitabit()
	{
		yield return new WaitForSeconds (8f);
		player1 = GameObject.FindGameObjectWithTag ("Player").transform;
		if (GameObject.FindGameObjectWithTag ("Player2") == null)
			;
		else 
		{
			player2 = GameObject.FindGameObjectWithTag ("Player2").transform;	
			twoPlayers = true;
		}
		ready = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player") && !hit) 
		{
			hit = true;
			sounds.Play ();
			Instantiate (deathAnimation, other.transform.position, deathAnimation.transform.rotation);
			other.GetComponent<PacManController> ().resetPosition ();
			StartCoroutine (waitAfterKill ());
		}
		else if (other.CompareTag ("Player2") && !hit && other.GetComponent<PacManController>().photonView.isMine) 
		{
			hit = true;
			sounds.Play ();
			Instantiate (deathAnimation, other.transform.position, deathAnimation.transform.rotation);
			other.GetComponent<PacManController> ().resetPosition ();
			StartCoroutine (waitAfterKill ());
		}
	}

	IEnumerator waitAfterKill()
	{
		ready = false;
		yield return new WaitForSeconds (2f);
		ready = true;
		hit = false;
	}

	void Update () 
	{
		if (ready) 
		{
			if (twoPlayers) 
			{
				float toP1 = (player1.position - transform.position).magnitude;
				float toP2 = (player2.position - transform.position).magnitude;
				if (toP1 >= toP2)
					agent.SetDestination (player2.position);
				else
					agent.SetDestination (player1.position);
			}
			else
				agent.SetDestination (player1.position);	
		}
	}
}
