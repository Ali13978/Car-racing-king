using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class PlayerInRoom : MonoBehaviour
{
    [SerializeField] private Text playerNameText;
    

    public void UpdatePlayerUI(string _playerName)
    {
        playerNameText.text = _playerName;
    }
}
