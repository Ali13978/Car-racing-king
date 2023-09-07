using UnityEngine;

namespace RacingGameKit.RGKCar
{
	[RequireComponent(typeof(RGKCar_Engine))]
	[AddComponentMenu("Racing Game Kit/RGKCar/RGKCar Basic Audio")]
	public class RGKCar_CarAudioBasic : MonoBehaviour
	{
		private RGKCar_Setup CarSetup;

		public float m_FxVolume = 1f;

		public float m_EngineVolume = 1f;

		public AudioRolloffMode RolloffMode;

		public float MinDistance = 1f;

		public float MaxDistance = 100f;

		public float EngineSpatialBlend = 1f;

		public float FxSpatialBlend = 1f;

		public AudioClip Engine;

		public float PitchLow = 0.8f;

		public float PitchHigh = 1.2f;

		public AudioClip Skid;

		private AudioSource engineSource;

		private AudioSource skidSource;

		private Race_Audio RaceAudio;

		private RGKCar_Engine CarEngine;

		private AudioSource CreateAudioSource(AudioClip clip, string Name)
		{
			GameObject gameObject = new GameObject("audio_" + Name);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
			audioSource.spatialBlend = 1f;
			audioSource.clip = clip;
			audioSource.loop = true;
			audioSource.volume = 0f;
			audioSource.Play();
			return audioSource;
		}

		private void Start()
		{
			RaceAudio = UnityEngine.Object.FindObjectOfType<Race_Audio>();
			if (RaceAudio != null)
			{
				RaceAudio.OnMuteChanged += RaceAudio_OnMuteChanged;
				RaceAudio.OnFxVolumeChanged += RaceAudio_OnFxVolumeChanged;
				RaceAudio.OnEngineVolumeChanged += RaceAudio_OnEngineVolumeChanged;
			}
			CarSetup = GetComponent<RGKCar_Setup>();
			CarEngine = GetComponent<RGKCar_Engine>();
			engineSource = CreateAudioSource(Engine, "engine");
			skidSource = CreateAudioSource(Skid, "skid");
			engineSource.rolloffMode = RolloffMode;
			engineSource.minDistance = MinDistance;
			engineSource.maxDistance = MaxDistance;
			engineSource.spatialBlend = EngineSpatialBlend;
			skidSource.rolloffMode = RolloffMode;
			skidSource.minDistance = MinDistance;
			skidSource.maxDistance = MaxDistance;
			skidSource.spatialBlend = FxSpatialBlend;
		}

		private void RaceAudio_OnEngineVolumeChanged(float Value)
		{
			m_EngineVolume = Value;
		}

		private void RaceAudio_OnFxVolumeChanged(float Value)
		{
			m_FxVolume = Value;
		}

		private void RaceAudio_OnMuteChanged(bool Mute)
		{
			engineSource.mute = Mute;
			skidSource.mute = Mute;
		}

		private void Update()
		{
			if (engineSource != null)
			{
				float value = 1.3f * CarEngine.RPM / CarSetup.EngineData.EngineMaxRPM;
				engineSource.pitch = Mathf.Clamp(value, PitchLow, PitchHigh);
				engineSource.volume = 0.4f + 0.6f * CarEngine.intThrottle;
				if (engineSource.volume > m_EngineVolume)
				{
					engineSource.volume = m_EngineVolume;
				}
			}
			if (skidSource != null)
			{
				skidSource.volume = Mathf.Clamp01(Mathf.Abs(CarEngine.TotalSlip) * 0.2f - 0.5f);
				if (skidSource.volume > m_FxVolume)
				{
					skidSource.volume = m_FxVolume;
				}
			}
		}

		public void UpdateSkidSound()
		{
			if (skidSource != null)
			{
				skidSource.clip = Skid;
				if (!skidSource.isPlaying)
				{
					skidSource.Play();
				}
			}
		}
	}
}
