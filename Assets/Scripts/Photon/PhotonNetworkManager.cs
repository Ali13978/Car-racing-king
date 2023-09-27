using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    public static PhotonNetworkManager Instance;
    public Action onJoinedRoomSucc;
    public Action onJoinedRoomFail;


    public static bool isMultiplayer;

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
        MainMenuScript.instance.LoadingScreenCanvasSetActive(true);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        MainMenuScript.instance.LoadingScreenCanvasSetActive(false);
        MainMenuScript.instance.multiplayerBtn.gameObject.SetActive(true);
        Debug.Log("Player connected to master server in " + PhotonNetwork.CloudRegion + " region");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        MainMenuScript.instance.LoadingScreenCanvasSetActive(false);
        MainMenuScript.instance.multiplayerBtn.gameObject.SetActive(false);
    }

    public void CreateRoom(string _levelName)
    {

        string _roomName = UnityEngine.Random.Range(1, 100000).ToString();
        ExitGames.Client.Photon.Hashtable _customRoomProperties = new ExitGames.Client.Photon.Hashtable();
        _customRoomProperties["LevelQuery"] = _levelName;

        RoomOptions options = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 6,
            CustomRoomProperties = _customRoomProperties
        };

        PhotonNetwork.CreateRoom(_roomName, options);
        Debug.Log("Create room with name: " + _roomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create photon room: Error code " + returnCode + "\nError message: " + message);
    }
    
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        onJoinedRoomSucc?.Invoke();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Failed to quick join error code: " + returnCode + "; message: " + message);
        onJoinedRoomFail?.Invoke();
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Joined room ");
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void QuickJoinRoom(string _levelName)
    {
        PhotonNetwork.JoinOrCreateRoom(_levelName, new RoomOptions { MaxPlayers = 6 }, TypedLobby.Default);
    }
    
    public void LeaveRoom()
    {
        Debug.Log("Left room");
        PhotonNetwork.LeaveRoom();
    }
}
