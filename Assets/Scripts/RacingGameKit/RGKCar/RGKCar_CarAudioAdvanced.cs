using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.RGKCar
{
	[AddComponentMenu("Racing Game Kit/RGKCar/RGKCar Advanced Audio")]
	public class RGKCar_CarAudioAdvanced : MonoBehaviour
	{
		public class RPMLevelStage
		{
			public float Pitch
			{
				get;
				set;
			}

			public float Volume
			{
				get;
				set;
			}

			public RPMLevel RPMLevel
			{
				get;
				set;
			}

			public void Update()
			{
				if (RPMLevel != null)
				{
					RPMLevel.Source.volume = Volume;
					RPMLevel.Source.pitch = Pitch;
				}
			}
		}

		[Serializable]
		public class RPMLevel
		{
			[SerializeField]
			private AudioClip offSound;

			[SerializeField]
			private AudioClip onSound;

			[SerializeField]
			private float rpmLow;

			[SerializeField]
			private float rpmHigh;

			[SerializeField]
			private AnimationCurve fadeCurve;

			[SerializeField]
			private float currentFade;

			[SerializeField]
			private AnimationCurve pitchCurve;

			[SerializeField]
			private float pitchMin;

			[SerializeField]
			private float pitchMax;

			public AnimationCurve FadeCurve => fadeCurve;

			public float CurrentFade
			{
				get
				{
					return currentFade;
				}
				set
				{
					currentFade = value;
				}
			}

			public AudioClip OffClip => offSound;

			public AudioClip OnClip => onSound;

			public AnimationCurve PitchCurve => pitchCurve;

			public float PitchMin => pitchMin;

			public float PitchMax => pitchMax;

			public float RpmLow => rpmLow;

			public float RpmHigh => rpmHigh;

			public AudioSource Source
			{
				get;
				set;
			}
		}

		private Race_Audio RaceAudio;

		public float m_FxVolume = 1f;

		public float m_EngineVolume = 1f;

		private bool m_IsMuted;

		public AudioRolloffMode RolloffMode;

		public float MinDistance = 1f;

		public float MaxDistance = 100f;

		public float EngineSpatialBlend = 1f;

		public float FxSpatialBlend = 1f;

		private RGKCar_Engine CarEngine;

		private RGKCar_Setup CarSetup;

		public AudioClip TurboPopHigh;

		public AudioClip TurboWaste;

		public AudioClip Skid;

		public AudioClip SpeedHiss;

		public float HissPitchLow = 0.5f;

		public float HissPitchHigh = 1.2f;

		public AudioClip BrakeDisk;

		public AudioClip BackfireShort;

		public AudioClip BackfireLong;

		public AudioClip Nitro;

		public AudioClip GearChange;

		private AudioSource AudioChannel1;

		private AudioSource AudioChannel2;

		private AudioSource AudioChannelTurbo;

		private AudioSource AudioChannelHiss;

		private AudioSource AudioChannelSkid;

		private AudioSource AudioChannelBackfire;

		private AudioSource AudioChannelNos;

		private AudioSource AudioChannelGearChange;

		private AudioSource AudioChannelBrakeDisk;

		private float EngineThrootle;

		private RPMLevelStage[] rpmLevelStages = new RPMLevelStage[2];

		private float EngineRPM;

		[SerializeField]
		private List<RPMLevel> RPMLevels = new List<RPMLevel>();

		[SerializeField]
		private AnimationCurve RevUpVolume;

		[SerializeField]
		private AnimationCurve RevDownVolume;

		public void Awake()
		{
			GameObject gameObject = GameObject.Find("_RaceManager");
			if (gameObject != null)
			{
				RaceAudio = gameObject.GetComponent<Race_Audio>();
			}
			if (RaceAudio != null)
			{
				RaceAudio.OnMuteChanged += RaceAudio_OnMuteChanged;
				RaceAudio.OnFxVolumeChanged += RaceAudio_OnFxVolumeChanged;
				RaceAudio.OnEngineVolumeChanged += RaceAudio_OnEngineVolumeChanged;
			}
			CarEngine = GetComponent<RGKCar_Engine>();
			CarSetup = GetComponent<RGKCar_Setup>();
			SetupAudioSources();
			for (int i = 0; i < RPMLevels.Count; i++)
			{
				RPMLevels[i].Source = ((i % 2 == 0) ? AudioChannel1 : AudioChannel2);
			}
			rpmLevelStages[0] = new RPMLevelStage();
			rpmLevelStages[1] = new RPMLevelStage();
			if (AudioChannel1 != null)
			{
				AudioChannel1.loop = true;
				AudioChannel1.volume = 0f;
			}
			if (AudioChannel2 != null)
			{
				AudioChannel2.loop = true;
				AudioChannel2.volume = 0f;
			}
		}

		private void RaceAudio_OnEngineVolumeChanged(float Value)
		{
			m_EngineVolume = Value;
		}

		private void RaceAudio_OnFxVolumeChanged(float Value)
		{
			m_FxVolume = Value;
			if (AudioChannelTurbo != null)
			{
				AudioChannelTurbo.volume = Value;
			}
			if (AudioChannelBackfire != null)
			{
				AudioChannelBackfire.volume = Value;
			}
			if (AudioChannelNos != null)
			{
				AudioChannelNos.volume = Value;
			}
			if (AudioChannelGearChange != null)
			{
				AudioChannelGearChange.volume = Value;
			}
			if (AudioChannelBrakeDisk != null)
			{
				AudioChannelBrakeDisk.volume = Value;
			}
		}

		private void RaceAudio_OnMuteChanged(bool Mute)
		{
			m_IsMuted = Mute;
			if (AudioChannelTurbo != null)
			{
				AudioChannelTurbo.mute = Mute;
			}
			if (AudioChannelHiss != null)
			{
				AudioChannelHiss.mute = Mute;
			}
			if (AudioChannelSkid != null)
			{
				AudioChannelSkid.mute = Mute;
			}
			if (AudioChannelBackfire != null)
			{
				AudioChannelBackfire.mute = Mute;
			}
			if (AudioChannelNos != null)
			{
				AudioChannelNos.mute = Mute;
			}
			if (AudioChannelGearChange != null)
			{
				AudioChannelGearChange.mute = Mute;
			}
			if (AudioChannelBrakeDisk != null)
			{
				AudioChannelBrakeDisk.mute = Mute;
			}
		}

		private void SetupAudioSources()
		{
			GameObject gameObject = new GameObject("audio_channel1");
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.AddComponent(typeof(AudioSource));
			AudioChannel1 = gameObject.GetComponent<AudioSource>();
			AudioChannel1.spatialBlend = EngineSpatialBlend;
			AudioChannel1.loop = true;
			AudioChannel1.volume = 1f;
			AudioChannel1.playOnAwake = false;
			AudioChannel1.rolloffMode = RolloffMode;
			AudioChannel1.minDistance = MinDistance;
			AudioChannel1.maxDistance = MaxDistance;
			GameObject gameObject2 = new GameObject("audio_channel2");
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.AddComponent(typeof(AudioSource));
			AudioChannel2 = gameObject2.GetComponent<AudioSource>();
			AudioChannel2.spatialBlend = EngineSpatialBlend;
			AudioChannel2.loop = true;
			AudioChannel2.volume = 1f;
			AudioChannel2.playOnAwake = false;
			AudioChannel2.rolloffMode = RolloffMode;
			AudioChannel2.minDistance = MinDistance;
			AudioChannel2.maxDistance = MaxDistance;
			if (TurboPopHigh != null)
			{
				GameObject gameObject3 = new GameObject("audio_turbo");
				gameObject3.transform.parent = base.transform;
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.transform.localRotation = Quaternion.identity;
				gameObject3.AddComponent(typeof(AudioSource));
				AudioChannelTurbo = gameObject3.GetComponent<AudioSource>();
				AudioChannelTurbo.spatialBlend = FxSpatialBlend;
				AudioChannelTurbo.loop = false;
				AudioChannelTurbo.volume = 1f;
				AudioChannelTurbo.playOnAwake = false;
				AudioChannelTurbo.rolloffMode = RolloffMode;
				AudioChannelTurbo.minDistance = MinDistance;
				AudioChannelTurbo.maxDistance = MaxDistance;
			}
			if (SpeedHiss != null)
			{
				GameObject gameObject4 = new GameObject("audio_hiss");
				gameObject4.transform.parent = base.transform;
				gameObject4.transform.localPosition = Vector3.zero;
				gameObject4.transform.localRotation = Quaternion.identity;
				gameObject4.AddComponent(typeof(AudioSource));
				AudioChannelHiss = gameObject4.GetComponent<AudioSource>();
				AudioChannelHiss.spatialBlend = FxSpatialBlend;
				AudioChannelHiss.loop = true;
				AudioChannelHiss.volume = 0f;
				AudioChannelHiss.pitch = 0f;
				AudioChannelHiss.clip = SpeedHiss;
				AudioChannelHiss.playOnAwake = true;
				AudioChannelHiss.Play();
				AudioChannelHiss.rolloffMode = RolloffMode;
				AudioChannelHiss.minDistance = MinDistance;
				AudioChannelHiss.maxDistance = MaxDistance;
			}
			GameObject gameObject5 = new GameObject("audio_skid");
			gameObject5.transform.parent = base.transform;
			gameObject5.transform.localPosition = Vector3.zero;
			gameObject5.transform.localRotation = Quaternion.identity;
			gameObject5.AddComponent(typeof(AudioSource));
			AudioChannelSkid = gameObject5.GetComponent<AudioSource>();
			AudioChannelSkid.spatialBlend = FxSpatialBlend;
			AudioChannelSkid.loop = true;
			AudioChannelSkid.volume = 0f;
			AudioChannelSkid.clip = Skid;
			AudioChannelSkid.playOnAwake = true;
			AudioChannelSkid.Play();
			AudioChannelSkid.rolloffMode = RolloffMode;
			AudioChannelSkid.minDistance = MinDistance;
			AudioChannelSkid.maxDistance = MaxDistance;
			if (BackfireShort != null || BackfireLong != null)
			{
				GameObject gameObject6 = new GameObject("audio_backfire");
				gameObject6.transform.parent = base.transform;
				gameObject6.transform.localPosition = Vector3.zero;
				gameObject6.transform.localRotation = Quaternion.identity;
				gameObject6.AddComponent(typeof(AudioSource));
				AudioChannelBackfire = gameObject6.GetComponent<AudioSource>();
				AudioChannelBackfire.spatialBlend = FxSpatialBlend;
				AudioChannelBackfire.loop = false;
				AudioChannelBackfire.volume = 1f;
				AudioChannelBackfire.clip = BackfireShort;
				AudioChannelBackfire.playOnAwake = false;
				AudioChannelBackfire.rolloffMode = RolloffMode;
				AudioChannelBackfire.minDistance = MinDistance;
				AudioChannelBackfire.maxDistance = MaxDistance;
			}
			if (Nitro != null)
			{
				GameObject gameObject7 = new GameObject("audio_nitro");
				gameObject7.transform.parent = base.transform;
				gameObject7.transform.localPosition = Vector3.zero;
				gameObject7.transform.localRotation = Quaternion.identity;
				gameObject7.AddComponent(typeof(AudioSource));
				AudioChannelNos = gameObject7.GetComponent<AudioSource>();
				AudioChannelNos.spatialBlend = FxSpatialBlend;
				AudioChannelNos.loop = true;
				AudioChannelNos.volume = 1f;
				AudioChannelNos.clip = Nitro;
				AudioChannelNos.playOnAwake = false;
				AudioChannelNos.rolloffMode = RolloffMode;
				AudioChannelNos.minDistance = MinDistance;
				AudioChannelNos.maxDistance = MaxDistance;
			}
			if (GearChange != null)
			{
				GameObject gameObject8 = new GameObject("audio_gearchange");
				gameObject8.transform.parent = base.transform;
				gameObject8.transform.localPosition = Vector3.zero;
				gameObject8.transform.localRotation = Quaternion.identity;
				gameObject8.AddComponent(typeof(AudioSource));
				AudioChannelGearChange = gameObject8.GetComponent<AudioSource>();
				AudioChannelGearChange.spatialBlend = FxSpatialBlend;
				AudioChannelGearChange.loop = false;
				AudioChannelGearChange.volume = 1f;
				AudioChannelGearChange.playOnAwake = false;
				AudioChannelGearChange.rolloffMode = RolloffMode;
				AudioChannelGearChange.minDistance = MinDistance;
				AudioChannelGearChange.maxDistance = MaxDistance;
			}
			if (BrakeDisk != null)
			{
				GameObject gameObject9 = new GameObject("audio_brakedisk");
				gameObject9.transform.parent = base.transform;
				gameObject9.transform.localPosition = Vector3.zero;
				gameObject9.transform.localRotation = Quaternion.identity;
				gameObject9.AddComponent(typeof(AudioSource));
				AudioChannelBrakeDisk = gameObject9.GetComponent<AudioSource>();
				AudioChannelBrakeDisk.spatialBlend = FxSpatialBlend;
				AudioChannelBrakeDisk.loop = true;
				AudioChannelBrakeDisk.volume = 1f;
				AudioChannelBrakeDisk.clip = BrakeDisk;
				AudioChannelBrakeDisk.playOnAwake = false;
				AudioChannelBrakeDisk.rolloffMode = RolloffMode;
				AudioChannelBrakeDisk.minDistance = MinDistance;
				AudioChannelBrakeDisk.maxDistance = MaxDistance;
			}
		}

		public void LateUpdate()
		{
			EngineThrootle = CarEngine.intThrottle;
			EngineRPM = CarEngine.RPM;
			for (int i = 0; i < RPMLevels.Count; i++)
			{
				if (!(EngineRPM >= RPMLevels[i].RpmLow) || !(EngineRPM < RPMLevels[i].RpmHigh))
				{
					continue;
				}
				if (i < RPMLevels.Count - 1 && EngineRPM >= RPMLevels[i + 1].RpmLow)
				{
					rpmLevelStages[0].RPMLevel = RPMLevels[i];
					rpmLevelStages[1].RPMLevel = RPMLevels[i + 1];
					if (!m_IsMuted)
					{
						rpmLevelStages[0].RPMLevel.Source.mute = false;
						rpmLevelStages[1].RPMLevel.Source.mute = false;
					}
					else
					{
						rpmLevelStages[0].RPMLevel.Source.mute = true;
						rpmLevelStages[1].RPMLevel.Source.mute = true;
					}
					break;
				}
				rpmLevelStages[0].RPMLevel = RPMLevels[i];
				rpmLevelStages[1].RPMLevel = null;
				if (!m_IsMuted)
				{
					if (rpmLevelStages[0].RPMLevel.Source == AudioChannel1 && AudioChannel2.isPlaying)
					{
						AudioChannel1.mute = false;
						AudioChannel2.mute = true;
					}
					else if (rpmLevelStages[0].RPMLevel.Source == AudioChannel2 && AudioChannel1.isPlaying)
					{
						AudioChannel1.mute = true;
						AudioChannel2.mute = false;
					}
				}
				else
				{
					AudioChannel1.mute = true;
					AudioChannel2.mute = true;
				}
				break;
			}
			for (int j = 0; j < rpmLevelStages.Length; j++)
			{
				RPMLevelStage rPMLevelStage = rpmLevelStages[j];
				RPMLevel rPMLevel = rPMLevelStage.RPMLevel;
				if (rPMLevel != null)
				{
					float num = Mathf.Clamp(EngineRPM, rPMLevel.RpmLow, rPMLevel.RpmHigh);
					float num2 = rPMLevel.RpmHigh - rPMLevel.RpmLow;
					float time = (num - rPMLevel.RpmLow) / num2;
					float num3 = rPMLevel.PitchMax - rPMLevel.PitchMin;
					rPMLevelStage.Pitch = rPMLevel.PitchMin + num3 * rPMLevel.PitchCurve.Evaluate(time);
					if (EngineThrootle > 0f)
					{
						rPMLevel.Source.clip = rPMLevel.OnClip;
					}
					else
					{
						rPMLevel.Source.clip = rPMLevel.OffClip;
					}
					rPMLevelStage.Volume = 1f;
					if (EngineThrootle > 0f)
					{
						rPMLevelStage.Volume = rPMLevelStage.Volume * RevUpVolume.Evaluate(EngineRPM / CarSetup.EngineData.EngineMaxRPM) * 1f;
					}
					else
					{
						rPMLevelStage.Volume = rPMLevelStage.Volume * RevDownVolume.Evaluate(EngineRPM / CarSetup.EngineData.EngineMaxRPM) * 1f;
					}
					if (rPMLevelStage.Volume > m_EngineVolume)
					{
						rPMLevelStage.Volume = m_EngineVolume;
					}
					if (!rPMLevel.Source.isPlaying)
					{
						rPMLevel.Source.Play();
					}
				}
			}
			if (rpmLevelStages[0].RPMLevel != null && rpmLevelStages[1].RPMLevel != null)
			{
				float num4 = rpmLevelStages[0].RPMLevel.RpmHigh - rpmLevelStages[1].RPMLevel.RpmLow;
				float num5 = (EngineRPM - rpmLevelStages[1].RPMLevel.RpmLow) / num4;
				rpmLevelStages[0].RPMLevel.CurrentFade = rpmLevelStages[0].RPMLevel.FadeCurve.Evaluate(1f - num5);
				rpmLevelStages[1].RPMLevel.CurrentFade = rpmLevelStages[0].RPMLevel.FadeCurve.Evaluate(num5);
				RPMLevelStage rPMLevelStage2 = rpmLevelStages[0];
				rPMLevelStage2.Volume *= rpmLevelStages[0].RPMLevel.CurrentFade;
				RPMLevelStage rPMLevelStage3 = rpmLevelStages[1];
				rPMLevelStage3.Volume *= rpmLevelStages[1].RPMLevel.CurrentFade;
			}
			rpmLevelStages[0].Update();
			rpmLevelStages[1].Update();
			ApplyHiss();
		}

		private void ApplyHiss()
		{
			if (!(SpeedHiss != null))
			{
				return;
			}
			if (CarEngine.SpeedAsKM > 70f)
			{
				if (!m_IsMuted)
				{
					AudioChannelHiss.mute = false;
				}
				AudioChannelHiss.volume = CarEngine.SpeedAsKM / 130f;
				if ((double)AudioChannelHiss.volume <= 0.4)
				{
					AudioChannelHiss.volume = 0.4f;
				}
				if (AudioChannelHiss.volume >= m_FxVolume)
				{
					AudioChannelHiss.volume = m_FxVolume;
				}
				AudioChannelHiss.pitch = CarEngine.SpeedAsKM / 130f;
				if (AudioChannelHiss.pitch < HissPitchLow)
				{
					AudioChannelHiss.pitch = HissPitchLow;
				}
				if (AudioChannelHiss.pitch > HissPitchHigh)
				{
					AudioChannelHiss.pitch = HissPitchHigh;
				}
			}
			else
			{
				AudioChannelHiss.mute = true;
			}
		}

		public void PopTurbo(float TurboValue)
		{
			if (TurboPopHigh != null && TurboWaste != null)
			{
				bool flag = true;
				if (TurboValue > 0.9f)
				{
					AudioChannelTurbo.clip = TurboPopHigh;
				}
				else if (TurboValue >= 0.5f && TurboValue < 0.9f)
				{
					AudioChannelTurbo.clip = TurboWaste;
				}
				else
				{
					flag = false;
				}
				if (!AudioChannelTurbo.isPlaying && flag)
				{
					AudioChannelTurbo.Play();
				}
			}
		}

		public void PopGear()
		{
			if (GearChange != null && AudioChannelGearChange != null)
			{
				AudioChannelGearChange.clip = GearChange;
				AudioChannelGearChange.Play();
			}
		}

		public void ApplyBrakeDisk(float Brake)
		{
			if (AudioChannelBrakeDisk != null)
			{
				AudioChannelBrakeDisk.volume = Brake;
				if (!AudioChannelBrakeDisk.isPlaying && Brake > 0f)
				{
					AudioChannelBrakeDisk.Play();
				}
				else if (Brake == 0f)
				{
					AudioChannelBrakeDisk.Stop();
				}
			}
		}

		public void ApplyNitro(bool IsUsing)
		{
			if (AudioChannelNos != null)
			{
				if (IsUsing && !AudioChannelNos.isPlaying)
				{
					AudioChannelNos.Play();
				}
				else if (!IsUsing && AudioChannelNos.isPlaying)
				{
					AudioChannelNos.Stop();
				}
			}
		}

		public bool PopBackfire(bool IsLong)
		{
			if (AudioChannelBackfire != null)
			{
				if (IsLong)
				{
					AudioChannelBackfire.clip = BackfireLong;
				}
				else
				{
					AudioChannelBackfire.clip = BackfireShort;
				}
				if (!AudioChannelBackfire.isPlaying)
				{
					AudioChannelBackfire.Play();
				}
				return AudioChannelBackfire.isPlaying;
			}
			return false;
		}

		private void Update()
		{
			if (AudioChannelSkid != null)
			{
				float num = Mathf.Abs(CarEngine.TotalSlip) * 0.1f - 0.3f;
				if (num > m_FxVolume)
				{
					AudioChannelSkid.volume = m_FxVolume;
				}
				else
				{
					AudioChannelSkid.volume = num;
				}
			}
		}

		public void UpdateSkidSound()
		{
			if (AudioChannelSkid != null)
			{
				AudioChannelSkid.clip = Skid;
				if (!AudioChannelSkid.isPlaying)
				{
					AudioChannelSkid.Play();
				}
			}
		}
	}
}
