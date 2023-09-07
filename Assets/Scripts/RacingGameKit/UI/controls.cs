using UnityEngine;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Main Menu UI")]
	[RequireComponent(typeof(RGKUI_Database))]
	public class controls : MonoBehaviour
	{
		public bool m_IsMobile;

		public bool m_UseTouchDrivePro;

		private bool m_IsGamePadActive;

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

		private void Start()
		{
			m_AudioUI = RGKUI_Utils.CreateAudioSource(base.transform, "audio_ui", Loop: false);
			m_AudioUI.volume = RGKUI_StaticData.m_ConfigAudioSFX;
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
				Object.FindObjectOfType<RGKUI_PauseMenu>().SetTouchDriveControls(RGKUI_StaticData.m_ConfigControl, RGKUI_StaticData.m_ConfigControlsFlipped);
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
