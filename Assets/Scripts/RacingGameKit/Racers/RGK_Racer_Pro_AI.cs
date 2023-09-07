using RacingGameKit.AI;
using RacingGameKit.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.Racers
{
	[AddComponentMenu("Racing Game Kit/Racers/RGK Racer - Pro AI")]
	public class RGK_Racer_Pro_AI : AI_Brain
	{
		[Serializable]
		public class SmootingData
		{
			public SmoothingBase m_0_30FPS;

			public SmoothingBase m_30_45FPS;

			public SmoothingBase m_45_60FPS;

			public SmoothingBase m_60_NFPS;
		}

		[Serializable]
		public class SmoothingBase
		{
			public float Min;

			public float Max;
		}

		[HideInInspector]
		private Race_Manager m_RaceManager;

		[HideInInspector]
		private Racer_Register m_RacerRegister;

		[HideInInspector]
		private List<Transform> m_Waypoints;

		[HideInInspector]
		private IRGKCarController m_RGKCarController;

		private int m_CurrentWaypointIndex;

		public float m_AiSteer;

		private float m_AiSteerTemp;

		public float m_AiThrottle;

		public float m_AiHandbrake;

		public bool m_AiIsBraking;

		public bool m_IsReversing;

		private bool m_IsSteeringLocked;

		[HideInInspector]
		public bool UseSteerSmoothing = true;

		[HideInInspector]
		public eSteerSmoothingMode SteerSmoothingMode;

		[HideInInspector]
		public SmootingData FPSBasedSmoothingdata;

		[HideInInspector]
		public float SteerSmoothingLow = 10f;

		[HideInInspector]
		public float SteerSmoothingHigh = 20f;

		public bool UseLerp;

		public float SteerSmoothingReleaseCoef = 0.25f;

		[HideInInspector]
		private float m_WPSoftBrakeSpeed;

		[HideInInspector]
		private float m_WPHardBrakeSpeed;

		[HideInInspector]
		public float m_StuckResetTimer;

		[HideInInspector]
		public float m_StuckResetWait = 2f;

		protected Vector3 ProbableWaypointPosition;

		private Vector3 RelativeWaypointPosition;

		private Vector3 ProbableWaypointPositionNew;

		private Vector3 RelativeWaypointPositionNew;

		private float m_NextWPDistance;

		private float m_EscapeDistance;

		private float RGKSteerCorrectionFactor;

		[HideInInspector]
		public float nextWPCoef = 10f;

		private float m_ReversingWPSideDot;

		private float m_ReversingWPSideDotPrev;

		[HideInInspector]
		private string currentLayerCache;

		[HideInInspector]
		private float layerChangeTimerValue = 7f;

		[HideInInspector]
		private float layerChangeTimer;

		[HideInInspector]
		private bool layerChangeStarted;

		private void Start()
		{
			Initialize_AI();
			GameObject gameObject = GameObject.Find("_RaceManager");
			m_RaceManager = gameObject.GetComponent<Race_Manager>();
			if (m_RaceManager == null)
			{
				UnityEngine.Debug.Log("Racemanager not found!");
			}
			m_RacerRegister = GetComponent<Racer_Register>();
			if (m_RacerRegister == null)
			{
				UnityEngine.Debug.Log("Race Register not found!");
			}
			m_RGKCarController = (IRGKCarController)GetComponent(typeof(IRGKCarController));
			if (m_RGKCarController == null)
			{
				UnityEngine.Debug.Log("Car Controller not found!");
			}
			GetWaypoints();
			if (m_RandomizeSpeedFactorOnStart)
			{
				AISpeedFactor = UnityEngine.Random.Range(AISpeedFactorMin, AISpeedFactorMax + 0.1f);
			}
		}

		private void FixedUpdate()
		{
			try
			{
				base.Speed = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
				if (!m_Sleep)
				{
					if (!m_IsReversing)
					{
						ExecuteRadar();
					}
					else
					{
						CalculateReversing();
					}
					if (AIWillEscape && AICanEscape)
					{
						m_AIWillRecalculate = true;
					}
					CalculateRoute();
					m_RGKCarController.ApplySteer(m_AiSteer);
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

		internal override void Update()
		{
			if (m_RaceManager.IsRaceStarted && !m_RacerRegister.IsRacerFinished && !m_IsReversing && m_RGKCarController.Gear == 0)
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
			base.Update();
		}

		private void GetWaypoints()
		{
			m_Waypoints = new List<Transform>();
			foreach (WayPointItem waypointItem in m_RaceManager.WaypointItems)
			{
				m_Waypoints.Add(waypointItem.transform);
			}
		}

		private void CalculateRoute()
		{
			m_AiIsBraking = false;
			float leftWide;
			float rightWide;
			if (m_AIWillRecalculate)
			{
				WayPointItem component = m_Waypoints[m_CurrentWaypointIndex].GetComponent<WayPointItem>();
				m_WPSoftBrakeSpeed = component.SoftBrakeSpeed;
				m_WPHardBrakeSpeed = component.HardBrakeSpeed;
				leftWide = component.LeftWide;
				rightWide = component.RightWide;
				Vector3 position = m_Waypoints[m_CurrentWaypointIndex].position;
				float x = position.x;
				Vector3 position2 = base.transform.position;
				float y = position2.y;
				Vector3 position3 = m_Waypoints[m_CurrentWaypointIndex].position;
				ProbableWaypointPosition = new Vector3(x, y, position3.z);
				RelativeWaypointPosition = base.transform.InverseTransformPoint(ProbableWaypointPosition);
				m_NextWPDistance = Vector3.Distance(base.transform.position, ProbableWaypointPosition);
				m_EscapeDistance = Mathf.Sin(RelativeWaypointPosition.x * ((float)Math.PI / 180f)) * m_NextWPDistance;
				Vector3 eulerAngles = m_Waypoints[m_CurrentWaypointIndex].eulerAngles;
				if (eulerAngles.y > 180f)
				{
					Vector3 eulerAngles2 = m_Waypoints[m_CurrentWaypointIndex].eulerAngles;
					if (eulerAngles2.y < 360f)
					{
						m_EscapeDistance *= -1f;
						UseSideCorrection = true;
						goto IL_0178;
					}
				}
				UseSideCorrection = false;
				goto IL_0178;
			}
			goto IL_01b0;
			IL_01b0:
			Vector3 position4 = m_Waypoints[m_CurrentWaypointIndex].position;
			float x2 = position4.x;
			Vector3 position5 = base.transform.position;
			float y2 = position5.y;
			Vector3 position6 = m_Waypoints[m_CurrentWaypointIndex].position;
			ProbableWaypointPositionNew = new Vector3(x2, y2, position6.z + m_EscapeDistance);
			RelativeWaypointPositionNew = base.transform.InverseTransformPoint(ProbableWaypointPositionNew);
			if (ShowNavDebugLines)
			{
				if (AIWillEscape && AICanEscape)
				{
					UnityEngine.Debug.DrawLine(base.transform.position, ProbableWaypointPositionNew, Color.black);
				}
				else
				{
					UnityEngine.Debug.DrawLine(base.transform.position, ProbableWaypointPositionNew, Color.yellow);
				}
				UnityEngine.Debug.DrawLine(base.transform.position, ProbableWaypointPosition, Color.cyan);
			}
			CalculateSmoothing();
			if (RelativeWaypointPositionNew.z < 0f && !ReverseLockedbyObstacle)
			{
				m_AIReversing = true;
				m_IsReversing = true;
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
					AISoftBraking = false;
					AIHardBraking = false;
					AISuperBraking = false;
				}
				m_AiSteer = RGKSteerCorrectionFactor;
				if (RelativeWaypointPositionNew.z > 22f)
				{
					m_IsSteeringLocked = false;
					m_AIReversing = false;
					m_IsReversing = false;
					m_AiThrottle = 1f;
				}
			}
			else
			{
				m_AiSteer = m_AiSteerTemp + SteerFactor;
				m_AIReversing = false;
				if (m_ReversingWPSideDotPrev != 0f)
				{
					m_ReversingWPSideDotPrev = 0f;
				}
			}
			if (RelativeWaypointPositionNew.z > 22f)
			{
				ReverseLockedbyObstacle = false;
			}
			if (Mathf.Abs(m_AiSteer) < 1.5f)
			{
				if (!m_AIReversing)
				{
					float num = RelativeWaypointPositionNew.z / RelativeWaypointPositionNew.magnitude - Mathf.Abs(m_AiSteer) / 4f;
					if (m_EnableThrottleBonus && m_RandomizeSpeedFactor && !m_RandomizeSpeedFactorOnStart)
					{
						AISpeedFactor = UnityEngine.Random.Range(AISpeedFactorMin, AISpeedFactorMax + 0.1f);
						m_AiThrottle = num * AISpeedFactor;
					}
					else if (m_EnableThrottleBonus)
					{
						m_AiThrottle = num * AISpeedFactor;
					}
					else
					{
						m_AiThrottle = num;
					}
				}
				else
				{
					m_AiThrottle = -0.5f;
				}
				if (!AIBrakingForForwardCollision)
				{
					if (m_WPSoftBrakeSpeed > 0f)
					{
						if (base.Speed > m_WPSoftBrakeSpeed && base.Speed < m_WPHardBrakeSpeed)
						{
							m_AiThrottle *= AISoftBrakeFactor;
							m_AiIsBraking = true;
						}
						else if (base.Speed > m_WPHardBrakeSpeed)
						{
							m_AiThrottle *= AIHardBrakeFactor;
							m_AiIsBraking = true;
						}
					}
				}
				else if (m_WPSoftBrakeSpeed > 0f && base.Speed > m_WPHardBrakeSpeed)
				{
					m_AiThrottle *= AIHardBrakeFactor;
					m_AiIsBraking = true;
				}
				if (AISuperBraking)
				{
					m_AiThrottle = AIHardBrakeFactor * 1.5f;
					m_AiIsBraking = true;
				}
				else if (AIHardBraking)
				{
					m_AiThrottle = AIHardBrakeFactor;
					m_AiIsBraking = true;
				}
				else if (AISoftBraking)
				{
					m_AiThrottle = AISoftBrakeFactor;
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
			if (!(RelativeWaypointPositionNew.magnitude < num2))
			{
				return;
			}
			m_CurrentWaypointIndex++;
			if (m_CurrentWaypointIndex >= m_Waypoints.Count)
			{
				if (m_RaceManager.RaceLaps <= 1)
				{
					if (m_RaceManager.StartPoint == m_RaceManager.FinishPoint)
					{
						m_CurrentWaypointIndex = 0;
					}
					else
					{
						m_CurrentWaypointIndex = m_Waypoints.Count - 1;
					}
					m_AIWillRecalculate = false;
					return;
				}
				m_CurrentWaypointIndex = 0;
			}
			m_AIWillRecalculate = true;
			return;
			IL_0178:
			m_EscapeDistance = ThinkNewEscapePoint(leftWide, rightWide, m_CurrentWaypointIndex, m_EscapeDistance, RelativeWaypointPosition.x, AIOldRoadPosition, RelativeWaypointPosition);
			m_AIWillRecalculate = false;
			goto IL_01b0;
		}

		private void CalculateSmoothing()
		{
			if (!AIWillEscape)
			{
				if (UseSteerSmoothing && UseLerp)
				{
					if (Mathf.Abs(RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude) < SteerSmoothingReleaseCoef)
					{
						m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
						return;
					}
					if (SteerSmoothingMode == eSteerSmoothingMode.Basic)
					{
						float num = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
						if ((num < 0f && m_AiSteerTemp > 0f) || (m_AiSteerTemp < 0f && num > 0f))
						{
							m_AiSteerTemp = 0f;
						}
						m_AiSteerTemp = Mathf.Lerp(m_AiSteerTemp, num, Time.deltaTime * UnityEngine.Random.Range(SteerSmoothingLow / 10f, SteerSmoothingHigh / 10f));
						return;
					}
					float workingFPS = m_RaceManager.WorkingFPS;
					if (workingFPS <= 30f)
					{
						m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude * Time.deltaTime * UnityEngine.Random.Range(FPSBasedSmoothingdata.m_0_30FPS.Min, FPSBasedSmoothingdata.m_0_30FPS.Max);
					}
					else if (workingFPS > 30f && workingFPS <= 45f)
					{
						m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude * Time.deltaTime * UnityEngine.Random.Range(FPSBasedSmoothingdata.m_30_45FPS.Min, FPSBasedSmoothingdata.m_30_45FPS.Max);
					}
					else if (workingFPS > 45f && workingFPS <= 60f)
					{
						m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude * Time.deltaTime * UnityEngine.Random.Range(FPSBasedSmoothingdata.m_45_60FPS.Min, FPSBasedSmoothingdata.m_45_60FPS.Max);
					}
					else if (workingFPS > 60f)
					{
						m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude * Time.deltaTime * UnityEngine.Random.Range(FPSBasedSmoothingdata.m_60_NFPS.Min, FPSBasedSmoothingdata.m_60_NFPS.Max);
					}
				}
				else if (UseSteerSmoothing && !UseLerp)
				{
					if (Mathf.Abs(RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude) < SteerSmoothingReleaseCoef)
					{
						m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude * Time.deltaTime * UnityEngine.Random.Range(SteerSmoothingLow, SteerSmoothingHigh);
					}
					else
					{
						m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
					}
				}
				else
				{
					m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
				}
			}
			else
			{
				m_AiSteerTemp = RelativeWaypointPositionNew.x / RelativeWaypointPositionNew.magnitude;
			}
		}

		protected void CheckIsCarStuck()
		{
			if (m_RacerRegister.IsRacerStarted && !m_RacerRegister.IsRacerFinished && !m_RacerRegister.IsRacerDestroyed)
			{
				if (base.Speed < 3f)
				{
					m_StuckResetTimer += Time.deltaTime;
				}
				else
				{
					m_StuckResetTimer = 0f;
				}
				if (m_StuckResetTimer > m_StuckResetWait)
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
			Transform transform = (m_CurrentWaypointIndex <= 0) ? m_Waypoints[0].transform : m_Waypoints[m_CurrentWaypointIndex - 1].transform;
			m_AiThrottle = 0f;
			m_AiSteer = 0f;
			m_AIReversing = false;
			m_IsReversing = false;
			m_RGKCarController.Gear = 2;
			m_RGKCarController.ApplyDrive(0f, 0f, HandBrake: false);
			m_RGKCarController.IsPreviouslyReversed = true;
			base.transform.rotation = Quaternion.LookRotation(transform.forward);
			base.transform.position = transform.position;
			base.transform.position += Vector3.up * 0.1f;
			base.transform.position += Vector3.right * 0.1f;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			m_StuckResetTimer = 0f;
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
			m_IsReversing = false;
			m_RGKCarController.Gear = 1;
			m_RGKCarController.ApplyDrive(0f, 0f, HandBrake: true);
			m_RGKCarController.IsPreviouslyReversed = true;
			base.transform.rotation = Quaternion.LookRotation(TargetPosition.forward);
			base.transform.position = TargetPosition.position;
			base.transform.position += Vector3.up * 0.1f;
			base.transform.position += Vector3.right * 0.1f;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			m_StuckResetTimer = 0f;
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
