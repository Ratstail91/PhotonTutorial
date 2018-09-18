using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

//https://doc.photonengine.com/en-us/pun/v2/getting-started/pun-intro

namespace Com.KRGameStudios.PhotonTutorial {
	//MonoBehaviourPunCallbacks is the PUN interface
	public class PhotonConnect : MonoBehaviourPunCallbacks {
		//the game's version
		private const string gameVersion = "0.1";

		//inspector access members
		[SerializeField]
		private byte maxPlayersPerRoom = 4;

		[SerializeField]
		private GameObject controlPanel;

		[SerializeField]
		private GameObject progressPanel;

		//private members
		bool isConnecting;

		//unity methods
		void Start() {
			//graphical feedback
			controlPanel.SetActive(true);
			progressPanel.SetActive(false);
		}

		//public access method
		public void ConnectToPhoton() {
			isConnecting = true;

			//graphical feedback
			controlPanel.SetActive(false);
			progressPanel.SetActive(true);

			//connect to photon using the settings in PhotonServerSettings
			PhotonNetwork.AutomaticallySyncScene = true;
			PhotonNetwork.GameVersion = gameVersion;
			PhotonNetwork.ConnectUsingSettings();

			Debug.Log("Connecting to photon...");
		}

		//PUN overrides
		public override void OnConnectedToMaster() {
			Debug.Log("Connected to photon master");

			if (isConnecting) {
//				PhotonNetwork.CreateRoom("roomName");
				PhotonNetwork.JoinRandomRoom();
//				PhotonNetwork.JoinRoom("roomName");
			} else {
				PhotonNetwork.Disconnect();
			}

			//create a specific room (invisible)
	//		RoomOptions roomOptions = new RoomOptions();
	//		roomOptions.IsVisible = false;
	//		roomOptions.MaxPlayers = 4;
	//		PhotonNetwork.JoinOrCreateRoom("roomName", roomOptions, TypedLobby.Default);
		}

		public override void OnDisconnected(DisconnectCause disconnectCause) {
			Debug.Log("Disconnected: " + disconnectCause.ToString());

			//graphical feedback
			controlPanel.SetActive(true);
			progressPanel.SetActive(false);
		}

		public override void OnCreatedRoom() {
			Debug.Log("Created room");

			//TODO
		}

		public override void OnJoinedRoom() {
			Debug.Log("Joined Room");

			//we are the master player
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
				Debug.Log("Loading the 'Room for 1'");

				PhotonNetwork.LoadLevel("Room for 1");
			}
		}

		public override void OnCreateRoomFailed(short returnCode, string message) {
			Debug.Log("Create room failed: (" + returnCode + ") " + message);

			//TODO
		}

		public override void OnJoinRoomFailed(short returnCode, string message) {
			Debug.Log("Join room failed: (" + returnCode + ") " + message);

			//TODO
		}

		public override void OnJoinRandomFailed(short returnCode, string message) {
			Debug.Log("Join random failed: (" + returnCode + ") " + message);

			//no rooms open, so create one instead
			PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom });
		}
	}
}