using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class LeaderboardsManager : MonoBehaviour
{
    #region Singleton
    public static LeaderboardsManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }
    #endregion

    private string leaderboardId = "Global_Leaderboard";
    private List<Leaderboardentity> entries;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void AddScore(int _score)
    {
        await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, _score);
    }

    public async Task<List<Leaderboardentity>> GetEntries()
    {
        GetScoresOptions options = new GetScoresOptions();
        options.Limit = 7;
        try
        {
            var result = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, options);

            string jsonData = JsonConvert.SerializeObject(result);

            FillEntriesFromJSON(jsonData);

            return entries;
        }

        catch (LeaderboardsException e)
        {
            Debug.LogError(e);
            return new List<Leaderboardentity>();
        }
    }

    public async Task<Leaderboardentity> GetPlayerScore()
    {
        try
        {
            var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);

            string jsonData = JsonConvert.SerializeObject(scoreResponse);
            Leaderboardentity playerData = JsonUtility.FromJson<Leaderboardentity>(jsonData);

            return playerData;
        }
        catch (LeaderboardsException e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    private void FillEntriesFromJSON(string jsonData)
    {
        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(jsonData);
        entries = data.results;
        entries = entries.OrderBy(entry => entry.rank).ToList();
    }
}

[System.Serializable]
class LeaderboardData
{
    public int limit;
    public int total;
    public List<Leaderboardentity> results;
}

[System.Serializable]
public class Leaderboardentity
{
    public string playerId;
    public string playerName;
    public int rank;
    public float score;
}