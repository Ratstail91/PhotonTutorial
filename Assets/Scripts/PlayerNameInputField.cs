using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.KRGameStudios.PhotonTutorial {
	public class PlayerNameInputField : MonoBehaviour {
		//constants
		const string playerNamePrefKey = "PlayerName"; //the key of a key-value pair

		//unity methods
		void Start() {
			string defaultName = string.Empty;
			InputField inputField = GetComponent<InputField>();
			if (inputField != null) {
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				inputField.text = defaultName;
			}
			PhotonNetwork.NickName = defaultName;
		}

		//public methods
		public void SetPlayerName(string value) {
			//error
			if (string.IsNullOrEmpty(value)) {
				Debug.Log("Player Name is null or empty");
				return;
			}
			PhotonNetwork.NickName = value;
			PlayerPrefs.SetString(playerNamePrefKey, value);
		}
	}
}