using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(InputField))]
public class PlayerNameInput : MonoBehaviour {

	static string playerNamePref = "PlayerName";


	void Start () 
	{
		string defaultName = "";
		InputField inputfield = GetComponent<InputField> ();
		if (inputfield != null) 
		{
			if (PlayerPrefs.HasKey (playerNamePref)) 
			{
				defaultName = PlayerPrefs.GetString (playerNamePref);
				inputfield.text = defaultName;
			}
		}

		PhotonNetwork.playerName = defaultName;
	}
	
	public void SetPlayerName(string value)
	{
		PhotonNetwork.playerName = value + " ";
		PlayerPrefs.SetString (playerNamePref, value);
	}
}
