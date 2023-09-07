using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Camera/RGK Camera Position")]
	public class RGK_CameraPositionHelper : MonoBehaviour
	{
		public float FieldOfValue = 60f;

		public bool BackCamera;

		public bool AllowLookSides;
	}
}
