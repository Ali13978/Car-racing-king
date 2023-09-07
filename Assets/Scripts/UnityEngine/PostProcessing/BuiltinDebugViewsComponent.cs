using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		private static class Uniforms
		{
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		private enum Pass
		{
			Depth,
			Normals,
			MovecOpacity,
			MovecImaging,
			MovecArrows
		}

		private class ArrowArray
		{
			public Mesh mesh
			{
				get;
				private set;
			}

			public int columnCount
			{
				get;
				private set;
			}

			public int rowCount
			{
				get;
				private set;
			}

			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[6]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				mesh.SetVertices(list);
				mesh.SetUVs(0, list2);
				mesh.SetIndices(array2, MeshTopology.Lines, 0);
				mesh.UploadMeshData(markNoLongerReadable: true);
				columnCount = columns;
				rowCount = rows;
			}

			public void Release()
			{
				GraphicsUtils.Destroy(mesh);
				mesh = null;
			}
		}

		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		private ArrowArray m_Arrows;

		public override bool active => base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);

		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			switch (mode)
			{
			case BuiltinDebugViewsModel.Mode.Normals:
				depthTextureMode |= DepthTextureMode.DepthNormals;
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				depthTextureMode |= (DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
				break;
			case BuiltinDebugViewsModel.Mode.Depth:
				depthTextureMode |= DepthTextureMode.Depth;
				break;
			}
			return depthTextureMode;
		}

		public override CameraEvent GetCameraEvent()
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			return (settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeImageEffects;
		}

		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			switch (settings.mode)
			{
			case BuiltinDebugViewsModel.Mode.Depth:
				DepthPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.Normals:
				DepthNormalsPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				MotionVectorsPass(cb);
				break;
			}
			context.Interrupt();
		}

		private void DepthPass(CommandBuffer cb)
		{
			Material mat = context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			BuiltinDebugViewsModel.DepthSettings depth = settings.depth;
			cb.SetGlobalFloat(Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 0);
		}

		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material mat = context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 1);
		}

		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = settings.motionVectors;
			int nameID = Uniforms._TempRT;
			cb.GetTemporaryRT(nameID, context.width, context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, nameID, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, context.width, context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(Uniforms._MainTex, nameID);
				cb.Blit(nameID, tempRT, material, 3);
				cb.ReleaseTemporaryRT(nameID);
				nameID = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				PrepareArrows();
				float num = 1f / (float)motionVectors.motionVectorsResolution;
				float x = num * (float)context.height / (float)context.width;
				cb.SetGlobalVector(Uniforms._Scale, new Vector2(x, num));
				cb.SetGlobalFloat(Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(Uniforms._MainTex, nameID);
			cb.Blit(nameID, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(nameID);
		}

		private void PrepareArrows()
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			int motionVectorsResolution = settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (m_Arrows == null)
			{
				m_Arrows = new ArrowArray();
			}
			if (m_Arrows.columnCount != num || m_Arrows.rowCount != motionVectorsResolution)
			{
				m_Arrows.Release();
				m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		public override void OnDisable()
		{
			if (m_Arrows != null)
			{
				m_Arrows.Release();
			}
			m_Arrows = null;
		}
	}
}
