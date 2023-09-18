//using Photon.Pun;
using System;
using UnityEngine;

namespace RacingGameKit.RGKCar
{
	[RequireComponent(typeof(RGKCar_Setup))]
	[AddComponentMenu("Racing Game Kit/RGKCar/RGKCar Engine")]
	public class RGKCar_Engine : MonoBehaviour
	{
		private RGKCar_Setup CarSetup;

		private RGKCar_CarAudioAdvanced CarAudio;

		private Vector3 engineOrientation = Vector3.right;

		public float throttle;

		public float brake;

		public float handbrake;

		public float steer;

		public int CurrentGear = 2;

		public int Gear;

		public string GearAuto = "D";

		public bool isClutch;

		private float clutchInteral;

		private int gearToChange;

		public float RPM;

		public float SpeedAsKM;

		public float SpeedAsMile;

		public float TurboFill;

		private float TurboDiff;

		public float slipRatio;

		public float engineAngularVelo;

		public float totalSlip;

		private bool isShifting;

		private float ShiftInternalCounter = 1f;

		private bool isBackfire;

		private bool isBackfireBlocked;

		private float BackfireBlockingInternal;

		private float BackfireInternalCounter;

		private bool isBackfireAudioPlaying;

		public float limitedTimeEngineVelo;

		private bool m_useNitro;

		public bool m_IsAuto = true;

		public bool m_StartHelper = true;

		public float m_StartHelperMultiplier = 2f;

		public float m_startHelperEnd = 40f;

		private int m_WheelsOnGroundCount;

		public float WaitForReverse = 2f;

		private float lockingTorque;

		public float intThrottle;

		public float TotalSlip
		{
			get
			{
				totalSlip = 0f;
				if (CarSetup != null)
				{
					RGKCar_Wheel[] wheels = CarSetup.Wheels;
					foreach (RGKCar_Wheel rGKCar_Wheel in wheels)
					{
						totalSlip += rGKCar_Wheel.slipVelo / (float)CarSetup.Wheels.Length;
					}
				}
				return totalSlip;
			}
		}

		public bool UseNitro
		{
			get
			{
				return m_useNitro;
			}
			set
			{
				m_useNitro = value;
				if (!value)
				{
					ResetNos();
				}
			}
		}

		public int WheelsOnGroundCount => m_WheelsOnGroundCount;

		private float Sqr(float x)
		{
			return x * x;
		}

		private void Start()
		{
            CarSetup = GetComponent<RGKCar_Setup>();
			CarAudio = GetComponent<RGKCar_CarAudioAdvanced>();
			m_IsAuto = CarSetup.EngineData.Automatic;
			ResetNos();
		}

		private float CalcEngineTorque()
		{
			float num;
			if (RPM < CarSetup.EngineData.EngineTorqueRPM)
			{
				num = CarSetup.EngineData.EngineMaxTorque * (0f - Sqr(RPM / CarSetup.EngineData.EngineTorqueRPM - 1f) + 1f);
			}
			else
			{
				float num2 = CarSetup.EngineData.EngineMaxPowerKw * 1000f / (CarSetup.EngineData.EnginePowerRPM * 2f * (float)Math.PI / 60f);
				float num3 = (CarSetup.EngineData.EngineMaxTorque - num2) / (2f * CarSetup.EngineData.EngineTorqueRPM * CarSetup.EngineData.EnginePowerRPM - Sqr(CarSetup.EngineData.EnginePowerRPM) - Sqr(CarSetup.EngineData.EngineTorqueRPM));
				float num4 = num3 * Sqr(RPM - CarSetup.EngineData.EngineTorqueRPM) + CarSetup.EngineData.EngineMaxTorque;
				num = ((!(num4 > 0f)) ? 0f : num4);
			}
			if (RPM > CarSetup.EngineData.EngineMaxRPM)
			{
				num *= 1f - (RPM - CarSetup.EngineData.EngineMaxRPM) * 0.006f;
				if (num < 0f)
				{
					num = 0f;
				}
			}
			if (RPM < 0f)
			{
				num = 0f;
			}
			return num;
		}

