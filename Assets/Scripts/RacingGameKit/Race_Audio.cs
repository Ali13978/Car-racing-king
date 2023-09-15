using RacingGameKit.EventHandlers;
using RacingGameKit.Interfaces;
using System;
using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Game Mechanics/Race Audio Manager")]
	[RequireComponent(typeof(Race_Manager))]
	public class Race_Audio : MonoBehaviour, IRGKRaceAudio
	{
		private GameObject GameCamera;

		public AudioClip RaceCountDown;

		public AudioClip RaceCheckpoint;

		public AudioClip RaceWrongCheckpoint;

		public AudioClip RaceStart;

		public AudioClip RaceWon;

		public AudioClip RaceLost;

		public AudioClip[] BackgroundMusic;

		public bool RandomBackground = true;

		public bool LoopBackground = true;

		public float BackgroundMusicVolume = 0.75f;

		private float BackgroundMusicVolumeTemp;

		public float EngineSoundVolume = 1f;

		private float EngineSoundVolumeTemp;

		public float EffectsSoundVolume = 1f;

		private float EffectsSoundVolumeTemp;

		public bool m_MuteAllSounds;

		private AudioSource m_BackgroundMusicPlayer;

		private AudioSource RaceSoundsPlayer;

		private bool IsBackgroundMusicStarted;

		private int LastPlayedIndex;

		private bool IsAudioEnabled = true;

		private Race_Manager RaceManager;

		public AudioSource BackgroundMusicAudioSource => m_BackgroundMusicPlayer;

		public bool PlayBackgroundMusic
		{
			set
			{
				IsBackgroundMusicStarted = value;
			}
		}

		[Obsolete("This property is obsolete. Use Mute instead.", false)]
		public bool MuteAllSounds
		{
			get
			{
				return m_MuteAllSounds;
			}
			set
			{
				SetMute(value);
			}
		}

		public bool Mute
		{
			get
			{
				return m_MuteAllSounds;
			}
			set
			{
				SetMute(value);
			}
		}

		public event AudioMuteChangedEventHandler OnMuteChanged;

		public event AudioFxVolumeChangedEventHandler OnFxVolumeChanged;

		public event AudioEngineVolumeChangedEventHandler OnEngineVolumeChanged;

		public event AudioBgMusicVolumeChangedEventHandler OnBgMusicVolumeChanged;

		private void Awake()
		{
			RaceManager = GetComponent<Race_Manager>();
		}

		private void FixedUpdate()
		{
			try
			{
				if (BackgroundMusicVolumeTemp != BackgroundMusicVolume)
				{
					if (this.OnBgMusicVolumeChanged != null)
					{
						this.OnBgMusicVolumeChanged(BackgroundMusicVolume);
					}
					BackgroundMusicVolumeTemp = BackgroundMusicVolume;
				}
				if (EngineSoundVolumeTemp != EngineSoundVolume)
				{
					if (this.OnEngineVolumeChanged != null)
					{
						this.OnEngineVolumeChanged(EngineSoundVolume);
					}
					EngineSoundVolumeTemp = EngineSoundVolume;
				}
				if (EffectsSoundVolumeTemp != EffectsSoundVolume)
				{
					if (this.OnFxVolumeChanged != null)
					{
						this.OnFxVolumeChanged(EffectsSoundVolume);
					}
					EffectsSoundVolumeTemp = EffectsSoundVolume;
				}
				if (IsAudioEnabled)
				{
					if (!m_BackgroundMusicPlayer.isPlaying && IsBackgroundMusicStarted && BackgroundMusic != null && BackgroundMusic.GetLength(0) > 0)
					{
						if (RandomBackground)
						{
							m_BackgroundMusicPlayer.clip = BackgroundMusic[UnityEngine.Random.Range(0, BackgroundMusic.Length)];
							m_BackgroundMusicPlayer.Play();
						}
						else
						{
							m_BackgroundMusicPlayer.clip = BackgroundMusic[LastPlayedIndex];
							m_BackgroundMusicPlayer.Play();
							LastPlayedIndex++;
							if (LoopBackground && LastPlayedIndex == BackgroundMusic.Length)
							{
								LastPlayedIndex = 0;
							}
						}
					}
					if (BackgroundMusic != null)
					{
						m_BackgroundMusicPlayer.volume = Mathf.Clamp01(BackgroundMusicVolume);
						m_BackgroundMusicPlayer.mute = m_MuteAllSounds;
					}
					if (RaceSoundsPlayer != null)
					{
						RaceSoundsPlayer.volume = Mathf.Clamp01(EffectsSoundVolume);
						RaceSoundsPlayer.mute = m_MuteAllSounds;
					}
				}
			}
			catch
			{
			}
		}

		private AudioSource CreateAudioSource(Transform Parent, string AudioSourceName)
		{
			GameObject gameObject = new GameObject("raceaudio_" + AudioSourceName);
			gameObject.transform.parent = Parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
			audioSource.loop = false;
			audioSource.spatialBlend = 1f;
			audioSource.Play();
			return audioSource;
		}

		public void InitAudio()
		{
			//if (RaceManager.SplitScreen)
			//{
			//	GameCamera = RaceManager.oAudioListener;
			//}
			//else
			//{
				GameCamera = RaceManager.oCamera1;
			//}
			if (GameCamera == null)
			{
				UnityEngine.Debug.LogWarning("RGK WARNING\r\nGameCamera not attached to Game Audio script. Please be sure _GameCamera object created or attached. Game Audio Disabled.");
				IsAudioEnabled = false;
			}
			else
			{
				m_BackgroundMusicPlayer = CreateAudioSource(GameCamera.transform, "bgmusic");
				RaceSoundsPlayer = CreateAudioSource(GameCamera.transform, "fx");
			}
			BackgroundMusicVolumeTemp = 0f;
			EffectsSoundVolumeTemp = 0f;
			EngineSoundVolumeTemp = 0f;
		}

		public void PlayAudio(eRaceAudioFXName AudioName)
		{
			switch (AudioName)
			{
			case eRaceAudioFXName.RaceCountdown:
				if (RaceCountDown != null && RaceSoundsPlayer != null)
				{
					RaceSoundsPlayer.clip = RaceCountDown;
					RaceSoundsPlayer.Play();
				}
				break;
			case eRaceAudioFXName.RaceStart:
				if (RaceStart != null && RaceSoundsPlayer != null)
				{
					RaceSoundsPlayer.clip = RaceStart;
					RaceSoundsPlayer.Play();
				}
				break;
			case eRaceAudioFXName.RaceWon:
				if (RaceWon != null && RaceSoundsPlayer != null)
				{
					RaceSoundsPlayer.clip = RaceWon;
					RaceSoundsPlayer.Play();
				}
				break;
			case eRaceAudioFXName.RaceLost:
				if (RaceLost != null && RaceSoundsPlayer != null)
				{
					RaceSoundsPlayer.clip = RaceLost;
					RaceSoundsPlayer.Play();
				}
				break;
			case eRaceAudioFXName.Checkpoint:
				if (RaceCheckpoint != null && RaceSoundsPlayer != null)
				{
					RaceSoundsPlayer.clip = RaceCheckpoint;
					RaceSoundsPlayer.Play();
				}
				break;
			case eRaceAudioFXName.WrongCheckpoint:
				if (RaceWrongCheckpoint != null && RaceSoundsPlayer != null)
				{
					RaceSoundsPlayer.clip = RaceWrongCheckpoint;
					RaceSoundsPlayer.Play();
				}
				break;
			}
		}

		private void SetMute(bool Value)
		{
			m_MuteAllSounds = Value;
			if (this.OnMuteChanged != null)
			{
				this.OnMuteChanged(Value);
			}
		}
	}
}
