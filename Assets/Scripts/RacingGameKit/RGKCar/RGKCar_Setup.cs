using RacingGameKit.Helpers;
using System;
using UnityEngine;

namespace RacingGameKit.RGKCar
{
	[AddComponentMenu("Racing Game Kit/RGKCar/RGKCar Setup")]
	public class RGKCar_Setup : MonoBehaviour
	{
		[Serializable]
		public class EngineDataBase
		{
			public float EngineMinRPM = 1000f;

			public float EngineMaxRPM = 8000f;

			public float GearDownRPM = 4500f;

			public float GearUpRPM = 7000f;

			public float ClutchTime = 0.2f;

			public float EngineMaxTorque = 350f;

			public float EngineTorqueRPM = 4000f;

			public float EngineMaxPowerKw = 75f;

			public float EnginePowerRPM = 6500f;

			public float EngineInteria = 1f;

			public float EngineFriction = 10f;

			public float EngineRPMFriction = 0.1f;

			public bool LimitEngineSpeed;

			public float LimitSpeedTo = 100f;

			public float[] Gears = new float[8]
			{
				-4f,
				0f,
				4f,
				2.5f,
				1.78f,
				1.36f,
				1.1f,
				0.89f
			};

			public float GearFinalRatio = 5.75f;

			public bool Automatic = true;

			public float DifferentialLock;

			public bool AutoReverse = true;

			public bool EnableSTM = true;

			public float StartTorqueMultipler = 2f;

			public float STMMaxSpeed = 40f;
		}

		[Serializable]
		public class WheelDataBase
		{
			public float FrontBrakeTorque = 2000f;

			public float RearBrakeTorque = 3500f;

			public float TrailerBrakeTorque = 3500f;

			public float HandBrakeTorque = 5000f;

			public float HandBrakeTorqueFront = 1000f;

			public float TrailerHandBrakeTorque = 5000f;

			public float MaxSteeringAngle = 30f;

			public float FrontWheelGrip = 1.5f;

			public float FrontWheelSideGrip = 1.5f;

			public float RearWheelGrip = 1.6f;

			public float RearWheelSideGrip = 1.6f;

			public float TrailerWheelGrip = 1.5f;

			public float TrailerWheelSideGrip = 1.5f;

			public float FrontWheelRadius = 0.32f;

			public float RearWheelRadius = 0.32f;

			public float TrailerWheelRadius = 0.32f;

			public float WheelInteria = 10f;

			public float FrictionTorque = 50f;

			public float MassFriction = 0.25f;

			public float SuspensionHeight = 0.25f;

			public float SuspensionStiffness = 5000f;

			public float SuspensionReleaseCoef = 50f;

			public Transform WheelBase;

			public float WheelBaseAlignment;

			public float BlurSwtichVelocity = 20f;

			public bool ShowForces;
		}

		[Serializable]
		public class AeroDynamicsBase
		{
			public bool EnableWing = true;

			public float WingDownforceCoef = -0.5f;

			public bool EnableAirFriction = true;

			public Vector3 AirFrictionCoef = new Vector3(0.5f, 0.5f, 0.05f);
		}

		[Serializable]
		public class NitroBase
		{
			public GameObject NosFireObject;

			public bool NitroEnable;

			public bool AutoFill = true;

			public float RefillSpeed = 1f;

			public float InitialAmount = 5f;

			public float NitroLeft;

			public float ForceBalance = 1f;

			public float ForceAdd = 25f;
		}

		[Serializable]
		public class LightsDataBase
		{
			public Transform[] HeadlightsLightObjects;

			public Light[] HeadlightsLights;

			public Transform[] BrakelightsLightObjects;

			public Transform[] BrakelightsLightFlareObjects;

			public Light[] BrakelightsLights;

			public Transform[] ReverseLightLightObjects;

			public Light[] ReverselightLights;

			public float DefaultBrakeLightIntensity = 0.7f;

			public bool LightsOn;
		}

		[Serializable]
		public class FXDataBase
		{
			public GameObject ExhaustBackfire;

			public bool BackFireOnGearUp;

			public float BackfireSeconds = 0.5f;

			public float BackfireBlockingSeconds = 5f;
		}

		internal Rigidbody vehicleRbody;

		public bool SetupMode;

