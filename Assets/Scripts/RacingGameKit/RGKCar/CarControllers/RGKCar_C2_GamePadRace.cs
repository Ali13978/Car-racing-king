using UnityEngine;

namespace RacingGameKit.RGKCar.CarControllers
{
	[RequireComponent(typeof(RGKCar_Engine))]
	[AddComponentMenu("Racing Game Kit/Vehicle Controllers/RGKCar C2 - GamePad Race")]
	public class RGKCar_C2_GamePadRace : RGKCar_C2_ControllerControllerBase
	{
		public bool SmoothThrottle;

		public bool SmoothSteering;

		public string ControlsThrottleBinding = "ThrottleP1";

		public string ControlsBrakeBinding = "BrakeP1";

		public string ControlsSteeringBinding = "SteerP1";

		public string ControlsHandbrakeBinding = "HandbrakeP1";

		public string ControlsShiftUpBinding = "ShiftUpP1";

		public string ControlsShiftDownBinding = "ShiftDownP1";

		public string ControlsHeadlightsBinding = "LightsP1";

		public string ControlsResetBinding = "ResetVehicleP1";

		public string ControlsNitroBinding = "NitroP1";

		public string ControlsCameraChangeBinding = "CameraChangeP1";

		public string ControlsCameraLookBackBinding = "CameraBackP1";

		public string ControlsCameraLookLeftBinding = "CameraLeftP1";

		public string ControlsCameraLookRightBinding = "CameraRightP1";

		private bool IsFirstGearShifted;

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
				if (UnityEngine.Input.GetAxisRaw(ControlsThrottleBinding) > 0f)
				{
					ThrottlePressed = true;
					BrakePressed = false;
				}
				else if (UnityEngine.Input.GetAxisRaw(ControlsBrakeBinding) < 0f)
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
				if (UnityEngine.Input.GetAxisRaw(ControlsThrottleBinding) > 0f)
				{
					ThrottlePressed = false;
					BrakePressed = true;
				}
				else if (UnityEngine.Input.GetAxisRaw(ControlsBrakeBinding) < 0f)
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
			throttleInput = UnityEngine.Input.GetAxisRaw(ControlsThrottleBinding);
			steerInput = 0f;
			steerInput = UnityEngine.Input.GetAxisRaw(ControlsSteeringBinding);
			if (UnityEngine.Input.GetAxisRaw(ControlsHandbrakeBinding) > 0f)
			{
				handbrake = 1f;
			}
			else
			{
				handbrake = 0f;
			}
			if (base.RaceManager.IsRaceStarted && !base.RacerRegister.IsRacerFinished)
			{
				base.CarSetup.EngineData.Automatic = true;
				if (!IsFirstGearShifted)
				{
					base.CarEngine.CurrentGear = 2;
					IsFirstGearShifted = true;
				}
				if (Input.GetButtonDown(ControlsShiftUpBinding))
				{
					base.CarEngine.ShiftUp();
				}
				if (Input.GetButtonDown(ControlsShiftDownBinding))
				{
					base.CarEngine.ShiftDown();
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
				steer = 0f;
				throttle = 0f;
				handbrake = 1f;
				brake = 0f;
				base.CarSetup.EngineData.Automatic = false;
				base.CarEngine.CurrentGear = 1;
				base.CarEngine.UseNitro = false;
			}
			else
			{
				steer = 0f;
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
			if (UnityEngine.Input.GetAxisRaw(ControlsCameraChangeBinding) > 0.5f)
			{
				base.RaceCamera.ChangeCamera();
			}
			if (UnityEngine.Input.GetAxisRaw(ControlsCameraLookBackBinding) > 0.5f)
			{
				base.RaceCamera.ShowBackView();
			}
			if (UnityEngine.Input.GetAxisRaw(ControlsCameraLookRightBinding) > 0.5f)
			{
				base.RaceCamera.ShowRightView();
			}
			if (UnityEngine.Input.GetAxisRaw(ControlsCameraLookLeftBinding) > 0.5f)
			{
				base.RaceCamera.ShowLeftView();
			}
			base.Update();
		}
	}
}
