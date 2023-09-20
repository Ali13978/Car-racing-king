using RacingGameKit.TouchDrive;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Pause Menu UI")]
	public class RGKUI_PauseMenu : RGKPauseMenuBase
	{
		private bool m_IsMobile;

		private bool m_IsTDPro;

		private bool m_IsGamePadActive;

		private Race_Manager m_RaceManager;

		private Race_Audio m_RaceAudio;

		private iRGKTDM m_TouchDrive;

        public GameObject m_TouchDriveManager;

		public GameObject m_PnlHud;

		public GameObject m_PnlPause;

		public GameObject m_PnlSettings;

		private CanvasGroup m_CanvasHud;

		private CanvasGroup m_CanvasPause;

		private CanvasGroup m_CanvasSettings;

		private CanvasGroup m_CanvasTouchDrive;

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

		private Toggle m_SettingsControllerType14;

		private Toggle m_SettingsControllerType15;

		private Slider m_SettingsControllerSensitivty;

		private Toggle m_SettingsControlsFlip;

		private int m_ConfigVideoQualityTemp;

		private bool m_ConfigParticlesTemp;

		private int m_ConfigControlsTemp;

		private bool m_ConfigControlsFlipTemp;

		private float m_ConfigControlsSensitivityTemp;

		private bool m_isPaused;

		public override bool IsPaused
		{
			get
			{
				return m_isPaused;
			}
			set
			{
				m_isPaused = value;
			}
		}

		private void Awake()
		{
			GameObject gameObject = GameObject.Find("_RaceManager");
			if (gameObject != null)
			{
				m_RaceManager = gameObject.GetComponent<Race_Manager>();
			}
			if (m_RaceManager == null)
			{
				UnityEngine.Debug.LogError("RaceManager not assigned to PauseMenu!");
			}
			else
			{
				m_RaceAudio = m_RaceManager.GetComponent<Race_Audio>();
				m_RaceManager.OnRaceInitiated += RaceManager_OnRaceInitiated;
				if (RGKUI_StaticData.m_FromMain)
				{
					m_RaceManager.RaceInitsOnStartup = false;
					m_RaceManager.RaceStartsOnStartup = false;
				}
				RGKTDM rGKTDM = (RGKTDM)Object.FindObjectOfType(typeof(RGKTDM));
				if (rGKTDM != null)
				{
					m_TouchDrive = (iRGKTDM)rGKTDM.gameObject.GetComponent(typeof(iRGKTDM));
					m_IsTDPro = m_TouchDrive.IsPro;
				}
			}
			RGKUI_StaticData.m_ConfigVideoQuality = 3;
			InitializeUIStuff();
			DedectPlatform();
			DedectGamePad();
			SetUIValuesToControls();
		}

		private void Start()
		{
			m_CanvasHud.alpha = 1f;
			m_CanvasPause.alpha = 0f;
			m_CanvasPause.blocksRaycasts = false;
			m_CanvasPause.interactable = false;
			ShowSettingsPanel(1);
			if (RGKUI_StaticData.m_FromMain && m_RaceManager != null)
			{
				m_RaceManager.AiSpawnMode = eAISpawnMode.OneTimeEach;
				m_RaceManager.AiNamingMode = eAINamingMode.Random;
				m_RaceManager.AiSpawnOrder = eAISpawnOrder.Order;
				if (RGKUI_StaticData.m_CurrentRaceTypeEnum == RaceTypeEnum.Speedtrap || RGKUI_StaticData.m_CurrentRaceTypeEnum == RaceTypeEnum.TimeAttack)
				{
					m_RaceManager.EnableCheckpointArrow = true;
					if (!m_RaceManager.CheckPoints.activeInHierarchy)
					{
						m_RaceManager.CheckPoints.gameObject.SetActive(value: true);
					}
				}
				else
				{
					m_RaceManager.EnableCheckpointArrow = false;
					if (m_RaceManager.CheckPoints.activeInHierarchy)
					{
						m_RaceManager.CheckPoints.gameObject.SetActive(value: false);
					}
				}
				m_RaceManager.RaceType = RGKUI_StaticData.m_CurrentRaceTypeEnum;
				m_RaceManager.SpeedTrapMode = RGKUI_StaticData.m_CurrentRaceSpeedTrapEnum;
				m_RaceManager.RaceLaps = RGKUI_StaticData.m_CurrentRaceLaps;
				if (RGKUI_StaticData.m_CurrentRaceAis != null)
				{
					int num = RGKUI_StaticData.m_CurrentRaceAis.Length;
					ChangeAIArraySize(num);
					for (int i = 0; i < num; i++)
					{
						m_RaceManager.AIRacerPrefab[i] = RGKUI_StaticData.m_CurrentRaceAis[i];
					}
					m_RaceManager.RacePlayers = num + 1;
				}
				else
				{
					m_RaceManager.RacePlayers = 1;
				}
				if (RGKUI_StaticData.m_SelectedVehiclePrefab != null)
				{
					m_RaceManager.HumanRacerPrefab = RGKUI_StaticData.m_SelectedVehiclePrefab;
				}
				m_RaceManager.InitRace();
				if (m_RaceAudio != null)
				{
					m_RaceAudio.BackgroundMusicVolume = RGKUI_StaticData.m_ConfigAudioMusic;
					m_RaceAudio.EffectsSoundVolume = RGKUI_StaticData.m_ConfigAudioSFX;
					m_RaceAudio.EngineSoundVolume = RGKUI_StaticData.m_ConfigAudioSFX;
				}
				SetTouchDriveControls(RGKUI_StaticData.m_ConfigControl, RGKUI_StaticData.m_ConfigControlsFlipped);
				if (!Application.isMobilePlatform)
				{
					SwitchDesktopControl(RGKUI_StaticData.m_ConfigControlEnableGamepad);
				}
			}
			else if (m_TouchDrive != null)
			{
				SetTouchDriveControls(2, IsFlipped: false);
				if (!m_RaceManager.RaceInitsOnStartup)
				{
					m_RaceManager.InitRace();
				}
			}
		}

		private void ChangeAIArraySize(int Amount)
		{
			GameObject[] aIRacerPrefab = new GameObject[Amount];
			m_RaceManager.AIRacerPrefab = aIRacerPrefab;
		}

		private void RaceManager_OnRaceInitiated()
		{
            if (PhotonNetworkManager.isMultiplayer)
                return;
			m_RaceManager.StartRace();
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

		private void SwitchDesktopControl(bool EnableGamePad)
		{
			RGKCarInputSwitcher component = m_RaceManager.Player1.GetComponent<RGKCarInputSwitcher>();
			if (component != null)
			{
				if (EnableGamePad && m_IsGamePadActive)
				{
					component.SwitchController(EnableGamePad);
				}
				else
				{
					component.SwitchController(ToGamePad: false);
				}
			}
		}

		public void SetTouchDriveControls(int Config, bool IsFlipped)
		{
			if (m_TouchDrive != null)
			{
				if (m_TouchDrive.IsPro)
				{
					m_TouchDrive.FlipPositions = IsFlipped;
				}
				switch (Config)
				{
				case 1:
					m_TouchDrive.SwitchTemplate(20);
					break;
				case 2:
					m_TouchDrive.SwitchTemplate(22);
					break;
				case 3:
					m_TouchDrive.SwitchTemplate(23);
					break;
				case 4:
					m_TouchDrive.SwitchTemplate(21);
					break;
				case 5:
					m_TouchDrive.SwitchTemplate(0);
					break;
				case 6:
					m_TouchDrive.SwitchTemplate(1);
					break;
				case 7:
					m_TouchDrive.SwitchTemplate(2);
					break;
				case 8:
					m_TouchDrive.SwitchTemplate(10);
					break;
				case 9:
					m_TouchDrive.SwitchTemplate(11);
					break;
				case 10:
					m_TouchDrive.SwitchTemplate(12);
					break;
				}
				m_TouchDrive.SteeringSensitivity = RGKUI_StaticData.m_ConfigControlSensitivity;
			}
		}

		private void Update()
		{
			if (m_TouchDrive != null)
			{
				if ((m_TouchDrive.TouchItems[10].IsPressed || Input.GetButtonUp("Cancel") || UnityEngine.Input.GetKeyUp(KeyCode.Escape)) && !IsPaused)
				{
					PauseGame();
				}
				else if (UnityEngine.Input.GetKeyUp(KeyCode.Escape) && IsPaused)
				{
					ResumeGame();
				}
			}
			else if ((Input.GetButtonUp("Cancel") || UnityEngine.Input.GetKeyUp(KeyCode.Escape)) && !IsPaused)
			{
				PauseGame();
			}
			else if (UnityEngine.Input.GetKeyUp(KeyCode.Escape) && IsPaused)
			{
				ResumeGame();
			}
		}

		private void InitializeUIStuff()
		{
			if (m_PnlHud != null)
			{
				m_CanvasHud = m_PnlHud.GetComponent<CanvasGroup>();
			}
			if (m_PnlPause != null)
			{
				m_CanvasPause = m_PnlPause.GetComponent<CanvasGroup>();
			}
			if (m_TouchDriveManager != null)
			{
				m_CanvasTouchDrive = m_TouchDriveManager.GetComponent<CanvasGroup>();
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
				m_SettingsControllerType14 = m_PnlSettings.gameObject.FindInChildren("chkController14").GetComponent<Toggle>();
				m_SettingsControllerType15 = m_PnlSettings.gameObject.FindInChildren("chkController15").GetComponent<Toggle>();
				if (m_CanvasControlPanelTouchDrivePro != null)
				{
					m_SettingsControllerSensitivty = m_CanvasControlPanelTouchDrivePro.gameObject.FindInChildren("sliderSettingsControlsSensitivity").GetComponent<Slider>();
					m_SettingsControlsFlip = m_CanvasControlPanelTouchDrivePro.gameObject.FindInChildren("chkTDProFlip").GetComponent<Toggle>();
				}
				else if (m_CanvasControlPanelTouchDrive != null)
				{
					m_SettingsControllerSensitivty = m_CanvasControlPanelTouchDrive.gameObject.FindInChildren("sliderSettingsControlsSensitivity").GetComponent<Slider>();
				}
			}
		}

		private void SetUIValuesToControls()
		{
			switch (RGKUI_StaticData.m_ConfigVideoQuality)
			{
			case 1:
				m_SettingsVideoQualityLow.isOn = true;
				m_SettingsVideoQualityMed.isOn = false;
				m_SettingsVideoQualityHigh.isOn = false;
				QualitySettings.SetQualityLevel(1, applyExpensiveChanges: false);
				break;
			case 2:
				m_SettingsVideoQualityMed.isOn = true;
				m_SettingsVideoQualityLow.isOn = false;
				m_SettingsVideoQualityHigh.isOn = false;
				QualitySettings.SetQualityLevel(2, applyExpensiveChanges: false);
				break;
			case 3:
				m_SettingsVideoQualityHigh.isOn = true;
				m_SettingsVideoQualityMed.isOn = false;
				m_SettingsVideoQualityLow.isOn = false;
				QualitySettings.SetQualityLevel(3, applyExpensiveChanges: false);
				break;
			}
			for (int i = 1; i <= 14; i++)
			{
				bool isOn = false;
				if (RGKUI_StaticData.m_ConfigControl == i)
				{
					isOn = true;
				}
				m_PnlSettings.gameObject.FindInChildren("chkController" + i + string.Empty).GetComponent<Toggle>().isOn = isOn;
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
			if (m_SettingsControllerSensitivty != null)
			{
				m_SettingsControllerSensitivty.value = RGKUI_StaticData.m_ConfigControlSensitivity;
			}
			if (m_SettingsControlsFlip != null)
			{
				m_SettingsControlsFlip.isOn = RGKUI_StaticData.m_ConfigControlsFlipped;
			}
			m_RaceAudio.BackgroundMusicVolume = RGKUI_StaticData.m_ConfigAudioMusic;
			m_RaceAudio.EffectsSoundVolume = RGKUI_StaticData.m_ConfigAudioSFX;
			m_RaceAudio.EngineSoundVolume = RGKUI_StaticData.m_ConfigAudioSFX;
		}

		public override void PauseGame()
		{
			if (!m_isPaused)
			{
				m_RaceAudio.Mute = true;
				m_CanvasHud.alpha = 0f;
				m_CanvasPause.alpha = 1f;
				m_CanvasPause.interactable = true;
				m_CanvasPause.blocksRaycasts = true;
				if (m_CanvasTouchDrive != null)
				{
					m_CanvasTouchDrive.alpha = 0f;
					m_CanvasTouchDrive.blocksRaycasts = false;
					m_CanvasTouchDrive.interactable = false;
				}
                if (!PhotonNetworkManager.isMultiplayer)
                    Time.timeScale = 0f;
				m_isPaused = true;
			}
		}

		public override void ResumeGame()
		{
			if (m_isPaused)
			{
				m_RaceAudio.Mute = false;
				m_CanvasHud.alpha = 1f;
				m_CanvasPause.alpha = 0f;
				m_CanvasPause.blocksRaycasts = false;
				m_CanvasPause.interactable = false;
				Time.timeScale = 1f;
				if (m_CanvasTouchDrive != null)
				{
					m_CanvasTouchDrive.alpha = 1f;
					m_CanvasTouchDrive.blocksRaycasts = true;
					m_CanvasTouchDrive.interactable = true;
				}
				m_isPaused = false;
			}
		}

		public void UI_Restart()
		{
			ResumeGame();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void UI_ExitToMain()
		{
			ResumeGame();
			RGKUI_StaticData.m_FromRace = true;
			SceneManager.LoadScene(0);
		}

		public void UI_ShowSettings()
		{
			if (m_CanvasSettings != null)
			{
				MonoBehaviour.print("setting button is pressed ");
				m_CanvasSettings.alpha = 1f;
				m_CanvasSettings.blocksRaycasts = true;
				m_CanvasSettings.interactable = true;
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
				if (m_IsMobile && !m_IsTDPro)
				{
					m_CanvasControlPanelTouchDrive.alpha = 1f;
					m_CanvasControlPanelTouchDrive.blocksRaycasts = true;
					m_CanvasControlPanelTouchDrive.interactable = true;
					m_CanvasControlPanelDesktop.alpha = 0f;
					m_CanvasControlPanelDesktop.blocksRaycasts = false;
					m_CanvasControlPanelDesktop.interactable = false;
				}
				else if (m_IsMobile && m_IsTDPro)
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

		public void SaveAndCloseSettingsPanel()
		{
			if (!(m_CanvasSettings != null))
			{
				return;
			}
			m_CanvasSettings.alpha = 0f;
			m_CanvasSettings.blocksRaycasts = false;
			m_CanvasSettings.interactable = false;
			RGKUI_StaticData.m_ConfigVideoQuality = m_ConfigVideoQualityTemp;
			RGKUI_StaticData.m_ConfigParticles = m_ConfigParticlesTemp;
			RGKUI_StaticData.m_ConfigAudioMusic = m_SettingsAudioMusic.value;
			RGKUI_StaticData.m_ConfigAudioSFX = m_SettingsAudioSFX.value;
			RGKUI_StaticData.m_ConfigControl = m_ConfigControlsTemp;
			RGKUI_StaticData.m_ConfigControlsFlipped = m_ConfigControlsFlipTemp;
			RGKUI_StaticData.m_ConfigControlSensitivity = m_ConfigControlsSensitivityTemp;
			m_RaceAudio.BackgroundMusicVolume = RGKUI_StaticData.m_ConfigAudioMusic;
			m_RaceAudio.EffectsSoundVolume = RGKUI_StaticData.m_ConfigAudioSFX;
			m_RaceAudio.EngineSoundVolume = RGKUI_StaticData.m_ConfigAudioSFX;
			SetQuality(m_ConfigVideoQualityTemp);
			SetTouchDriveControls(RGKUI_StaticData.m_ConfigControl, RGKUI_StaticData.m_ConfigControlsFlipped);
			if (!Application.isMobilePlatform)
			{
				if (m_ConfigControlsTemp == 15)
				{
					RGKUI_StaticData.m_ConfigControlEnableGamepad = true;
					SwitchDesktopControl(EnableGamePad: true);
				}
				else if (m_ConfigControlsTemp == 14)
				{
					RGKUI_StaticData.m_ConfigControlEnableGamepad = false;
					SwitchDesktopControl(EnableGamePad: false);
				}
			}
		}

		private void SetQuality(int QualityIndex)
		{
			QualitySettings.SetQualityLevel(QualityIndex, applyExpensiveChanges: false);
		}

		public void SetSettingsParticles(bool Value)
		{
			m_ConfigParticlesTemp = Value;
		}

		public void SetSettingsFlipControls(bool Value)
		{
			m_ConfigControlsFlipTemp = Value;
		}

		public void SetSettingsVideoQualityTemp(int Selected)
		{
			m_ConfigVideoQualityTemp = Selected;
		}

		public void SetSettingsControllerTemp(int Selected)
		{
			m_ConfigControlsTemp = Selected;
		}

		public void SetSettingsControlsSensitivity(float Value)
		{
			m_ConfigControlsSensitivityTemp = Value;
		}
	}
}
