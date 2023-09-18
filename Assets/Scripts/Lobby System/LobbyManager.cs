using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LobbyManager : MonoBehaviour
{   
    int currentPlayersCount = 0;
    

    #region Singleton
    public static LobbyManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        LobbyPlayersEdited();
    }

    //public async void JoinLobby(string lobbyCode, Action joiningSucessful, Action joiningFailed)
    //{
    //    try
    //    {
    //        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
    //        options.Player = GetPlayer();

    //        joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
    //        joiningSucessful?.Invoke();
    //    }
    //    catch(LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //        joiningFailed?.Invoke();
    //    }
    //}

    public void QuickJoinLobby(Action QuickJoinSucessful, Action QuickJoinFailed, string levelName)
    {
        PhotonNetworkManager.Instance.onJoinedRoomSucc = QuickJoinSucessful;
        PhotonNetworkManager.Instance.QuickJoinRoom(levelName);
    }
    
    

    private void LobbyPlayersEdited()
    {
        int newPlayersCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (newPlayersCount != currentPlayersCount)
        {
            Debug.Log("Player added or removed");
            currentPlayersCount = newPlayersCount;
            MainMenuScript.instance.editLobbyPlayersAction?.Invoke();
        }
    }
}