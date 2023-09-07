using UnityEngine;

namespace RacingGameKit.RGKCar.CarControllers
{
	[RequireComponent(typeof(RGKCar_Engine))]
	[AddComponentMenu("Racing Game Kit/Vehicle Controllers/RGKCar C2 - Free Drive")]
	public class RGKCar_C2_FreeDrive : RGKCar_C2_ControllerControllerBase
	{
		public bool SmoothThrottle = true;

		public bool SmoothSteering = true;

		public string ControlsThrottleBinding = "Throttle";

		public string ControlsBrakeBinding = "Brake";

		public string ControlsSteeringBinding = "Horizontal";

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
			ThrottlePressed = ((UnityEngine.Input.GetAxisRaw(ControlsThrottleBinding) > 0f) ? true : false);
			BrakePressed = ((UnityEngine.Input.GetAxisRaw(ControlsBrakeBinding) > 0f) ? true : false);
			if (base.CarSetup.EngineData.Automatic && base.CarEngine.CurrentGear == 0)
			{
				ThrottlePressed = ((UnityEngine.Input.GetAxisRaw(ControlsBrakeBinding) > 0f) ? true : false);
				BrakePressed = ((UnityEngine.Input.GetAxisRaw(ControlsThrottleBinding) > 0f) ? true : false);
			}
			throttleInput = UnityEngine.Input.GetAxisRaw(ControlsThrottleBinding);
			steerInput = 0f;
			steerInput = UnityEngine.Input.GetAxisRaw(ControlsSteeringBinding);
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
