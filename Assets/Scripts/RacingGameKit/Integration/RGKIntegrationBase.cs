using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.Integration
{
	[AddComponentMenu("")]
	public class RGKIntegrationBase : MonoBehaviour
	{
		private bool m_IsAiController;

		private Race_Manager m_RaceManager;

		private Racer_Register m_RacerRegister;

		private Race_Camera m_RaceCamera;

		private GameObject m_WPContainer;

		private List<Transform> m_WPItems;

		private float m_layerChangeTimerValue = 5f;

		private string m_currentLayerCache;

		private float m_layerChangeTimer;

		private bool m_layerChangeStarted;

		public bool IsAiController
		{
			get
			{
				return m_IsAiController;
			}
			set
			{
				m_IsAiController = value;
			}
		}

		public Race_Manager RaceManager => m_RaceManager;

		public Racer_Register RacerRegister => m_RacerRegister;

		public Race_Camera RaceCamera => m_RaceCamera;

		public virtual void Start()
		{
			GameObject gameObject = GameObject.Find("_RaceManager");
			if (gameObject != null)
			{
				m_RaceManager = gameObject.GetComponent<Race_Manager>();
				m_RacerRegister = base.transform.GetComponent<Racer_Register>();
				m_WPContainer = m_RaceManager.Waypoints;
				GetWaypoints();
				if (!m_IsAiController)
				{
					m_RaceCamera = m_RaceManager.oCamera1.GetComponent<Race_Camera>();
				}
			}
			else if (!m_IsAiController)
			{
				m_RaceCamera = UnityEngine.Object.FindObjectOfType<Race_Camera>();
			}
		}

		public virtual void Update()
		{
			if (m_layerChangeStarted)
			{
				layerChangeForIgnoreProcessor();
			}
		}

		protected void CheckIfRecoverable()
		{
			if (m_RacerRegister.IsRacerStarted && !m_RacerRegister.IsRacerFinished && !m_RacerRegister.IsRacerDestroyed)
			{
				RecoverCar();
				UnityEngine.Debug.Log("Resetting Player Car");
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
