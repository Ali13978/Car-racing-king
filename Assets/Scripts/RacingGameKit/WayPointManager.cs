using RacingGameKit.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Waypoint/WayPoint Manager")]
	[RequireComponent(typeof(SplineInterpolator))]
	[ExecuteInEditMode]
	public class WayPointManager : MonoBehaviour
	{
		[HideInInspector]
		public bool ShowHelperIcons = true;

		[HideInInspector]
		public bool ShowHelperSpline = true;

		[HideInInspector]
		public bool CloseSpline;

		[HideInInspector]
		public Color HelperSplineColor = Color.red;

		private int SplineSoftnessFactor = 10;

		private float Duration = 10f;

		private void Start()
		{
		}

		public List<WayPointItem> GetWaypointItems()
		{
			List<Component> list = new List<Component>(base.gameObject.GetComponentsInChildren(typeof(WayPointItem)));
			List<WayPointItem> list2 = new List<WayPointItem>();
			foreach (Component item in list)
			{
				list2.Add((WayPointItem)item);
			}
			list2.Sort((WayPointItem a, WayPointItem b) => Convert.ToInt32(a.transform.name).CompareTo(Convert.ToInt32(b.transform.name)));
			return list2;
		}

		public Transform[] GetTransforms()
		{
			List<Component> list = new List<Component>(base.gameObject.GetComponentsInChildren(typeof(Transform)));
			List<Transform> list2 = new List<Transform>();
			foreach (Component item in list)
			{
				list2.Add((Transform)item);
			}
			list2.Remove(base.gameObject.transform);
			list2.Sort((Transform a, Transform b) => Convert.ToInt32(a.name).CompareTo(Convert.ToInt32(b.name)));
			SplineSoftnessFactor = list2.Count * 3;
			return list2.ToArray();
		}

		private void OnDrawGizmos()
		{
			if (!ShowHelperSpline)
			{
				return;
			}
			Transform[] transforms = GetTransforms();
			if (transforms.Length < 2)
			{
				return;
			}
			if (SplineSoftnessFactor < transforms.Length)
			{
				SplineSoftnessFactor = transforms.Length;
			}
			SplineInterpolator component = GetComponent<SplineInterpolator>();
			SetupSplineInterpolator(component, transforms);
			component.StartInterpolation(null, bRotations: false, eWrapMode.ONCE);
			Vector3 from = transforms[0].position;
			for (int i = 1; i <= SplineSoftnessFactor; i++)
			{
				float timeParam = (float)i * Duration / (float)SplineSoftnessFactor;
				Vector3 vector = component.GetHermiteAtTime(timeParam);
				if (i == 0)
				{
					vector = transforms[0].position;
				}
				Gizmos.color = HelperSplineColor;
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
		}

		public void ShowHideChildIcons(bool ShowIcons)
		{
			Transform[] transforms = GetTransforms();
			Transform[] array = transforms;
			foreach (Transform transform in array)
			{
				WayPointItem component = transform.GetComponent<WayPointItem>();
				if (component != null)
				{
					component.ShowIconGizmo = ShowIcons;
				}
			}
		}

		public void ShowHideChildLines(bool ShowLines)
		{
			Transform[] transforms = GetTransforms();
			Transform[] array = transforms;
			foreach (Transform transform in array)
			{
				WayPointItem component = transform.GetComponent<WayPointItem>();
				if (component != null)
				{
					component.ShowLines = ShowLines;
				}
			}
		}

		private void FixedUpdate()
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			if (base.transform.position != vector)
			{
				base.transform.position = vector;
			}
		}

		private void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
		{
			interp.Reset();
			float num = (!CloseSpline) ? (Duration / (float)(trans.Length - 1)) : (Duration / (float)trans.Length);
			int i;
			for (i = 0; i < trans.Length; i++)
			{
				interp.AddPoint(trans[i].position, trans[i].rotation, num * (float)i, new Vector2(0f, 1f));
			}
			if (CloseSpline)
			{
				interp.SetAutoCloseMode(num * (float)i);
			}
		}
	}
}
