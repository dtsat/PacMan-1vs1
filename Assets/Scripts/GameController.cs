using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text p1Score, p2Score, winner;
	private GameObject[] pellets;
	public int total = 243, p1S = 0, p2S = 0;
	private AudioSource[] sounds;

	void Start () 
	{
		sounds = GetComponents<AudioSource> ();
		StartCoroutine (waitabit ());
	}

	IEnumerator waitabit()
	{
		yield return new WaitForSeconds (5f);

		sounds [0].Play ();
	}

	public void scorePoint(int i)
	{
		
			if (i == 0) {
			p1Score.text = ("" + ++p1S);
			} else if (i == 1) {
				p2Score.text = ("" + ++p2S);
				Debug.Log ("BREAKPELLET " + PhotonNetwork.player.UserId);
			}

			total--;
			if (total <= 0) {
				sounds [0].Stop ();
				sounds [1].Play ();
				if (p1S > p2S)
					winner.text = "PLAYER 1 WINS";
				else if (p2S > p1S)
					winner.text = "PLAYER 2 WINS";
				else
					winner.text = "TIE GAME!";
			}

	}


	void Update () 
	{
	
	}
}
