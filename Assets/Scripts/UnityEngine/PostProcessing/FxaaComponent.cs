namespace UnityEngine.PostProcessing
{
	public sealed class FxaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		private static class Uniforms
		{
			internal static readonly int _QualitySettings = Shader.PropertyToID("_QualitySettings");

			internal static readonly int _ConsoleSettings = Shader.PropertyToID("_ConsoleSettings");
		}

		public override bool active
		{
			get
			{
				int result;
				if (base.model.enabled)
				{
					AntialiasingModel.Settings settings = base.model.settings;
					if (settings.method == AntialiasingModel.Method.Fxaa)
					{
						result = ((!context.interrupted) ? 1 : 0);
						goto IL_0039;
					}
				}
				result = 0;
				goto IL_0039;
				IL_0039:
				return (byte)result != 0;
			}
		}

		public void Render(RenderTexture source, RenderTexture destination)
		{
			AntialiasingModel.Settings settings = base.model.settings;
			AntialiasingModel.FxaaSettings fxaaSettings = settings.fxaaSettings;
			Material material = context.materialFactory.Get("Hidden/Post FX/FXAA");
			AntialiasingModel.FxaaQualitySettings fxaaQualitySettings = AntialiasingModel.FxaaQualitySettings.presets[(int)fxaaSettings.preset];
			AntialiasingModel.FxaaConsoleSettings fxaaConsoleSettings = AntialiasingModel.FxaaConsoleSettings.presets[(int)fxaaSettings.preset];
			material.SetVector(Uniforms._QualitySettings, new Vector3(fxaaQualitySettings.subpixelAliasingRemovalAmount, fxaaQualitySettings.edgeDetectionThreshold, fxaaQualitySettings.minimumRequiredLuminance));
			material.SetVector(Uniforms._ConsoleSettings, new Vector4(fxaaConsoleSettings.subpixelSpreadAmount, fxaaConsoleSettings.edgeSharpnessAmount, fxaaConsoleSettings.edgeDetectionThreshold, fxaaConsoleSettings.minimumRequiredLuminance));
			Graphics.Blit(source, destination, material, 0);
		}
	}
}
