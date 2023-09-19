using CinemaDirector;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AliScripts.AliExtras;

public class MainMenuScript : MonoBehaviourPunCallbacks
{
	public enum MainMenuState
	{
		PlayScreen,
		SettingsScreen,
		environmentSelectionScreen,
		levelSelectionScreen,
		VehicleSelectionScreen,
		LoadingScreen
	}

    #region Singleton
    public static MainMenuScript instance;
    private void Awake()
    {
        PlayerPrefs.SetInt("Cash", 10000000);
        instance = this;
    }

    #endregion
    [SerializeField] GameObject moneyGameObject;
    [SerializeField] Button multiplayerBtn;

    [Header("------------Pannels-----------")]
    [SerializeField] GameObject mainMenuPannel;
    [SerializeField] GameObject lobbyPannel;
    //[SerializeField] GameObject createJoinRoomPannel;
    [SerializeField] GameObject setupNamePannel;

    //[Header("------------Create/Join Lobby System-----------")]
    //[SerializeField] Button createRoomBtn;
    //[SerializeField] InputField joinCodeInputField;
    //[SerializeField] Button joinBtn;

    [Header("------------Lobby Pannel-----------")]
    [SerializeField] Text lobbyCodeText; 
    [SerializeField] GameObject playersHolder;
    [SerializeField] GameObject playerInLobbyPrefab;
    [SerializeField] Button startGameBtn;
    public Action editLobbyPlayersAction;

    [Header("------------Setup name Pannel-----------")]
    [SerializeField] InputField NameInputField;
    [SerializeField] Button doneBtn;

    [SerializeField] Transform campoint;

    [SerializeField] Cutscene cutscene;

    [SerializeField] GameObject MainCamera;

    [SerializeField] GameObject[] CarStats;

    [SerializeField] int vehicleCounter;

	[Header("Stars")]
    [SerializeField] Transform[] starsCanvas;

	private Transform[] starsCanvasArray;

	private const int ON = 0;

	private const int OFF = 1;

	[Header("Sound Clips")]
    [SerializeField] AudioClip click;

    [SerializeField] AudioClip clickBass;

    [SerializeField] AudioClip clickTreble;

    [SerializeField] AudioClip bgMusic;

    [SerializeField] AudioSource musicSource;

    [SerializeField] AudioSource clickSource;

	[Header("Links")]
    //[SerializeField] string bundleID;

    //[SerializeField] string developerAccountID;

    //[SerializeField] string instagramLink;

    //[SerializeField] string twitterLink;

	private int soundStatus;

	private int musicStatus;

	[Header("Main Menu State")]
    [SerializeField] MainMenuState currentState;

	[Header("Canvas Elements")]
    [SerializeField] GameObject playScreenCanvas;

    [SerializeField] GameObject settingsScreenCanvas;

    [SerializeField] GameObject environmentScreenCanvas;

    [SerializeField] GameObject vehicleSelectionCanvas;

    [SerializeField] GameObject loadingScrenCanvas;

    [SerializeField] GameObject[] levelScreenCanvas;

	[Header(" --- Environment Locks --- ")]
    [SerializeField] GameObject[] environmentLocks;

	[Header(" --- Environment Levels Cleared Text --- ")]
    [SerializeField] Text[] environmentLevelsClearedCounter;

	[Header("Settings Panel")]
    [SerializeField] GameObject[] soundOnOff;

    [SerializeField] GameObject[] musicOnOff;

	[Header("Levels Panel")]
    [SerializeField] GameObject[] levelObjects;

    [SerializeField] GameObject[] selectedTextures;

	[TextArea(3, 10)]
    [SerializeField] string[] missionObjectives;

    [SerializeField] Text levelSelectionText;

    [SerializeField] Text levelNumberText;

    [SerializeField] int selectedLevel;

	[Header("Vehicles Panel")]
    [SerializeField] GameObject[] vehicles;

    [SerializeField] string[] vehicleNames;

    [SerializeField] string[] handling;

    [SerializeField] string[] topSpeed;

    [SerializeField] string[] acceleration;

    [SerializeField] Text topSpeedPlaceHolder;

    [SerializeField] Text accelerationPlaceHolder;

