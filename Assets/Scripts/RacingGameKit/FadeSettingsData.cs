using System;
using UnityEngine;

namespace RacingGameKit
{
	[Serializable]
	public class FadeSettingsData
	{
		public bool EnableFadeOnStart = true;

		public Texture2D MaskTexture;

		public float FadeTime = 2f;

		public bool UseCurveForFade;

		public AnimationCurve FadeCurve;
	}
}
