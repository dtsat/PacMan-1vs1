using UnityEngine;
using System.Collections;

public class PacMove : MonoBehaviour {

	private NavMeshAgent agent;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
	}
	

	void FixedUpdate () 
	{
		if (Input.GetKey (KeyCode.UpArrow)) 
			transform.position += Vector3.forward* 2f * Time.deltaTime;
		else if (Input.GetKey (KeyCode.DownArrow))
			transform.position += Vector3.back* 2f * Time.deltaTime;
		else if (Input.GetKey (KeyCode.LeftArrow))
			transform.position += Vector3.left* 2f * Time.deltaTime;
		else if (Input.GetKey (KeyCode.RightArrow))
			transform.position += Vector3.right* 2f * Time.deltaTime;
	}
}
