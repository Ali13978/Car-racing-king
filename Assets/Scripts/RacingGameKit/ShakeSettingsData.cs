using System;

namespace RacingGameKit
{
	[Serializable]
	public class ShakeSettingsData
	{
		public bool ShakeOnStart;

		public float ShakeFrom = 2.5f;

		public float ShakeFadoutRes = 10f;

		public bool ShakeOnHighspeed;

		public float MaxShake = 2.5f;

		public float ShakeStartSpeed = 200f;
	}
}
