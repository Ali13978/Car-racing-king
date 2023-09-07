using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace RainDropEffect
{
	[ExecuteInEditMode]
	public class RainDrawer : MonoBehaviour
	{
		[NonSerialized]
		[HideInInspector]
		public int RenderQueue = 3000;

		[NonSerialized]
		[HideInInspector]
		public Vector3 CameraPos;

		[NonSerialized]
		[HideInInspector]
		public Color OverlayColor;

		[NonSerialized]
		[HideInInspector]
		public Texture NormalMap;

		[NonSerialized]
		[HideInInspector]
		public Texture ReliefTexture;

		[NonSerialized]
		[HideInInspector]
		public float DistortionStrength;

		[NonSerialized]
		[HideInInspector]
		public float ReliefValue;

		[NonSerialized]
		[HideInInspector]
		public float Shiness;

		[NonSerialized]
		[HideInInspector]
		public float Blur;

		[NonSerialized]
		[HideInInspector]
		public float Darkness;

		[NonSerialized]
		[HideInInspector]
		public RainDropTools.RainDropShaderType ShaderType;

		private Material material;

		private MeshFilter meshFilter;

		private Mesh mesh;

		private MeshRenderer meshRenderer;

		private bool changed;

		public bool IsEnabled => meshRenderer != null && meshRenderer.enabled;

		public void Refresh()
		{
			changed = true;
		}

		public void Hide()
		{
			if (meshRenderer != null)
			{
				meshRenderer.enabled = false;
			}
		}

		public void Show()
		{
			if (changed)
			{
				UnityEngine.Object.DestroyImmediate(meshRenderer);
				UnityEngine.Object.DestroyImmediate(meshFilter);
				meshRenderer = null;
				meshFilter = null;
				material = null;
				mesh = null;
				changed = false;
			}
			if (NormalMap != null)
			{
				if (ShaderType == RainDropTools.RainDropShaderType.Cheap)
				{
					if (DistortionStrength == 0f)
					{
						Hide();
						return;
					}
				}
				else if (DistortionStrength == 0f && ReliefValue == 0f && OverlayColor.a == 0f && Blur == 0f)
				{
					Hide();
					return;
				}
				if (material == null)
				{
					material = RainDropTools.CreateRainMaterial(ShaderType, RenderQueue);
				}
				if (meshFilter == null)
				{
					meshFilter = base.gameObject.AddComponent<MeshFilter>();
				}
				if (meshRenderer == null)
				{
					meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
				}
				if (mesh == null)
				{
					mesh = RainDropTools.CreateQuadMesh();
				}
				if (material.shader.name != RainDropTools.GetShaderName(ShaderType))
				{
					material = RainDropTools.CreateRainMaterial(ShaderType, material.renderQueue);
				}
				if (material != null && mesh != null && meshFilter != null)
				{
					meshFilter.mesh = mesh;
					meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
					meshRenderer.material = material;
					meshRenderer.lightProbeUsage = LightProbeUsage.Off;
					meshRenderer.enabled = true;
					RainDropTools.ApplyRainMaterialValue(material, ShaderType, NormalMap, ReliefTexture, DistortionStrength, OverlayColor, ReliefValue, Blur, Darkness);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Normal Map is null!");
				Hide();
			}
		}
	}
}
