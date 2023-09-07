using RacingGameKit;
using RacingGameKit.RGKCar;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EarthFX
{
	[AddComponentMenu("Racing Game Kit/EarthFX/EarthFX Processor")]
	public class EarthFX_Processor : MonoBehaviour
	{
		private AudioSource m_AudioChannelSurface;

		private RGKCar_CarAudioAdvanced m_CarAudio;

		private RGKCar_CarAudioBasic m_CarAudioBasic;

		public AudioRolloffMode m_RolloffMode;

		public float m_MinDistance = 1f;

		public float m_MaxDistance = 100f;

		public float m_FxVolume = 1f;

		public float m_SpatialBlend = 0.7f;

		public bool m_IgnoreSurfaceSounds;

		public bool m_IgnoreBrakeSounds;

		private int m_iSound;

		private int m_iBrake;

		public bool m_IgnoreTrails;

		private AudioClip m_SurfaceSound;

		private AudioClip BrakeSound;

		private Transform cachedTransform;

		private Rigidbody m_CachedRBody;

		private EarthFX_Manager m_EarthFXManager;

		private Component[] m_EarthFXWheels;

		private float m_DefinedForwardDrag;

		private float m_DefinedAngularDrag;

		private float m_SurfaceForwardDrag;

		private float m_SurfaceAngularDrag;

		private float m_VehicleSpeed;

		private bool m_IsEarthFXManagerFound;

		private void Awake()
		{
			GameObject gameObject = GameObject.Find("_EarthFXManager");
			if (gameObject != null)
			{
				m_EarthFXManager = gameObject.GetComponent<EarthFX_Manager>();
				if (m_EarthFXManager != null)
				{
					m_IsEarthFXManagerFound = true;
				}
			}
			if (!m_IsEarthFXManagerFound)
			{
				UnityEngine.Debug.LogWarning("EARTHFX MANAGER FOUND!\r\nIts name should \"_EarthFXManager\" in the hierarchy. Please check if exits but named different!");
				return;
			}
			m_EarthFXWheels = base.transform.GetComponentsInChildren(typeof(IEarthFX));
			if (m_EarthFXWheels != null)
			{
				Component[] earthFXWheels = m_EarthFXWheels;
				for (int i = 0; i < earthFXWheels.Length; i++)
				{
					IEarthFX earthFX = (IEarthFX)earthFXWheels[i];
					earthFX.EarthFXEnabled = true;
					earthFX.LastSkid = -1;
					earthFX.LastTrail = -1;
				}
			}
		}

		public void Start()
		{
			Racer_Register component = GetComponent<Racer_Register>();
			if (component != null)
			{
				Race_Manager raceManager = component.RaceManager;
				Race_Audio component2 = raceManager.GetComponent<Race_Audio>();
				if (component2 != null)
				{
					component2.OnMuteChanged += RaceAudio_OnMuteChanged;
					component2.OnFxVolumeChanged += RaceAudio_OnFxVolumeChanged;
				}
			}
			cachedTransform = base.transform;
			m_CachedRBody = cachedTransform.GetComponent<Rigidbody>();
			m_DefinedAngularDrag = m_CachedRBody.angularDrag;
			m_DefinedForwardDrag = m_CachedRBody.drag;
			m_CarAudio = GetComponent<RGKCar_CarAudioAdvanced>();
			m_CarAudioBasic = GetComponent<RGKCar_CarAudioBasic>();
			SetupAudioSources();
			if (m_EarthFXWheels == null)
			{
				return;
			}
			Component[] earthFXWheels = m_EarthFXWheels;
			for (int i = 0; i < earthFXWheels.Length; i++)
			{
				IEarthFX earthFX = (IEarthFX)earthFXWheels[i];
				earthFX.EarthFXEnabled = true;
				earthFX.LastSkid = -1;
				earthFX.LastTrail = -1;
				if (earthFX.SurfaceFxCache == null)
				{
					earthFX.SurfaceFxCache = new List<SfxCache>();
				}
				SfxCache sfxCache = new SfxCache();
				sfxCache.FxName = m_EarthFXManager.GlobalFX.FxName;
				if (m_EarthFXManager.GlobalFX.BrakeSmoke != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(m_EarthFXManager.GlobalFX.BrakeSmoke, earthFX.WheelTransform.position, Quaternion.identity, earthFX.WheelTransform.parent);
					ParticleSystem component3 = gameObject.GetComponent<ParticleSystem>();
					if (component3 != null)
					{
						component3.Stop();
					}
					sfxCache.BrakeSmoke = component3;
				}
				if (m_EarthFXManager.GlobalFX.TrailSmoke != null)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(m_EarthFXManager.GlobalFX.TrailSmoke, earthFX.WheelTransform.position, Quaternion.identity, earthFX.WheelTransform.parent);
					ParticleSystem component4 = gameObject2.GetComponent<ParticleSystem>();
					if (component4 != null)
					{
						component4.Stop();
					}
					sfxCache.TrailSmoke = component4;
				}
				if (m_EarthFXManager.GlobalFX.Splatter != null)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(m_EarthFXManager.GlobalFX.Splatter, earthFX.WheelTransform.position, Quaternion.identity, earthFX.WheelTransform.parent);
					ParticleSystem component5 = gameObject3.GetComponent<ParticleSystem>();
					if (component5 != null)
					{
						component5.Stop();
					}
					sfxCache.Splatter = component5;
				}
				sfxCache.TrailSkid = m_EarthFXManager.GlobalFX.TrailSkid;
				sfxCache.BrakeSkid = m_EarthFXManager.GlobalFX.BrakeSkid;
				earthFX.SurfaceFxCache.Add(sfxCache);
				if (m_EarthFXManager.SurfaceFX != null)
				{
					foreach (EarthFX_Manager.EarthFXData item in m_EarthFXManager.SurfaceFX)
					{
						SfxCache sfxCache2 = new SfxCache();
						sfxCache2.FxName = item.FxName;
						if (item.BrakeSmoke != null)
						{
							GameObject gameObject4 = UnityEngine.Object.Instantiate(item.BrakeSmoke, earthFX.WheelTransform.position, Quaternion.identity, earthFX.WheelTransform.parent);
							ParticleSystem component6 = gameObject4.GetComponent<ParticleSystem>();
							if (component6 != null)
							{
								component6.Stop();
							}
							sfxCache2.BrakeSmoke = component6;
						}
						if (item.TrailSmoke != null)
						{
							GameObject gameObject5 = UnityEngine.Object.Instantiate(item.TrailSmoke, earthFX.WheelTransform.position, Quaternion.identity, earthFX.WheelTransform.parent);
							ParticleSystem component7 = gameObject5.GetComponent<ParticleSystem>();
							if (component7 != null)
							{
								component7.Stop();
							}
							sfxCache2.TrailSmoke = component7;
						}
						if (item.Splatter != null)
						{
							GameObject gameObject6 = UnityEngine.Object.Instantiate(item.Splatter, earthFX.WheelTransform.position, Quaternion.identity, earthFX.WheelTransform.parent);
							ParticleSystem component8 = gameObject6.GetComponent<ParticleSystem>();
							if (component8 != null)
							{
								component8.Stop();
							}
							sfxCache2.Splatter = component8;
						}
						sfxCache2.TrailSkid = item.TrailSkid;
						sfxCache2.BrakeSkid = item.BrakeSkid;
						earthFX.SurfaceFxCache.Add(sfxCache2);
					}
				}
			}
		}

		private void RaceAudio_OnFxVolumeChanged(float Value)
		{
			m_FxVolume = Value;
		}

		private void RaceAudio_OnMuteChanged(bool Mute)
		{
			if (m_AudioChannelSurface != null)
			{
				m_AudioChannelSurface.mute = Mute;
			}
		}

		private void SetupAudioSources()
		{
			GameObject gameObject = new GameObject("earthfx_surfacesound");
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.AddComponent(typeof(AudioSource));
			m_AudioChannelSurface = gameObject.GetComponent<AudioSource>();
			m_AudioChannelSurface.loop = true;
			m_AudioChannelSurface.volume = 0f;
			m_AudioChannelSurface.pitch = 1f;
			m_AudioChannelSurface.clip = m_SurfaceSound;
			m_AudioChannelSurface.playOnAwake = false;
			m_AudioChannelSurface.rolloffMode = m_RolloffMode;
			m_AudioChannelSurface.minDistance = m_MinDistance;
			m_AudioChannelSurface.maxDistance = m_MaxDistance;
		}

		private void FixedUpdate()
		{
			if (!m_IsEarthFXManagerFound)
			{
				return;
			}
			m_VehicleSpeed = m_CachedRBody.velocity.magnitude * 3.6f;
			if (m_EarthFXWheels == null || !(m_EarthFXManager != null))
			{
				return;
			}
			Component[] earthFXWheels = m_EarthFXWheels;
			for (int i = 0; i < earthFXWheels.Length; i++)
			{
				IEarthFX earthFX = (IEarthFX)earthFXWheels[i];
				earthFX.EarthFXEnabled = true;
				if (earthFX.OnGround)
				{
					if (m_EarthFXManager.SurfaceFX.Count == 0)
					{
						CastFX(m_EarthFXManager.GlobalFX, earthFX);
						continue;
					}
					Texture2D texture2D = null;
					string b = string.Empty;
					if (Terrain.activeTerrain != null)
					{
						b = Terrain.activeTerrain.name;
					}
					if (earthFX.HitToSurface.collider.gameObject.name == b)
					{
						int mainTexture = SurfaceTextureDedector.GetMainTexture(earthFX.WheelTransform.position);
						texture2D = Terrain.activeTerrain.terrainData.splatPrototypes[mainTexture].texture;
					}
					else if (earthFX.HitToSurface.collider.gameObject.GetComponent<Renderer>() != null && earthFX.HitToSurface.collider.gameObject.GetComponent<Renderer>().material.mainTexture != null)
					{
						texture2D = (earthFX.HitToSurface.collider.gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D);
					}
					if (texture2D != null && earthFX.PreviousSurface != texture2D.name)
					{
						earthFX.SurfaceChanged = true;
						earthFX.LastSkid = -1;
						earthFX.LastTrail = -1;
						earthFX.PreviousSurface = texture2D.name;
					}
					bool flag = false;
					foreach (EarthFX_Manager.EarthFXData item in m_EarthFXManager.SurfaceFX)
					{
						if (item.FxTexture == texture2D)
						{
							flag = true;
							CastFX(item, earthFX);
						}
					}
					if (!flag)
					{
						CastFX(m_EarthFXManager.GlobalFX, earthFX);
					}
					if (earthFX.SurfaceChanged)
					{
						earthFX.SurfaceChanged = false;
					}
				}
				else
				{
					earthFX.LastSkid = -1;
					earthFX.LastTrail = -1;
				}
			}
		}

		private void CastFX(EarthFX_Manager.EarthFXData FX, IEarthFX Wheel)
		{
			SfxCache sfxCache = null;
			foreach (SfxCache item in Wheel.SurfaceFxCache)
			{
				if (item.FxName == FX.FxName)
				{
					sfxCache = item;
					break;
				}
			}
			if (sfxCache.BrakeSkid != null)
			{
				if (Wheel.SkidObject != sfxCache.BrakeSkid)
				{
					Wheel.SkidObject = sfxCache.BrakeSkid;
					Wheel.SkidMark = sfxCache.BrakeSkid.GetComponent<RGKCar_Skidmarks>();
				}
				if (Mathf.Abs(Wheel.SlipRatio) > FX.BrakeSkidStartSlip && !Wheel.SurfaceChanged)
				{
					Wheel.LastSkid = Wheel.SkidMark.AddSkidMark(Wheel.HitToSurface.point, Wheel.HitToSurface.normal, Mathf.Abs(Wheel.SlipRatio), Wheel.LastSkid);
				}
				else
				{
					Wheel.LastSkid = -1;
				}
			}
			if (sfxCache.TrailSkid != null && !m_IgnoreTrails)
			{
				if (Wheel.TrailObject != sfxCache.TrailSkid)
				{
					Wheel.TrailObject = sfxCache.TrailSkid;
					Wheel.TrailMark = sfxCache.TrailSkid.GetComponent<RGKCar_Skidmarks>();
				}
				if (Math.Abs(Wheel.AngularVelocity) > 5f)
				{
					Wheel.LastTrail = Wheel.TrailMark.AddSkidMark(Wheel.HitToSurface.point, Wheel.HitToSurface.normal, (float)Mathf.Abs(1) - 0.2f, Wheel.LastTrail);
				}
			}
			if (sfxCache.BrakeSmoke != null && Mathf.Abs(Wheel.SlipRatio) > FX.BrakeSkidStartSlip)
			{
				if (Wheel.SkidSmoke != sfxCache.BrakeSmoke)
				{
					Wheel.SkidSmoke = sfxCache.BrakeSmoke.GetComponent<ParticleSystem>();
				}
				Wheel.SkidSmoke.transform.position = Wheel.HitToSurface.point;
				ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
				emitParams.position = Wheel.HitToSurface.point + new Vector3(UnityEngine.Random.Range(0f, 0.25f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
				emitParams.velocity = new Vector3(Wheel.SlipVelo * 0.05f, 0f);
				Wheel.SkidSmoke.Emit(emitParams, 1);
			}
			if (sfxCache.TrailSmoke != null)
			{
				if (Wheel.TrailSmoke != sfxCache.TrailSmoke)
				{
					Wheel.TrailSmoke = sfxCache.TrailSmoke.GetComponent<ParticleSystem>();
				}
				if (Mathf.Abs(Wheel.AngularVelocity) > FX.TrailSmokeStartVelocity)
				{
					Wheel.TrailSmoke.transform.position = Wheel.HitToSurface.point;
					ParticleSystem.EmitParams emitParams2 = default(ParticleSystem.EmitParams);
					emitParams2.position = Wheel.HitToSurface.point + new Vector3(UnityEngine.Random.Range(0f, 0.25f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
					emitParams2.velocity = new Vector3(Wheel.SlipVelo * 0.05f, 0f);
					Wheel.TrailSmoke.Emit(emitParams2, 1);
				}
			}
			if (sfxCache.Splatter != null && Math.Abs(Wheel.AngularVelocity) > 5f && !Wheel.SurfaceChanged)
			{
				if (Wheel.Splatter != sfxCache.Splatter)
				{
					Wheel.Splatter = sfxCache.Splatter.GetComponent<ParticleSystem>();
				}
				Wheel.Splatter.transform.position = Wheel.HitToSurface.point;
				ParticleSystem.EmitParams emitParams3 = default(ParticleSystem.EmitParams);
				emitParams3.position = Wheel.HitToSurface.point + new Vector3(UnityEngine.Random.Range(0f, 0.25f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
				emitParams3.velocity = new Vector3(Wheel.SlipVelo * 0.05f, 0f);
				Wheel.Splatter.Emit(emitParams3, 1);
			}
			if (FX.EnableGripDecrease)
			{
				if (FX.ForwardGripLosePercent > 0f)
				{
					Wheel.Grip = Wheel.DefinedGrip - Wheel.DefinedGrip * (FX.ForwardGripLosePercent / 100f);
				}
				else
				{
					Wheel.Grip = Wheel.DefinedGrip;
				}
				if (FX.SidewaysGripLosePercent > 0f)
				{
					Wheel.SideGrip = Wheel.DefinedSideGrip - Wheel.DefinedSideGrip * (FX.SidewaysGripLosePercent / 100f);
				}
				else
				{
					Wheel.SideGrip = Wheel.DefinedSideGrip;
				}
			}
			else
			{
				Wheel.Grip = Wheel.DefinedGrip;
				Wheel.SideGrip = Wheel.DefinedSideGrip;
			}
			if (FX.EnableSpeedDeceleration)
			{
				if (FX.ForwardDrag != 0f)
				{
					if (m_SurfaceForwardDrag != FX.ForwardDrag)
					{
						m_SurfaceForwardDrag = FX.ForwardDrag;
					}
				}
				else
				{
					m_SurfaceForwardDrag = 0f;
				}
				if (FX.AngularDrag != 0f)
				{
					if (m_SurfaceAngularDrag != FX.AngularDrag)
					{
						m_SurfaceAngularDrag = FX.AngularDrag;
					}
				}
				else
				{
					m_SurfaceAngularDrag = 0f;
				}
				ApplySurfaceDrag(m_DefinedForwardDrag, m_DefinedAngularDrag, m_SurfaceForwardDrag, m_SurfaceAngularDrag);
			}
			else
			{
				m_CachedRBody.drag = m_DefinedForwardDrag;
				m_CachedRBody.angularDrag = m_DefinedAngularDrag;
			}
			if (FX.SurfaceDriveSound != null)
			{
				ProcessSound(FX.SurfaceDriveSound);
			}
			else
			{
				m_AudioChannelSurface.mute = true;
			}
			if ((bool)m_CarAudio)
			{
				ProcessBrakes(FX.BrakeSound);
			}
			else if ((bool)m_CarAudioBasic)
			{
				ProcessBrakesBasic(FX.BrakeSound);
			}
		}

		private void ProcessSound(AudioClip SurfaceAudio)
		{
			if (!m_IgnoreSurfaceSounds)
			{
				if (m_SurfaceSound != SurfaceAudio)
				{
					m_SurfaceSound = SurfaceAudio;
					m_iSound = 1;
				}
				else if (m_iSound < m_EarthFXWheels.Length)
				{
					m_iSound++;
				}
				if (m_iSound >= 2)
				{
					ApplySurfaceSound();
				}
			}
		}

		private void ProcessBrakes(AudioClip BrakeAudio)
		{
			if (!m_IgnoreSurfaceSounds)
			{
				if (BrakeAudio == null)
				{
					m_CarAudio.Skid = null;
					m_CarAudio.UpdateSkidSound();
				}
				if (BrakeSound != BrakeAudio)
				{
					BrakeSound = BrakeAudio;
					m_iBrake = 1;
				}
				else if (m_iBrake < m_EarthFXWheels.Length)
				{
					m_iBrake++;
				}
				if (m_iBrake >= 3)
				{
					m_CarAudio.Skid = BrakeSound;
					m_CarAudio.UpdateSkidSound();
				}
			}
		}

		private void ProcessBrakesBasic(AudioClip BrakeAudio)
		{
			if (!m_IgnoreSurfaceSounds)
			{
				if (BrakeAudio == null)
				{
					m_CarAudioBasic.Skid = null;
					m_CarAudioBasic.UpdateSkidSound();
				}
				if (BrakeSound != BrakeAudio)
				{
					BrakeSound = BrakeAudio;
					m_iBrake = 1;
				}
				else if (m_iBrake < m_EarthFXWheels.Length)
				{
					m_iBrake++;
				}
				if (m_iBrake >= 3)
				{
					m_CarAudioBasic.Skid = BrakeSound;
					m_CarAudioBasic.UpdateSkidSound();
				}
			}
		}

		private void ApplySurfaceDrag(float dForwardDrag, float dAngularDrag, float ForwardDrag, float AngularDrag)
		{
			if (ForwardDrag == 0f)
			{
				ForwardDrag = dForwardDrag;
			}
			if (AngularDrag == 0f)
			{
				AngularDrag = dAngularDrag;
			}
			m_CachedRBody.drag = ForwardDrag;
			m_CachedRBody.angularDrag = AngularDrag;
		}

		private void ApplySurfaceSound()
		{
			if (m_VehicleSpeed > 3f && m_SurfaceSound != null)
			{
				m_AudioChannelSurface.clip = m_SurfaceSound;
				m_AudioChannelSurface.mute = false;
				m_AudioChannelSurface.volume = m_VehicleSpeed / 100f;
				if ((double)m_FxVolume >= 0.3 && (double)m_AudioChannelSurface.volume < 0.3)
				{
					m_AudioChannelSurface.volume = 0.3f;
				}
				if (m_AudioChannelSurface.volume > m_FxVolume)
				{
					m_AudioChannelSurface.volume = m_FxVolume;
				}
				if (!m_AudioChannelSurface.isPlaying)
				{
					m_AudioChannelSurface.Play();
				}
			}
			else
			{
				m_AudioChannelSurface.clip = null;
				m_AudioChannelSurface.mute = true;
			}
		}
	}
}
