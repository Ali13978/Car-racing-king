using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;


public class LobbyManager : MonoBehaviour
{
    //Lobby hostLobby;
    Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    
    int currentPlayersCount = 0;
    

    #region Singleton
    public static LobbyManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    private void Update()
    {
        if (joinedLobby == null)
            return;
        LobbyPlayersEdited();
        HeartbeatHandler();
        HandleLobbyPollsForUpdates();
    }

    public async void CreateLobby(bool _isPrivate, UnityAction lobbyCreationSucessful, UnityAction LobbyCreationFailed, string levelName)
    {
        try
        {
            int maxPlayers = 9;
            CreateLobbyOptions options = new CreateLobbyOptions();

            string _roomName = UnityEngine.Random.Range(1, 100000).ToString();

            options.IsPrivate = _isPrivate;
            options.Player = GetPlayer();
            options.Data = new Dictionary<string, DataObject>
            {
                {"PhotonRoomName", new DataObject(DataObject.VisibilityOptions.Public, _roomName) },
                {
                    "LevelQuery",
                    new DataObject( visibility: DataObject.VisibilityOptions.Public, levelName, index: DataObject.IndexOptions.S1)
                }
            };

            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(_roomName, maxPlayers, options);

            PhotonNetworkManager.Instance.CreateRoom(_roomName);

            currentPlayersCount = 0;
            //joinedLobby = hostLobby;

            lobbyCreationSucessful?.Invoke();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
            LobbyCreationFailed?.Invoke();
        }
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

    public async void QuickJoinLobby(UnityAction QuickJoinSucessful, UnityAction QuickJoinFailed, string levelName)
    {
        try
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();
            options.Player = GetPlayer();
            options.Filter = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.S1, op: QueryFilter.OpOptions.EQ, value: levelName)
            };

            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            PhotonNetworkManager.Instance.JoinRoom(joinedLobby.Data["PhotonRoomName"].Value);
            QuickJoinSucessful?.Invoke();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
            CreateLobby(false, QuickJoinSucessful, QuickJoinFailed, levelName);
        }
    }

    private Player GetPlayer()
    {
        string playerName = "Player";

        if (PlayerPrefs.HasKey("PlayerName"))
            playerName = PlayerPrefs.GetString("PlayerName");

        Player player = new Player {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };

        return player;
    }

    public async void LeaveLobby(Action leaveLobbySucessful, Action leavelobbyFailed)
    {
        if (joinedLobby == null)
            return;
        try
        {   
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            //hostLobby = null;
            joinedLobby = null;
            leaveLobbySucessful?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            leavelobbyFailed?.Invoke();
        }
    }

    public void KickPlayer(string playerId, Action KickedLobbySucessful, Action KickedlobbyFailed)
    {
        if (!IsHost())
            return;
        try
        {
            LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            //hostLobby = null;
            joinedLobby = null;
            KickedLobbySucessful?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            KickedlobbyFailed?.Invoke();
        }
    }

    private async void HeartbeatHandler()
    {
        if (!IsHost())
            return;
        heartbeatTimer -= Time.deltaTime;
        if (heartbeatTimer < 0)
        {
            float heartbeatTimerMax = 15;
            heartbeatTimer = heartbeatTimerMax;
            await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
        }
    }

    private async void HandleLobbyPollsForUpdates()
    {
        if (joinedLobby == null)
            return;
        lobbyUpdateTimer -= Time.deltaTime;
        if (lobbyUpdateTimer < 0)
        {
            float lobbyTimerMax = 1.5f;
            lobbyUpdateTimer = lobbyTimerMax;
            Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

            joinedLobby = lobby;
        }

    }

    public string GetLobbyCode()
    {
        if (joinedLobby == null)
            return "";
        else
            return joinedLobby.LobbyCode;
    }

    public bool IsHost()
    {
        if(joinedLobby.HostId == AuthenticationService.Instance.PlayerId)
            return true;
        else
            return false;
    }

    public Lobby GetJoinedLobby()
    {
        if (joinedLobby == null)
            return null;
        else
            return joinedLobby;
    }

    private void LobbyPlayersEdited()
    {
        List<Unity.Services.Lobbies.Models.Player> LobbyPlayers;
        LobbyPlayers = joinedLobby.Players;

        int newPlayersCount = LobbyPlayers.Count;

        if (newPlayersCount != currentPlayersCount)
        {
            Debug.Log("Player added or removed");
            currentPlayersCount = newPlayersCount;
            MainMenuScript.instance.editLobbyPlayersAction?.Invoke();
        }
    }
}