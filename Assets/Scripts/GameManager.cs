using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.KRGameStudios.PhotonTutorial {
	public class GameManager : MonoBehaviourPunCallbacks {
		//public access members
		public GameObject playerPrefab;

		//unity methods
		void Start() {
			if (RiceController.staticRice == null) {
				PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
			} else {
				Debug.Log("Ignoring scene load for level " + Application.loadedLevelName);
			}
		}

		//Pun methods
		public override void OnPlayerEnteredRoom(Player other) {
			Debug.Log("Player entering room: " + other.NickName); //not seen if you're the player

			if (PhotonNetwork.IsMasterClient) {
				Debug.Log("OnPlayerEnteredRoom IsMasterClient: " + PhotonNetwork.IsMasterClient);
				LoadArena();
			}
		}

		public override void OnPlayerLeftRoom(Player other) {
			Debug.Log("Player left room: " + other.NickName);

			if (PhotonNetwork.IsMasterClient) {
				Debug.Log("OnPlayerLeftRoom IsMasterClient: " + PhotonNetwork.IsMasterClient);
				LoadArena();
			}
		}

		public override void OnLeftRoom() {
			SceneManager.LoadScene(0);
		}

		//public access methods
		public void LeaveRoom() {
			PhotonNetwork.LeaveRoom();
		}

		//private access methods
		void LoadArena() {
			if (!PhotonNetwork.IsMasterClient) {
				Debug.Log("Tryinh to load a level while not the master client");
			}
			Debug.Log("Loading level " + PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
		}
	}
}