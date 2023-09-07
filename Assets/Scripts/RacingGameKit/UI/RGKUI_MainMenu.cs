using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Main Menu UI")]
	[RequireComponent(typeof(RGKUI_Database))]
	public class RGKUI_MainMenu : MonoBehaviour
	{
		public bool m_IsMobile;

		public bool m_UseTouchDrivePro;

		private bool m_IsGamePadActive;

		public RectTransform m_PnlFrame;

		public RectTransform m_PnlSettings;

		public RectTransform m_PnlTracks;

		public RectTransform m_PnlVehicles;

		public RectTransform m_PnlLoading;

		public Transform m_ShowCaseCarLocation;

		private GameObject m_ShowCaseCarInScreen;

		public AudioClip m_MenuBackgroundMusic;

		public AudioClip[] m_UiAudioClips;

		private CanvasGroup m_CanvasFrame;

		private CanvasGroup m_CanvasLoading;

		private Slider m_ProgressLoading;

		private CanvasGroup m_CanvasTracks;

		private Text m_RaceTitle;

		private Text m_TrackName;

		private Text m_RaceInfo;

		private Text m_TrackRaceType;

		private Text m_TrackLaps;

		private Text m_TrackOpponents;

		private Image m_TrackImage;

		private CanvasGroup m_CanvasVehicles;

		private Text m_VehicleName3D;

		private Text m_VehicleSpeed;

		private Text m_VehiclePower;

		private Text m_VehicleAcc;

		private Text m_VehicleDrive;

		private CanvasGroup m_CanvasSettings;

		private CanvasGroup m_CanvasVideoPanel;

		private CanvasGroup m_CanvasAudioPanel;

		private CanvasGroup m_CanvasControlPanelTouchDrive;

		private CanvasGroup m_CanvasControlPanelTouchDrivePro;

		private CanvasGroup m_CanvasControlPanelDesktop;

		private Toggle m_SettingsVideoQualityLow;

		private Toggle m_SettingsVideoQualityMed;

		private Toggle m_SettingsVideoQualityHigh;

		private Toggle m_SettingsVideoParticles;

		private Slider m_SettingsAudioMusic;

		private Slider m_SettingsAudioSFX;

		private Toggle m_SettingsControllerType1;

		private Toggle m_SettingsControllerType2;

		private Toggle m_SettingsControllerType3;

		private Toggle m_SettingsControllerType14;

		private Toggle m_SettingsControllerType15;

		private Slider m_SettingsControllerSensitivty;

		private int m_ConfigVideoQualityTemp;

		private int m_ConfigControlsTemp;

		private bool m_ConfigControlsFlipTemp;

		private float m_ConfigControlsSensitivityTemp;

		private int m_SelectedCarIndex;

		private int m_SelectedTrackIndex;

		private RGKUI_Database m_DataBase;

		private AsyncOperation m_AsyncLoadingProcess;

		private AudioSource m_AudioSourceBG;

		private AudioSource m_AudioUI;

		private void Awake()
		{
			InitializeUI();
		}

		private void Start()
		{
			SetUIValuesToControls();
			if (m_MenuBackgroundMusic != null)
			{
				m_AudioSourceBG = RGKUI_Utils.CreateAudioSource(base.transform, "audio_bg", Loop: true);
				m_AudioSourceBG.clip = m_MenuBackgroundMusic;
				m_AudioSourceBG.Play();
				m_AudioSourceBG.volume = RGKUI_StaticData.m_ConfigAudioMusic;
			}
			m_AudioUI = RGKUI_Utils.CreateAudioSource(base.transform, "audio_ui", Loop: false);
			m_AudioUI.volume = RGKUI_StaticData.m_ConfigAudioSFX;
			DedectPlatform();
			DedectGamePad();
		}

		private void Update()
		{
			if (m_AsyncLoadingProcess != null && m_ProgressLoading != null)
			{
				m_ProgressLoading.value = m_AsyncLoadingProcess.progress * 100f;
			}
		}

		private void DedectPlatform()
		{
			if (Application.isMobilePlatform)
			{
				m_IsMobile = true;
			}
		}

		private void DedectGamePad()
		{
			string[] joystickNames = Input.GetJoystickNames();
			if (joystickNames.Length > 0)
			{
				for (int i = 0; i < joystickNames.Length; i++)
				{
					if (joystickNames[i].Contains("Xbox 360"))
					{
						m_IsGamePadActive = true;
					}
				}
			}
			if (m_IsGamePadActive)
			{
				m_SettingsControllerType15.interactable = true;
			}
		}

		private void InitializeUI()
		{
			if (m_PnlVehicles != null)
			{
				m_CanvasVehicles = m_PnlVehicles.GetComponent<CanvasGroup>();
				m_VehicleName3D = m_PnlVehicles.gameObject.FindInChildren("txtVehicleName3D").GetComponent<Text>();
				m_VehicleSpeed = m_PnlVehicles.gameObject.FindInChildren("lblSpeedVal").GetComponent<Text>();
				m_VehiclePower = m_PnlVehicles.gameObject.FindInChildren("lblPowerval").GetComponent<Text>();
				m_VehicleAcc = m_PnlVehicles.gameObject.FindInChildren("lblAccVal").GetComponent<Text>();
				m_VehicleDrive = m_PnlVehicles.gameObject.FindInChildren("lblDrivetrainVal").GetComponent<Text>();
			}
			if (m_PnlSettings != null)
			{
				m_CanvasSettings = m_PnlSettings.GetComponent<CanvasGroup>();
				m_CanvasVideoPanel = m_PnlSettings.gameObject.FindInChildren("pnlVideoPanel").GetComponent<CanvasGroup>();
				m_CanvasAudioPanel = m_PnlSettings.gameObject.FindInChildren("pnlAudioPanel").GetComponent<CanvasGroup>();
				m_CanvasControlPanelTouchDrive = m_PnlSettings.gameObject.FindInChildren("pnlControlsPanelTouchDrive").GetComponent<CanvasGroup>();
				m_CanvasControlPanelTouchDrivePro = m_PnlSettings.gameObject.FindInChildren("pnlControlsPanelTouchDrivePro").GetComponent<CanvasGroup>();
				m_CanvasControlPanelDesktop = m_PnlSettings.gameObject.FindInChildren("pnlControlsPanelDesktop").GetComponent<CanvasGroup>();
				m_SettingsVideoQualityLow = m_PnlSettings.gameObject.FindInChildren("chkVideoLow").GetComponent<Toggle>();
				m_SettingsVideoQualityMed = m_PnlSettings.gameObject.FindInChildren("chkVideoMedium").GetComponent<Toggle>();
				m_SettingsVideoQualityHigh = m_PnlSettings.gameObject.FindInChildren("chkVideoHigh").GetComponent<Toggle>();
				m_SettingsVideoParticles = m_PnlSettings.gameObject.FindInChildren("chkVideoParticles").GetComponent<Toggle>();
				m_SettingsAudioMusic = m_PnlSettings.gameObject.FindInChildren("sliderSettingsAudioMusic").GetComponent<Slider>();
				m_SettingsAudioSFX = m_PnlSettings.gameObject.FindInChildren("sliderSettingsAudioSfx").GetComponent<Slider>();
				m_SettingsControllerType1 = m_PnlSettings.gameObject.FindInChildren("chkController1").GetComponent<Toggle>();
				m_SettingsControllerType2 = m_PnlSettings.gameObject.FindInChildren("chkController2").GetComponent<Toggle>();
				m_SettingsControllerType3 = m_PnlSettings.gameObject.FindInChildren("chkController3").GetComponent<Toggle>();
				m_SettingsControllerType14 = m_PnlSettings.gameObject.FindInChildren("chkController4").GetComponent<Toggle>();
				m_SettingsControllerType15 = m_PnlSettings.gameObject.FindInChildren("chkController5").GetComponent<Toggle>();
				m_SettingsControllerSensitivty = m_PnlSettings.gameObject.FindInChildren("sliderSettingsControlsSensitivity").GetComponent<Slider>();
			}
			if (m_PnlTracks != null)
			{
				m_CanvasTracks = m_PnlTracks.GetComponent<CanvasGroup>();
				m_RaceTitle = m_PnlTracks.gameObject.FindInChildren("lblRaceTitle").GetComponent<Text>();
				m_TrackName = m_PnlTracks.gameObject.FindInChildren("lblTrackName").GetComponent<Text>();
				m_RaceInfo = m_PnlTracks.gameObject.FindInChildren("lblRaceInfo").GetComponent<Text>();
				m_TrackRaceType = m_PnlTracks.gameObject.FindInChildren("lblTypeVal").GetComponent<Text>();
				m_TrackLaps = m_PnlTracks.gameObject.FindInChildren("lblLapsVal").GetComponent<Text>();
				m_TrackOpponents = m_PnlTracks.gameObject.FindInChildren("lblOppVal").GetComponent<Text>();
				m_TrackImage = m_PnlTracks.gameObject.FindInChildren("imgTrackImage").GetComponent<Image>();
			}
			if (m_PnlLoading != null)
			{
				m_CanvasLoading = m_PnlLoading.GetComponent<CanvasGroup>();
				m_ProgressLoading = m_PnlLoading.gameObject.FindInChildren("sliderProgress").GetComponent<Slider>();
			}
			if (m_PnlFrame != null)
			{
				m_CanvasFrame = m_PnlFrame.GetComponent<CanvasGroup>();
			}
			if (RGKUI_StaticData.m_FromRace)
			{
				m_SelectedTrackIndex = RGKUI_StaticData.m_SelectedTrackIndex;
				m_SelectedCarIndex = RGKUI_StaticData.m_SelectedCarIndex;
			}
			m_DataBase = GetComponent<RGKUI_Database>();
			if (m_DataBase != null)
			{
				LoadTrackDetails(m_SelectedTrackIndex);
				LoadCarDetails(m_SelectedCarIndex);
			}
		}

		private void SetUIValuesToControls()
		{
			switch (RGKUI_StaticData.m_ConfigVideoQuality)
			{
			case 1:
				m_SettingsVideoQualityLow.isOn = true;
				break;
			case 2:
				m_SettingsVideoQualityMed.isOn = true;
				break;
			case 3:
				m_SettingsVideoQualityHigh.isOn = true;
				break;
			}
			switch (RGKUI_StaticData.m_ConfigControl)
			{
			case 1:
				m_SettingsControllerType1.isOn = true;
				m_SettingsControllerType2.isOn = false;
				m_SettingsControllerType3.isOn = false;
				break;
			case 2:
			case 4:
			case 5:
				m_SettingsControllerType2.isOn = true;
				m_SettingsControllerType1.isOn = false;
				m_SettingsControllerType3.isOn = false;
				break;
			case 3:
				m_SettingsControllerType3.isOn = true;
				m_SettingsControllerType2.isOn = false;
				m_SettingsControllerType1.isOn = false;
				break;
			}
			if (RGKUI_StaticData.m_ConfigControlEnableGamepad && m_IsGamePadActive)
			{
				m_SettingsControllerType14.isOn = false;
				m_SettingsControllerType15.isOn = true;
			}
			else
			{
				m_SettingsControllerType14.isOn = true;
				m_SettingsControllerType15.isOn = false;
			}
			m_SettingsVideoParticles.isOn = RGKUI_StaticData.m_ConfigParticles;
			m_SettingsAudioMusic.value = RGKUI_StaticData.m_ConfigAudioMusic;
			m_SettingsAudioSFX.value = RGKUI_StaticData.m_ConfigAudioSFX;
			m_SettingsControllerSensitivty.value = RGKUI_StaticData.m_ConfigControlSensitivity;
		}

		public void ShowCarSelectionPanel()
		{
			if (m_CanvasVehicles != null)
			{
				m_CanvasVehicles.alpha = 1f;
				m_CanvasVehicles.blocksRaycasts = true;
				m_CanvasVehicles.interactable = true;
			}
			if (m_CanvasTracks != null)
			{
				m_CanvasTracks.alpha = 0f;
				m_CanvasTracks.blocksRaycasts = false;
				m_CanvasTracks.interactable = false;
			}
		}

		public void ShowTrackSelectionPanel()
		{
			if (m_CanvasTracks != null)
			{
				m_CanvasTracks.alpha = 1f;
				m_CanvasTracks.blocksRaycasts = true;
				m_CanvasTracks.interactable = true;
			}
			if (m_CanvasVehicles != null)
			{
				m_CanvasVehicles.alpha = 0f;
				m_CanvasVehicles.blocksRaycasts = false;
				m_CanvasVehicles.interactable = false;
			}
		}

		public void ShowLoadRaceLevel()
		{
			HideAllPanels();
			if (m_CanvasLoading != null)
			{
				m_CanvasLoading.alpha = 1f;
				m_CanvasLoading.blocksRaycasts = true;
				m_CanvasLoading.interactable = true;
			}
			LoadSelectedLevel();
		}

		private void HideAllPanels()
		{
			if (m_CanvasTracks != null)
			{
				m_CanvasTracks.alpha = 0f;
				m_CanvasTracks.blocksRaycasts = false;
				m_CanvasTracks.interactable = false;
			}
			if (m_CanvasVehicles != null)
			{
				m_CanvasVehicles.alpha = 0f;
				m_CanvasVehicles.blocksRaycasts = false;
				m_CanvasVehicles.interactable = false;
			}
			if (m_CanvasFrame != null)
			{
				m_CanvasFrame.alpha = 0f;
				m_CanvasFrame.blocksRaycasts = false;
				m_CanvasFrame.interactable = false;
			}
		}

		public void ShowSettingsPanel()
		{
			if (m_CanvasSettings != null)
			{
				m_CanvasSettings.alpha = 1f;
				m_CanvasSettings.blocksRaycasts = true;
				m_CanvasSettings.interactable = true;
				ShowSettingsPanel(1);
			}
		}

		public void ShowSettingsPanel(int Panel)
		{
			switch (Panel)
			{
			case 1:
				m_CanvasVideoPanel.alpha = 1f;
				m_CanvasVideoPanel.blocksRaycasts = true;
				m_CanvasVideoPanel.interactable = true;
				m_CanvasAudioPanel.alpha = 0f;
				m_CanvasAudioPanel.blocksRaycasts = false;
				m_CanvasAudioPanel.interactable = false;
				m_CanvasControlPanelTouchDrive.alpha = 0f;
				m_CanvasControlPanelTouchDrive.blocksRaycasts = false;
				m_CanvasControlPanelTouchDrive.interactable = false;
				m_CanvasControlPanelTouchDrivePro.alpha = 0f;
				m_CanvasControlPanelTouchDrivePro.blocksRaycasts = false;
				m_CanvasControlPanelTouchDrivePro.interactable = false;
				m_CanvasControlPanelDesktop.alpha = 0f;
				m_CanvasControlPanelDesktop.blocksRaycasts = false;
				m_CanvasControlPanelDesktop.interactable = false;
				break;
			case 2:
				m_CanvasAudioPanel.alpha = 1f;
				m_CanvasAudioPanel.blocksRaycasts = true;
				m_CanvasAudioPanel.interactable = true;
				m_CanvasVideoPanel.alpha = 0f;
				m_CanvasVideoPanel.blocksRaycasts = false;
				m_CanvasVideoPanel.interactable = false;
				m_CanvasControlPanelTouchDrive.alpha = 0f;
				m_CanvasControlPanelTouchDrive.blocksRaycasts = false;
				m_CanvasControlPanelTouchDrive.interactable = false;
				m_CanvasControlPanelTouchDrivePro.alpha = 0f;
				m_CanvasControlPanelTouchDrivePro.blocksRaycasts = false;
				m_CanvasControlPanelTouchDrivePro.interactable = false;
				m_CanvasControlPanelDesktop.alpha = 0f;
				m_CanvasControlPanelDesktop.blocksRaycasts = false;
				m_CanvasControlPanelDesktop.interactable = false;
				break;
			case 3:
				if (m_IsMobile && !m_UseTouchDrivePro)
				{
					m_CanvasControlPanelTouchDrive.alpha = 1f;
					m_CanvasControlPanelTouchDrive.blocksRaycasts = true;
					m_CanvasControlPanelTouchDrive.interactable = true;
					m_CanvasControlPanelDesktop.alpha = 0f;
					m_CanvasControlPanelDesktop.blocksRaycasts = false;
					m_CanvasControlPanelDesktop.interactable = false;
				}
				else if (m_IsMobile && m_UseTouchDrivePro)
				{
					m_CanvasControlPanelTouchDrivePro.alpha = 1f;
					m_CanvasControlPanelTouchDrivePro.blocksRaycasts = true;
					m_CanvasControlPanelTouchDrivePro.interactable = true;
					m_CanvasControlPanelDesktop.alpha = 0f;
					m_CanvasControlPanelDesktop.blocksRaycasts = false;
					m_CanvasControlPanelDesktop.interactable = false;
				}
				else
				{
					m_CanvasControlPanelTouchDrive.alpha = 0f;
					m_CanvasControlPanelTouchDrive.blocksRaycasts = false;
					m_CanvasControlPanelTouchDrive.interactable = false;
					m_CanvasControlPanelDesktop.alpha = 1f;
					m_CanvasControlPanelDesktop.blocksRaycasts = true;
					m_CanvasControlPanelDesktop.interactable = true;
				}
				m_CanvasVideoPanel.alpha = 0f;
				m_CanvasVideoPanel.blocksRaycasts = false;
				m_CanvasVideoPanel.interactable = false;
				m_CanvasAudioPanel.alpha = 0f;
				m_CanvasAudioPanel.blocksRaycasts = false;
				m_CanvasAudioPanel.interactable = false;
				break;
			}
		}

		public void PlayUISound(int ClipIndex)
		{
			if (m_UiAudioClips.Length > 0 && ClipIndex > -1 && ClipIndex < m_UiAudioClips.Length)
			{
				m_AudioUI.PlayOneShot(m_UiAudioClips[ClipIndex]);
			}
		}

		private void LoadSelectedLevel()
		{
			RGKUI_StaticData.m_SelectedTrackIndex = m_SelectedTrackIndex;
			RGKUI_StaticData.m_CurrentRaceLaps = m_DataBase.RaceData[m_SelectedTrackIndex].RaceLaps;
			RGKUI_StaticData.m_CurrentRaceTypeEnum = m_DataBase.RaceData[m_SelectedTrackIndex].TrackRaceTypeEnum;
			RGKUI_StaticData.m_CurrentRaceSpeedTrapEnum = m_DataBase.RaceData[m_SelectedTrackIndex].SpeedTrapEnum;
			if (m_DataBase.RaceData[m_SelectedTrackIndex].TrackAiIndexes != string.Empty)
			{
				string[] array = m_DataBase.RaceData[m_SelectedTrackIndex].TrackAiIndexes.Split(',');
				int num = array.Length;
				ChangeAIArraySize(num);
				for (int i = 0; i < num; i++)
				{
					RGKUI_StaticData.m_CurrentRaceAis[i] = m_DataBase.AiData[Convert.ToInt16(array[i])].AiPrefab;
				}
			}
			RGKUI_StaticData.m_SelectedCarIndex = m_SelectedCarIndex;
			RGKUI_StaticData.m_FromMain = true;
			RGKUI_StaticData.m_SelectedVehiclePrefab = m_DataBase.CarData[m_SelectedCarIndex].PlayerPrefab;
			StartCoroutine(LoadLevel(m_DataBase.RaceData[m_SelectedTrackIndex].TrackIndex));
		}

		private void ChangeAIArraySize(int Amount)
		{
			GameObject[] array = RGKUI_StaticData.m_CurrentRaceAis = new GameObject[Amount];
		}

		private IEnumerator LoadLevel(int TrackIndex)
		{
			m_AsyncLoadingProcess = SceneManager.LoadSceneAsync(TrackIndex);
			yield return m_AsyncLoadingProcess;
		}

		public void UI_QuitApp()
		{
			Application.Quit();
		}

		private void LoadCarDetails(int SelectedIndex)
		{
			if (m_DataBase.CarData != null && m_DataBase.CarData.Length > 0)
			{
				RGKUI_CarData rGKUI_CarData = m_DataBase.CarData[SelectedIndex];
				m_VehicleName3D.text = rGKUI_CarData.CarName.Replace("||", "\r\n");
				m_VehicleSpeed.text = rGKUI_CarData.CarSpeed;
				m_VehiclePower.text = rGKUI_CarData.CarPower;
				m_VehicleAcc.text = rGKUI_CarData.CarAcc;
				m_VehicleDrive.text = rGKUI_CarData.CarDrive;
				if (m_ShowCaseCarInScreen != null)
				{
					UnityEngine.Object.DestroyImmediate(m_ShowCaseCarInScreen);
				}
				if (rGKUI_CarData.ShowcasePrefab != null)
				{
					m_ShowCaseCarInScreen = UnityEngine.Object.Instantiate(rGKUI_CarData.ShowcasePrefab);
					m_ShowCaseCarInScreen.transform.position = m_ShowCaseCarLocation.position;
					m_ShowCaseCarInScreen.transform.rotation = m_ShowCaseCarLocation.rotation;
				}
			}
		}

		public void ShowNextCar()
		{
			if (m_DataBase != null && m_SelectedCarIndex + 1 <= m_DataBase.CarData.Length - 1)
			{
				m_SelectedCarIndex++;
				LoadCarDetails(m_SelectedCarIndex);
			}
		}

		public void ShowPrevCar()
		{
			if (m_DataBase != null && m_SelectedCarIndex > 0)
			{
				m_SelectedCarIndex--;
				LoadCarDetails(m_SelectedCarIndex);
			}
		}

		private void LoadTrackDetails(int SelectedIndex)
		{
			if (m_DataBase.RaceData != null && m_DataBase.RaceData.Length > 0)
			{
				RGKUI_RaceData rGKUI_RaceData = m_DataBase.RaceData[SelectedIndex];
				m_RaceTitle.text = rGKUI_RaceData.RaceTitle;
				m_TrackName.text = rGKUI_RaceData.TrackName;
				m_RaceInfo.text = rGKUI_RaceData.RaceInfo;
				m_TrackRaceType.text = rGKUI_RaceData.RaceType;
				m_TrackLaps.text = rGKUI_RaceData.RaceLaps.ToString();
				m_TrackOpponents.text = rGKUI_RaceData.Opponents.ToString();
				m_TrackImage.sprite = rGKUI_RaceData.TrackSprite;
			}
		}

		public void ShowNextTrack()
		{
			if (m_DataBase != null && m_SelectedTrackIndex + 1 <= m_DataBase.RaceData.Length - 1)
			{
				m_SelectedTrackIndex++;
				LoadTrackDetails(m_SelectedTrackIndex);
			}
		}

		public void ShowPrevTrack()
		{
			if (m_DataBase != null && m_SelectedTrackIndex > 0)
			{
				m_SelectedTrackIndex--;
				LoadTrackDetails(m_SelectedTrackIndex);
			}
		}

		public void SaveAndCloseSettingsPanel()
		{
			if (m_CanvasSettings != null)
			{
				m_CanvasSettings.alpha = 0f;
				m_CanvasSettings.blocksRaycasts = false;
				m_CanvasSettings.interactable = false;
				RGKUI_StaticData.m_ConfigVideoQuality = m_ConfigVideoQualityTemp;
				RGKUI_StaticData.m_ConfigParticles = m_SettingsVideoParticles.isOn;
				RGKUI_StaticData.m_ConfigAudioMusic = m_SettingsAudioMusic.value;
				RGKUI_StaticData.m_ConfigAudioSFX = m_SettingsAudioSFX.value;
				RGKUI_StaticData.m_ConfigControl = m_ConfigControlsTemp;
				RGKUI_StaticData.m_ConfigControlSensitivity = m_ConfigControlsSensitivityTemp;
				RGKUI_StaticData.m_ConfigControlsFlipped = m_ConfigControlsFlipTemp;
				if (m_ConfigControlsTemp == 4)
				{
					RGKUI_StaticData.m_ConfigControlEnableGamepad = false;
				}
				else if (m_ConfigControlsTemp == 5)
				{
					RGKUI_StaticData.m_ConfigControlEnableGamepad = true;
				}
				if (m_AudioSourceBG != null)
				{
					m_AudioSourceBG.volume = RGKUI_StaticData.m_ConfigAudioMusic;
				}
				if (m_AudioUI != null)
				{
					m_AudioUI.volume = RGKUI_StaticData.m_ConfigAudioSFX;
				}
				SetQuality(m_ConfigVideoQualityTemp);
			}
		}

		private void SetQuality(int QualityIndex)
		{
			QualitySettings.SetQualityLevel(QualityIndex, applyExpensiveChanges: false);
		}

		public void SetSettingsVideoQualityTemp(int Selected)
		{
			m_ConfigVideoQualityTemp = Selected;
		}

		public void SetSettingsControllerTemp(int Selected)
		{
			m_ConfigControlsTemp = Selected;
		}

		public void SetSettingsFlipControls(bool Value)
		{
			m_ConfigControlsFlipTemp = Value;
		}

		public void SetSettingsControlsSensitivity(float Value)
		{
			m_ConfigControlsSensitivityTemp = Value;
		}
	}
}
