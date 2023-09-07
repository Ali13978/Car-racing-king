using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.Integration
{
	[AddComponentMenu("")]
	public class RGKIntegrationHelper : MonoBehaviour
	{
		private static Race_Manager m_RaceManager;

		private static Race_Camera m_RaceCamera;

		private static GameObject m_WPContainer;

		private static List<Transform> m_Waypoints;

		private static Transform m_LastRecovered;

		private static string m_currentLayerCache;

		private static float m_layerChangeTimerValue = 5f;

		private static float m_layerChangeTimer;

		private static bool m_layerChangeStarted;

		public static Race_Manager RaceManager => m_RaceManager;

		public static Race_Camera RaceCamera => m_RaceCamera;

		public static void Init()
		{
			GameObject gameObject = GameObject.Find("_RaceManager");
			if (gameObject != null)
			{
				m_RaceManager = gameObject.GetComponent<Race_Manager>();
				m_WPContainer = m_RaceManager.Waypoints;
				GetWaypoints();
				m_RaceCamera = m_RaceManager.oCamera1.GetComponent<Race_Camera>();
			}
		}

		private void Update()
		{
			if (m_layerChangeStarted)
			{
				layerChangeForIgnoreProcessor();
			}
		}

		private static void GetWaypoints()
		{
			Transform[] componentsInChildren = m_WPContainer.GetComponentsInChildren<Transform>();
			m_Waypoints = new List<Transform>();
			Transform[] array = componentsInChildren;
			foreach (Transform transform in array)
			{
				if (transform != m_WPContainer.transform)
				{
					m_Waypoints.Add(transform);
				}
			}
		}

		public static void RecoverCar(GameObject Target)
		{
			m_LastRecovered = Target.transform;
			Resetcar(Target);
			UnityEngine.Debug.Log("Resetting Player Car");
		}

		private static void Resetcar(GameObject Target)
		{
			Transform[] array = m_Waypoints.ToArray();
			Transform ClosestWP = GetClosestWP(array, Target.transform.position);
			Vector3 vector = Target.transform.InverseTransformPoint(ClosestWP.transform.position);
			int num = 0;
			num = ((!(vector.z > 0f)) ? (Array.FindIndex(array, (Transform tr) => tr.name == ClosestWP.name) + 1) : (Array.FindIndex(array, (Transform tr) => tr.name == ClosestWP.name) - 1));
			Vector3 vector2 = (num <= 0) ? array[0].position : NearestPoint(ClosestWP.position, array[num].position, Target.transform.position);
			UnityEngine.Debug.DrawLine(Target.transform.position, vector2);
			Target.transform.rotation = Quaternion.LookRotation(ClosestWP.forward);
			Target.transform.position = vector2;
			Target.transform.position += Vector3.up * 0.1f;
			Target.transform.position += Vector3.right * 0.1f;
			Target.GetComponent<Rigidbody>().velocity = Vector3.zero;
			Target.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			m_layerChangeTimer = m_layerChangeTimerValue;
			m_currentLayerCache = LayerMask.LayerToName(Target.gameObject.layer);
			ChangeLayersRecursively(Target.transform, "IGNORE");
			m_layerChangeStarted = true;
		}

		private static void layerChangeForIgnoreProcessor()
		{
			m_layerChangeTimer -= Time.deltaTime;
			if (m_layerChangeTimer <= 0f)
			{
				ChangeLayersRecursively(m_LastRecovered.transform, m_currentLayerCache);
				m_layerChangeTimer = m_layerChangeTimerValue;
				m_layerChangeStarted = false;
			}
		}

		private static void ChangeLayersRecursively(Transform trans, string LayerName)
		{
			for (int i = 0; i < trans.childCount; i++)
			{
				trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(LayerName);
				ChangeLayersRecursively(trans.GetChild(i), LayerName);
			}
		}

		private static Transform GetClosestWP(Transform[] DPs, Vector3 myPosition)
		{
			Transform result = null;
			float num = float.PositiveInfinity;
			foreach (Transform transform in DPs)
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

		private static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
			float d = Vector3.Dot(point - lineStart, vector) / Vector3.Dot(vector, vector);
			return lineStart + d * vector;
		}

		private static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = lineEnd - lineStart;
			Vector3 vector2 = Vector3.Normalize(vector);
			float value = Vector3.Dot(point - lineStart, vector2) / Vector3.Dot(vector2, vector2);
			return lineStart + Mathf.Clamp(value, 0f, Vector3.Magnitude(vector)) * vector2;
		}
	}
}
