using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		private static class Uniforms
		{
			internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			internal static readonly int _Radius = Shader.PropertyToID("_Radius");

			internal static readonly int _FogParams = Shader.PropertyToID("_FogParams");

			internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			internal static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");

			internal static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

			internal static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

			internal static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");

			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		private enum OcclusionSource
		{
			DepthTexture,
			DepthNormalsTexture,
			GBuffer
		}

		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		private readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[2]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.CameraTarget
		};

		private OcclusionSource occlusionSource
		{
			get
			{
				if (context.isGBufferAvailable)
				{
					AmbientOcclusionModel.Settings settings = base.model.settings;
					if (!settings.forceForwardCompatibility)
					{
						return OcclusionSource.GBuffer;
					}
				}
				AmbientOcclusionModel.Settings settings2 = base.model.settings;
				if (settings2.highPrecision)
				{
					if (context.isGBufferAvailable)
					{
						AmbientOcclusionModel.Settings settings3 = base.model.settings;
						if (!settings3.forceForwardCompatibility)
						{
							goto IL_006c;
						}
					}
					return OcclusionSource.DepthTexture;
				}
				goto IL_006c;
				IL_006c:
				return OcclusionSource.DepthNormalsTexture;
			}
		}

		private bool ambientOnlySupported
		{
			get
			{
				int result;
				if (context.isHdr)
				{
					AmbientOcclusionModel.Settings settings = base.model.settings;
					if (settings.ambientOnly && context.isGBufferAvailable)
					{
						AmbientOcclusionModel.Settings settings2 = base.model.settings;
						result = ((!settings2.forceForwardCompatibility) ? 1 : 0);
						goto IL_0051;
					}
				}
				result = 0;
				goto IL_0051;
				IL_0051:
				return (byte)result != 0;
			}
		}

		public override bool active
		{
			get
			{
				int result;
				if (base.model.enabled)
				{
					AmbientOcclusionModel.Settings settings = base.model.settings;
					if (settings.intensity > 0f)
					{
						result = ((!context.interrupted) ? 1 : 0);
						goto IL_003e;
					}
				}
				result = 0;
				goto IL_003e;
				IL_003e:
				return (byte)result != 0;
			}
		}

		public override DepthTextureMode GetCameraFlags()
		{
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (occlusionSource == OcclusionSource.DepthTexture)
			{
				depthTextureMode |= DepthTextureMode.Depth;
			}
			if (occlusionSource != OcclusionSource.GBuffer)
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		public override CameraEvent GetCameraEvent()
		{
			return (!ambientOnlySupported || context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion)) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeReflections;
		}

		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material mat = context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material = context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material.shaderKeywords = null;
			material.SetFloat(Uniforms._Intensity, settings.intensity);
			material.SetFloat(Uniforms._Radius, settings.radius);
			material.SetFloat(Uniforms._Downsample, (!settings.downsampling) ? 1f : 0.5f);
			material.SetInt(Uniforms._SampleCount, (int)settings.sampleCount);
			if (!context.isGBufferAvailable && RenderSettings.fog)
			{
				material.SetVector(Uniforms._FogParams, new Vector3(RenderSettings.fogDensity, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance));
				switch (RenderSettings.fogMode)
				{
				case FogMode.Linear:
					material.EnableKeyword("FOG_LINEAR");
					break;
				case FogMode.Exponential:
					material.EnableKeyword("FOG_EXP");
					break;
				case FogMode.ExponentialSquared:
					material.EnableKeyword("FOG_EXP2");
					break;
				}
			}
			else
			{
				material.EnableKeyword("FOG_OFF");
			}
			int width = context.width;
			int height = context.height;
			int num = (!settings.downsampling) ? 1 : 2;
			int occlusionTexture = Uniforms._OcclusionTexture1;
			cb.GetTemporaryRT(occlusionTexture, width / num, height / num, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.Blit(null, occlusionTexture, material, (int)occlusionSource);
			int occlusionTexture2 = Uniforms._OcclusionTexture2;
			cb.GetTemporaryRT(occlusionTexture2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(Uniforms._MainTex, occlusionTexture);
			cb.Blit(occlusionTexture, occlusionTexture2, material, (occlusionSource != OcclusionSource.GBuffer) ? 3 : 4);
			cb.ReleaseTemporaryRT(occlusionTexture);
			occlusionTexture = Uniforms._OcclusionTexture;
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(Uniforms._MainTex, occlusionTexture2);
			cb.Blit(occlusionTexture2, occlusionTexture, material, 5);
			cb.ReleaseTemporaryRT(occlusionTexture2);
			if (context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				cb.SetGlobalTexture(Uniforms._MainTex, occlusionTexture);
				cb.Blit(occlusionTexture, BuiltinRenderTextureType.CameraTarget, material, 8);
				context.Interrupt();
			}
			else if (ambientOnlySupported)
			{
				cb.SetRenderTarget(m_MRT, BuiltinRenderTextureType.CameraTarget);
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 7);
			}
			else
			{
				RenderTextureFormat format = (!context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
				int tempRT = Uniforms._TempRT;
				cb.GetTemporaryRT(tempRT, context.width, context.height, 0, FilterMode.Bilinear, format);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, mat, 0);
				cb.SetGlobalTexture(Uniforms._MainTex, tempRT);
				cb.Blit(tempRT, BuiltinRenderTextureType.CameraTarget, material, 6);
				cb.ReleaseTemporaryRT(tempRT);
			}
			cb.ReleaseTemporaryRT(occlusionTexture);
		}
	}
}
