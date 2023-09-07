using System;

namespace RacingGameKit
{
	[Serializable]
	public class OutsideCameraSettings
	{
		public float Distance = 5f;

		public float Height = 1f;

		public float HeightDamping = 3.5f;

		public float RotationDamping = 4.5f;

		public float CameraAngle;
	}
}
