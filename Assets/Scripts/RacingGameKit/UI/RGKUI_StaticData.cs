using UnityEngine;

namespace RacingGameKit.UI
{
	public static class RGKUI_StaticData
	{
		public static GameObject m_SelectedVehiclePrefab;

		public static int m_SelectedCarIndex;

		public static int m_SelectedTrackIndex;

		public static GameObject[] m_CurrentRaceAis;

		public static int m_CurrentRaceLaps;

		public static RaceTypeEnum m_CurrentRaceTypeEnum;

		public static eSpeedTrapMode m_CurrentRaceSpeedTrapEnum;

		public static int m_ConfigVideoQuality = 1;

		public static bool m_ConfigParticles = true;

		public static float m_ConfigAudioMusic = 0.7f;

		public static float m_ConfigAudioSFX = 0.8f;

		public static int m_ConfigControl = 2;

		public static bool m_ConfigControlsFlipped;

		public static float m_ConfigControlSensitivity = 1f;

		public static bool m_ConfigControlEnableGamepad;

		public static bool m_FromRace;

		public static bool m_FromMain;
	}
}
