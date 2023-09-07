using RacingGameKit.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.AI
{
	[AddComponentMenu("")]
	public class AI_Basic : MonoBehaviour, IRGKRacer
	{
		public bool ShowNavDebugLines = true;

		public bool DrawCollisonGizmo = true;

		public LayerMask AIRacerDedectionLayer;

		public LayerMask ObstacleDedectionLayer;

		public float m_DetectionRadius = 4f;

		public float AISoftBrakeFactor = -0.25f;

		public float AIHardBrakeFactor = -0.45f;

		public float m_DetectionFrequency = 0.5f;

		public float CollisionAvoidAngle = 10f;

		public float CollisionAvoidFactor = 0.05f;

		public float CollisionAvoidTime = 1f;

		private Transform m_CachedReverseTransform;

		private Transform m_MyRootTransform;

		private List<Collider> m_DetectedObjects;

		private List<IRGKRacer> ThreatRivals;

		[HideInInspector]
		public bool AIWillAvoid;

		private float DedectionFrequencyOriginal;

		private float NextDedectCycle;

		private float m_CollisionAvoidCos = 0.707f;

		internal bool WillReCalculate = true;

		[HideInInspector]
		public eAIRoadPosition AICurrentRoadPosition;

		[HideInInspector]
		public float OverlappedCount;

		[HideInInspector]
		public float DedectedRacerCount;

		private float SteerCoef;

		protected float SteerFactor;

		private float VehicleSpeed;

		private float ReverseSensorDistance = 1f;

		private float ReverseSensorAngle = 45f;

		private float ReverseSensorWide;

		public bool m_AIReversing;

		public bool ReverseLockedbyObstacle;

		public bool Sleep;

		private List<Collider> m_MyColliders;

		protected bool AISoftBraking;

		private float m_WatchingColliderCenterOffset;

		private Collider m_WatchingCollider;

		private float m_fDot;

		public Vector3 Velocity
		{
			get
			{
				if (m_MyRootTransform == null)
				{
					m_MyRootTransform = base.transform.GetComponent<Transform>();
					if (m_MyRootTransform == null)
					{
						m_MyRootTransform = base.transform.parent.GetComponent<Transform>();
					}
				}
				return m_MyRootTransform.forward * VehicleSpeed;
			}
		}

		public float Speed
		{
			get
			{
				return VehicleSpeed;
			}
			set
			{
				VehicleSpeed = value;
			}
		}

		public Transform CachedTransform => base.transform;

		public GameObject CachedGameObject => base.gameObject;

		public eAIRoadPosition CurrentRoadPosition => AICurrentRoadPosition;

		public Vector3 Position => m_MyRootTransform.position;

		public float EscapeDistance => 0f;

		protected void Initialize_AI()
		{
			m_CachedReverseTransform = base.transform.Find("_ReversePoint");
			if (m_CachedReverseTransform == null)
			{
				UnityEngine.Debug.Log("Reverse Point Missing!");
			}
			if (m_MyRootTransform == null)
			{
				m_MyRootTransform = base.transform.GetComponent<Transform>();
				if (m_MyRootTransform == null)
				{
					m_MyRootTransform = base.transform.parent.GetComponent<Transform>();
				}
			}
			AICurrentRoadPosition = eAIRoadPosition.UnKnown;
			DedectionFrequencyOriginal = m_DetectionFrequency;
			m_MyColliders = new List<Collider>();
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				m_MyColliders.Add(componentsInChildren[i]);
			}
		}

		public void ExecuteRadar()
		{
			m_CollisionAvoidCos = Mathf.Cos(CollisionAvoidAngle * ((float)Math.PI / 180f));
			NextDedectCycle -= Time.deltaTime;
			if (NextDedectCycle < 0.1f)
			{
				NextDedectCycle = m_DetectionFrequency;
				DetectAIs();
				SteerFactor = CalculateSteer();
				ReCalculateDedection();
			}
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
					UnityEngine.Debug.DrawRay(vector, vector4, Color.magenta);
					UnityEngine.Debug.DrawRay(vector2, vector5, Color.magenta);
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

		private void ReCalculateDedection()
		{
			if (m_DetectionFrequency > 0f)
			{
				if (ThreatRivals.Count == 1)
				{
					m_DetectionFrequency = DedectionFrequencyOriginal / 2f;
				}
				else if (ThreatRivals.Count > 2)
				{
					m_DetectionFrequency = DedectionFrequencyOriginal / 3f;
				}
				else
				{
					m_DetectionFrequency = DedectionFrequencyOriginal;
				}
			}
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
			Collider[] array = Physics.OverlapSphere(Position, m_DetectionRadius, AIRacerDedectionLayer);
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

		private GameObject FindParent(Transform ObjectForParentSearch)
		{
			if (ObjectForParentSearch.parent == null)
			{
				return ObjectForParentSearch.gameObject;
			}
			return FindParent(ObjectForParentSearch.parent);
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
					float detectionRadius = m_DetectionRadius;
					float num2 = PredictNearestApproachTime(threatRival);
					if (num2 >= 0f && num2 < num)
					{
						Vector3 ourPosition = Vector3.zero;
						Vector3 hisPosition = Vector3.zero;
						float num3 = ComputeNearestApproachPositions(threatRival, num2, ref ourPosition, ref hisPosition);
						if (num3 < detectionRadius)
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
					m_fDot = Vector3.Dot(lhs, base.transform.forward);
					if ((double)m_fDot < 0.2)
					{
						SteerCoef = 0f;
					}
				}
				else if (num4 > m_CollisionAvoidCos)
				{
					Vector3 lhs2 = iRGKRacer.Position - Position;
					float num6 = Vector3.Dot(lhs2, base.transform.right);
					SteerCoef = ((!(num6 > 0f)) ? CollisionAvoidFactor : (0f - CollisionAvoidFactor));
					m_fDot = Vector3.Dot(lhs2, base.transform.forward);
					if ((double)m_fDot < 0.2)
					{
						SteerCoef = 0f;
					}
				}
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
				if (m_MyRootTransform == null)
				{
					m_MyRootTransform = base.transform;
				}
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(Position, m_DetectionRadius);
			}
		}

		private void WriteToConsole(string Message)
		{
		}
	}
}
