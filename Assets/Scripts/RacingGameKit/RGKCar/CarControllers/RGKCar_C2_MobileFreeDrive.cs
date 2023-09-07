using RacingGameKit.TouchDrive;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RacingGameKit.RGKCar.CarControllers
{
	[RequireComponent(typeof(RGKCar_Engine))]
	[AddComponentMenu("Racing Game Kit/Vehicle Controllers/RGKCar C2 - Mobile Freedrive")]
	public class RGKCar_C2_MobileFreeDrive : RGKCar_C2_ControllerControllerBase
	{
		public GameObject RGKMobileControls;

		public iRGKTDM touchDriveManager;

		public bool SmoothThrottle = true;

		public bool SmoothSteering;

		public bool AutoThrottle;

		public bool InvertAxis;

		public bool UseXAxis;

		public bool UseAccSteer;

		public bool UseTouchSteer;

		public bool UseButtonSteer;

		public float AccSteerTreshhold = 0.025f;

		public float SteeringSensitivity = 1f;

		public bool UseCurve;

		public AnimationCurve SteeringCurve;

		private bool AutoGear = true;

		private bool m_IsProTouchDrive;

		private bool m_IsFirstGearShifted;

		private bool EnableRaceStuff = true;

		public override void Start()
		{
			if (RGKMobileControls == null)
			{
				RGKMobileControls = GameObject.Find("_TouchDriveProManager");
			}
			if (RGKMobileControls == null)
			{
				RGKMobileControls = GameObject.Find("_TouchDriveFreeManager");
			}
			else
			{
				m_IsProTouchDrive = true;
			}
			if (RGKMobileControls == null)
			{
				UnityEngine.Debug.LogWarning("TOUCHDRIVE WARNING : TouchDrive Manager Not Found!");
			}
			else
			{
				touchDriveManager = (RGKMobileControls.GetComponent(typeof(iRGKTDM)) as iRGKTDM);
			}
			if (touchDriveManager == null && m_IsProTouchDrive)
			{
				UnityEngine.Debug.LogError("TOUCHDRIVE ERROR : TouchDrive Pro Manager found but it doesn't implement iRGKTDM interface. Please check documentation.");
			}
			if (touchDriveManager != null)
			{
				touchDriveManager.SwitchTemplate(20);
			}
			base.Start();
			Input.multiTouchEnabled = true;
		}

		public override void Update()
		{
			if (touchDriveManager == null)
			{
				return;
			}
			if (touchDriveManager.EnableDrivingOptions)
			{
				AutoThrottle = touchDriveManager.EnableAutoThrottle;
				UseXAxis = touchDriveManager.UseXAxis;
				InvertAxis = touchDriveManager.InvertAxis;
				SteeringSensitivity = touchDriveManager.SteeringSensitivity;
				AutoGear = touchDriveManager.EnableAutoGear;
				UseTouchSteer = touchDriveManager.EnableTouchWheelSteer;
				UseAccSteer = touchDriveManager.EnableTiltSteer;
			}
			smoothThrottle = SmoothThrottle;
			smoothSteering = SmoothSteering;
			base.CarSetup.EngineData.Automatic = AutoGear;
			if (!m_IsFirstGearShifted)
			{
				m_IsFirstGearShifted = true;
			}
			if (!AutoThrottle)
			{
				if (base.CarEngine.CurrentGear > 0)
				{
					if (touchDriveManager.TouchItems[1] != null && touchDriveManager.TouchItems[1].IsPressed)
					{
						ThrottlePressed = true;
						BrakePressed = false;
					}
					else
					{
						ThrottlePressed = false;
					}
					if (touchDriveManager.TouchItems[2] != null && touchDriveManager.TouchItems[2].IsPressed)
					{
						BrakePressed = true;
						ThrottlePressed = false;
					}
					else
					{
						BrakePressed = false;
					}
				}
				else if (base.CarEngine.CurrentGear == 0)
				{
					if (touchDriveManager.TouchItems[1] != null && touchDriveManager.TouchItems[1].IsPressed)
					{
						BrakePressed = true;
						ThrottlePressed = false;
					}
					else
					{
						BrakePressed = false;
					}
					if (touchDriveManager.TouchItems[2] != null && touchDriveManager.TouchItems[2].IsPressed)
					{
						ThrottlePressed = true;
						BrakePressed = false;
					}
					else
					{
						ThrottlePressed = false;
					}
				}
			}
			else if (base.CarEngine.CurrentGear > 0)
			{
				if (!touchDriveManager.TouchItems[2].IsPressed)
				{
					ThrottlePressed = true;
					BrakePressed = false;
				}
				else
				{
					ThrottlePressed = false;
					BrakePressed = true;
				}
			}
			else if (!touchDriveManager.TouchItems[2].IsPressed)
			{
				ThrottlePressed = false;
				BrakePressed = true;
			}
			else
			{
				ThrottlePressed = true;
				BrakePressed = false;
			}
			if (touchDriveManager.TouchItems[0] != null && UseTouchSteer)
			{
				steerInput = touchDriveManager.TouchItems[0].CurrentFloat;
			}
			else if (UseAccSteer)
			{
				if (!UseCurve)
				{
					if (UseXAxis)
					{
						Vector3 acceleration = Input.acceleration;
						if (Math.Abs(acceleration.x) >= AccSteerTreshhold)
						{
							if (!InvertAxis)
							{
								Vector3 acceleration2 = Input.acceleration;
								steerInput = Mathf.Clamp(acceleration2.x * SteeringSensitivity / 2f, -1f, 1f);
							}
							else
							{
								Vector3 acceleration3 = Input.acceleration;
								steerInput = Mathf.Clamp((0f - acceleration3.x) * SteeringSensitivity / 2f, -1f, 1f);
							}
						}
						else
						{
							steerInput = 0f;
						}
					}
					else
					{
						Vector3 acceleration4 = Input.acceleration;
						if (Math.Abs(acceleration4.y) >= AccSteerTreshhold)
						{
							if (!InvertAxis)
							{
								Vector3 acceleration5 = Input.acceleration;
								steerInput = Mathf.Clamp(acceleration5.y * SteeringSensitivity / 2f, -1f, 1f);
							}
							else
							{
								Vector3 acceleration6 = Input.acceleration;
								steerInput = Mathf.Clamp((0f - acceleration6.y) * SteeringSensitivity / 2f, -1f, 1f);
							}
						}
						else
						{
							steerInput = 0f;
						}
					}
				}
				else if (UseXAxis)
				{
					if (!InvertAxis)
					{
						Vector3 acceleration7 = Input.acceleration;
						if (acceleration7.x < 0f)
						{
							AnimationCurve steeringCurve = SteeringCurve;
							Vector3 acceleration8 = Input.acceleration;
							steerInput = 0f - Mathf.Clamp(steeringCurve.Evaluate(Math.Abs(acceleration8.x)), -1f, 1f);
						}
						else
						{
							AnimationCurve steeringCurve2 = SteeringCurve;
							Vector3 acceleration9 = Input.acceleration;
							steerInput = Mathf.Clamp(steeringCurve2.Evaluate(Math.Abs(acceleration9.x)), -1f, 1f);
						}
					}
					else
					{
						AnimationCurve steeringCurve3 = SteeringCurve;
						Vector3 acceleration10 = Input.acceleration;
						steerInput = Mathf.Clamp(0f - steeringCurve3.Evaluate(acceleration10.x), -1f, 1f);
					}
				}
				else if (!InvertAxis)
				{
					AnimationCurve steeringCurve4 = SteeringCurve;
					Vector3 acceleration11 = Input.acceleration;
					steerInput = Mathf.Clamp(steeringCurve4.Evaluate(acceleration11.y), -1f, 1f);
				}
				else
				{
					AnimationCurve steeringCurve5 = SteeringCurve;
					Vector3 acceleration12 = Input.acceleration;
					steerInput = Mathf.Clamp(0f - steeringCurve5.Evaluate(acceleration12.y), -1f, 1f);
				}
			}
			else if (touchDriveManager.TouchItems[3] != null && touchDriveManager.TouchItems[3].IsPressed)
			{
				steerInput = -1f;
			}
			else if (touchDriveManager.TouchItems[4] != null && touchDriveManager.TouchItems[4].IsPressed)
			{
				steerInput = 1f;
			}
			else
			{
				steerInput = 0f;
			}
			if (EnableRaceStuff)
			{
				if (touchDriveManager.TouchItems[5] != null && touchDriveManager.TouchItems[5].IsPressed)
				{
					base.CarEngine.ShiftUp();
				}
				if (touchDriveManager.TouchItems[6] != null && touchDriveManager.TouchItems[6].IsPressed)
				{
					base.CarEngine.ShiftDown();
				}
				if (touchDriveManager.TouchItems[9] != null && touchDriveManager.TouchItems[9].IsPressed)
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
				if (touchDriveManager.TouchItems[11] != null && touchDriveManager.TouchItems[11].IsPressed)
				{
					base.CarEngine.UseNitro = true;
				}
				else
				{
					base.CarEngine.UseNitro = false;
				}
				if (touchDriveManager.TouchItems[12] != null && touchDriveManager.TouchItems[12].IsPressed)
				{
					base.CarEngine.handbrake = 1f;
				}
				else
				{
					base.CarEngine.handbrake = 0f;
				}
			}
			if (base.RaceCamera != null)
			{
				if (touchDriveManager.TouchItems[7] != null && touchDriveManager.TouchItems[7].IsPressed)
				{
					base.RaceCamera.ChangeCamera();
					touchDriveManager.TouchItems[7].IsPressed = false;
				}
				if (touchDriveManager.TouchItems[8] != null && touchDriveManager.TouchItems[8].IsPressed)
				{
					base.RaceCamera.ShowBackView();
				}
			}
			base.Update();
		}
	}
}
