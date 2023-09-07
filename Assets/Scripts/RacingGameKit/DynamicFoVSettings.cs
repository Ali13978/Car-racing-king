using System;
using UnityEngine;

namespace RacingGameKit
{
	[Serializable]
	public class DynamicFoVSettings
	{
		public bool Enabled = true;

		public float StartSpeed = 20f;

		public float MaxSpeed = 250f;

		public float MinFov = 60f;

		public float MaxFov = 70f;

		public AnimationCurve ActionFovEffectCurve;
	}
}
