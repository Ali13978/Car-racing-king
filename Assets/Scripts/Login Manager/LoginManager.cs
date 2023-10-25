using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class LoginManager : MonoBehaviour
{
    string playerId;

    #region Singleton
    public static LoginManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public string GetPlayerId()
    {
        return playerId;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        playerId = AuthenticationService.Instance.PlayerId;
    }

    public async void UpdatePlayerName(string playerName)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }
}