		private void FixedUpdate()
		{

            if (isClutch)
			{
				CurrentGear = 1;
				intThrottle = 0f;
			}
			else
			{
				intThrottle = throttle;
			}
			float num = CarSetup.EngineData.Gears[CurrentGear] * CarSetup.EngineData.GearFinalRatio;
			float num2 = CarSetup.EngineData.EngineInteria * Sqr(num);
			float num3 = CarSetup.EngineData.EngineFriction + RPM * CarSetup.EngineData.EngineRPMFriction;
			float num4 = 0f;
			bool flag = true;
			if (!isClutch && flag)
			{
				num4 = (CalcEngineTorque() + Mathf.Abs(num3)) * intThrottle;
			}
			slipRatio = 0f;
			if (num == 0f)
			{
				float num5 = (num4 - num3) / CarSetup.EngineData.EngineInteria;
				engineAngularVelo += num5 * Time.deltaTime * 3f;
				if (engineAngularVelo < 0f)
				{
					engineAngularVelo = 0f;
				}
				GetComponent<Rigidbody>().AddTorque(-engineOrientation * num4);
			}
			else
			{
				int num6 = 0;
				RGKCar_Wheel[] wheels = CarSetup.Wheels;
				foreach (RGKCar_Wheel rGKCar_Wheel in wheels)
				{
					if (rGKCar_Wheel.isPowered)
					{
						num6++;
					}
				}
				float num7 = 0.8f / (float)num6;
				float num8 = 0f;
				RGKCar_Wheel[] wheels2 = CarSetup.Wheels;
				foreach (RGKCar_Wheel rGKCar_Wheel2 in wheels2)
				{
					if (rGKCar_Wheel2.isPowered)
					{
						num8 += rGKCar_Wheel2.angularVelocity * num7;
					}
				}
				if (m_StartHelper && (CurrentGear == 2 || CurrentGear == 0) && handbrake == 0f && SpeedAsKM < m_startHelperEnd)
				{
					if (m_StartHelperMultiplier < 1f)
					{
						m_StartHelperMultiplier = 1f;
					}
					num4 *= m_StartHelperMultiplier;
				}
				float num9 = 0f;
				m_WheelsOnGroundCount = 0;
				RGKCar_Wheel[] wheels3 = CarSetup.Wheels;
				foreach (RGKCar_Wheel rGKCar_Wheel3 in wheels3)
				{
					if (rGKCar_Wheel3.isPowered)
					{
						if (!isClutch)
						{
							if (rGKCar_Wheel3.angularVelocity * CarSetup.EngineData.GearFinalRatio * CarSetup.EngineData.Gears[CurrentGear] * (25f / (float)Math.PI) > CarSetup.EngineData.EngineMaxRPM)
							{
								rGKCar_Wheel3.angularVelocity = CarSetup.EngineData.EngineMaxRPM / (CarSetup.EngineData.GearFinalRatio * CarSetup.EngineData.Gears[CurrentGear] * (25f / (float)Math.PI));
							}
							if (CarSetup.EngineData.LimitEngineSpeed && SpeedAsKM >= CarSetup.EngineData.LimitSpeedTo && rGKCar_Wheel3.angularVelocity * CarSetup.EngineData.GearFinalRatio * CarSetup.EngineData.Gears[CurrentGear] * (25f / (float)Math.PI) > limitedTimeEngineVelo)
							{
								rGKCar_Wheel3.angularVelocity = limitedTimeEngineVelo / (CarSetup.EngineData.GearFinalRatio * CarSetup.EngineData.Gears[CurrentGear] * (25f / (float)Math.PI));
							}
							lockingTorque = (num8 - rGKCar_Wheel3.angularVelocity) * CarSetup.EngineData.DifferentialLock;
							rGKCar_Wheel3.drivetrainInertia = num2 * num7;
							rGKCar_Wheel3.driveFrictionTorque = num3 * Mathf.Abs(num) * num7;
							rGKCar_Wheel3.driveTorque = num4 * num * num7 + lockingTorque;
						}
						slipRatio += rGKCar_Wheel3.slipRatio * num7;
						if (rGKCar_Wheel3.onGround)
						{
							num9 += 1f;
						}
					}
					if (rGKCar_Wheel3.onGround)
					{
						m_WheelsOnGroundCount++;
					}
				}
				flag = ((num9 == (float)num6) ? true : false);
				engineAngularVelo = num8 * num;
				if (engineAngularVelo < 0f)
				{
					engineAngularVelo = 0f;
				}
			}
			if (CarSetup.EngineData.LimitEngineSpeed && SpeedAsKM >= CarSetup.EngineData.LimitSpeedTo)
			{
				RPM = limitedTimeEngineVelo;
			}
			else
			{
				RPM = engineAngularVelo * (30f / (float)Math.PI);
				limitedTimeEngineVelo = RPM;
			}
			float num10 = CarSetup.EngineData.EngineMinRPM;
			if (CurrentGear == 2 || CurrentGear == 0)
			{
				num10 += intThrottle * (float)UnityEngine.Random.Range(3000, 3250);
			}
			if (RPM < num10)
			{
				RPM = num10;
			}
			if (RPM > CarSetup.EngineData.EngineMaxRPM)
			{
				RPM = CarSetup.EngineData.EngineMaxRPM;
			}
			TurboDiff = CarSetup.EngineData.EnginePowerRPM - CarSetup.EngineData.EngineTorqueRPM;
			if (brake > 0f)
			{
				TurboFill = 0f;
			}
			if (intThrottle > 0f)
			{
				FillTurbo(RPM);
			}
			float num11 = CarSetup.EngineData.EngineMaxRPM * (0.5f + 0.5f * intThrottle);
			if (RPM >= num11 - 2000f && CurrentGear > 0 && intThrottle > 0f)
			{
				RPM += UnityEngine.Random.Range(20, 100);
			}
			else if (RPM >= num11 - 1500f && CurrentGear > 0 && intThrottle > 0f)
			{
				RPM += UnityEngine.Random.Range(100, 250);
			}
			else if (RPM >= num11 - 1000f && CurrentGear > 0 && intThrottle > 0f)
			{
				RPM += UnityEngine.Random.Range(50, 350);
			}
			if (m_IsAuto && !isClutch)
			{
				if (CarSetup.EngineData.AutoReverse)
				{
					if ((CurrentGear == 1 || CurrentGear == 2) && brake > 0f && SpeedAsKM < 10f)
					{
						CurrentGear = 0;
						ShiftDown();
					}
					else if (CurrentGear == 0 && brake > 0f)
					{
						handbrake = 1f;
						if (SpeedAsKM < 10f)
						{
							CurrentGear = 2;
						}
					}
				}
				if (RPM >= CarSetup.EngineData.GearUpRPM && CurrentGear > 0 && intThrottle > 0f && CurrentGear < CarSetup.EngineData.Gears.Length - 1)
				{
					ShiftUp();
					if (CarAudio != null && CurrentGear > 2)
					{
						CarAudio.PopTurbo(TurboFill);
					}
					TurboFill = 0f;
					isClutch = true;
				}
				else if (RPM <= CarSetup.EngineData.GearDownRPM && CurrentGear > 2 && SpeedAsKM < 30f)
				{
					ShiftTo(2);
					isClutch = true;
				}
				else if (RPM <= CarSetup.EngineData.GearDownRPM && CurrentGear > 2)
				{
					ShiftDown();
					isClutch = true;
				}
				if (intThrottle < 0f && RPM <= CarSetup.EngineData.EngineMinRPM)
				{
					CurrentGear = ((CurrentGear == 0) ? 2 : 0);
				}
			}
			int num12 = Gear = CurrentGear - 1;
			if (num12 == 0)
			{
				GearAuto = "N";
			}
			if (num12 > 0)
			{
				GearAuto = "D";
			}
			if (num12 < 0)
			{
				GearAuto = "R";
			}
			SpeedAsKM = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
			SpeedAsMile = SpeedAsKM * 0.6214f;
			if (CarSetup.MiscFX.ExhaustBackfire != null)
			{
				if (brake > 0f && !isBackfire && !isBackfireBlocked && CurrentGear > 2)
				{
					isBackfire = true;
					BackfireInternalCounter = UnityEngine.Random.Range(0.1f, 0.6f);
					if (!isBackfireBlocked)
					{
						BackfireBlockingInternal = 0f;
					}
				}
				DoBackfire();
			}
			if (CarAudio != null)
			{
				if (brake > 0f)
				{
					CarAudio.ApplyBrakeDisk(brake);
				}
				else
				{
					CarAudio.ApplyBrakeDisk(0f);
				}
			}
			RGKCar_Wheel[] wheels4 = CarSetup.Wheels;
			foreach (RGKCar_Wheel rGKCar_Wheel4 in wheels4)
			{
				rGKCar_Wheel4.brake = brake;
				rGKCar_Wheel4.steering = steer;
				rGKCar_Wheel4.handbrake = handbrake;
			}
			if (isClutch)
			{
				DoClutch();
			}
			UseNitroInternal();
		}

