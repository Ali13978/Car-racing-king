using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RainDropTools : MonoBehaviour
{
	public enum RainDropShaderType
	{
		Expensive,
		Cheap,
		NoDistortion
	}

	public static string SHADER_FORWARD = "RainDrop/Internal/RainDistortion (Forward)";

	public static string SHADER_CHEAP = "RainDrop/Internal/RainDistortion (Mobile)";

	public static string SHADER_NO_DISTORTION = "RainDrop/Internal/RainNoDistortion";

	public static string GetShaderName(RainDropShaderType shaderType)
	{
		switch (shaderType)
		{
		case RainDropShaderType.Expensive:
			return SHADER_FORWARD;
		case RainDropShaderType.Cheap:
			return SHADER_CHEAP;
		case RainDropShaderType.NoDistortion:
			return SHADER_NO_DISTORTION;
		default:
			return string.Empty;
		}
	}

	public static Material CreateRainMaterial(RainDropShaderType shaderType, int renderQueue)
	{
		Shader shader = Shader.Find(GetShaderName(shaderType));
		Material material = new Material(shader);
		material.renderQueue = renderQueue;
		return material;
	}

	public static void ApplyRainMaterialValue(Material material, RainDropShaderType shaderType, Texture normalMap, Texture overlayTexture = null, float distortionValue = 0f, Color? overlayColor = default(Color?), float reliefValue = 0f, float blur = 0f, float darkness = 0f)
	{
		switch (shaderType)
		{
		case RainDropShaderType.Expensive:
			material.SetColor("_Color", (!overlayColor.HasValue) ? Color.white : overlayColor.Value);
			material.SetFloat("_Strength", distortionValue);
			material.SetFloat("_Relief", reliefValue);
			if (blur != 0f)
			{
				material.EnableKeyword("BLUR");
				material.SetFloat("_Blur", blur);
			}
			else
			{
				material.DisableKeyword("BLUR");
				material.SetFloat("_Blur", blur);
			}
			material.SetFloat("_Darkness", darkness);
			material.SetTexture("_Distortion", normalMap);
			material.SetTexture("_ReliefTex", overlayTexture);
			break;
		case RainDropShaderType.Cheap:
			material.SetFloat("_Strength", distortionValue);
			material.SetTexture("_Distortion", normalMap);
			break;
		case RainDropShaderType.NoDistortion:
			material.SetTexture("_MainTex", overlayTexture);
			material.SetTexture("_Distortion", normalMap);
			material.SetColor("_Color", (!overlayColor.HasValue) ? Color.white : overlayColor.Value);
			material.SetFloat("_Darkness", darkness);
			material.SetFloat("_Relief", reliefValue);
			break;
		}
	}

	public static Mesh CreateQuadMesh()
	{
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(-1f, -1f, 0f)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(0f, 0f)
		};
		int[] triangles = new int[6]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		Mesh mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		mesh.name = "Rain Mesh";
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.MarkDynamic();
		mesh.RecalculateBounds();
		return mesh;
	}

	public static Transform CreateHiddenObject(string name, Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = name;
		gameObject.transform.parent = parent;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		return gameObject.transform;
	}

	public static float Random(float min, float max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	public static int Random(int min, int max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	public static void DestroyChildren(Transform t)
	{
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static void DestroyChildrenImmediate(Transform t)
	{
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.DestroyImmediate(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static Vector2 GetCameraOrthographicSize(Camera cam)
	{
		float num = cam.orthographicSize * 2f;
		float x = num * cam.aspect;
		return new Vector2(x, num);
	}

	public static Vector3 GetSpawnLocalPos(Transform parent, Camera cam, float offsetX, float offsetY)
	{
		Vector2 cameraOrthographicSize = GetCameraOrthographicSize(cam);
		Vector3 a = new Vector3(cameraOrthographicSize.x * offsetX + Random((0f - cameraOrthographicSize.x) / 2f, cameraOrthographicSize.x / 2f), cameraOrthographicSize.y * offsetY + Random((0f - cameraOrthographicSize.y) / 2f, cameraOrthographicSize.y / 2f), 0f);
		return parent.InverseTransformPoint(a + parent.position);
	}

	public static KeyValuePair<T1, T2> GetWeightedElement<T1, T2>(List<KeyValuePair<T1, T2>> list) where T2 : IComparable
	{
		if (list.Count == 0)
		{
			return list.FirstOrDefault();
		}
		float max = (float)list.Sum((KeyValuePair<T1, T2> t) => Convert.ToDouble(t.Value));
		float num = Random(0f, max);
		float num2 = 0f;
		foreach (KeyValuePair<T1, T2> item in list)
		{
			for (float num3 = num2; (double)num3 < Convert.ToDouble(item.Value) + (double)num2; num3 += 1f)
			{
				if (num3 >= num)
				{
					return item;
				}
			}
			num2 += (float)Convert.ToDouble(item.Value);
		}
		return list.First();
	}
}
