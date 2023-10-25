using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardPlayer : MonoBehaviour
{
    [SerializeField] Text RankText;
    [SerializeField] Text NameText;
    [SerializeField] Text ScoreText;

    public void UpdateUI(string _rank, string _name, string _score)
    {
        RankText.text = _rank;
        NameText.text = _name;
        ScoreText.text = _score;
    }
}