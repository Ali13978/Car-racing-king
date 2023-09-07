using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.RGKCar.CarControllers
{
	[AddComponentMenu("")]
	public class RGKCar_C2_ControllerControllerBase : MonoBehaviour
	{
		internal bool m_IsAiController;

		private RGKCar_Setup m_CarSetup;

		private RGKCar_Wheel[] CarWheels;

		private RGKCar_Engine m_CarEngine;

		private Race_Manager m_RaceManager;

		private Racer_Register m_RacerRegister;

		private Race_Camera m_RaceCamera;

		private GameObject m_WPContainer;

		private List<Transform> m_WPItems;

		public float throttleInput;

		public float steerInput;

		public float brake;

		public float throttle;

		public float steer;

		public int gear;

		public float handbrake;

		public bool ThrottlePressed;

		public bool BrakePressed;

		public float throttleSpeed = 0.5f;

		public float throttleReleaseSpeed = 0.5f;

		public float throttleSpeedOnVel = 0.5f;

		public float throttleReleaseSpeedOnVel = 0.5f;

		public float steerSpeed = 0.1f;

		public float steerReleaseSpeed = 0.05f;

		public float steerSpeedOnVel = 0.02f;

		public float steerReleaseSpeedOnVel = 0.02f;

		public float steerCorrectionFactor = 2.5f;

		public bool tractionControl = true;

		public bool AutoRecover = true;

		private Transform[] brakeLightsTextures;

		private Transform[] brakeLightsFlareObjects;

		private Transform[] headLightTextures;

		private Transform[] reverseLightTextures;

		private Light[] headLightsLightObjects;

		private Light[] brakeLightsLightObject;

		private Light[] reverseLightsLightObject;

		private float m_DefaultBrakeLightIntensity;

		public bool m_IsLightsOn;

		private bool m_IsBrakeLightsOn;

		private bool m_IsReverseLightsOn;

		private bool m_LightsOldState;

		private bool m_BrakeLightOldState;

		private bool m_ReverseLightOldState;

		protected bool smoothThrottle = true;

		protected bool smoothSteering = true;

		private float m_layerChangeTimerValue = 5f;

		private string m_currentLayerCache;

		private float m_layerChangeTimer;

		private bool m_layerChangeStarted;

		public RGKCar_Setup CarSetup => m_CarSetup;

		public RGKCar_Engine CarEngine => m_CarEngine;

		public Race_Manager RaceManager => m_RaceManager;

		public Racer_Register RacerRegister => m_RacerRegister;

		public Race_Camera RaceCamera => m_RaceCamera;

		public virtual void Start()
		{
			m_CarSetup = GetComponent<RGKCar_Setup>();
			m_CarEngine = GetComponent<RGKCar_Engine>();
			CarWheels = CarSetup.Wheels;
			brakeLightsFlareObjects = CarSetup.LightsData.BrakelightsLightFlareObjects;
			brakeLightsTextures = CarSetup.LightsData.BrakelightsLightObjects;
			headLightTextures = CarSetup.LightsData.HeadlightsLightObjects;
			reverseLightTextures = CarSetup.LightsData.ReverseLightLightObjects;
			headLightsLightObjects = CarSetup.LightsData.HeadlightsLights;
			brakeLightsLightObject = CarSetup.LightsData.BrakelightsLights;
			reverseLightsLightObject = CarSetup.LightsData.ReverselightLights;
			m_IsLightsOn = CarSetup.LightsData.LightsOn;
			m_LightsOldState = m_IsLightsOn;
			m_BrakeLightOldState = m_IsLightsOn;
			GameObject gameObject = GameObject.Find("_RaceManager");
			if (gameObject != null)
			{
				m_RaceManager = gameObject.GetComponent<Race_Manager>();
				m_RacerRegister = base.transform.GetComponent<Racer_Register>();
				m_WPContainer = m_RaceManager.Waypoints;
				GetWaypoints();
				if (!m_IsAiController)
				{
					if (!m_RaceManager.SplitScreen)
					{
						m_RaceCamera = m_RaceManager.oCamera1.GetComponent<Race_Camera>();
					}
					else if (base.transform.gameObject == m_RaceManager.Player1.gameObject)
					{
						m_RaceCamera = m_RaceManager.oCamera1.GetComponent<Race_Camera>();
					}
					else if (base.transform.gameObject == m_RaceManager.Player2.gameObject)
					{
						m_RaceCamera = m_RaceManager.oCamera2.GetComponent<Race_Camera>();
					}
					else
					{
						UnityEngine.Debug.LogWarning("There's an error in split screen configuration. Cannot determinate which camera belongs this player..");
					}
				}
			}
			else if (!m_IsAiController)
			{
				m_RaceCamera = UnityEngine.Object.FindObjectOfType<Race_Camera>();
			}
		}

		public virtual void Update()
		{
			if (Vector3.Dot(base.transform.up, Vector3.down) > 0f && AutoRecover)
			{
				CheckIfRecoverable();
			}
			DoUpdate();
			if (m_layerChangeStarted)
			{
				layerChangeForIgnoreProcessor();
			}
		}

		internal void DoUpdate()
		{
			Vector3 forward = base.transform.forward;
			float magnitude = GetComponent<Rigidbody>().velocity.magnitude;
			Vector3 lhs = GetComponent<Rigidbody>().velocity * (1f / magnitude);
			Vector3 vector = Vector3.Cross(lhs, forward);
			float num = 0f - Mathf.Asin(Mathf.Clamp(vector.y, -1f, 1f));
			float num2 = num / (CarWheels[0].maxSteeringAngle * ((float)Math.PI / 180f));
			if (magnitude < 1f)
			{
				num2 = 0f;
			}
			if (smoothSteering)
			{
				if (steerInput < 0f)
				{
					steerInput = -1f;
				}
				if (steerInput > 0f)
				{
					steerInput = 1f;
				}
				if (steerInput < steer)
				{
					float num3 = (!(steer > 0f)) ? (1f / (steerSpeed + steerSpeedOnVel * magnitude)) : (1f / (steerReleaseSpeed + steerReleaseSpeedOnVel * magnitude));
					if (steer > num2)
					{
						num3 *= 1f + (steer - num2) * steerCorrectionFactor;
					}
					steer -= num3 * Time.deltaTime;
					if (steerInput > steer)
					{
						steer = steerInput;
					}
				}
				else if (steerInput > steer)
				{
					float num4 = (!(steer < 0f)) ? (1f / (steerSpeed + steerSpeedOnVel * magnitude)) : (1f / (steerReleaseSpeed + steerReleaseSpeedOnVel * magnitude));
					if (steer < num2)
					{
						num4 *= 1f + (num2 - steer) * steerCorrectionFactor;
					}
					steer += num4 * Time.deltaTime;
					if (steerInput < steer)
					{
						steer = steerInput;
					}
				}
			}
			else
			{
				steer = steerInput;
			}
			if (smoothThrottle)
			{
				if (ThrottlePressed)
				{
					if (tractionControl)
					{
						throttle += Time.deltaTime / throttleSpeed;
					}
					else
					{
						throttle = 1f;
					}
					brake = 0f;
				}
				else if (CarEngine.slipRatio < 0.2f)
				{
					throttle -= Time.deltaTime / throttleReleaseSpeed;
				}
				else
				{
					throttle -= Time.deltaTime / throttleReleaseSpeedOnVel;
				}
				throttle = Mathf.Clamp01(throttle);
				if (BrakePressed)
				{
					if (CarEngine.slipRatio < 0.2f)
					{
						brake += Time.deltaTime / throttleSpeed;
					}
					else
					{
						brake += Time.deltaTime / throttleSpeedOnVel;
					}
					throttle = 0f;
				}
				else if (CarEngine.slipRatio < 0.2f)
				{
					brake -= Time.deltaTime / throttleReleaseSpeed;
				}
				else
				{
					brake -= Time.deltaTime / throttleReleaseSpeedOnVel;
				}
				brake = Mathf.Clamp01(brake);
			}
			else if (CarEngine.Gear >= 0)
			{
				if (throttleInput < 0f)
				{
					brake = Mathf.Abs(throttleInput);
					throttle = 0f;
				}
				if (throttleInput > 0f)
				{
					throttle = throttleInput;
					brake = 0f;
				}
				if (throttleInput == 0f)
				{
					throttle = 0f;
					brake = 0f;
				}
			}
			else
			{
				if (throttleInput < 0f)
				{
					brake = 0f;
					throttle = Mathf.Abs(throttleInput);
				}
				if (throttleInput > 0f)
				{
					throttle = 0f;
					brake = throttleInput;
				}
				if (throttleInput == 0f)
				{
					throttle = 0f;
					brake = 0f;
				}
			}
			FeedEngine();
			ProcessLights();
		}

		internal void FeedEngine()
		{
			CarEngine.throttle = throttle;
			CarEngine.brake = brake;
			CarEngine.steer = steer;
			CarEngine.handbrake = handbrake;
		}

		internal void ProcessLights()
		{
			CarSetup.LightsData.LightsOn = m_IsLightsOn;
			if (brake > 0f)
			{
				m_IsBrakeLightsOn = true;
			}
			else
			{
				m_IsBrakeLightsOn = false;
			}
			if (m_IsBrakeLightsOn != m_BrakeLightOldState && brakeLightsFlareObjects != null)
			{
				Transform[] array = brakeLightsFlareObjects;
				foreach (Transform transform in array)
				{
					if (transform != null)
					{
						transform.gameObject.SetActive(m_IsBrakeLightsOn);
					}
				}
			}
			if (m_IsBrakeLightsOn != m_BrakeLightOldState || m_IsLightsOn != m_LightsOldState)
			{
				if (m_IsBrakeLightsOn)
				{
					Transform[] array2 = brakeLightsTextures;
					foreach (Transform transform2 in array2)
					{
						if (transform2 != null)
						{
							Material[] materials = transform2.GetComponent<Renderer>().materials;
							foreach (Material material in materials)
							{
								material.SetFloat("_Intensity", Mathf.Lerp(0f, 3f, 2f));
							}
						}
					}
					Light[] array3 = brakeLightsLightObject;
					foreach (Light light in array3)
					{
						light.enabled = true;
					}
				}
				else
				{
					float value = 0f;
					if (m_IsLightsOn)
					{
						value = m_DefaultBrakeLightIntensity;
					}
					Transform[] array4 = brakeLightsTextures;
					foreach (Transform transform3 in array4)
					{
						if (transform3 != null)
						{
							Material[] materials2 = transform3.GetComponent<Renderer>().materials;
							foreach (Material material2 in materials2)
							{
								material2.SetFloat("_Intensity", value);
							}
						}
					}
					Light[] array5 = brakeLightsLightObject;
					foreach (Light light2 in array5)
					{
						light2.enabled = false;
					}
				}
				m_BrakeLightOldState = m_IsBrakeLightsOn;
			}
			if (m_IsLightsOn != m_LightsOldState)
			{
				if (m_IsLightsOn)
				{
					Light[] array6 = headLightsLightObjects;
					foreach (Light light3 in array6)
					{
						light3.enabled = true;
					}
					Transform[] array7 = headLightTextures;
					foreach (Transform transform4 in array7)
					{
						if (transform4 != null)
						{
							Material[] materials3 = transform4.GetComponent<Renderer>().materials;
							foreach (Material material3 in materials3)
							{
								material3.SetFloat("_Intensity", 3f);
							}
						}
					}
					m_DefaultBrakeLightIntensity = CarSetup.LightsData.DefaultBrakeLightIntensity;
				}
				else
				{
					Light[] array8 = headLightsLightObjects;
					foreach (Light light4 in array8)
					{
						light4.enabled = false;
					}
					Transform[] array9 = headLightTextures;
					foreach (Transform transform5 in array9)
					{
						if (transform5 != null)
						{
							Material[] materials4 = transform5.GetComponent<Renderer>().materials;
							foreach (Material material4 in materials4)
							{
								material4.SetFloat("_Intensity", 0f);
							}
						}
					}
					m_DefaultBrakeLightIntensity = 0f;
				}
				m_LightsOldState = m_IsLightsOn;
				m_BrakeLightOldState = m_IsLightsOn;
			}
			if (CarEngine.Gear == -1)
			{
				m_IsReverseLightsOn = true;
			}
			else
			{
				m_IsReverseLightsOn = false;
			}
			if (m_IsReverseLightsOn == m_ReverseLightOldState)
			{
				return;
			}
			if (m_IsReverseLightsOn)
			{
				Light[] array10 = reverseLightsLightObject;
				foreach (Light light5 in array10)
				{
					light5.enabled = true;
				}
				Transform[] array11 = reverseLightTextures;
				foreach (Transform transform6 in array11)
				{
					if (transform6 != null)
					{
						Material[] materials5 = transform6.GetComponent<Renderer>().materials;
						foreach (Material material5 in materials5)
						{
							material5.SetFloat("_Intensity", 3f);
						}
					}
				}
			}
			else
			{
				Light[] array12 = reverseLightsLightObject;
				foreach (Light light6 in array12)
				{
					light6.enabled = false;
				}
				Transform[] array13 = reverseLightTextures;
				foreach (Transform transform7 in array13)
				{
					if (transform7 != null)
					{
						Material[] materials6 = transform7.GetComponent<Renderer>().materials;
						foreach (Material material6 in materials6)
						{
							material6.SetFloat("_Intensity", 0f);
						}
					}
				}
			}
			m_ReverseLightOldState = m_IsReverseLightsOn;
		}

		protected void CheckIfRecoverable()
		{
			if (m_RacerRegister != null)
			{
				if (m_RacerRegister.IsRacerStarted && !m_RacerRegister.IsRacerFinished && !m_RacerRegister.IsRacerDestroyed)
				{
					RecoverCar();
					UnityEngine.Debug.Log("Resetting Player Car");
				}
			}
			else
			{
				RecoverCar(base.transform);
			}
		}

		public void RecoverCar()
		{
			Transform[] wPs = m_WPItems.ToArray();
			Transform closestWP = GetClosestWP(wPs, base.transform.position);
			base.transform.rotation = Quaternion.LookRotation(closestWP.forward);
			base.transform.position = closestWP.position;
			base.transform.position += Vector3.up * 0.1f;
			base.transform.position += Vector3.right * 0.1f;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			m_layerChangeTimer = m_layerChangeTimerValue;
			m_currentLayerCache = LayerMask.LayerToName(base.gameObject.layer);
			ChangeLayersRecursively(base.gameObject.transform, "IGNORE");
			m_layerChangeStarted = true;
		}

		public void RecoverCar(Transform TargetPosition)
		{
			base.transform.rotation = Quaternion.LookRotation(TargetPosition.forward);
			base.transform.position = TargetPosition.position;
			base.transform.position += Vector3.up * 0.1f;
			base.transform.position += Vector3.right * 0.1f;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			m_layerChangeTimer = m_layerChangeTimerValue;
			m_currentLayerCache = LayerMask.LayerToName(base.gameObject.layer);
			ChangeLayersRecursively(base.gameObject.transform, "IGNORE");
			m_layerChangeStarted = true;
		}

		private void GetWaypoints()
		{
			Transform[] componentsInChildren = m_WPContainer.GetComponentsInChildren<Transform>();
			m_WPItems = new List<Transform>();
			Transform[] array = componentsInChildren;
			foreach (Transform transform in array)
			{
				if (transform != m_WPContainer.transform)
				{
					m_WPItems.Add(transform);
				}
			}
		}

		internal Transform GetClosestWP(Transform[] WPs, Vector3 myPosition)
		{
			Transform result = null;
			float num = float.PositiveInfinity;
			foreach (Transform transform in WPs)
			{
				float num2 = Vector3.Distance(transform.position, myPosition);
				if (num2 < num)
				{
					result = transform;
					num = num2;
				}
			}
			return result;
		}

		private void layerChangeForIgnoreProcessor()
		{
			m_layerChangeTimer -= Time.deltaTime;
			if (m_layerChangeTimer <= 0f)
			{
				ChangeLayersRecursively(base.gameObject.transform, m_currentLayerCache);
				m_layerChangeTimer = m_layerChangeTimerValue;
				m_layerChangeStarted = false;
			}
		}

		public void ChangeLayersRecursively(Transform trans, string LayerName)
		{
			for (int i = 0; i < trans.childCount; i++)
			{
				trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(LayerName);
				ChangeLayersRecursively(trans.GetChild(i), LayerName);
			}
		}

		public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
			float d = Vector3.Dot(point - lineStart, vector) / Vector3.Dot(vector, vector);
			return lineStart + d * vector;
		}

		public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = lineEnd - lineStart;
			Vector3 vector2 = Vector3.Normalize(vector);
			float value = Vector3.Dot(point - lineStart, vector2) / Vector3.Dot(vector2, vector2);
			return lineStart + Mathf.Clamp(value, 0f, Vector3.Magnitude(vector)) * vector2;
		}
	}
}
