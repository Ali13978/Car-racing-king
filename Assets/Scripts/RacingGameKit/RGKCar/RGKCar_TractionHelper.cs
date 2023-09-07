using UnityEngine;

namespace RacingGameKit.RGKCar
{
	[AddComponentMenu("Racing Game Kit/RGKCar/RGKCar Helpers/Traction Helper")]
	public class RGKCar_TractionHelper : MonoBehaviour
	{
		public RGKCar_Wheel[] front;

		public float compensationFactor = 0.1f;

		private float oldGrip;

		private float angle;

		private float angularVelo;

		private void Start()
		{
			oldGrip = front[0].wheelGrip;
		}

		private void Update()
		{
			Vector3 forward = base.transform.forward;
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			velocity -= base.transform.up * Vector3.Dot(velocity, base.transform.up);
			velocity.Normalize();
			angle = 0f - Mathf.Asin(Vector3.Dot(Vector3.Cross(forward, velocity), base.transform.up));
			Vector3 angularVelocity = GetComponent<Rigidbody>().angularVelocity;
			angularVelo = angularVelocity.y;
			RGKCar_Wheel[] array = front;
			foreach (RGKCar_Wheel rGKCar_Wheel in array)
			{
				if (angle * rGKCar_Wheel.steering < 0f)
				{
					rGKCar_Wheel.wheelGrip = oldGrip * (1f - Mathf.Clamp01(compensationFactor * Mathf.Abs(angularVelo)));
				}
				else
				{
					rGKCar_Wheel.wheelGrip = oldGrip;
				}
			}
		}
	}
}
