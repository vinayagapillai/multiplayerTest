using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private bool isConnecting;
    private const string gameVersion = "0.1";
    private const int maxPlayers = 2;

    [SerializeField] private GameObject findingOponentText;
    [SerializeField] private GameObject joinButton;


    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

    private void Start()
    {

    }


    public void JoinGame()
    {
        joinButton.SetActive(false);
        findingOponentText.SetActive(true);
        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to mster");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        findingOponentText.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("no clients, so creating new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers });
    }

    public override void OnJoinedRoom()
    {
        
        if (PhotonNetwork.CurrentRoom.PlayerCount != maxPlayers)
        {
            findingOponentText.SetActive(true);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("main");
        }
    }
}
