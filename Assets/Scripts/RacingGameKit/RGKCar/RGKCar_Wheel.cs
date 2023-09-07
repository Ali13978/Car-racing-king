using EarthFX;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RacingGameKit.RGKCar
{
	[AddComponentMenu("Racing Game Kit/RGKCar/RGKCar Wheel")]
	public class RGKCar_Wheel : MonoBehaviour, IEarthFX
	{
		public GameObject WheelModel;

		public GameObject WheelBlurModel;

		public GameObject DiskModel;

		public Vector3 DiskModelOffset;

		public GameObject CaliperModel;

		public Vector3 CaliperModelOffset;

		public WheelLocationEnum WheelLocation;

		public bool isPowered;

		private bool NoSuspension;

		[HideInInspector]
		public float suspensionHeight = 0.2f;

		[HideInInspector]
		public float suspensionStiffness = 5000f;

		[HideInInspector]
		public float suspensionRelease = 50f;

		[HideInInspector]
		public float brakeTorque = 4000f;

		[HideInInspector]
		public float handbrakeTorque;

		public float definedGrip = 1f;

		public float wheelGrip = 1f;

		public float definedSideGrip = 1f;

		public float sideGrip = 1f;

		public float maxSteeringAngle;

		[HideInInspector]
		public float slipVeloLimiter = 20f;

		[HideInInspector]
		public float radius = 0.34f;

		[HideInInspector]
		public float blurSwitchVelocity = 20f;

		[HideInInspector]
		public bool ShowForces;

		[HideInInspector]
		public bool BurnoutStart = true;

		[HideInInspector]
		public float BurnoutStartDuration = 1f;

		public float inertia = 2.2f;

		public float frictionTorque = 10f;

		public float massFraction = 0.25f;

		private float[] a = new float[15]
		{
			1.5f,
			-40f,
			1600f,
			2600f,
			8.7f,
			0.014f,
			-0.24f,
			1f,
			-0.03f,
			-0.0013f,
			-0.06f,
			-8.5f,
			-0.29f,
			17.8f,
			-2.4f
		};

		private float[] b = new float[11]
		{
			1.5f,
			-80f,
			1950f,
			23.3f,
			390f,
			0.05f,
			0f,
			0.055f,
			-0.024f,
			0.014f,
			0.26f
		};

		[HideInInspector]
		public float driveTorque;

		[HideInInspector]
		public float driveFrictionTorque;

		[HideInInspector]
		public float brake;

		[HideInInspector]
		public float handbrake;

		[HideInInspector]
		public float steering;

		[HideInInspector]
		public float drivetrainInertia;

		[HideInInspector]
		public float suspensionForceInput;

		public float angularVelocity;

		public float oAngularVelocity;

		public float slipRatio;

		public float slipVelo;

		public float compression = 0.5f;

		private float fullCompressionSpringForce;

		public Vector3 wheelVelo;

		public Vector3 localVelo;

		private Vector3 groundNormal;

		private float rotation;

		private float normalForce;

		private Vector3 suspensionForce;

		public Vector3 roadForce;

		private Vector3 up;

		private Vector3 right;

		private Vector3 forwardNormal;

		private Vector3 rightNormal;

		private Quaternion localRotation = Quaternion.identity;

		private Quaternion inverseLocalRotation = Quaternion.identity;

		public float slipAngle;

		private Rigidbody body;

		private float maxSlip;

		private float maxAngle;

		private float oldAngle;

		public float totalFrictionTorque;

		public int slipRes;

		private float longitunalSlipVelo;

		private float lateralSlipVelo;

		private float groundCalc;

		private Vector3 force;

		public bool onGround;

		public LineRenderer suspensionLineRenderer;

		private RaycastHit hit;

		public bool UseEarthFX;

		public int m_LastSkid = -1;

		public int m_lastTrail = -1;

		public GameObject SkidmarkObject;

		public RGKCar_Skidmarks m_skidNotEarthFX;

		public GameObject m_SkidObject;

		public GameObject m_TrailObject;

		public RGKCar_Skidmarks m_SkidMarks;

		public RGKCar_Skidmarks m_TrailMarks;

		private ParticleSystem m_SkidSmoke;

		private bool m_IsInstantiated;

		private ParticleSystem m_TrailSmoke;

		private ParticleSystem m_Splatter;

		private List<SfxCache> m_SurfaceFxCache;

		public string m_previousSurface = string.Empty;

		public bool m_surfaceChanged;

		public float camberAngle;

		public float invSlipRes;

		private float slipFactor = 10f;

		float IEarthFX.SlipRatio => slipRatio;

		float IEarthFX.SlipVelo => slipVelo;

		bool IEarthFX.OnGround => onGround;

		public RaycastHit HitToSurface => hit;

		public float AngularVelocity => angularVelocity;

		public float Grip
		{
			get
			{
				return wheelGrip;
			}
			set
			{
				wheelGrip = value;
			}
		}

		public float SideGrip
		{
			get
			{
				return sideGrip;
			}
			set
			{
				sideGrip = value;
			}
		}

		public float DefinedGrip
		{
			get
			{
				return definedGrip;
			}
			set
			{
				definedGrip = value;
			}
		}

		public float DefinedSideGrip
		{
			get
			{
				return definedSideGrip;
			}
			set
			{
				definedSideGrip = value;
			}
		}

		public Transform WheelTransform => WheelModel.transform;

		public int LastSkid
		{
			get
			{
				return m_LastSkid;
			}
			set
			{
				m_LastSkid = value;
			}
		}

		public int LastTrail
		{
			get
			{
				return m_lastTrail;
			}
			set
			{
				m_lastTrail = value;
			}
		}

		public bool EarthFXEnabled
		{
			set
			{
				UseEarthFX = value;
			}
		}

		public string PreviousSurface
		{
			get
			{
				return m_previousSurface;
			}
			set
			{
				m_previousSurface = value;
			}
		}

		public bool SurfaceChanged
		{
			get
			{
				return m_surfaceChanged;
			}
			set
			{
				m_surfaceChanged = value;
			}
		}

		public GameObject SkidObject
		{
			get
			{
				return m_SkidObject;
			}
			set
			{
				m_SkidObject = value;
			}
		}

		public GameObject TrailObject
		{
			get
			{
				return m_TrailObject;
			}
			set
			{
				m_TrailObject = value;
			}
		}

		public ParticleSystem SkidSmoke
		{
			get
			{
				return m_SkidSmoke;
			}
			set
			{
				m_SkidSmoke = value;
			}
		}

		public ParticleSystem TrailSmoke
		{
			get
			{
				return m_TrailSmoke;
			}
			set
			{
				m_TrailSmoke = value;
			}
		}

		public ParticleSystem Splatter
		{
			get
			{
				return m_Splatter;
			}
			set
			{
				m_Splatter = value;
			}
		}

		public RGKCar_Skidmarks SkidMark
		{
			get
			{
				return m_SkidMarks;
			}
			set
			{
				m_SkidMarks = value;
			}
		}

		public RGKCar_Skidmarks TrailMark
		{
			get
			{
				return m_TrailMarks;
			}
			set
			{
				m_TrailMarks = value;
			}
		}

		public List<SfxCache> SurfaceFxCache
		{
			get
			{
				return m_SurfaceFxCache;
			}
			set
			{
				m_SurfaceFxCache = value;
			}
		}

		private void Awake()
		{
			suspensionLineRenderer = base.gameObject.AddComponent<LineRenderer>();
			suspensionLineRenderer.material = new Material(Shader.Find("Diffuse"));
			suspensionLineRenderer.material.color = Color.red;
			suspensionLineRenderer.SetWidth(0.01f, 0.1f);
			suspensionLineRenderer.useWorldSpace = false;
			suspensionLineRenderer.shadowCastingMode = ShadowCastingMode.Off;
		}

		private void Start()
		{
			Transform transform = base.transform;
			while (transform != null && transform.GetComponent<Rigidbody>() == null)
			{
				transform = transform.parent;
			}
			if (transform != null)
			{
				body = transform.GetComponent<Rigidbody>();
			}
			InitSlipMaxima();
			UpdateSkidmarkObjects();
		}

		public void UpdateSkidmarkObjects()
		{
			if (!UseEarthFX && SkidmarkObject != null && !m_IsInstantiated)
			{
				m_skidNotEarthFX = SkidmarkObject.GetComponentInChildren<RGKCar_Skidmarks>();
				m_SkidSmoke = SkidmarkObject.GetComponentInChildren<ParticleSystem>();
				ParticleSystem particleSystem = UnityEngine.Object.Instantiate(m_SkidSmoke, base.transform.position, Quaternion.identity, base.transform);
				m_SkidSmoke = particleSystem.GetComponentInChildren<ParticleSystem>();
				m_IsInstantiated = true;
			}
		}

		private void FixedUpdate()
		{
			if (!UseEarthFX)
			{
				wheelGrip = definedGrip;
				sideGrip = definedSideGrip;
			}
			if (suspensionHeight < 0.05f)
			{
				suspensionHeight = 0.05f;
			}
			suspensionLineRenderer.enabled = ShowForces;
			Vector3 position = base.transform.position;
			up = base.transform.up;
			onGround = Physics.Raycast(position, -up, out hit, suspensionHeight + radius);
			if (onGround && hit.collider.isTrigger)
			{
				return;
			}
			if (onGround)
			{
				float num = body.mass * massFraction * 2f;
				Vector3 gravity = Physics.gravity;
				fullCompressionSpringForce = num * (0f - gravity.y);
				groundNormal = base.transform.InverseTransformDirection(inverseLocalRotation * hit.normal);
				if (!NoSuspension)
				{
					compression = 1f - (hit.distance - radius) / suspensionHeight;
				}
				suspensionForce = SuspensionForce();
				suspensionLineRenderer.SetPosition(1, new Vector3(0f, 0.0005f * suspensionForce.y, 0f));
				wheelVelo = body.GetPointVelocity(position);
				localVelo = base.transform.InverseTransformDirection(inverseLocalRotation * wheelVelo);
				roadForce = RoadForce();
				body.AddForceAtPosition(suspensionForce + roadForce, position);
			}
			else
			{
				compression = 0f;
				suspensionForce = Vector3.zero;
				suspensionLineRenderer.SetPosition(1, new Vector3(0f, 0.0005f * suspensionForce.y, 0f));
				slipRatio = 0f;
				slipVelo = 0f;
			}
			if (!onGround || !UseEarthFX)
			{
				if (onGround && m_skidNotEarthFX != null && (double)Mathf.Abs(slipRatio) > 0.15)
				{
					m_LastSkid = m_skidNotEarthFX.AddSkidMark(hit.point, hit.normal, Mathf.Abs(slipRatio), m_LastSkid);
					if (m_SkidSmoke != null)
					{
						m_SkidSmoke.transform.position = hit.point;
						ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
						emitParams.position = hit.point + new Vector3(UnityEngine.Random.Range(0f, 0.25f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
						emitParams.velocity = new Vector3(slipVelo * 0.05f, 0f);
						m_SkidSmoke.Emit(emitParams, 1);
					}
				}
				else
				{
					m_LastSkid = -1;
				}
			}
			compression = Mathf.Clamp01(compression);
			rotation += angularVelocity * Time.deltaTime;
			UpdateModels();
		}

		private float CalcLongitudinalForce(float Fz, float slip)
		{
			Fz *= 0.001f;
			float num = Fz * Fz;
			slip *= 100f;
			float num2 = Fz * Fz;
			float num3 = b[1] * num2 + b[2] * Fz;
			float num4 = (b[3] * num + b[4] * Fz) * Mathf.Exp((0f - b[5]) * Fz) / (b[0] * num3);
			float num5 = b[6] * num + b[7] * Fz + b[8];
			float num6 = b[9] * Fz + b[10];
			float num7 = 0f;
			return num3 * Mathf.Sin(b[0] * Mathf.Atan(num4 * (1f - num5) * (slip + num6) + num5 * Mathf.Atan(num4 * (slip + num6)))) + num7;
		}

		private float CalcLateralForce(float Fz, float slipAngle)
		{
			Fz *= 0.001f;
			float num = Fz * Fz;
			Vector3 from = base.transform.TransformDirection(Vector3.up);
			Vector3 to = base.transform.root.transform.TransformDirection(Vector3.up);
			camberAngle = Vector3.Angle(from, to);
			slipAngle *= 180f / (float)Math.PI;
			float num2 = (a[1] * num + a[2] * Fz) * sideGrip;
			float num3 = a[3] * Mathf.Sin(2f * Mathf.Atan(Fz / a[4])) * (1f - a[5] * Mathf.Abs(camberAngle)) / (a[0] * num2);
			float num4 = slipAngle + a[9] * Fz + a[10];
			float num5 = a[6] * Fz + a[7];
			float num6 = (a[11] * Fz + a[12]) * camberAngle * Fz + a[12] * Fz + a[13];
			return num2 * Mathf.Sin(a[0] * Mathf.Atan(num4 * num3 + num5 * (Mathf.Atan(num4 * num3) - num4 * num3))) + num6;
		}

		private float CalcLongitudinalForceUnit(float Fz, float slip)
		{
			return CalcLongitudinalForce(Fz, slip * maxSlip);
		}

		private float CalcLateralForceUnit(float Fz, float slipAngle)
		{
			return CalcLateralForce(Fz, slipAngle * maxAngle);
		}

		private Vector3 CombinedForce(float Fz, float slip, float slipAngle)
		{
			float num = slip / maxSlip;
			float num2 = slipAngle / maxAngle;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 <= Mathf.Epsilon)
			{
				return Vector3.zero;
			}
			if (slip < -0.8f)
			{
				return -localVelo.normalized * (Mathf.Abs(num2 / num3 * CalcLateralForceUnit(Fz, num3)) + Mathf.Abs(num / num3 * CalcLongitudinalForceUnit(Fz, num3)));
			}
			Vector3 vector = new Vector3(0f, 0f - groundNormal.z, groundNormal.y);
			return Vector3.right * num2 / num3 * CalcLateralForceUnit(Fz, num3) + vector * num / num3 * CalcLongitudinalForceUnit(Fz, num3);
		}

		private void InitSlipMaxima()
		{
			float num = 0f;
			float num2 = 0.001f;
			while (true)
			{
				float num3 = CalcLongitudinalForce(4000f, num2);
				if (!(num < num3))
				{
					break;
				}
				num = num3;
				num2 += 0.001f;
			}
			maxSlip = num2 - 0.001f;
			num = 0f;
			float num4 = 0.001f;
			while (true)
			{
				float num5 = CalcLateralForce(4000f, num4);
				if (!(num < num5))
				{
					break;
				}
				num = num5;
				num4 += 0.001f;
			}
			maxAngle = num4 - 0.001f;
		}

		private Vector3 SuspensionForce()
		{
			float num = normalForce = ((!(suspensionRelease > 0f)) ? (compression * fullCompressionSpringForce) : (compression * fullCompressionSpringForce * Time.deltaTime * suspensionRelease));
			float num2 = Vector3.Dot(localVelo, groundNormal) * suspensionStiffness;
			return (num - num2 + suspensionForceInput) * up;
		}

		private float SlipRatio()
		{
			float num = Vector3.Dot(wheelVelo, forwardNormal);
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = Mathf.Abs(num);
			float num3 = Mathf.Clamp01(num2 / 4f);
			float num4 = angularVelocity * radius;
			slipRatio = (num4 - num) / num2 * num3;
			return slipRatio;
		}

		private float SlipAngle()
		{
			Vector3 vector = localVelo;
			vector.y = 0f;
			if (vector.sqrMagnitude < float.Epsilon)
			{
				return 0f;
			}
			Vector3 normalized = vector.normalized;
			float x = normalized.x;
			Mathf.Clamp(x, -1f, 1f);
			float num = Mathf.Clamp01(localVelo.magnitude / 2f);
			return (0f - Mathf.Asin(x)) * num * num;
		}

		private Vector3 RoadForce()
		{
			slipRes = (int)((100f - Mathf.Abs(angularVelocity)) / slipFactor);
			if (slipRes < 1)
			{
				slipRes = 1;
			}
			invSlipRes = 1f / (float)slipRes;
			float num = inertia + drivetrainInertia;
			float num2 = driveTorque * Time.deltaTime * invSlipRes / num;
			totalFrictionTorque = brakeTorque * brake + handbrakeTorque * handbrake + frictionTorque + driveFrictionTorque / 2f;
			float num3 = totalFrictionTorque * Time.deltaTime * invSlipRes / num;
			Vector3 result = Vector3.zero;
			float num4 = maxSteeringAngle * steering;
			for (int i = 0; i < slipRes; i++)
			{
				float num5 = (float)i * 1f / (float)slipRes;
				localRotation = Quaternion.Euler(0f, oldAngle + (num4 - oldAngle) * num5, 0f);
				inverseLocalRotation = Quaternion.Inverse(localRotation);
				forwardNormal = base.transform.TransformDirection(localRotation * Vector3.forward);
				right = base.transform.TransformDirection(localRotation * Vector3.right);
				groundCalc = Vector3.Dot(right, groundNormal);
				rightNormal = right - groundNormal * groundCalc;
				forwardNormal = Vector3.Cross(rightNormal, groundNormal);
				slipRatio = SlipRatio();
				slipAngle = SlipAngle();
				if (brake > 0f)
				{
					force = invSlipRes * wheelGrip * 1.5f * CombinedForce(normalForce, slipRatio / 2f, slipAngle / 2f);
				}
				else
				{
					force = invSlipRes * wheelGrip * CombinedForce(normalForce, slipRatio, slipAngle);
				}
				Vector3 vector = base.transform.TransformDirection(localRotation * force);
				angularVelocity -= force.z * radius * Time.deltaTime / num;
				angularVelocity += num2;
				if (Mathf.Abs(angularVelocity) > num3)
				{
					angularVelocity -= num3 * Mathf.Sign(angularVelocity);
				}
				else
				{
					angularVelocity = 0f;
				}
				result += vector;
			}
			longitunalSlipVelo = Mathf.Abs(angularVelocity * radius - Vector3.Dot(wheelVelo, forwardNormal));
			lateralSlipVelo = Vector3.Dot(wheelVelo, right);
			slipVelo = Mathf.Sqrt(longitunalSlipVelo * longitunalSlipVelo + lateralSlipVelo * lateralSlipVelo);
			oldAngle = num4;
			return result;
		}

		private void UpdateModels()
		{
			if (WheelModel != null)
			{
				WheelModel.transform.localPosition = Vector3.up * (compression - 1f) * suspensionHeight;
				WheelModel.transform.localRotation = Quaternion.Euler(57.29578f * rotation, maxSteeringAngle * steering, 0f);
				if (WheelBlurModel != null)
				{
					WheelBlurModel.transform.localPosition = Vector3.up * (compression - 1f) * suspensionHeight;
					WheelBlurModel.transform.localRotation = Quaternion.Euler(57.29578f * rotation, maxSteeringAngle * steering, 0f);
				}
			}
			if (WheelBlurModel != null)
			{
				if (angularVelocity > blurSwitchVelocity)
				{
					WheelBlurModel.SetActive(value: true);
					WheelModel.SetActive(value: false);
				}
				else
				{
					WheelModel.SetActive(value: true);
					WheelBlurModel.SetActive(value: false);
				}
			}
			if (DiskModel != null)
			{
				DiskModel.transform.localPosition = WheelModel.transform.localPosition + DiskModelOffset;
				DiskModel.transform.localRotation = Quaternion.Euler(57.29578f * rotation, maxSteeringAngle * steering, 0f);
			}
			if (CaliperModel != null)
			{
				CaliperModel.transform.localPosition = WheelModel.transform.localPosition + CaliperModelOffset;
				CaliperModel.transform.localRotation = Quaternion.Euler(0f, maxSteeringAngle * steering, 0f);
			}
		}
	}
}
