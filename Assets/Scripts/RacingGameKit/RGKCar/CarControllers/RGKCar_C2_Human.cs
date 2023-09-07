using UnityEngine;

namespace RacingGameKit.RGKCar.CarControllers
{
	[RequireComponent(typeof(RGKCar_Engine))]
	[AddComponentMenu("Racing Game Kit/Vehicle Controllers/RGKCar C2 - Human Controller")]
	public class RGKCar_C2_Human : RGKCar_C2_ControllerControllerBase
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

		public string ControlsCameraChangeBinding = "CameraChange";

		public string ControlsCameraLookBackBinding = "CameraBack";

		public string ControlsCameraLookLeftBinding = "CameraLeft";

		public string ControlsCameraLookRightBinding = "CameraRight";

		private bool m_IsFirstGearShifted;

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
			if (base.RaceManager.IsRaceStarted && !base.RacerRegister.IsRacerFinished)
			{
				base.CarSetup.EngineData.Automatic = true;
				if (!m_IsFirstGearShifted)
				{
					base.CarEngine.CurrentGear = 2;
					m_IsFirstGearShifted = true;
				}
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
				if (Input.GetButtonDown(ControlsResetBinding))
				{
					CheckIfRecoverable();
				}
			}
			else if (base.RacerRegister.IsRacerFinished)
			{
				steerInput = 0f;
				throttle = 0f;
				throttleInput = 0f;
				brake = 0f;
				handbrake = 1f;
				base.CarSetup.EngineData.Automatic = false;
				base.CarEngine.CurrentGear = 1;
				base.CarEngine.UseNitro = false;
				ThrottlePressed = false;
			}
			else
			{
				steerInput = 0f;
				base.CarSetup.EngineData.Automatic = false;
				base.CarEngine.CurrentGear = 1;
				handbrake = 1f;
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
			if (Input.GetButtonUp(ControlsCameraChangeBinding))
			{
				base.RaceCamera.ChangeCamera();
			}
			if (Input.GetButton(ControlsCameraLookBackBinding))
			{
				base.RaceCamera.ShowBackView();
			}
			if (Input.GetButton(ControlsCameraLookRightBinding))
			{
				base.RaceCamera.ShowRightView();
			}
			if (Input.GetButton(ControlsCameraLookLeftBinding))
			{
				base.RaceCamera.ShowLeftView();
			}
			base.Update();
		}
	}
}
