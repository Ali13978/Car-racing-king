using UnityEngine;

namespace RacingGameKit.RGKCar
{
	[AddComponentMenu("Racing Game Kit/RGKCar/RGKCar Helpers/Sway Bar")]
	public class RGKCar_SwayBar : MonoBehaviour
	{
		public RGKCar_Wheel wheel1;

		public RGKCar_Wheel wheel2;

		public float coefficient = 5000f;

		private void FixedUpdate()
		{
			if (wheel1 != null && wheel2 != null && base.enabled)
			{
				float num = (wheel1.compression - wheel2.compression) * coefficient;
				wheel1.suspensionForceInput = num;
				wheel2.suspensionForceInput = 0f - num;
			}
		}
	}
}