    [SerializeField] Text handlingPlaceHolder;

    [SerializeField] Text vehicleNameText;

    [SerializeField] string[] vehiclePrices;

    [SerializeField] Text vehiclePricePlaceHolder;

    [SerializeField] Text cashPlaceHolder;

    [SerializeField] GameObject buyButton;

    [SerializeField] GameObject DefaultButton;

    [SerializeField] GameObject DefaultButton2;

    [SerializeField] GameObject DefaultButton3;

    [SerializeField] GameObject displayMessageText;

    [SerializeField] string lessCashMessage;

    [SerializeField] string selectVehicleMessage;

	[Header("Loading Screen")]
    [SerializeField] Texture truck;

    [SerializeField] Texture blackBG;

    [SerializeField] Texture blueBG;

	[Header("Ads Stuff")]
    //[SerializeField] static int videoCash = 1000;

	public GameObject noAdsCanvas;

	private AsyncOperation loadingProgress;

	private float counter;

    [SerializeField] GameObject panel;

    private void Start()
	{
        Time.timeScale = 1f;

        currentState = MainMenuState.PlayScreen;
		vehicleCounter = 0;
		AudioListener.volume = 1f;
		if (!PlayerPrefs.HasKey("FirstTime"))
		{
			PlayerPrefs.SetInt("FirstTime", 1);
			PlayerPrefs.SetInt("Sound", 0);
			PlayerPrefs.SetInt("Music", 0);
			PlayerPrefs.SetInt("LevelsCleared", 1);
			soundStatus = 0;
			musicStatus = 0;
			PlayerPrefs.SetInt("Cash", 45000);

            mainMenuPannel.SetActive(false);
            setupNamePannel.SetActive(true);
		}
		else
		{
			soundStatus = PlayerPrefs.GetInt("Sound");
			musicStatus = PlayerPrefs.GetInt("Music");
		}
		musicSource.clip = bgMusic;
		if (musicStatus == 0)
		{
			musicSource.Play();
			musicSource.loop = true;
		}
		else
		{
			UnityEngine.Debug.Log("Music is off");
		}

        multiplayerBtn.onClick.AddListener(() => {
            moneyGameObject.SetActive(true);
            playScreenOptions(4);
            SetIsMultiplayer(true);
            //createJoinRoomPannel.SetActive(false);
        });
        //createRoomBtn.onClick.AddListener(() => {
        //    moneyGameObject.SetActive(true);
        //    playScreenOptions(4);
        //    SetIsMultiplayer(true);
        //    createJoinRoomPannel.SetActive(false);
        //});

        //joinBtn.onClick.AddListener(() => {
        //    loadingScrenCanvas.SetActive(true);
        //    createJoinRoomPannel.SetActive(false);

        //    string lobbyCode = joinCodeInputField.text;
        //    if (string.IsNullOrEmpty(lobbyCode))
        //        return;

        //    LobbyManager.instance.JoinLobby(lobbyCode, () => {
        //        Unity.Services.Lobbies.Models.Lobby joinedLobby = LobbyManager.instance.GetJoinedLobby();
        //        lobbyCodeText.text = joinedLobby.LobbyCode;
        //        loadingScrenCanvas.SetActive(false);
        //        lobbyPannel.SetActive(true);
        //    }, () => {
        //        loadingScrenCanvas.SetActive(false);
        //        mainMenuPannel.SetActive(true);
        //    });
        //});
        
        editLobbyPlayersAction = () => {
            DestroyChildren(playersHolder);

            Room currentRoom = PhotonNetwork.CurrentRoom;

            foreach (var i in currentRoom.Players.Values)
            {
                GameObject player = Instantiate(playerInLobbyPrefab, playersHolder.transform);
                PlayerInRoom _player = player.GetComponent<PlayerInRoom>();

                _player.UpdatePlayerUI(i.NickName);
            }

            if (!PhotonNetwork.IsMasterClient)
                startGameBtn.gameObject.SetActive(false);
        };

        doneBtn.onClick.AddListener(() => {
            string playerName = NameInputField.text;

            if (string.IsNullOrEmpty(playerName))
                return;

            PlayerPrefs.SetString("PlayerName", playerName);
            mainMenuPannel.SetActive(true);
            setupNamePannel.SetActive(false);
            PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("PlayerName");
        });

        PhotonNetwork.AutomaticallySyncScene = true;

        startGameBtn.onClick.AddListener(()=>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                photonView.RPC("EnableLoadingScreen", RpcTarget.All);
                PhotonNetwork.LoadLevel(selectedLevel);
            }
        });

        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("PlayerName");
    }

    [PunRPC]
    private void EnableLoadingScreen()
    {
        lobbyPannel.SetActive(false);
        LoadingScreenCanvasSetActive(true);
    }

    private void Update()
	{
		cashPlaceHolder.text = PlayerPrefs.GetInt("Cash").ToString();
	}
    

    public void SetIsMultiplayer(bool _isMultiplayer)
    {
        PhotonNetworkManager.isMultiplayer = _isMultiplayer;
    }

	public void playScreenOptions(int option)
	{
		switch (option)
		{
		case 0:
			break;
		case 5:
			break;
		case 1:
			buttonClickTreble();
			//Application.OpenURL("https://play.google.com/store/apps/developer?id=Mega+Gamers+Production");
			break;
		case 2:
			buttonClickTreble();
			currentState = MainMenuState.SettingsScreen;
			settingsScreenCanvas.SetActive(value: true);
			soundOnOff[soundStatus].SetActive(value: true);
			musicOnOff[musicStatus].SetActive(value: true);
			break;
		case 3:
			buttonClickTreble();
			//Application.OpenURL(developerAccountID);
			break;
		case 4:
			buttonClick();
			currentState = MainMenuState.environmentSelectionScreen;
			updateEnvironmentStatus();
			playScreenCanvas.SetActive(value: false);
			environmentScreenCanvas.SetActive(value: true);
			break;
		case 6:
			buttonClickTreble();
			//Application.OpenURL("https://play.google.com/store/apps/details?id=com.mg.kingofracing");
			break;
		case 7:
			buttonClickBass();
			Application.Quit();
			break;
		case 8:
			buttonClickTreble();
			//Application.OpenURL("https://play.google.com/store/apps/details?id=com.mg.racingcardrivingsimulator");
			break;
		default:
			UnityEngine.Debug.Log("Invalid argument in Play Screen Dialogue");
			break;
		}
	}

	public void environmentScreenOption(int option)
	{
		switch (option)
		{
		case 0:
			buttonClick();
			currentState = MainMenuState.levelSelectionScreen;
			environmentScreenCanvas.SetActive(value: false);
			levelScreenCanvas[0].SetActive(value: true);
			PlayerPrefs.SetInt("environmentNumber", 0);
			break;
		case 1:
			buttonClick();
			currentState = MainMenuState.levelSelectionScreen;
			environmentScreenCanvas.SetActive(value: false);
			levelScreenCanvas[1].SetActive(value: true);
			PlayerPrefs.SetInt("environmentNumber", 1);
			break;
		case 2:
			buttonClick();
			currentState = MainMenuState.levelSelectionScreen;
			environmentScreenCanvas.SetActive(value: false);
			levelScreenCanvas[2].SetActive(value: true);
			PlayerPrefs.SetInt("environmentNumber", 2);
			break;
		case 3:
			buttonClick();
			currentState = MainMenuState.levelSelectionScreen;
			environmentScreenCanvas.SetActive(value: false);
			levelScreenCanvas[3].SetActive(value: true);
			PlayerPrefs.SetInt("environmentNumber", 3);
			break;
		case 4:
			buttonClick();
			currentState = MainMenuState.levelSelectionScreen;
			environmentScreenCanvas.SetActive(value: false);
			levelScreenCanvas[4].SetActive(value: true);
			PlayerPrefs.SetInt("environmentNumber", 4);
			break;
		case 5:
			buttonClickBass();
			currentState = MainMenuState.PlayScreen;
			environmentScreenCanvas.SetActive(value: false);
			playScreenCanvas.SetActive(value: true);
			break;
		default:
			UnityEngine.Debug.Log("Invalid entry in level screen options");
			break;
		}
	}

	public void levelSelection(int clickedLevel)
	{
		GameObject[] carStats = CarStats;
		foreach (GameObject gameObject in carStats)
		{
			gameObject.SetActive(value: true);
		}
		buttonClick();
		selectedLevel = clickedLevel;
		MonoBehaviour.print("--------------- " + selectedLevel);
		currentState = MainMenuState.VehicleSelectionScreen;
		int j = 0;
		for (int num = levelScreenCanvas.Length; j < num; j++)
		{
			levelScreenCanvas[j].SetActive(value: false);
		}
		vehicleSelectionCanvas.SetActive(value: true);
		if (PlayerPrefs.GetInt("Vehicle " + vehicleCounter.ToString()) == 1)
		{
			buyButton.SetActive(value: false);
		}
		else
		{
			buyButton.SetActive(value: true);
		}
		camposChange();
		updateCashOptions(vehicleCounter);
	}

	public void levelSelectionBack()
	{
		buttonClickBass();
		currentState = MainMenuState.environmentSelectionScreen;
		int i = 0;
		for (int num = levelScreenCanvas.Length; i < num; i++)
		{
			levelScreenCanvas[i].SetActive(value: false);
		}
		environmentScreenCanvas.SetActive(value: true);
	}

	public void carSelectionOptions(int option)
	{
		switch (option)
		{
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 0:
		{
			buttonClickBass();
			currentState = MainMenuState.levelSelectionScreen;
			vehicleSelectionCanvas.SetActive(value: false);
			levelScreenCanvas[PlayerPrefs.GetInt("environmentNumber")].SetActive(value: true);
			GameObject[] carStats = CarStats;
			foreach (GameObject gameObject in carStats)
			{
				gameObject.SetActive(value: false);
			}
			cutscene.Play();
			break;
		}
		case 1:
			buttonClick();
                if (PlayerPrefs.GetInt("Vehicle " + vehicleCounter.ToString()) == 1)
                {
                    if (PhotonNetworkManager.isMultiplayer)
                    {
                        loadingScrenCanvas.SetActive(true);
                        //createJoinRoomPannel.SetActive(false);

                        LobbyManager.instance.QuickJoinLobby(() =>
                        {
                            Room currentRoom = PhotonNetwork.CurrentRoom;
                            lobbyCodeText.text = currentRoom.Name;
                            loadingScrenCanvas.SetActive(false);
                            lobbyPannel.SetActive(true);

                            PlayerPrefs.SetInt("SelectedVehicle", vehicleCounter);
                            int selectedCarNumber = vehicleCounter;

                            // Get the local player
                            Player localPlayer = Photon.Pun.PhotonNetwork.LocalPlayer;

                            // Create a custom properties dictionary to hold the selected car number
                            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
                            customProperties["SelectedCarNumber"] = selectedCarNumber;

                            // Set the custom properties for the local player and broadcast them to others
                            localPlayer.SetCustomProperties(customProperties);

                        }, () =>
                        {
                            loadingScrenCanvas.SetActive(false);
                            mainMenuPannel.SetActive(true);
                        },
                        selectedLevel.ToString());
                    }
                    else
                    {
                        currentState = MainMenuState.LoadingScreen;
                        vehicleSelectionCanvas.SetActive(value: false);
                        loadingScrenCanvas.SetActive(value: true);
                        PlayerPrefs.SetInt("SelectedVehicle", vehicleCounter);
                        Debug.Log("Vehicle Counter: " + vehicleCounter.ToString());
                        loadingProgress = SceneManager.LoadSceneAsync(selectedLevel);
                        StartCoroutine(showProgress());
                    }
                }
                else
                {
                    displayMessageText.SetActive(value: true);
                    displayMessageText.GetComponentInChildren<Text>().text = selectVehicleMessage;
                }
			break;
		case 2:
		{
			buttonClickTreble();
			int num2;
			if (vehicleCounter == 0)
			{
				vehicleCounter = 4;
				num2 = 0;
			}
			else
			{
				vehicleCounter--;
				num2 = vehicleCounter + 1;
			}
			vehicles[num2].SetActive(value: false);
			vehicles[vehicleCounter].SetActive(value: true);
			updateCashOptions(vehicleCounter);
			if (PlayerPrefs.GetInt("Vehicle " + vehicleCounter.ToString()) == 1)
			{
				buyButton.SetActive(value: false);
			}
			else
			{
				buyButton.SetActive(value: true);
			}
			break;
		}
		case 3:
		{
			buttonClickTreble();
			int num;
			if (vehicleCounter == 4)
			{
				vehicleCounter = 0;
				num = 4;
			}
			else
			{
				vehicleCounter++;
				num = vehicleCounter - 1;
			}
			vehicles[num].SetActive(value: false);
			vehicles[vehicleCounter].SetActive(value: true);
			updateCashOptions(vehicleCounter);
			if (PlayerPrefs.GetInt("Vehicle " + vehicleCounter.ToString()) == 1)
			{
				buyButton.SetActive(value: false);
			}
			else
			{
				buyButton.SetActive(value: true);
			}
			break;
		}
		default:
			UnityEngine.Debug.Log("Invalid argument in Play Screen Dialogue");
			break;
		}
	}

	public void updateEnvironmentStatus()
	{
		if (PlayerPrefs.GetInt("environment0levelCleared") >= 3)
		{
			environmentLocks[0].SetActive(value: false);
		}
		if (PlayerPrefs.GetInt("environment1levelCleared") >= 3)
		{
			environmentLocks[1].SetActive(value: false);
		}
		if (PlayerPrefs.GetInt("environment2levelCleared") >= 3)
		{
			environmentLocks[2].SetActive(value: false);
		}
		if (PlayerPrefs.GetInt("environment3levelCleared") >= 3)
		{
			environmentLocks[3].SetActive(value: false);
		}
		environmentLevelsClearedCounter[0].text = PlayerPrefs.GetInt("environment0levelCleared") + "/5";
		environmentLevelsClearedCounter[1].text = PlayerPrefs.GetInt("environment1levelCleared") + "/5";
		environmentLevelsClearedCounter[2].text = PlayerPrefs.GetInt("environment2levelCleared") + "/5";
		environmentLevelsClearedCounter[3].text = PlayerPrefs.GetInt("environment3levelCleared") + "/5";
		environmentLevelsClearedCounter[4].text = PlayerPrefs.GetInt("environment4levelCleared") + "/5";
		MonoBehaviour.print(PlayerPrefs.GetInt("environment0levelCleared") + " ------------------------------ ");
	}

	private void updateLevels()
	{
		int i;
		for (i = 0; i < PlayerPrefs.GetInt("LevelsCleared"); i++)
		{
			levelObjects[i].SetActive(value: false);
		}
		i = (selectedLevel = i - 1);
		updateLevelSelection(selectedLevel);
		levelSelectionText.text = missionObjectives[selectedLevel];
		updateLevelDescription(selectedLevel);
		updateLevelLabel(selectedLevel);
	}

	private void starsCalculate()
	{
		for (int i = 0; i < 15; i++)
		{
			starsCanvasArray = new Transform[starsCanvas[i].childCount];
			for (int j = 0; j < starsCanvas[i].childCount; j++)
			{
				starsCanvasArray[j] = starsCanvas[i].GetChild(j);
			}
			for (int k = 0; k < PlayerPrefs.GetInt(i + 1 + "levelStars"); k++)
			{
				starsCanvasArray[k].gameObject.SetActive(value: true);
			}
		}
	}

	private void updateLevelDescription(int level)
	{
		levelSelectionText.text = missionObjectives[level];
	}

	private void updateLevelSelection(int level)
	{
		GameObject[] array = selectedTextures;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(value: false);
		}
		selectedTextures[level].SetActive(value: true);
	}

	private void updateLevelLabel(int level)
	{
		levelNumberText.text = "Level NO: " + (level + 1).ToString();
	}

	public void switchSoundStatus()
	{
		if (PlayerPrefs.GetInt("Sound") == 0)
		{
			PlayerPrefs.SetInt("Sound", 1);
			soundStatus = 1;
			soundOnOff[1].SetActive(value: true);
		}
		else
		{
			PlayerPrefs.SetInt("Sound", 0);
			soundStatus = 0;
			soundOnOff[1].SetActive(value: false);
		}
	}

	public void switchMusicStatus()
	{
		if (PlayerPrefs.GetInt("Music") == 0)
		{
			PlayerPrefs.SetInt("Music", 1);
			musicStatus = 1;
			musicOnOff[1].SetActive(value: true);
			musicSource.Stop();
		}
		else
		{
			PlayerPrefs.SetInt("Music", 0);
			musicStatus = 0;
			musicOnOff[1].SetActive(value: false);
			musicSource.Play();
		}
	}

	private void updateVehicleName(int veh)
	{
	}

	private void updateVehicleStats(int veh)
	{
		topSpeedPlaceHolder.text = topSpeed[veh];
		accelerationPlaceHolder.text = acceleration[veh];
		handlingPlaceHolder.text = handling[veh];
	}

	public void disappearMessage()
	{
		displayMessageText.SetActive(value: false);
	}

	public void buyVehicle()
	{
		buttonClick();
		if (PlayerPrefs.GetInt("Cash") >= int.Parse(vehiclePrices[vehicleCounter]))
		{
			PlayerPrefs.SetInt("Vehicle " + vehicleCounter.ToString(), 1);
			PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") - int.Parse(vehiclePrices[vehicleCounter]));
			buyButton.SetActive(value: false);
			updateCashOptions(vehicleCounter);
		}
		else
		{
			displayMessageText.SetActive(value: true);
			displayMessageText.GetComponentInChildren<Text>().text = lessCashMessage;
			Invoke("disappearMessage", 10f);
		}
	}

	private void updateCashOptions(int veh)
	{
		vehiclePricePlaceHolder.text = vehiclePrices[veh];
		cashPlaceHolder.text = PlayerPrefs.GetInt("Cash").ToString();
		if (PlayerPrefs.GetInt("Vehicle " + veh.ToString()) == 0)
		{
			buyButton.SetActive(value: true);
		}
		else
		{
			buyButton.SetActive(value: false);
		}
	}

	private void startPanel()
	{
		loadingScrenCanvas.SetActive(value: false);
		panel.SetActive(value: false);
	}

	private void OnGUI()
	{
		MainMenuState mainMenuState = currentState;
		if (mainMenuState == MainMenuState.LoadingScreen)
		{
			float num = counter;
			GUI.DrawTexture(new Rect(0f, (float)Screen.height * 0.9f, Screen.width, (float)Screen.height * 0.1f), blackBG);
			GUI.DrawTexture(new Rect(num / 100f * (float)Screen.width, (float)Screen.height * 0.9f, (float)Screen.width * 0.1f, (float)Screen.height * 0.1f), truck);
			GUI.DrawTexture(new Rect(0f, (float)Screen.height * 0.9f, num / 100f * (float)Screen.width, (float)Screen.height * 0.1f), blueBG);
		}
	}

	private IEnumerator showProgress()
	{
		while (!loadingProgress.isDone)
		{
			if (counter < 60f)
			{
				counter += 0.5f;
			}
			else if (counter < 80f)
			{
				counter += 0.05f;
			}
			else if (counter < 100f)
			{
				counter += 0.01f;
			}
			yield return null;
		}
		counter = 100f;
	}

	public void settingsBackButton()
	{
		settingsScreenCanvas.SetActive(value: false);
		currentState = MainMenuState.PlayScreen;
	}

	private void camposChange()
	{
		cutscene.Pause();
		cutscene.Stop();
		MainCamera.transform.position = campoint.position;
		MainCamera.transform.rotation = campoint.rotation;
	}

	private void buttonClick()
	{
		clickSource.PlayOneShot(click);
	}

	private void buttonClickBass()
	{
		clickSource.PlayOneShot(clickBass);
	}

	private void buttonClickTreble()
	{
		clickSource.PlayOneShot(clickTreble);
	}

	public void CallVideoAds()
	{
		NoAdsAvailable();
		cashPlaceHolder.text = PlayerPrefs.GetInt("Cash").ToString();
	}

	public void NoAdsAvailable()
	{
		noAdsCanvas.SetActive(value: true);
	}

	public void closeNoAdsMessage()
	{
		noAdsCanvas.SetActive(value: false);
	}

    public void LoadingScreenCanvasSetActive(bool isActive)
    {
        loadingScrenCanvas.SetActive(isActive);
    }
}
