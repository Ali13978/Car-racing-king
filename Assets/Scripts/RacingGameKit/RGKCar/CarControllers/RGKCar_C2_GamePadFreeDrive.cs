using UnityEngine;

namespace RacingGameKit.RGKCar.CarControllers
{
	[RequireComponent(typeof(RGKCar_Engine))]
	[AddComponentMenu("Racing Game Kit/Vehicle Controllers/RGKCar C2 - GamePad Freedrive")]
	public class RGKCar_C2_GamePadFreeDrive : RGKCar_C2_ControllerControllerBase
	{
		public bool SmoothThrottle;

		public bool SmoothSteering;

		public string ControlsThrottleAxisBinding = "Throttle";

		public string ControlsBrakeAxisBinding = "Brake";

		public string ControlsSteeringAxisBinding = "Horizontal";

		public string ControlsHandbrakeBinding = "Handbrake";

		public string ControlsShiftUpBinding = "ShiftUp";

		public string ControlsShiftDownBinding = "ShiftDown";

		public string ControlsHeadlightsBinding = "Lights";

		public string ControlsResetBinding = "ResetVehicle";

		public string ControlsNitroBinding = "Nitro";

		public override void Start()
		{
			base.Start();
		}

		public override void Update()
		{
			smoothThrottle = SmoothThrottle;
			smoothSteering = SmoothSteering;
			if (base.CarSetup.EngineData.Automatic && base.CarEngine.CurrentGear > 0)
			{
				if (UnityEngine.Input.GetAxisRaw(ControlsThrottleAxisBinding) > 0f)
				{
					ThrottlePressed = true;
					BrakePressed = false;
				}
				else if (UnityEngine.Input.GetAxisRaw(ControlsBrakeAxisBinding) < 0f)
				{
					ThrottlePressed = false;
					BrakePressed = true;
				}
				else
				{
					ThrottlePressed = false;
					BrakePressed = false;
				}
			}
			else if (base.CarSetup.EngineData.Automatic && base.CarEngine.CurrentGear == 0)
			{
				if (UnityEngine.Input.GetAxisRaw(ControlsThrottleAxisBinding) > 0f)
				{
					ThrottlePressed = false;
					BrakePressed = true;
				}
				else if (UnityEngine.Input.GetAxisRaw(ControlsBrakeAxisBinding) < 0f)
				{
					ThrottlePressed = true;
					BrakePressed = false;
				}
				else
				{
					ThrottlePressed = false;
					BrakePressed = false;
				}
			}
			throttleInput = UnityEngine.Input.GetAxisRaw(ControlsThrottleAxisBinding);
			steerInput = 0f;
			steerInput = UnityEngine.Input.GetAxisRaw(ControlsSteeringAxisBinding);
			if (Input.GetButtonDown(ControlsShiftUpBinding))
			{
				base.CarEngine.ShiftUp();
			}
			if (Input.GetButtonDown(ControlsShiftDownBinding))
			{
				base.CarEngine.ShiftDown();
			}
			if (UnityEngine.Input.GetAxisRaw(ControlsHandbrakeBinding) > 0f)
			{
				handbrake = 1f;
			}
			else
			{
				handbrake = 0f;
			}
			if (Input.GetButton(ControlsNitroBinding))
			{
				base.CarEngine.UseNitro = true;
			}
			else
			{
				base.CarEngine.UseNitro = false;
			}
			if (Input.GetButtonDown(ControlsHeadlightsBinding))
			{
				if (m_IsLightsOn)
				{
					m_IsLightsOn = false;
				}
				else
				{
					m_IsLightsOn = true;
				}
			}
			base.Update();
		}
	}
}
