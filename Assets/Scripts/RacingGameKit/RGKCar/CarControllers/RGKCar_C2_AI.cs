using RacingGameKit.Interfaces;
using UnityEngine;

namespace RacingGameKit.RGKCar.CarControllers
{
	[RequireComponent(typeof(RGKCar_Engine))]
	[AddComponentMenu("Racing Game Kit/Vehicle Controllers/RGKCar C2 - AI Controller")]
	public class RGKCar_C2_AI : RGKCar_C2_ControllerControllerBase, IRGKCarController
	{
		public bool isReversing;

		public bool isBraking;

		public bool isPreviouslyReversed;

		private bool IsFirstGearShifted;

		public bool m_EnableRevs = true;

		public float m_RevCycleMin = 0.5f;

		public float m_RevCycleMax = 1f;

		private float m_RecCycleRand = 1f;

		private float m_RevTimer;

		private bool m_IsIdle;

		private bool m_IsInRev = true;

		public float Speed => base.CarEngine.SpeedAsKM;

		public float Rpm => base.CarEngine.RPM;

		public bool IsReversing
		{
			set
			{
				isReversing = value;
			}
		}

		public bool IsPreviouslyReversed
		{
			set
			{
				isPreviouslyReversed = value;
			}
		}

		public bool IsBraking
		{
			set
			{
				isBraking = value;
			}
		}

		public int Gear
		{
			get
			{
				return base.CarEngine.CurrentGear;
			}
			set
			{
				gear = value;
				base.CarEngine.CurrentGear = value;
			}
		}

		public float MaxSteer => 1f;

		public override void Start()
		{
			m_IsAiController = true;
			base.Start();
		}

		public override void Update()
		{
			if (!m_IsIdle || !m_EnableRevs)
			{
				return;
			}
			if (m_IsInRev)
			{
				m_RevTimer += Time.deltaTime;
				if (m_RevTimer >= m_RecCycleRand)
				{
					m_IsInRev = false;
				}
				return;
			}
			m_RevTimer -= Time.deltaTime * 3f;
			if (m_RevTimer <= 0f)
			{
				m_RecCycleRand = Random.Range(m_RevCycleMin, m_RevCycleMax);
				m_IsInRev = true;
			}
		}

		public void ApplyDrive(float Throttle, float Brake, bool HandBrake)
		{
			if (base.RaceManager == null)
			{
				return;
			}
			throttle = Mathf.Clamp(Throttle, -1f, 1.5f);
			brake = Brake;
			if (base.RaceManager.IsRaceStarted && !base.RacerRegister.IsRacerFinished && !base.RacerRegister.IsRacerDestroyed)
			{
				handbrake = 0f;
				base.CarSetup.EngineData.Automatic = true;
				if (!IsFirstGearShifted)
				{
					handbrake = 0f;
					base.CarEngine.CurrentGear = 2;
					IsFirstGearShifted = true;
				}
			}
			else if (base.RacerRegister.IsRacerFinished && !base.RaceManager.AiContinuesAfterFinish && !base.RacerRegister.IsRacerDestroyed && !base.RaceManager.PlayerContinuesAfterFinish)
			{
				steer = 0f;
				throttle = 0f;
				brake = 0f;
				handbrake = 1f;
				base.CarSetup.EngineData.Automatic = false;
				gear = 1;
				base.CarEngine.CurrentGear = 1;
				isReversing = false;
			}
			else if (base.RaceManager.IsRaceStarted && base.RacerRegister.IsRacerDestroyed)
			{
				steer = 0f;
				throttle = 0f;
				brake = 0f;
				handbrake = 1f;
				base.CarSetup.EngineData.Automatic = false;
				gear = 1;
				base.CarEngine.CurrentGear = 1;
				isReversing = false;
			}
			else if (!base.RaceManager.IsRaceStarted && base.RaceManager.IsCounting)
			{
				m_IsIdle = false;
				steer = 0f;
				throttle = 0.8f;
				base.CarSetup.EngineData.Automatic = false;
				gear = 1;
				base.CarEngine.CurrentGear = 1;
				handbrake = 1f;
			}
			else if (!base.RaceManager.IsRaceStarted && !base.RaceManager.IsCounting)
			{
				m_IsIdle = true;
				steer = 0f;
				if (m_IsInRev)
				{
					throttle = 0.8f;
				}
				else
				{
					throttle = 0.5f;
				}
				base.CarSetup.EngineData.Automatic = false;
				gear = 1;
				base.CarEngine.CurrentGear = 1;
				handbrake = 1f;
			}
			if (isReversing)
			{
				isPreviouslyReversed = true;
				gear = 0;
				throttle = Brake;
				brake = 0f;
				base.CarEngine.CurrentGear = 0;
			}
			if (isPreviouslyReversed && !isReversing)
			{
				gear = 2;
				base.CarEngine.CurrentGear = 2;
				isPreviouslyReversed = false;
			}
			if (isBraking)
			{
				base.CarEngine.brake = brake;
			}
			FeedEngine();
			ProcessLights();
		}

		public void ApplySteer(float Steer)
		{
			steer = Steer;
		}

		public void ShiftGears()
		{
		}
	}
}