		private void DoClutch()
		{
			if (isClutch)
			{
				clutchInteral -= Time.deltaTime;
				CurrentGear = 1;
				if (clutchInteral <= 0f)
				{
					isClutch = false;
					clutchInteral = CarSetup.EngineData.ClutchTime;
					CurrentGear = gearToChange;
				}
			}
		}

		public void ShiftUp()
		{
			if (CurrentGear >= CarSetup.EngineData.Gears.Length - 1 || isClutch)
			{
				return;
			}
			clutchInteral = CarSetup.EngineData.ClutchTime;
			gearToChange = CurrentGear + 1;
			isClutch = true;
			if (CarAudio != null)
			{
				CarAudio.PopGear();
			}
			if (!CarSetup.MiscFX.BackFireOnGearUp || !(CarSetup.MiscFX.ExhaustBackfire != null))
			{
				return;
			}
			if (!isBackfire && CurrentGear >= 2 && !m_useNitro)
			{
				isBackfire = true;
				BackfireInternalCounter = UnityEngine.Random.Range(0.1f, 0.6f);
				if (!isBackfireBlocked)
				{
					BackfireBlockingInternal = 0f;
				}
			}
			DoBackfire();
		}

		public void ShiftTo(int GearIndex)
		{
			if (!isClutch)
			{
				clutchInteral = CarSetup.EngineData.ClutchTime;
				gearToChange = GearIndex;
				isClutch = true;
				if (CarAudio != null)
				{
					CarAudio.PopGear();
				}
			}
		}

