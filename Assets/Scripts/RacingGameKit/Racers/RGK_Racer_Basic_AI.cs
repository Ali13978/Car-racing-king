using RacingGameKit.AI;
using RacingGameKit.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.Racers
{
	[AddComponentMenu("Racing Game Kit/Racers/RGK Racer - Basic AI")]
	public class RGK_Racer_Basic_AI : AI_Basic
	{
		[HideInInspector]
		private Race_Manager m_RaceManager;

		[HideInInspector]
		private Racer_Register m_RaceRegister;

		[HideInInspector]
		private IRGKCarController m_RGKCarController;

		[HideInInspector]
		private List<Transform> m_WPItems;

		private int m_CurrentWaypointIndex;

		public bool UseSteerSmoothing = true;

		public float SteerSmoothingLow = 10f;

		public float SteerSmoothingHigh = 15f;

		public bool UseLerp = true;

		public float SteerSmoothingReleaseCoef = 0.25f;

		public float StuckResetTimer;

		public float StuckResetWait = 2f;

		public bool UseThrottleBonus = true;

		public bool RandomizeBonusUsage = true;

		public float ThrottleBonus = 1f;

		public bool m_AiIsBraking;

		private float m_WPSoftBrakeSpeed;

		private float m_WPHardBrakeSpeed;

		public float m_AiSteer;

		public float m_AiThrottle;

		private float m_AiHandbrake;

		public Vector3 ProbableWaypointPosition;

		public Vector3 RelativeWaypointPosition;

		public Vector3 ProbableWaypointPositionNew;

		public Vector3 RelativeWaypointPositionNew;

		public float RGKSteerFirst;

		private float RGKSteerCorrectionFactor;

		public bool m_IsSteeringLocked;

		public float nextWPCoef = 10f;

		public float RGKSteerLast;

		private float m_DirectionFactor;

		private float m_ReversingWPSideDot;

		private float m_ReversingWPSideDotPrev;

		private float m_NextWPDistance;

		private string currentLayerCache;

		private float layerChangeTimerValue = 7f;

		private float layerChangeTimer;

		private bool layerChangeStarted;

		private void Start()
		{
			Initialize_AI();
			GameObject gameObject = GameObject.Find("_RaceManager");
			m_RaceManager = gameObject.GetComponent<Race_Manager>();
			m_RaceRegister = base.transform.GetComponent<Racer_Register>();
			m_RGKCarController = (IRGKCarController)base.transform.GetComponent(typeof(IRGKCarController));
			GetWaypoints();
		}

		private void FixedUpdate()
		{
			try
			{
				base.Speed = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
				if (!Sleep)
				{
					if (!m_AIReversing)
					{
						ExecuteRadar();
					}
					else
					{
						CalculateReversing();
					}
					CalculateRoute();
					m_RGKCarController.IsReversing = m_AIReversing;
					m_RGKCarController.IsBraking = m_AiIsBraking;
					CheckIsCarStuck();
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message + " " + ex.StackTrace);
			}
		}

		private void Update()
		{
			if (m_RaceManager.IsRaceStarted && !m_RaceRegister.IsRacerFinished && !m_AIReversing && m_RGKCarController.Gear == 0)
			{
				m_RGKCarController.Gear = 2;
			}
			m_RGKCarController.ApplySteer(m_AiSteer);
			if (m_AiThrottle >= 0f)
			{
				m_RGKCarController.ApplyDrive(m_AiThrottle, 0f, Convert.ToBoolean(m_AiHandbrake));
			}
			else
			{
				m_RGKCarController.ApplyDrive(0f, Math.Abs(m_AiThrottle), Convert.ToBoolean(m_AiHandbrake));
			}
		}

		private void GetWaypoints()
		{
			m_WPItems = new List<Transform>();
			foreach (WayPointItem waypointItem in m_RaceManager.WaypointItems)
			{
				m_WPItems.Add(waypointItem.transform);
			}
		}

		private void CalculateRoute()
		{
			if (AIWillAvoid)
			{
				WillReCalculate = true;
			}
			if (WillReCalculate)
			{
				WayPointItem component = m_WPItems[m_CurrentWaypointIndex].GetComponent<WayPointItem>();
				m_WPSoftBrakeSpeed = component.SoftBrakeSpeed;
				m_WPHardBrakeSpeed = component.HardBrakeSpeed;
				float leftWide = component.LeftWide;
				float rightWide = component.RightWide;
				m_AiIsBraking = false;
				Vector3 position = m_WPItems[m_CurrentWaypointIndex].position;
				float x = position.x;
				Vector3 position2 = base.transform.position;
				float y = position2.y;
				Vector3 position3 = m_WPItems[m_CurrentWaypointIndex].position;
				ProbableWaypointPosition = new Vector3(x, y, position3.z);
				RelativeWaypointPosition = base.transform.InverseTransformPoint(ProbableWaypointPosition);
				m_NextWPDistance = Vector3.Distance(base.transform.position, ProbableWaypointPosition);
				m_DirectionFactor = Mathf.Sin(RelativeWaypointPosition.x * ((float)Math.PI / 180f)) * m_NextWPDistance;
				Vector3 eulerAngles = m_WPItems[m_CurrentWaypointIndex].eulerAngles;
				if (eulerAngles.y > 180f)
				{
					Vector3 eulerAngles2 = m_WPItems[m_CurrentWaypointIndex].eulerAngles;
					if (eulerAngles2.y < 360f)
					{
						m_DirectionFactor *= -1f;
					}
				}
				if (m_DirectionFactor > 0f && Mathf.Abs(m_DirectionFactor) > rightWide)
				{
					m_DirectionFactor = rightWide;
				}
				else if (m_DirectionFactor < 0f && Mathf.Abs(m_DirectionFactor) > leftWide)
				{
					m_DirectionFactor = leftWide * -1f;
				}
				WillReCalculate = false;
			}
			Vector3 position4 = m_WPItems[m_CurrentWaypointIndex].position;
			float x2 = position4.x;
			Vector3 position5 = base.transform.position;
			float y2 = position5.y;
			Vector3 position6 = m_WPItems[m_CurrentWaypointIndex].position;
			ProbableWaypointPositionNew = new Vector3(x2, y2, position6.z + m_DirectionFactor);
			RelativeWaypointPositionNew = base.transform.InverseTransformPoint(ProbableWaypointPositionNew);
			if (ShowNavDebugLines)
			{
				UnityEngine.Debug.DrawLine(base.transform.position, ProbableWaypointPosition, Color.cyan);
				UnityEngine.Debug.DrawLine(base.transform.position, ProbableWaypointPositionNew, Color.yellow);
			}
			CalculateSteerSmoothing();
			if (RelativeWaypointPositionNew.z < 0f && !ReverseLockedbyObstacle)
			{
				m_AIReversing = true;
			}
			if (m_AIReversing)
			{
				Vector3 lhs = ProbableWaypointPosition - base.Position;
				m_ReversingWPSideDot = Vector3.Dot(lhs, base.transform.right);
				if (m_ReversingWPSideDotPrev != 0f && ((m_ReversingWPSideDotPrev > 0f && m_ReversingWPSideDot < 0f) || (m_ReversingWPSideDotPrev < 0f && m_ReversingWPSideDot > 0f)))
				{
					m_IsSteeringLocked = false;
				}
				if (!m_IsSteeringLocked)
				{
					m_ReversingWPSideDotPrev = m_ReversingWPSideDot;
					if (m_ReversingWPSideDot > 0f)
					{
						RGKSteerCorrectionFactor = -1f;
					}
					else
					{
						RGKSteerCorrectionFactor = 1f;
					}
					m_IsSteeringLocked = true;
				}
				m_AiSteer = RGKSteerCorrectionFactor;
				if (RelativeWaypointPositionNew.z > 22f)
				{
					m_IsSteeringLocked = false;
					m_AIReversing = false;
				}
			}
			else
			{
				m_AiSteer = RGKSteerFirst + SteerFactor;
				m_AIReversing = false;
			}
			if (RelativeWaypointPositionNew.z > 22f)
			{
				ReverseLockedbyObstacle = false;
			}
			if (Mathf.Abs(m_AiSteer) < 1.5f)
			{
				if (!m_AIReversing)
				{
					if (UseThrottleBonus)
					{
						float num = ThrottleBonus;
						if (RandomizeBonusUsage)
						{
							num = UnityEngine.Random.Range(1f, ThrottleBonus + 0.1f);
						}
						m_AiThrottle = (RelativeWaypointPositionNew.z / RelativeWaypointPositionNew.magnitude - Mathf.Abs(m_AiSteer) / 2f) * num;
					}
					else
					{
						m_AiThrottle = RelativeWaypointPositionNew.z / RelativeWaypointPositionNew.magnitude - Mathf.Abs(m_AiSteer) / 2f;
					}
				}
				else
				{
					m_AiThrottle = -0.5f;
				}
				if (m_WPSoftBrakeSpeed > 0f)
				{
					if (base.Speed > m_WPSoftBrakeSpeed && base.Speed < m_WPHardBrakeSpeed)
					{
						m_AiThrottle = AISoftBrakeFactor;
						m_AiIsBraking = true;
					}
					else if (base.Speed > m_WPHardBrakeSpeed)
					{
						m_AiThrottle = AIHardBrakeFactor;
						m_AiIsBraking = true;
					}
				}
				if (AISoftBraking)
				{
					m_AiThrottle *= AISoftBrakeFactor;
					m_AiIsBraking = true;
				}
			}
			else
			{
				m_AiThrottle = 0f;
			}
			if (ReverseLockedbyObstacle)
			{
				if (m_AiSteer > 0f)
				{
					m_AiSteer = 1f;
				}
				else
				{
					m_AiSteer = -1f;
				}
				m_IsSteeringLocked = true;
				m_AiThrottle = 1f;
			}
			float num2 = UnityEngine.Random.Range(nextWPCoef, nextWPCoef + 3f);
			if (!(RelativeWaypointPositionNew.magnitude <= num2))
			{
				return;
			}
			m_CurrentWaypointIndex++;
			if (m_CurrentWaypointIndex >= m_WPItems.Count)
			{
				if (m_RaceManager.RaceLaps <= 1)
				{
					if (m_RaceManager.StartPoint == m_RaceManager.FinishPoint)
					{
						m_CurrentWaypointIndex = 0;
					}
					else
					{
						m_CurrentWaypointIndex = m_WPItems.Count - 1;
					}
					WillReCalculate = false;
					return;
				}
				m_CurrentWaypointIndex = 0;
			}
			WillReCalculate = true;
		}

		private void CalculateSteerSmoothing()
		{
			if (UseSteerSmoothing && UseLerp)
			{
				if (Mathf.Abs(RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude) < SteerSmoothingReleaseCoef)
				{
					RGKSteerFirst = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
					return;
				}
				float num = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
				if ((num < 0f && RGKSteerFirst > 0f) || (RGKSteerFirst < 0f && num > 0f))
				{
					RGKSteerFirst = 0f;
				}
				RGKSteerFirst = Mathf.Lerp(RGKSteerFirst, num, Time.deltaTime * UnityEngine.Random.Range(SteerSmoothingLow / 10f, SteerSmoothingHigh / 10f));
			}
			else if (UseSteerSmoothing && !UseLerp)
			{
				if (Mathf.Abs(RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude) < SteerSmoothingReleaseCoef)
				{
					RGKSteerFirst = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude * Time.deltaTime * UnityEngine.Random.Range(SteerSmoothingLow, SteerSmoothingHigh);
				}
				else
				{
					RGKSteerFirst = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
				}
			}
			else
			{
				RGKSteerFirst = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
			}
		}

		protected void CheckIsCarStuck()
		{
			if (m_RaceRegister.IsRacerStarted && !m_RaceRegister.IsRacerFinished && !m_RaceRegister.IsRacerDestroyed)
			{
				if (base.Speed < 3f)
				{
					StuckResetTimer += Time.deltaTime;
				}
				else
				{
					StuckResetTimer = 0f;
				}
				if (StuckResetTimer > StuckResetWait)
				{
					RecoverCar();
				}
			}
			if (layerChangeStarted)
			{
				layerChangeForIgnoreProcessor();
			}
		}

		private void RecoverCar()
		{
			Transform transform = (m_CurrentWaypointIndex <= 0) ? m_WPItems[0].transform : m_WPItems[m_CurrentWaypointIndex - 1].transform;
			m_AiThrottle = 0f;
			m_AiSteer = 0f;
			m_AIReversing = false;
			m_RGKCarController.Gear = 2;
			m_RGKCarController.ApplyDrive(0f, 0f, HandBrake: false);
			base.transform.rotation = Quaternion.LookRotation(transform.forward);
			base.transform.position = transform.position;
			base.transform.position += Vector3.up * 0.1f;
			base.transform.position += Vector3.right * 0.1f;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			StuckResetTimer = 0f;
			layerChangeTimer = layerChangeTimerValue;
			currentLayerCache = LayerMask.LayerToName(base.gameObject.layer);
			ChangeLayersRecursively(base.gameObject.transform, "IGNORE");
			layerChangeStarted = true;
		}

		private void RecoverCar(Transform TargetPosition)
		{
			UnityEngine.Debug.DrawLine(base.transform.position, TargetPosition.position);
			m_AiThrottle = 0f;
			m_AiSteer = 0f;
			m_AIReversing = false;
			m_RGKCarController.Gear = 1;
			m_RGKCarController.ApplyDrive(0f, 0f, HandBrake: true);
			m_RGKCarController.IsPreviouslyReversed = true;
			base.transform.rotation = Quaternion.LookRotation(TargetPosition.forward);
			base.transform.position = TargetPosition.position;
			base.transform.position += Vector3.up * 0.1f;
			base.transform.position += Vector3.right * 0.1f;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			StuckResetTimer = 0f;
			layerChangeTimer = layerChangeTimerValue;
			currentLayerCache = LayerMask.LayerToName(base.gameObject.layer);
			ChangeLayersRecursively(base.gameObject.transform, "IGNORE");
			layerChangeStarted = true;
		}

		private void layerChangeForIgnoreProcessor()
		{
			layerChangeTimer -= Time.deltaTime;
			if (layerChangeTimer <= 0f)
			{
				ChangeLayersRecursively(base.gameObject.transform, currentLayerCache);
				layerChangeTimer = layerChangeTimerValue;
			}
		}

		private void ChangeLayersRecursively(Transform trans, string LayerName)
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
