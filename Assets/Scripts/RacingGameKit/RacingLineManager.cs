using RacingGameKit.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Racing Line/Racing Line Manager")]
	[ExecuteInEditMode]
	public class RacingLineManager : MonoBehaviour
	{
		[Serializable]
		private class SurfaceProperties
		{
			public Vector3 Position = Vector3.zero;

			public Vector3 Normal = Vector3.zero;
		}

		[Serializable]
		public class MarkSection
		{
			public Vector3 pos = Vector3.zero;

			public Vector3 normal = Vector3.zero;

			public Vector4 tangent = Vector4.zero;

			public Vector3 posl = Vector3.zero;

			public Vector3 posr = Vector3.zero;

			public float intensity;

			public int lastIndex;
		}

		public bool ShowHelperIcons = true;

		public bool ShowHelperSpline = true;

		public Color HelperSplineColor = Color.green;

		public bool ClosedRacingLine;

		public float RacingLineWidth = 1.6f;

		public bool StickToGround;

		public float GroundOffset = 0.02f;

		public int MeshResolution = 128;

		private float textureOffset;

		public int TextureTile = 10;

		public GameObject WaypointContainer;

		private int LineSoftnessFactor = 10;

		private int lastindex = -1;

		private float ColliRadius = 0.2f;

		private int numMarks;

		public MarkSection[] RacingLineParts;

		private float Duration = 1f;

		private bool updateMesh;

		[HideInInspector]
		public bool enableCopyMode;

		public Mesh GeneratedMesh;

		public bool m_UnlockEdit;

		public bool m_IsRendered;

		public void ShowHideChildIcons(bool ShowIcons)
		{
			Transform[] transforms = GetTransforms();
			Transform[] array = transforms;
			foreach (Transform transform in array)
			{
				RacingLineItem component = transform.GetComponent<RacingLineItem>();
				if (component != null)
				{
					component.ShowIconGizmo = ShowIcons;
				}
			}
		}

		public void CreateRacingLine(string LevelName)
		{
			CheckIfMeshAlreadyCreated();
			Transform[] transforms = GetTransforms();
			if (transforms.Length < 2)
			{
				return;
			}
			if (MeshResolution < 4)
			{
				MeshResolution = 4;
				LineSoftnessFactor = MeshResolution - 2;
				return;
			}
			LineSoftnessFactor = MeshResolution - 2;
			SplineInterpolator component = GetComponent<SplineInterpolator>();
			SetupSplineInterpolator(component, transforms);
			component.StartInterpolation(null, bRotations: false, eWrapMode.ONCE);
			for (int i = 0; i <= LineSoftnessFactor; i++)
			{
				float timeParam = (float)i * Duration / (float)LineSoftnessFactor;
				Vector3 hermiteAtTime = component.GetHermiteAtTime(timeParam);
				SurfaceProperties surfaceProperties = CastToCollider(GroundOffset, hermiteAtTime, new Vector3(0f, -1f, 0f), 0f, 1f);
				lastindex = AddLineNode(surfaceProperties.Position, surfaceProperties.Normal, 1f, lastindex);
			}
			UpdateMeshFilter(LevelName);
			lastindex = -1;
			numMarks = 0;
		}

		public int AddLineNode(Vector3 pos, Vector3 normal, float intensity, int lIndex)
		{
			if (intensity > 1f)
			{
				intensity = 1f;
			}
			if (intensity < 0f)
			{
				return -1;
			}
			MarkSection markSection = RacingLineParts[numMarks % MeshResolution];
			markSection.pos = pos + normal * GroundOffset;
			markSection.normal = normal;
			markSection.intensity = intensity;
			markSection.lastIndex = lIndex;
			if (lIndex != -1)
			{
				MarkSection markSection2 = RacingLineParts[lIndex % MeshResolution];
				Vector3 lhs = markSection.pos - markSection2.pos;
				Vector3 normalized = Vector3.Cross(lhs, normal).normalized;
				markSection.posl = markSection.pos + normalized * RacingLineWidth * 0.5f;
				markSection.posr = markSection.pos - normalized * RacingLineWidth * 0.5f;
				markSection.tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
				if (markSection2.lastIndex == -1)
				{
					MarkSection markSection3 = RacingLineParts[numMarks % MeshResolution - 1];
					markSection2.tangent = markSection3.tangent;
					markSection2.posl = markSection3.pos + normalized * RacingLineWidth * 0.5f;
					markSection2.posr = markSection3.pos - normalized * RacingLineWidth * 0.5f;
				}
			}
			numMarks++;
			updateMesh = true;
			return numMarks - 1;
		}

		private void UpdateMeshFilter(string LevelName)
		{
			if (!updateMesh)
			{
				return;
			}
			updateMesh = false;
			Mesh sharedMesh = GetComponent<MeshFilter>().sharedMesh;
			sharedMesh.Clear();
			int num = 0;
			for (int i = 0; i < numMarks && i < MeshResolution; i++)
			{
				if (RacingLineParts[i].lastIndex != -1 && RacingLineParts[i].lastIndex > numMarks - MeshResolution)
				{
					num++;
				}
			}
			Vector3[] array = new Vector3[num * 4];
			Vector3[] array2 = new Vector3[num * 4];
			Vector4[] array3 = new Vector4[num * 4];
			Color[] array4 = new Color[num * 4];
			Vector2[] array5 = new Vector2[num * 4];
			int[] array6 = new int[num * 6];
			num = 0;
			for (int j = 0; j < numMarks && j < MeshResolution; j++)
			{
				if (RacingLineParts[j].lastIndex != -1 && RacingLineParts[j].lastIndex > numMarks - MeshResolution)
				{
					MarkSection markSection = RacingLineParts[j];
					MarkSection markSection2 = RacingLineParts[markSection.lastIndex % MeshResolution];
					float magnitude = (markSection2.posr - markSection.posr).magnitude;
					float y = magnitude / (float)TextureTile + textureOffset;
					array[num * 4] = markSection2.posl;
					array[num * 4 + 1] = markSection2.posr;
					array[num * 4 + 2] = markSection.posl;
					array[num * 4 + 3] = markSection.posr;
					array2[num * 4] = markSection2.normal;
					array2[num * 4 + 1] = markSection2.normal;
					array2[num * 4 + 2] = markSection.normal;
					array2[num * 4 + 3] = markSection.normal;
					array3[num * 4] = markSection2.tangent;
					array3[num * 4 + 1] = markSection2.tangent;
					array3[num * 4 + 2] = markSection.tangent;
					array3[num * 4 + 3] = markSection.tangent;
					array4[num * 4] = new Color(0f, 0f, 0f, markSection2.intensity);
					array4[num * 4 + 1] = new Color(0f, 0f, 0f, markSection2.intensity);
					array4[num * 4 + 2] = new Color(0f, 0f, 0f, markSection.intensity);
					array4[num * 4 + 3] = new Color(0f, 0f, 0f, markSection.intensity);
					array5[num * 4] = new Vector2(0f, textureOffset);
					array5[num * 4 + 1] = new Vector2(1f, textureOffset);
					array5[num * 4 + 2] = new Vector2(0f, y);
					array5[num * 4 + 3] = new Vector2(1f, y);
					textureOffset = magnitude / (float)TextureTile % 1f + textureOffset;
					array6[num * 6] = num * 4;
					array6[num * 6 + 2] = num * 4 + 1;
					array6[num * 6 + 1] = num * 4 + 2;
					array6[num * 6 + 3] = num * 4 + 2;
					array6[num * 6 + 5] = num * 4 + 1;
					array6[num * 6 + 4] = num * 4 + 3;
					num++;
				}
			}
			sharedMesh.vertices = array;
			sharedMesh.normals = array2;
			sharedMesh.tangents = array3;
			sharedMesh.triangles = array6;
			sharedMesh.colors = array4;
			sharedMesh.uv = array5;
			textureOffset = 0f;
			sharedMesh.name = "RacingLine_" + LevelName;
			GeneratedMesh = sharedMesh;
		}

		private Transform[] GetTransforms()
		{
			List<Transform> list2;
			if (WaypointContainer != null)
			{
				List<Component> list = new List<Component>(WaypointContainer.GetComponentsInChildren(typeof(Transform)));
				list2 = new List<Transform>();
				foreach (Component item in list)
				{
					list2.Add((Transform)item);
				}
				list2.Remove(WaypointContainer.gameObject.transform);
			}
			else
			{
				List<Component> list = new List<Component>(base.transform.GetComponentsInChildren(typeof(Transform)));
				list2 = new List<Transform>();
				foreach (Component item2 in list)
				{
					list2.Add((Transform)item2);
				}
				list2.Remove(base.transform.gameObject.transform);
			}
			list2.Sort((Transform a, Transform b) => Convert.ToInt32(a.name).CompareTo(Convert.ToInt32(b.name)));
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
			if (LineSoftnessFactor < transforms.Length)
			{
				LineSoftnessFactor = transforms.Length;
			}
			SplineInterpolator component = GetComponent<SplineInterpolator>();
			SetupSplineInterpolator(component, transforms);
			component.StartInterpolation(null, bRotations: false, eWrapMode.ONCE);
			Vector3 from = transforms[0].position;
			for (int i = 1; i <= LineSoftnessFactor; i++)
			{
				float timeParam = (float)i * Duration / (float)LineSoftnessFactor;
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

		private void CheckIfMeshAlreadyCreated()
		{
			MeshFilter component = GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				Mesh mesh = new Mesh();
				mesh.name = "RacingLineMesh";
				mesh.hideFlags = HideFlags.None;
				component.sharedMesh = mesh;
			}
			lastindex = -1;
			RacingLineParts = new MarkSection[MeshResolution];
			for (int i = 0; i < MeshResolution; i++)
			{
				RacingLineParts[i] = new MarkSection();
			}
		}

		public void CleanMesh()
		{
			MeshFilter component = GetComponent<MeshFilter>();
			if (component.sharedMesh != null)
			{
				component.sharedMesh = null;
				component.mesh = null;
			}
		}

		public void CopyWaypointItemsAsNode()
		{
			if (!(WaypointContainer == null))
			{
				Transform[] transforms = GetTransforms();
				Transform[] array = transforms;
				foreach (Transform transform in array)
				{
					GameObject gameObject = new GameObject(transform.name);
					gameObject.transform.position = transform.position;
					gameObject.transform.rotation = transform.rotation;
					gameObject.transform.parent = base.transform;
					gameObject.AddComponent<RacingLineItem>();
				}
				enableCopyMode = false;
				WaypointContainer = null;
			}
		}

		public void UpdateRacingLine(string LevelName)
		{
			CreateRacingLine(LevelName);
		}

		private SurfaceProperties CastToCollider(float GroundOffset, Vector3 fromPos, Vector3 forward, float minDistance, float maxDistance)
		{
			SurfaceProperties surfaceProperties;
			if (!StickToGround)
			{
				surfaceProperties = new SurfaceProperties();
				surfaceProperties.Position = fromPos;
				surfaceProperties.Normal = new Vector3(0f, 1f, 0f);
				return surfaceProperties;
			}
			Vector3 position = fromPos;
			Vector3 normal = new Vector3(0f, 1f, 0f);
			Ray ray = new Ray(fromPos, forward);
			bool flag = false;
			if ((!(maxDistance > 0f)) ? Physics.SphereCast(ray, ColliRadius, out RaycastHit hitInfo) : Physics.SphereCast(ray, ColliRadius, out hitInfo, maxDistance))
			{
				position = hitInfo.point;
				normal = hitInfo.normal;
			}
			else if (minDistance > 0f)
			{
				position = fromPos + forward.normalized * minDistance;
				position += Vector3.up.normalized * GroundOffset;
			}
			surfaceProperties = new SurfaceProperties();
			surfaceProperties.Position = position;
			surfaceProperties.Normal = normal;
			return surfaceProperties;
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
			float num = (!ClosedRacingLine) ? (Duration / (float)(trans.Length - 1)) : (Duration / (float)trans.Length);
			int i;
			for (i = 0; i < trans.Length; i++)
			{
				interp.AddPoint(trans[i].position, trans[i].rotation, num * (float)i, new Vector2(0f, 1f));
			}
			if (ClosedRacingLine)
			{
				interp.SetAutoCloseMode(num * (float)i);
			}
		}
	}
}