		public bool RigidbodySleepOnAwake;

		public float CarMass = 1000f;

		public float CarInteria = 1f;

		public Transform CenterOfMass;

		public DriveEnum Drive = DriveEnum.RWD;

		public RGKCar_Wheel[] Wheels;

		public WheelDataBase WheelData;

		public EngineDataBase EngineData;

		public LightsDataBase LightsData;

		public AeroDynamicsBase AeroDynamicsData;

		public FXDataBase MiscFX;

		public NitroBase Nitro;

		public bool UseEarthFX;

		public string SkidmarkObjectName;

		private GameObject SkidMarks;

		private void Awake()
		{
			vehicleRbody = base.transform.GetComponent<Rigidbody>();
			if (RigidbodySleepOnAwake)
			{
				GetComponent<Rigidbody>().Sleep();
			}
		}

		private void Start()
		{
			Nitro.NitroLeft = Nitro.InitialAmount;
			if (CenterOfMass != null)
			{
				GetComponent<Rigidbody>().centerOfMass = CenterOfMass.localPosition;
			}
			if (WheelData.WheelBase != null)
			{
				WheelData.WheelBase.localPosition = new Vector3(0f, WheelData.WheelBaseAlignment);
			}
			if (SkidmarkObjectName != string.Empty)
			{
				SkidMarks = GameObject.Find(SkidmarkObjectName);
			}
			if (CarInteria < 0.01f)
			{
				CarInteria = 0.01f;
			}
			GetComponent<Rigidbody>().inertiaTensor *= CarInteria;
			updateVehicleConfig();
		}

