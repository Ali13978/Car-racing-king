using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using QFSW.QC;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    public static PhotonNetworkManager Instance;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player connected to master server in " + PhotonNetwork.CloudRegion + " region");
    }

    [Command]
    public void CreateRoom(string _roomName)
    {
        RoomOptions options = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 6
        };
        PhotonNetwork.CreateRoom(_roomName, options);
        Debug.Log("Create room with name: " + _roomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create photon room: Error code " + returnCode + "\nError message: " + message);
    }

    [Command]
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
    }

    [Command]
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Joined room ");
            PhotonNetwork.LoadLevel(1);
        }
    }

    [Command]
    public void LeaveRoom()
    {
        Debug.Log("Left room");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