		public void ShiftDown()
		{
			if (CurrentGear > 0 && !isClutch)
			{
				clutchInteral = CarSetup.EngineData.ClutchTime;
				gearToChange = CurrentGear - 1;
				isClutch = true;
				if (CarAudio != null)
				{
					CarAudio.PopGear();
				}
			}
		}

		private void DoBackfire()
		{
			if (m_useNitro)
			{
				return;
			}
			if (isBackfireBlocked)
			{
				BackfireBlockingInternal += Time.deltaTime;
				if (BackfireBlockingInternal >= CarSetup.MiscFX.BackfireBlockingSeconds)
				{
					BackfireBlockingInternal = 0f;
					isBackfireBlocked = false;
				}
			}
			if (isBackfire)
			{
				BackfireInternalCounter -= Time.deltaTime;
				if (BackfireInternalCounter >= 0f)
				{
					CarSetup.MiscFX.ExhaustBackfire.SetActive(value: true);
					if (CarAudio != null && !isBackfireAudioPlaying)
					{
						if (BackfireInternalCounter > CarSetup.MiscFX.BackfireSeconds)
						{
							isBackfireAudioPlaying = CarAudio.PopBackfire(IsLong: true);
						}
						else
						{
							isBackfireAudioPlaying = CarAudio.PopBackfire(IsLong: false);
						}
					}
					isBackfireBlocked = true;
				}
				else
				{
					isBackfire = false;
					isBackfireAudioPlaying = false;
					resetBackFire();
				}
			}
			else
			{
				resetBackFire();
			}
		}

		private void resetBackFire()
		{
			if (CarSetup.MiscFX.ExhaustBackfire.activeSelf)
			{
				CarSetup.MiscFX.ExhaustBackfire.SetActive(value: false);
			}
		}

		private void FillTurbo(float eRPM)
		{
			if (eRPM >= CarSetup.EngineData.EngineTorqueRPM && eRPM <= CarSetup.EngineData.EnginePowerRPM)
			{
				TurboFill += RPM * TurboDiff / CarSetup.EngineData.EnginePowerRPM / TurboDiff * Time.deltaTime;
				if (TurboFill > 1f)
				{
					TurboFill = 1f;
				}
			}
		}

		public void ShiftDone()
		{
			if (isShifting)
			{
				ShiftInternalCounter -= Time.deltaTime;
				if (ShiftInternalCounter < 0f)
				{
					isShifting = false;
					ShiftInternalCounter = 1f;
				}
			}
		}

		public void UseNitroInternal()
		{
			if (m_useNitro && throttle > 0f && Gear >= 0)
			{
				if (CarSetup.Nitro.NitroLeft > 0f)
				{
					GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().transform.forward * CarSetup.Nitro.ForceAdd * Mathf.Clamp01(CarSetup.Nitro.ForceBalance / 10f), ForceMode.Acceleration);
					CarSetup.Nitro.NitroLeft -= Time.deltaTime * Mathf.Clamp01(CarSetup.Nitro.ForceBalance / 10f) * 2f;
					if (CarSetup.Nitro.NosFireObject != null && !CarSetup.Nitro.NosFireObject.activeSelf)
					{
						CarSetup.Nitro.NosFireObject.SetActive(value: true);
					}
					if (CarAudio != null)
					{
						CarAudio.ApplyNitro(IsUsing: true);
					}
				}
				else
				{
					ResetNos();
				}
			}
			else
			{
				ResetNos();
				if (CarSetup.Nitro.AutoFill && CarSetup.Nitro.NitroLeft < CarSetup.Nitro.InitialAmount)
				{
					CarSetup.Nitro.NitroLeft += Time.deltaTime / CarSetup.Nitro.RefillSpeed * 4f;
				}
			}
		}

		private void ResetNos()
		{
			if (CarSetup != null)
			{
				if (CarSetup.Nitro.NosFireObject != null && CarSetup.Nitro.NosFireObject.activeSelf)
				{
					CarSetup.Nitro.NosFireObject.SetActive(value: false);
				}
				if (CarAudio != null)
				{
					CarAudio.ApplyNitro(IsUsing: false);
				}
			}
		}
	}
}