		private void FixedUpdate()
		{
			if (SetupMode)
			{
				updateVehicleConfig();
			}
			if (vehicleRbody != null && AeroDynamicsData.EnableWing)
			{
				float d = AeroDynamicsData.WingDownforceCoef * GetComponent<Rigidbody>().velocity.sqrMagnitude;
				GetComponent<Rigidbody>().AddForceAtPosition(d * base.transform.up, base.transform.position);
			}
			if (vehicleRbody != null && AeroDynamicsData.EnableAirFriction)
			{
				Vector3 a = base.transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
				Vector3 direction = Vector3.Scale(Vector3.Scale(b: new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z)), a: a), -2f * AeroDynamicsData.AirFrictionCoef);
				GetComponent<Rigidbody>().AddForce(base.transform.TransformDirection(direction));
			}
		}

		private void updateVehicleConfig()
		{
			if (CenterOfMass != null)
			{
				GetComponent<Rigidbody>().centerOfMass = CenterOfMass.localPosition;
			}
			if (WheelData.WheelBase != null)
			{
				WheelData.WheelBase.localPosition = new Vector3(0f, WheelData.WheelBaseAlignment);
			}
			if (CarMass < 1f)
			{
				CarMass = 1f;
			}
			GetComponent<Rigidbody>().mass = CarMass;
			RGKCar_Wheel[] wheels = Wheels;
			foreach (RGKCar_Wheel rGKCar_Wheel in wheels)
			{
				if (rGKCar_Wheel.WheelLocation == WheelLocationEnum.Front)
				{
					if (Drive == DriveEnum.FWD || Drive == DriveEnum.AWD)
					{
						rGKCar_Wheel.isPowered = true;
					}
					rGKCar_Wheel.maxSteeringAngle = WheelData.MaxSteeringAngle;
					rGKCar_Wheel.suspensionHeight = WheelData.SuspensionHeight;
					rGKCar_Wheel.suspensionStiffness = WheelData.SuspensionStiffness;
					rGKCar_Wheel.brakeTorque = WheelData.FrontBrakeTorque;
					rGKCar_Wheel.handbrakeTorque = WheelData.HandBrakeTorqueFront;
					rGKCar_Wheel.definedGrip = WheelData.FrontWheelGrip;
					rGKCar_Wheel.definedSideGrip = WheelData.FrontWheelSideGrip;
					rGKCar_Wheel.radius = WheelData.FrontWheelRadius;
				}
				if (rGKCar_Wheel.WheelLocation == WheelLocationEnum.Rear)
				{
					if (Drive == DriveEnum.RWD || Drive == DriveEnum.AWD)
					{
						rGKCar_Wheel.isPowered = true;
					}
					rGKCar_Wheel.maxSteeringAngle = 0f;
					rGKCar_Wheel.suspensionHeight = WheelData.SuspensionHeight;
					rGKCar_Wheel.suspensionStiffness = WheelData.SuspensionStiffness;
					rGKCar_Wheel.brakeTorque = WheelData.RearBrakeTorque;
					rGKCar_Wheel.handbrakeTorque = WheelData.HandBrakeTorque;
					rGKCar_Wheel.definedGrip = WheelData.RearWheelGrip;
					rGKCar_Wheel.radius = WheelData.RearWheelRadius;
					rGKCar_Wheel.definedSideGrip = WheelData.RearWheelSideGrip;
				}
				if (rGKCar_Wheel.WheelLocation == WheelLocationEnum.Trailer)
				{
					rGKCar_Wheel.isPowered = false;
					rGKCar_Wheel.maxSteeringAngle = 0f;
					rGKCar_Wheel.suspensionHeight = WheelData.SuspensionHeight;
					rGKCar_Wheel.suspensionStiffness = WheelData.SuspensionStiffness;
					rGKCar_Wheel.brakeTorque = WheelData.TrailerBrakeTorque;
					rGKCar_Wheel.handbrakeTorque = WheelData.TrailerHandBrakeTorque;
					rGKCar_Wheel.definedGrip = WheelData.TrailerWheelGrip;
					rGKCar_Wheel.radius = WheelData.TrailerWheelRadius;
					rGKCar_Wheel.definedSideGrip = WheelData.TrailerWheelSideGrip;
				}
				rGKCar_Wheel.suspensionRelease = WheelData.SuspensionReleaseCoef;
				rGKCar_Wheel.ShowForces = WheelData.ShowForces;
				rGKCar_Wheel.inertia = WheelData.WheelInteria;
				rGKCar_Wheel.frictionTorque = WheelData.FrictionTorque;
				rGKCar_Wheel.massFraction = WheelData.MassFriction;
				rGKCar_Wheel.blurSwitchVelocity = WheelData.BlurSwtichVelocity;
				rGKCar_Wheel.SkidmarkObject = SkidMarks;
				rGKCar_Wheel.UpdateSkidmarkObjects();
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (WheelData.WheelBase != null)
			{
				if (WheelData.WheelBase.GetComponent<GizmoHelper>() == null)
				{
					WheelData.WheelBase.gameObject.AddComponent<GizmoHelper>();
					WheelData.WheelBase.GetComponent<GizmoHelper>().GizmoColor = Color.yellow;
				}
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(WheelData.WheelBase.transform.position, 0.08f);
			}
			if (Wheels != null)
			{
				Gizmos.color = Color.red;
				RGKCar_Wheel[] wheels = Wheels;
				foreach (RGKCar_Wheel rGKCar_Wheel in wheels)
				{
					if (rGKCar_Wheel != null)
					{
						if (rGKCar_Wheel.WheelLocation == WheelLocationEnum.Front)
						{
							Gizmos.DrawWireSphere(rGKCar_Wheel.WheelModel.transform.position, WheelData.FrontWheelRadius);
						}
						else if (rGKCar_Wheel.WheelLocation == WheelLocationEnum.Rear)
						{
							Gizmos.DrawWireSphere(rGKCar_Wheel.WheelModel.transform.position, WheelData.RearWheelRadius);
						}
						Gizmos.color = Color.yellow;
						if (WheelData.WheelBase != null)
						{
							Gizmos.DrawLine(rGKCar_Wheel.WheelModel.transform.position, WheelData.WheelBase.transform.position);
						}
						Gizmos.color = Color.red;
					}
				}
			}
			Gizmos.color = Color.cyan;
			if (CenterOfMass != null)
			{
				if (CenterOfMass.GetComponent<GizmoHelper>() == null)
				{
					CenterOfMass.gameObject.AddComponent<GizmoHelper>();
					CenterOfMass.GetComponent<GizmoHelper>().GizmoColor = Color.cyan;
				}
				Gizmos.DrawWireSphere(CenterOfMass.transform.position, 0.08f);
			}
			else if (vehicleRbody != null)
			{
				Gizmos.DrawWireSphere(vehicleRbody.centerOfMass, 0.08f);
			}
		}
	}
}
