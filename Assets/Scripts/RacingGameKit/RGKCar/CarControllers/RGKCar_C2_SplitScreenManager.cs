using UnityEngine;

namespace RacingGameKit.RGKCar.CarControllers
{
	[AddComponentMenu("Racing Game Kit/Vehicle Controllers/RGKCar C2 - Split Screen Manager")]
	public class RGKCar_C2_SplitScreenManager : MonoBehaviour
	{
		public eSplitScreenController P1_Controller;

		[Space]
		public eSplitScreenController P2_Controller = eSplitScreenController.Gamepad;

		[Space]
		public string P1_Throttle = "ThrottleP1";

		public string P1_Brake = "ThrottleP1";

		public string P1_Steer = "SteerP1";

		public string P1_Handbrake = "HandbrakeP1";

		public string P1_Nitro = "NitroP1";

		public string P1_Lights = "LightsP1";

		public string P1_Reset = "ResetVehicleP1";

		public string P1_ShiftUp = "ShiftUpP1";

		public string P1_ShiftDown = "ShiftDownP1";

		public string P1_CameraChange = "CameraChangeP1";

		public string P1_CameraBack = "CameraBackP1";

		public string P1_CameraLeft = "CameraLeftP1";

		public string P1_CameraRight = "CameraRightP1";

		[Space]
		public string P2_Throttle = "ThrottleP2";

		public string P2_Brake = "BrakeP2";

		public string P2_Steer = "SteerP2";

		public string P2_Handbrake = "HandbrakeP2";

		public string P2_Nitro = "NitroP2";

		public string P2_Lights = "LightsP2";

		public string P2_Reset = "ResetVehicleP2";

		public string P2_ShiftUp = "ShiftUpP2";

		public string P2_ShiftDown = "ShiftDownP2";

		public string P2_CameraChange = "CameraChangeP2";

		public string P2_CameraBack = "CameraBackP2";

		public string P2_CameraLeft = "CameraLeftP2";

		public string P2_CameraRight = "CameraRightP2";
	}
}
