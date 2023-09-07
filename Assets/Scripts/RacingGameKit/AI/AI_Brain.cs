using RacingGameKit.Helpers;
using RacingGameKit.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.AI
{
	[AddComponentMenu("")]
	public class AI_Brain : MonoBehaviour, IRGKRacer
	{
		public string m_LastMessage = string.Empty;

		public string m_LastDedectedObjectName = string.Empty;

		public TextAsset AiConfigFile;

		public bool EnableDebugMessages;

		public bool ShowNavDebugLines = true;

		public bool DrawCollisonGizmo = true;

		[HideInInspector]
		private Transform m_CachedDistanceTransform;

		[HideInInspector]
		private Transform m_CachedReverseTransform;

		[HideInInspector]
		private Transform m_MyRootTransform;

		private List<Collider> m_MyColliders;

		[HideInInspector]
		public bool RandomizeBehaviors;

		[HideInInspector]
		public eAIProLevel AILevelBehavior;

		[HideInInspector]
		public eAIBehavior AIStateBehavior;

		[HideInInspector]
		public eAIStress AIStressBehavior;

		public LayerMask AIRacerDedectionLayer;

		public LayerMask ObstacleDedectionLayer;

		public float DedectionRadius = 4f;

		public float DedectionFrequency = 0.4f;

		[HideInInspector]
		private float DedectionFrequencyOriginal;

		[HideInInspector]
		private float NextDedectionCycle;

		[HideInInspector]
		public float CollisionAvoidAngle = 10f;

		[HideInInspector]
		private float m_CollisionAvoidCos = 0.707f;

		[HideInInspector]
		public float CollisionAvoidTime = 1f;

		[HideInInspector]
		public float CollisionAvoidFactor = 0.04f;

		[HideInInspector]
		protected bool m_AIWillRecalculate = true;

		public eAIRoadPosition AICurrentRoadPosition;

		[HideInInspector]
		protected eAIRoadPosition AIOldRoadPosition;

		public eAIRivalPosition AIAvoidingRivalPosition;

		public eAIRoadPosition AIAvoidingRivalRoadPosition;

		public bool m_EnableThrottleBonus;

		public bool m_RandomizeSpeedFactor;

		public bool m_RandomizeSpeedFactorOnStart = true;

		public float AISpeedFactorMin = 1f;

		public float AISpeedFactorMax = 1f;

		public float AISpeedFactor = 1f;

		[HideInInspector]
		public float AISteerFactor = 1f;

		[HideInInspector]
		public float AIEscapeFactor = 1f;

		[HideInInspector]
		public float AISoftBrakeFactor = -0.1f;

		[HideInInspector]
		public float AIHardBrakeFactor = -0.3f;

		protected bool AISoftBraking;

		protected bool AIHardBraking;

		protected bool AISuperBraking;

		protected bool AIBrakingForForwardCollision;

		public float ForwardSensorDistance = 15f;

		public float ForwardSensorDistanceHighSpeed = 25f;

		public AnimationCurve ForwardSensorSpeedCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		[HideInInspector]
		private float ForwardSensorDistanceOriginal;

		[HideInInspector]
		public float ForwardSensorAngle = 7f;

		[HideInInspector]
		public float ForwardSensorWide;

		[HideInInspector]
		public float SideSensorDistance = 15f;

		[HideInInspector]
		public float SideSensorAngle = 30f;

		[HideInInspector]
		public float SideSensorWide;

		[HideInInspector]
		public float WallSensorDistance = 1f;

		[HideInInspector]
		public float ReverseSensorDistance = 1f;

		private float ReverseSensorAngle = 45f;

		private float ReverseSensorWide;

		[HideInInspector]
		public bool m_AIReversing;

		[HideInInspector]
		public bool ReverseLockedbyObstacle;

		public bool AIWillAvoid;

		public bool AICanEscape = true;

		public bool AIWillEscape;

		private List<Collider> m_DetectedObjects;

		private List<IRGKRacer> ThreatRivals;

		[HideInInspector]
		public float SteerFactor;

		[HideInInspector]
		private float m_VehicleSpeed;

		private float m_PredictedAiCollisionTime;

		private bool LeftSensorOK = true;

		private bool RightSensorOK = true;

		[HideInInspector]
		public bool ObstacleAvoidingEnabled = true;

		[HideInInspector]
		public float ObstacleDetectionDistance = 15f;

		[HideInInspector]
		public float ObstacleAvoidFactor = 0.2f;

		[HideInInspector]
		public bool ObstacleAvoidToLeft;

		[HideInInspector]
		public bool ObstacleAvoidToRight;

		[HideInInspector]
		public bool ObstacleIsAvoiding;

		[HideInInspector]
		public float OverlappedCount;

		[HideInInspector]
		public float DedectedRacerCount;

		private float SteerCoef;

		[HideInInspector]
		public bool UseSideCorrection;

		public bool m_Sleep;

		private float m_DecidedEscape;

		private float m_CalculatedEscape;

		protected int m_DebugStateThinkNewEscapePoint;

		private float PredictedCollisionTimeOld;

		private float m_WatchingColliderCenterOffset;

		private Collider m_WatchingCollider;

		private RaycastHit TargetHit;

		private float DistHit1;

		private float DistHit2;

		private float DistHit3;

		private List<RaycastHit> lDist = new List<RaycastHit>();

		public Vector3 Velocity
		{
			get
			{
				if (m_MyRootTransform == null)
				{
					return Vector3.zero;
				}
				return m_MyRootTransform.forward * m_VehicleSpeed;
			}
		}

		public float Speed
		{
			get
			{
				return m_VehicleSpeed;
			}
			set
			{
				m_VehicleSpeed = value;
			}
		}

		public Transform CachedTransform => m_MyRootTransform;

		public GameObject CachedGameObject => m_MyRootTransform.gameObject;

		public eAIRoadPosition CurrentRoadPosition
		{
			get
			{
				return AICurrentRoadPosition;
			}
			set
			{
				AICurrentRoadPosition = value;
			}
		}

		public Vector3 Position => m_MyRootTransform.position;

		public float EscapeDistance => m_CalculatedEscape;

		protected void Initialize_AI()
		{
			m_MyRootTransform = GetParent(base.transform).transform;
			AICurrentRoadPosition = eAIRoadPosition.UnKnown;
			AIOldRoadPosition = eAIRoadPosition.UnKnown;
			DedectionFrequencyOriginal = DedectionFrequency;
			ForwardSensorDistanceOriginal = ForwardSensorDistance;
			if (RandomizeBehaviors)
			{
				AILevelBehavior = (eAIProLevel)UnityEngine.Random.Range(0, 3);
				AIStateBehavior = (eAIBehavior)UnityEngine.Random.Range(0, 3);
			}
			BuildAICoefficiencies();
			m_CachedDistanceTransform = m_MyRootTransform.Find("_DistancePoint");
			m_CachedReverseTransform = m_MyRootTransform.Find("_ReversePoint");
			if (m_CachedDistanceTransform == null)
			{
				UnityEngine.Debug.Log("RGK WARNING\r\nDistance checker object not found! Be sure _DistancePoint named object placed under child!");
			}
			if (m_CachedReverseTransform == null)
			{
				UnityEngine.Debug.Log("Reverse Point Missing!");
			}
			m_MyColliders = new List<Collider>();
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				m_MyColliders.Add(componentsInChildren[i]);
			}
		}

		private void BuildAICoefficiencies()
		{
			switch (AILevelBehavior)
			{
			case eAIProLevel.Novice:
				AiConfigFile = (TextAsset)Resources.Load("ai_setup_novice", typeof(TextAsset));
				LoadAiConfiguration("ai_setup_novice");
				break;
			case eAIProLevel.Intermediate:
				AiConfigFile = (TextAsset)Resources.Load("ai_setup_intermediate", typeof(TextAsset));
				LoadAiConfiguration("ai_setup_intermediate");
				break;
			case eAIProLevel.Pro:
				AiConfigFile = (TextAsset)Resources.Load("ai_setup_pro", typeof(TextAsset));
				LoadAiConfiguration("ai_setup_pro");
				break;
			case eAIProLevel.Custom:
				if (AiConfigFile != null)
				{
					LoadAiConfiguration(AiConfigFile.name);
				}
				break;
			}
		}

		private void LoadAiConfiguration(string ConfigurationFile)
		{
			try
			{
				string text = JsonUtils.ReadJsonFile(ConfigurationFile).Replace("\r\n", string.Empty);
				if (text != string.Empty)
				{
					JSONObject jSONObject = new JSONObject(text);
					if (jSONObject.HasField("softbrake_factor"))
					{
						switch (Convert.ToInt16(jSONObject.GetField("ai_behavior").str))
						{
						case 1:
							AIStateBehavior = eAIBehavior.Normal;
							break;
						case 2:
							AIStateBehavior = eAIBehavior.UnConfident;
							break;
						case 3:
							AIStateBehavior = eAIBehavior.Aggresive;
							break;
						}
						AISoftBrakeFactor = float.Parse(jSONObject.GetField("softbrake_factor").str);
						AIHardBrakeFactor = float.Parse(jSONObject.GetField("hardbrake_factor").str);
						DedectionRadius = float.Parse(jSONObject.GetField("detection_radius").str);
						DedectionFrequency = float.Parse(jSONObject.GetField("detection_frequency").str);
						CollisionAvoidAngle = float.Parse(jSONObject.GetField("collision_avoid_angle").str);
						CollisionAvoidTime = float.Parse(jSONObject.GetField("collision_avoid_time").str);
						CollisionAvoidFactor = float.Parse(jSONObject.GetField("collision_avoid_factor").str);
						m_EnableThrottleBonus = jSONObject.GetField("enable_speed_factor").b;
						m_RandomizeSpeedFactor = jSONObject.GetField("random_speed_factor").b;
						m_RandomizeSpeedFactorOnStart = jSONObject.GetField("random_speed_factor_on_start").b;
						AISpeedFactorMin = float.Parse(jSONObject.GetField("speed_factor").str);
						AISpeedFactorMax = float.Parse(jSONObject.GetField("speed_factor_max").str);
						if (AISpeedFactorMax < AISpeedFactorMin)
						{
							AISpeedFactorMax = AISpeedFactorMin;
						}
						if (!m_RandomizeSpeedFactor)
						{
							AISpeedFactor = float.Parse(jSONObject.GetField("speed_factor").str);
						}
						ForwardSensorDistance = float.Parse(jSONObject.GetField("forward_sensor_distance").str);
						ForwardSensorDistanceHighSpeed = float.Parse(jSONObject.GetField("forward_sensor_distance_high_speed").str);
						ForwardSensorAngle = float.Parse(jSONObject.GetField("forward_sensor_angle").str);
						SideSensorDistance = float.Parse(jSONObject.GetField("side_sensor_distance").str);
						SideSensorAngle = float.Parse(jSONObject.GetField("side_sensor_angle").str);
						WallSensorDistance = float.Parse(jSONObject.GetField("wall_sensor_distance").str);
						ReverseSensorDistance = float.Parse(jSONObject.GetField("reverse_sensor_distance").str);
						ObstacleAvoidingEnabled = jSONObject.GetField("enable_obstacle_avoid").b;
						ObstacleDetectionDistance = float.Parse(jSONObject.GetField("obstacle_dedection_distance").str);
						ObstacleAvoidFactor = float.Parse(jSONObject.GetField("obstacle_avoid_factor").str);
						if (Application.isEditor)
						{
							UnityEngine.Debug.Log("RGK INFORMATON: AI settings file \"" + ConfigurationFile + "\" loaded succesfully!");
						}
					}
				}
			}
			catch
			{
				UnityEngine.Debug.LogError("RGK ERROR: Invalid AI Config File in " + ConfigurationFile);
				UnityEngine.Debug.Break();
			}
		}

		public void LoadAiConfiguration(TextAsset ConfigurationFile)
		{
			LoadAiConfiguration(ConfigurationFile.name);
		}

		public JSONObject buildJSONData()
		{
			string str = JsonUtils.ReadJsonFile(AiConfigFile.name);
			JSONObject jSONObject = new JSONObject(str);
			if (jSONObject == null)
			{
				UnityEngine.Debug.LogWarning("RGK Ai \r\nImproper JSon File! Please use ai_setup_template.txt for creating your own configuration");
				return null;
			}
			switch (AIStateBehavior)
			{
			case eAIBehavior.Normal:
				jSONObject.GetField("ai_behavior").str = "1";
				break;
			case eAIBehavior.UnConfident:
				jSONObject.GetField("ai_behavior").str = "2";
				break;
			case eAIBehavior.Aggresive:
				jSONObject.GetField("ai_behavior").str = "3";
				break;
			}
			jSONObject.GetField("softbrake_factor").str = AISoftBrakeFactor.ToString();
			jSONObject.GetField("hardbrake_factor").str = AIHardBrakeFactor.ToString();
			jSONObject.GetField("detection_radius").str = DedectionRadius.ToString();
			jSONObject.GetField("detection_frequency").str = DedectionFrequency.ToString();
			jSONObject.GetField("collision_avoid_angle").str = CollisionAvoidAngle.ToString();
			jSONObject.GetField("collision_avoid_time").str = CollisionAvoidTime.ToString();
			jSONObject.GetField("collision_avoid_factor").str = CollisionAvoidFactor.ToString();
			jSONObject.GetField("enable_speed_factor").b = m_EnableThrottleBonus;
			jSONObject.GetField("random_speed_factor").b = m_RandomizeSpeedFactor;
			jSONObject.GetField("random_speed_factor_on_start").b = m_RandomizeSpeedFactorOnStart;
			jSONObject.GetField("speed_factor").str = AISpeedFactorMin.ToString();
			jSONObject.GetField("speed_factor_max").str = AISpeedFactorMax.ToString();
			jSONObject.GetField("forward_sensor_distance").str = ForwardSensorDistance.ToString();
			jSONObject.GetField("forward_sensor_distance_high_speed").str = ForwardSensorDistanceHighSpeed.ToString();
			jSONObject.GetField("forward_sensor_angle").str = ForwardSensorAngle.ToString();
			jSONObject.GetField("side_sensor_distance").str = SideSensorDistance.ToString();
			jSONObject.GetField("side_sensor_angle").str = SideSensorAngle.ToString();
			jSONObject.GetField("wall_sensor_distance").str = WallSensorDistance.ToString();
			jSONObject.GetField("reverse_sensor_distance").str = ReverseSensorDistance.ToString();
			jSONObject.GetField("enable_obstacle_avoid").b = ObstacleAvoidingEnabled;
			jSONObject.GetField("obstacle_dedection_distance").str = ObstacleDetectionDistance.ToString();
			jSONObject.GetField("obstacle_avoid_factor").str = ObstacleAvoidFactor.ToString();
			return jSONObject;
		}

		private void SaveConfiguration()
		{
		}

		public void ExecuteRadar()
		{
			m_CollisionAvoidCos = Mathf.Cos(CollisionAvoidAngle * ((float)Math.PI / 180f));
			NextDedectionCycle -= Time.deltaTime;
			if (NextDedectionCycle <= 0f)
			{
				NextDedectionCycle = DedectionFrequency;
				DetectAIs();
				CalculateStress();
				SteerFactor = CalculateSteer();
				if (SteerFactor != 0f)
				{
					AIWillAvoid = true;
				}
				else
				{
					AIWillAvoid = false;
				}
				CalculateEscaping();
				ReCalculateDedection(ForceHalf: false);
			}
			if (AIBrakingForForwardCollision)
			{
				CalculateEscaping();
			}
		}

		private void ReCalculateDedection(bool ForceHalf)
		{
			if (!(DedectionFrequency > 0f))
			{
				return;
			}
			if (!ForceHalf)
			{
				if (ThreatRivals.Count == 1 && AIWillEscape)
				{
					DedectionFrequency = DedectionFrequencyOriginal / 2f;
				}
				else if (ThreatRivals.Count > 2)
				{
					DedectionFrequency = DedectionFrequencyOriginal / 3f;
				}
				else
				{
					DedectionFrequency = DedectionFrequencyOriginal;
				}
			}
			else
			{
				DedectionFrequency = DedectionFrequencyOriginal / 2f;
			}
		}

		private void CalculateStress()
		{
			if (ThreatRivals.Count == 0)
			{
				AIStressBehavior = eAIStress.Normal;
			}
			else if (ThreatRivals.Count >= 1 && ThreatRivals.Count < 2)
			{
				AIStressBehavior = eAIStress.Awared;
			}
			else if (ThreatRivals.Count >= 2)
			{
				AIStressBehavior = eAIStress.Stressed;
			}
			switch (AIStressBehavior)
			{
			case eAIStress.Normal:
				break;
			case eAIStress.Relaxed:
				break;
			case eAIStress.Awared:
				break;
			case eAIStress.Stressed:
				break;
			}
		}

		internal virtual void Update()
		{
			if (!AIBrakingForForwardCollision)
			{
				ForwardSensorDistance = ForwardSensorDistanceHighSpeed * ForwardSensorSpeedCurve.Evaluate(Speed / 150f);
				if (ForwardSensorDistance < ForwardSensorDistanceOriginal)
				{
					ForwardSensorDistance = ForwardSensorDistanceOriginal;
				}
				if (ForwardSensorDistance > ForwardSensorDistanceHighSpeed)
				{
					ForwardSensorDistance = ForwardSensorDistanceHighSpeed;
				}
			}
		}

		protected float ThinkNewEscapePoint(float WideLeft, float WideRight, int CurrentWaypointIndex, float CalculatedEscape, float CurrentRelativePos, eAIRoadPosition AIRoadPosition, Vector3 CurrentWaypointPosition)
		{
			m_DecidedEscape = 0f;
			m_CalculatedEscape = CalculatedEscape;
			m_DecidedEscape = CalculatedEscape;
			m_DebugStateThinkNewEscapePoint = 0;
			if (AIWillAvoid && !AIWillEscape)
			{
				m_DebugStateThinkNewEscapePoint = 1;
			}
			else if (AIWillAvoid && AIWillEscape)
			{
				bool flag = false;
				bool flag2 = true;
				if (LeftSensorOK)
				{
					Vector3 vector = m_MyRootTransform.position + new Vector3(-3f, 0f, 0f);
					Vector3 vector2 = m_MyRootTransform.position + new Vector3(3f, 0f, 0f);
					Vector3 vector3 = m_MyRootTransform.TransformDirection(-3f, 0f, 0f);
					if (ShowNavDebugLines)
					{
						UnityEngine.Debug.DrawRay(vector, vector3, Color.green);
						UnityEngine.Debug.DrawRay(vector2, vector3, Color.green);
					}
					if (Physics.Raycast(vector, vector3, out RaycastHit hitInfo, 3f, AIRacerDedectionLayer) || Physics.Raycast(vector2, vector3, out hitInfo, 3f, AIRacerDedectionLayer))
					{
						if (ShowNavDebugLines)
						{
							UnityEngine.Debug.DrawRay(vector, vector3, Color.black);
							UnityEngine.Debug.DrawRay(vector2, vector3, Color.black);
						}
						GameObject parent = GetParent(hitInfo.transform);
						if (parent.GetComponent(typeof(IRGKRacer)) != null)
						{
							flag2 = false;
						}
					}
					if (flag2)
					{
						m_DecidedEscape = UnityEngine.Random.Range(WideLeft / 2f, WideLeft);
						WriteToConsole("decided to escape left");
					}
					flag = true;
				}
				else if (RightSensorOK)
				{
					Vector3 vector4 = m_MyRootTransform.position + new Vector3(-3f, 0f, 0f);
					Vector3 start = m_MyRootTransform.position + new Vector3(3f, 0f, 0f);
					Vector3 vector5 = m_MyRootTransform.TransformDirection(3f, 0f, 0f);
					if (ShowNavDebugLines)
					{
						UnityEngine.Debug.DrawRay(vector4, vector5, Color.yellow);
						UnityEngine.Debug.DrawRay(start, vector5, Color.yellow);
					}
					if (Physics.Raycast(vector4, vector5, out RaycastHit hitInfo2, 3f, AIRacerDedectionLayer) || Physics.Raycast(vector4, vector5, out hitInfo2, 3f, AIRacerDedectionLayer))
					{
						if (ShowNavDebugLines)
						{
							UnityEngine.Debug.DrawRay(vector4, vector5, Color.black);
							UnityEngine.Debug.DrawRay(start, vector5, Color.black);
						}
						GameObject parent2 = GetParent(hitInfo2.transform);
						if (parent2.GetComponent(typeof(IRGKRacer)) != null)
						{
							flag2 = false;
						}
					}
					if (flag2)
					{
						m_DecidedEscape = UnityEngine.Random.Range(WideRight / 2f, WideRight);
						WriteToConsole("decided to escape right");
					}
				}
				if (flag2 && flag)
				{
					m_DecidedEscape *= -1f;
				}
				m_DebugStateThinkNewEscapePoint = 2;
			}
			else
			{
				if (m_CalculatedEscape > 0f && Mathf.Abs(m_CalculatedEscape) > WideRight)
				{
					m_DecidedEscape = WideRight;
				}
				else if (m_CalculatedEscape < 0f && Mathf.Abs(m_CalculatedEscape) > WideLeft)
				{
					m_DecidedEscape = WideLeft * -1f;
				}
				else
				{
					m_DecidedEscape = CalculatedEscape;
				}
				m_DebugStateThinkNewEscapePoint = 3;
			}
			if (UseSideCorrection)
			{
				if (AICurrentRoadPosition == eAIRoadPosition.Right && m_DecidedEscape > 0f)
				{
					m_DecidedEscape *= -1f;
				}
				if (AICurrentRoadPosition == eAIRoadPosition.Left && m_DecidedEscape < 0f)
				{
					m_DecidedEscape *= -1f;
				}
			}
			return m_DecidedEscape;
		}

		private void OnCollisionStay(Collision collisionInfo)
		{
			if (m_AIReversing)
			{
				return;
			}
			m_WatchingColliderCenterOffset = 0f;
			if (!m_DetectedObjects.Find((Collider x) => x = collisionInfo.collider))
			{
				return;
			}
			m_WatchingCollider = collisionInfo.collider;
			Vector3 lhs = collisionInfo.transform.position - Position;
			m_WatchingColliderCenterOffset = Vector3.Dot(lhs, base.transform.forward);
			IRGKRacer iRGKRacer = (IRGKRacer)GetParent(m_WatchingCollider.transform).GetComponent(typeof(IRGKRacer));
			if (iRGKRacer != null)
			{
				if (iRGKRacer.Speed + 5f > Speed && m_WatchingColliderCenterOffset > 0.2f)
				{
					Vector3 lhs2 = collisionInfo.transform.position - Position;
					float num = Vector3.Dot(lhs2, base.transform.right);
					SteerFactor = ((!(num > 0f)) ? CollisionAvoidFactor : (0f - CollisionAvoidFactor));
					AISoftBraking = true;
				}
				else if (m_WatchingColliderCenterOffset < 0.2f)
				{
					Vector3 lhs3 = collisionInfo.transform.position - Position;
					float num2 = Vector3.Dot(lhs3, base.transform.right);
					SteerFactor = ((!(num2 > 0f)) ? CollisionAvoidFactor : (0f - CollisionAvoidFactor));
				}
			}
		}

		private void OnCollisionExit(Collision collisionInfo)
		{
			if (m_WatchingCollider == collisionInfo.collider)
			{
				AISoftBraking = false;
				m_WatchingCollider = null;
			}
		}

		private void CalculateEscaping()
		{
			DistHit1 = 0f;
			DistHit2 = 0f;
			DistHit3 = 0f;
			SideSensorWide = Mathf.Tan(SideSensorAngle * ((float)Math.PI / 180f)) * SideSensorDistance;
			ForwardSensorWide = Mathf.Tan(ForwardSensorAngle * ((float)Math.PI / 180f)) * ForwardSensorDistance;
			Transform cachedDistanceTransform = m_CachedDistanceTransform;
			Vector3 position = cachedDistanceTransform.position;
			Transform transform = cachedDistanceTransform;
			Vector3 right = Vector3.right;
			Vector3 localPosition = cachedDistanceTransform.localPosition;
			Vector3 vector = position + transform.TransformDirection(right * (localPosition.x - 0.35f));
			Vector3 position2 = cachedDistanceTransform.position;
			Transform transform2 = cachedDistanceTransform;
			Vector3 right2 = Vector3.right;
			Vector3 localPosition2 = cachedDistanceTransform.localPosition;
			Vector3 vector2 = position2 + transform2.TransformDirection(right2 * (localPosition2.x + 0.35f));
			Vector3 vector3 = cachedDistanceTransform.TransformDirection(Vector3.forward * ForwardSensorDistance);
			Vector3 vector4 = cachedDistanceTransform.TransformDirection(Vector3.left * ForwardSensorWide + Vector3.forward * ForwardSensorDistance);
			Vector3 vector5 = cachedDistanceTransform.TransformDirection(Vector3.right * ForwardSensorWide + Vector3.forward * ForwardSensorDistance);
			Vector3 position3 = cachedDistanceTransform.position;
			Transform transform3 = cachedDistanceTransform;
			Vector3 right3 = Vector3.right;
			Vector3 localPosition3 = cachedDistanceTransform.localPosition;
			Vector3 vector6 = position3 + transform3.TransformDirection(right3 * (localPosition3.x - 0.5f));
			Vector3 position4 = cachedDistanceTransform.position;
			Transform transform4 = cachedDistanceTransform;
			Vector3 right4 = Vector3.right;
			Vector3 localPosition4 = cachedDistanceTransform.localPosition;
			Vector3 vector7 = position4 + transform4.TransformDirection(right4 * (localPosition4.x + 0.5f));
			Vector3 vector8 = cachedDistanceTransform.TransformDirection(Vector3.left * SideSensorWide + Vector3.forward * SideSensorDistance);
			Vector3 vector9 = cachedDistanceTransform.TransformDirection(Vector3.right * SideSensorWide + Vector3.forward * SideSensorDistance);
			Vector3 position5 = cachedDistanceTransform.position;
			Transform transform5 = cachedDistanceTransform;
			Vector3 right5 = Vector3.right;
			Vector3 localPosition5 = cachedDistanceTransform.localPosition;
			Vector3 vector10 = position5 + transform5.TransformDirection(right5 * (localPosition5.x - 0.5f));
			Vector3 position6 = cachedDistanceTransform.position;
			Transform transform6 = cachedDistanceTransform;
			Vector3 right6 = Vector3.right;
			Vector3 localPosition6 = cachedDistanceTransform.localPosition;
			Vector3 vector11 = position6 + transform6.TransformDirection(right6 * (localPosition6.x + 0.5f));
			Vector3 vector12 = cachedDistanceTransform.TransformDirection(0f - WallSensorDistance, 0f, 0f);
			Vector3 vector13 = cachedDistanceTransform.TransformDirection(WallSensorDistance, 0f, 0f);
			if (ShowNavDebugLines)
			{
				UnityEngine.Debug.DrawRay(cachedDistanceTransform.position, vector3, Color.blue);
				UnityEngine.Debug.DrawRay(vector, vector4, Color.blue);
				UnityEngine.Debug.DrawRay(vector2, vector5, Color.blue);
				UnityEngine.Debug.DrawRay(vector10, vector12, Color.magenta);
				UnityEngine.Debug.DrawRay(vector11, vector13, Color.magenta);
			}
			bool flag = false;
			if (Physics.Raycast(cachedDistanceTransform.position, vector3, out RaycastHit hitInfo, ForwardSensorDistance, AIRacerDedectionLayer))
			{
				flag = true;
			}
			if (Physics.Raycast(vector, vector4, out RaycastHit hitInfo2, ForwardSensorDistance, AIRacerDedectionLayer))
			{
				flag = true;
			}
			if (Physics.Raycast(vector2, vector5, out RaycastHit hitInfo3, ForwardSensorDistance, AIRacerDedectionLayer))
			{
				flag = true;
			}
			if (flag)
			{
				if (hitInfo.transform != null && !hitInfo.collider.isTrigger)
				{
					DistHit1 = hitInfo.distance;
					if (ShowNavDebugLines)
					{
						UnityEngine.Debug.DrawRay(cachedDistanceTransform.position, vector3, Color.green);
					}
				}
				if (hitInfo2.transform != null && !hitInfo2.collider.isTrigger)
				{
					DistHit2 = hitInfo2.distance;
					if (ShowNavDebugLines)
					{
						UnityEngine.Debug.DrawRay(vector, vector4, Color.green);
					}
				}
				if (hitInfo3.transform != null && !hitInfo3.collider.isTrigger)
				{
					DistHit3 = hitInfo3.distance;
					if (ShowNavDebugLines)
					{
						UnityEngine.Debug.DrawRay(vector2, vector5, Color.green);
					}
				}
				lDist.Clear();
				if (DistHit1 > 0f)
				{
					lDist.Add(hitInfo);
				}
				if (DistHit2 > 0f)
				{
					lDist.Add(hitInfo2);
				}
				if (DistHit3 > 0f)
				{
					lDist.Add(hitInfo3);
				}
				if (lDist.Count > 0)
				{
					lDist.Sort((RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
					TargetHit = lDist[0];
					m_LastDedectedObjectName = TargetHit.collider.name;
					GameObject parent = GetParent(TargetHit.transform);
					IRGKRacer iRGKRacer = (IRGKRacer)parent.GetComponent(typeof(IRGKRacer));
					if (iRGKRacer != null)
					{
						if (Speed - iRGKRacer.Speed > 0f)
						{
							m_PredictedAiCollisionTime = PredictNearestApproachTime(iRGKRacer);
							if (EnableDebugMessages)
							{
								UnityEngine.Debug.Log(m_PredictedAiCollisionTime);
							}
							if (DecideBreaking(m_PredictedAiCollisionTime))
							{
								if (m_PredictedAiCollisionTime <= 0.55f && Speed > 50f)
								{
									WriteToConsole("I'm faster - Decided to Brake!");
									AISoftBraking = true;
									if (m_PredictedAiCollisionTime <= 0.4f)
									{
										AISoftBraking = false;
										AIHardBraking = true;
									}
									if (m_PredictedAiCollisionTime <= 0.2f)
									{
										AISoftBraking = false;
										AIHardBraking = false;
										AISuperBraking = true;
									}
									AIBrakingForForwardCollision = true;
								}
							}
							else
							{
								if (m_PredictedAiCollisionTime < 1.5f)
								{
									if (ShowNavDebugLines)
									{
										UnityEngine.Debug.DrawRay(vector6, vector8, Color.green);
										UnityEngine.Debug.DrawRay(vector7, vector9, Color.green);
									}
									LeftSensorOK = true;
									RightSensorOK = true;
									if (Physics.Raycast(vector6, vector8, out RaycastHit _, SideSensorDistance, AIRacerDedectionLayer))
									{
										LeftSensorOK = false;
									}
									if (Physics.Raycast(vector7, vector9, out RaycastHit _, SideSensorDistance, AIRacerDedectionLayer))
									{
										RightSensorOK = false;
									}
									if (LeftSensorOK || RightSensorOK)
									{
										WriteToConsole("I'm faster - looking for escape! ");
										AIWillAvoid = true;
										AICanEscape = true;
										AIWillEscape = true;
									}
									else
									{
										WriteToConsole("I'm faster - Sensors sux");
										AICanEscape = false;
										AIWillEscape = false;
									}
								}
								if (m_PredictedAiCollisionTime <= 0.65f && Speed > 50f)
								{
									AICanEscape = false;
									AIWillEscape = false;
									WriteToConsole("Oh Sh=t!");
									AISoftBraking = true;
									if (m_PredictedAiCollisionTime <= 0.4f)
									{
										AISoftBraking = false;
										AIHardBraking = true;
									}
									if (m_PredictedAiCollisionTime <= 0.2f)
									{
										AISoftBraking = false;
										AIHardBraking = false;
										AISuperBraking = true;
									}
									AIBrakingForForwardCollision = true;
								}
							}
						}
						else
						{
							if (AISoftBraking)
							{
								AISoftBraking = false;
							}
							if (AIHardBraking)
							{
								AIHardBraking = false;
							}
							if (AISuperBraking)
							{
								AISuperBraking = false;
							}
							AICanEscape = false;
							AIWillEscape = false;
							AIBrakingForForwardCollision = false;
							WriteToConsole("There's an AI that faster then me...");
							m_PredictedAiCollisionTime = PredictNearestApproachTime(iRGKRacer);
							if (m_PredictedAiCollisionTime > 0f && m_PredictedAiCollisionTime <= 0.03f && Speed > 100f)
							{
								WriteToConsole("So close, backing a bit...");
								AISoftBraking = true;
							}
						}
					}
					else
					{
						WriteToConsole("There's something that not an AI..");
					}
				}
			}
			else
			{
				ForwardSensorDistance = ForwardSensorDistanceOriginal;
				if (AISoftBraking)
				{
					AISoftBraking = false;
				}
				if (AIHardBraking)
				{
					AIHardBraking = false;
				}
				if (AISuperBraking)
				{
					AISuperBraking = false;
				}
				AICanEscape = false;
				AIWillEscape = false;
				WriteToConsole("Nothing at front now");
				AIBrakingForForwardCollision = false;
			}
			if (!ObstacleAvoidingEnabled || m_AIReversing)
			{
				return;
			}

            RaycastHit hitInfo6;

			if (Physics.Raycast(cachedDistanceTransform.position, vector3, out  hitInfo6, ObstacleDetectionDistance, ObstacleDedectionLayer) || Physics.Raycast(vector, vector4, out hitInfo6, ObstacleDetectionDistance, ObstacleDedectionLayer) || Physics.Raycast(vector2, vector5, out hitInfo6, ObstacleDetectionDistance, ObstacleDedectionLayer) || Physics.Raycast(vector10, vector12, out hitInfo6, WallSensorDistance, ObstacleDedectionLayer) || Physics.Raycast(vector11, vector13, out hitInfo6, WallSensorDistance, ObstacleDedectionLayer))
			{
				float num = PredictCollisionTime(hitInfo6.transform.gameObject);
				if (PredictedCollisionTimeOld == 0f)
				{
					PredictedCollisionTimeOld = num;
					ObstacleIsAvoiding = true;
					WriteToConsole("Going to hit something in " + num);
					if (ShowNavDebugLines)
					{
						UnityEngine.Debug.DrawRay(vector6, vector8, Color.magenta);
						UnityEngine.Debug.DrawRay(vector7, vector9, Color.magenta);
					}
					ObstacleAvoidToLeft = true;
					ObstacleAvoidToRight = true;
					if (Physics.Raycast(vector6, vector8, out RaycastHit _, SideSensorDistance, ObstacleDedectionLayer))
					{
						ObstacleAvoidToLeft = false;
					}
					if (Physics.Raycast(vector7, vector9, out RaycastHit _, SideSensorDistance, ObstacleDedectionLayer))
					{
						ObstacleAvoidToRight = false;
					}
				}
				else
				{
					PredictedCollisionTimeOld = 0f;
				}
			}
			else
			{
				ObstacleIsAvoiding = false;
			}
			if (ObstacleIsAvoiding)
			{
				DecideObstacleAvoid(ObstacleAvoidToLeft, ObstacleAvoidToRight, AIAvoidingRivalPosition);
			}
		}

		protected void CalculateReversing()
		{
			if (!(m_CachedReverseTransform != null))
			{
				return;
			}
			ReverseSensorWide = Mathf.Tan(ReverseSensorAngle * ((float)Math.PI / 180f)) * ReverseSensorDistance;
			Transform cachedReverseTransform = m_CachedReverseTransform;
			Vector3 position = cachedReverseTransform.position;
			Transform transform = cachedReverseTransform;
			Vector3 right = Vector3.right;
			Vector3 localPosition = cachedReverseTransform.localPosition;
			Vector3 vector = position + transform.TransformDirection(right * (localPosition.x - 0.35f));
			Vector3 position2 = cachedReverseTransform.position;
			Transform transform2 = cachedReverseTransform;
			Vector3 right2 = Vector3.right;
			Vector3 localPosition2 = cachedReverseTransform.localPosition;
			Vector3 vector2 = position2 + transform2.TransformDirection(right2 * (localPosition2.x + 0.35f));
			Vector3 vector3 = cachedReverseTransform.TransformDirection(Vector3.back * ReverseSensorDistance);
			Vector3 vector4 = cachedReverseTransform.TransformDirection(Vector3.left * ReverseSensorWide + Vector3.back * ReverseSensorWide);
			Vector3 vector5 = cachedReverseTransform.TransformDirection(Vector3.right * ReverseSensorWide + Vector3.back * ReverseSensorWide);
			if (m_AIReversing)
			{
				if (ShowNavDebugLines)
				{
					UnityEngine.Debug.DrawRay(cachedReverseTransform.position, vector3, Color.magenta);
					UnityEngine.Debug.DrawRay(vector, vector4, Color.yellow);
					UnityEngine.Debug.DrawRay(vector2, vector5, Color.green);
				}
				bool flag = false;
				if (Physics.Raycast(cachedReverseTransform.position, vector3, out RaycastHit hitInfo, ReverseSensorDistance, ObstacleDedectionLayer))
				{
					flag = true;
				}
				if (Physics.Raycast(vector, vector4, out hitInfo, ReverseSensorDistance, ObstacleDedectionLayer))
				{
					flag = true;
				}
				if (Physics.Raycast(vector2, vector5, out hitInfo, ReverseSensorDistance, ObstacleDedectionLayer))
				{
					flag = true;
				}
				if (Physics.Raycast(cachedReverseTransform.position, vector3, out hitInfo, ReverseSensorDistance, AIRacerDedectionLayer))
				{
					flag = true;
				}
				if (Physics.Raycast(vector, vector4, out hitInfo, ReverseSensorDistance, AIRacerDedectionLayer))
				{
					flag = true;
				}
				if (Physics.Raycast(vector2, vector5, out hitInfo, ReverseSensorDistance, AIRacerDedectionLayer))
				{
					flag = true;
				}
				if (flag)
				{
					ReverseLockedbyObstacle = true;
					m_AIReversing = false;
				}
			}
		}

		private bool DecideBreaking(float PredictedColTime)
		{
			bool result = false;
			if (PredictedColTime < 1f)
			{
				if (AIStateBehavior == eAIBehavior.UnConfident)
				{
					result = true;
				}
				else if (AIStateBehavior == eAIBehavior.Normal && AIStressBehavior == eAIStress.Stressed)
				{
					result = true;
				}
				if (!AICanEscape)
				{
					result = true;
				}
			}
			else
			{
				ReCalculateDedection(ForceHalf: true);
				WriteToConsole("Decided not to brake but will watch front car!");
			}
			return result;
		}

		private void DetectAIs()
		{
			if (m_DetectedObjects == null)
			{
				m_DetectedObjects = new List<Collider>();
			}
			m_DetectedObjects.Clear();
			if (ThreatRivals == null)
			{
				ThreatRivals = new List<IRGKRacer>();
			}
			ThreatRivals.Clear();
			Collider[] array = Physics.OverlapSphere(Position, DedectionRadius, AIRacerDedectionLayer);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				foreach (Collider myCollider in m_MyColliders)
				{
					if (myCollider == collider)
					{
						break;
					}
					m_DetectedObjects.Add(collider);
					GameObject gameObject = null;
					gameObject = GetParent(collider.transform);
					IRGKRacer iRGKRacer = (IRGKRacer)gameObject.GetComponent(typeof(IRGKRacer));
					if (iRGKRacer != null && gameObject != base.gameObject)
					{
						ThreatRivals.Add(iRGKRacer);
						if (ShowNavDebugLines)
						{
							UnityEngine.Debug.DrawLine(Position, collider.transform.position, Color.magenta);
						}
					}
				}
			}
			OverlappedCount = m_DetectedObjects.Count;
			DedectedRacerCount = ThreatRivals.Count;
		}

		private void DecideObstacleAvoid(bool CanAvoidToLeft, bool CaAvoidToRight, eAIRivalPosition AvoidingRivalPosition)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (ObstacleAvoidToLeft)
			{
				flag2 = ((AvoidingRivalPosition != eAIRivalPosition.Left) ? true : false);
			}
			if (ObstacleAvoidToRight)
			{
				flag3 = ((AvoidingRivalPosition != eAIRivalPosition.Right) ? true : false);
			}
			if (flag3 && flag2)
			{
				if (AICurrentRoadPosition == eAIRoadPosition.Left)
				{
					SteerFactor = ObstacleAvoidFactor;
				}
				else if (AICurrentRoadPosition == eAIRoadPosition.Right)
				{
					SteerFactor -= ObstacleAvoidFactor;
				}
			}
			else if (flag2 && !flag3)
			{
				SteerFactor -= ObstacleAvoidFactor;
			}
			else if (flag3 && !flag2)
			{
				SteerFactor += ObstacleAvoidFactor;
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				AIHardBraking = true;
			}
		}

		protected float CalculateSteer()
		{
			SteerCoef = 0f;
			IRGKRacer iRGKRacer = null;
			float num = CollisionAvoidTime;
			Vector3 a = Vector3.zero;
			foreach (IRGKRacer threatRival in ThreatRivals)
			{
				if (threatRival != this)
				{
					float dedectionRadius = DedectionRadius;
					float num2 = PredictNearestApproachTime(threatRival);
					if (num2 >= 0f && num2 < num)
					{
						Vector3 ourPosition = Vector3.zero;
						Vector3 hisPosition = Vector3.zero;
						float num3 = ComputeNearestApproachPositions(threatRival, num2, ref ourPosition, ref hisPosition);
						if (num3 < dedectionRadius)
						{
							num = num2;
							iRGKRacer = threatRival;
							a = hisPosition;
						}
					}
				}
			}
			if (iRGKRacer != null)
			{
				float num4 = 0f;
				if (iRGKRacer.CachedTransform != null)
				{
					Vector3.Dot(base.transform.forward, iRGKRacer.CachedTransform.forward);
				}
				if (num4 < 0f - m_CollisionAvoidCos)
				{
					Vector3 lhs = a - Position;
					float num5 = Vector3.Dot(lhs, base.transform.right);
					SteerCoef = ((!(num5 > 0f)) ? CollisionAvoidFactor : (0f - CollisionAvoidFactor));
				}
				else if (num4 > m_CollisionAvoidCos)
				{
					Vector3 lhs2 = iRGKRacer.Position - Position;
					float num6 = Vector3.Dot(lhs2, base.transform.right);
					SteerCoef = ((!(num6 > 0f)) ? CollisionAvoidFactor : (0f - CollisionAvoidFactor));
					AIAvoidingRivalPosition = ((num6 > 0f) ? eAIRivalPosition.Right : eAIRivalPosition.Left);
					if (m_WatchingColliderCenterOffset < 0.2f)
					{
						SteerCoef = 0f;
					}
				}
			}
			else
			{
				AIAvoidingRivalPosition = eAIRivalPosition.NoRival;
			}
			return SteerCoef;
		}

		public float PredictNearestApproachTime(IRGKRacer other)
		{
			Vector3 velocity = Velocity;
			Vector3 velocity2 = other.Velocity;
			Vector3 a = velocity2 - velocity;
			float magnitude = a.magnitude;
			if (magnitude == 0f)
			{
				return 0f;
			}
			Vector3 lhs = a / magnitude;
			Vector3 rhs = Position - other.Position;
			float num = Vector3.Dot(lhs, rhs);
			return num / magnitude;
		}

		public float PredictCollisionTime(GameObject other)
		{
			Vector3 velocity = Velocity;
			Vector3 forward = other.transform.forward;
			Vector3 a = forward - velocity;
			float magnitude = a.magnitude;
			if (magnitude == 0f)
			{
				return 0f;
			}
			Vector3 lhs = a / magnitude;
			Vector3 rhs = Position - other.transform.position;
			float num = Vector3.Dot(lhs, rhs);
			return num / magnitude;
		}

		protected float ComputeNearestApproachPositions(IRGKRacer other, float time, ref Vector3 ourPosition, ref Vector3 hisPosition)
		{
			return ComputeNearestApproachPositions(other, time, ref ourPosition, ref hisPosition, Speed, m_MyRootTransform.forward);
		}

		protected float ComputeNearestApproachPositions(IRGKRacer other, float time, ref Vector3 ourPosition, ref Vector3 hisPosition, float ourSpeed, Vector3 ourForward)
		{
			if (other.CachedTransform == null)
			{
				return 0f;
			}
			Vector3 b = ourForward * ourSpeed * time;
			Vector3 b2 = other.CachedTransform.forward * other.Speed * time;
			ourPosition = Position + b;
			hisPosition = other.Position + b2;
			return Vector3.Distance(ourPosition, hisPosition);
		}

		private GameObject GetParent(Transform ParentSeekingObject)
		{
			if (ParentSeekingObject.transform.parent != null)
			{
				return GetParent(ParentSeekingObject.transform.parent.transform);
			}
			return ParentSeekingObject.gameObject;
		}

		private void OnDrawGizmos()
		{
			if (DrawCollisonGizmo)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(base.transform.position, DedectionRadius);
			}
		}

		private void WriteToConsole(string Message)
		{
			m_LastMessage = Message;
			if (Application.isEditor && EnableDebugMessages)
			{
				UnityEngine.Debug.Log(Message);
			}
		}

		private int FindLayerIndex(LayerMask LookupLayer)
		{
			int result = 0;
			for (int i = 0; i < 32; i++)
			{
				if ((int)LookupLayer == ((int)LookupLayer | (1 << i)))
				{
					result = i;
				}
			}
			return result;
		}
	}
}
