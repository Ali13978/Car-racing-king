using RacingGameKit.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Camera/RGK Rame Camera")]
	public class Race_Camera : MonoBehaviour, IRGKCamera
	{
		public Transform target;

		private Transform m_CachedTarget;

		private Camera m_Camera;

		private List<Transform> m_TargetObjects;

		public bool DisableTargetChange;

		public int LockFPSTo = 30;

		private bool isStartupAnimationEnabled;

		private bool isStartUpAnimationComplete;

		private int currentCountDown;

		private List<RGK_CameraPositionHelper> m_CameraPosition;

		private RGK_CameraPositionHelper m_BackCameraPosition;

		[HideInInspector]
		public int m_CurrentCameraPosition = -1;

		private bool m_PositionChaning;

		public OutsideCameraSettings GameCameraSettings;

		public ShakeSettingsData ShakeSettings;

		public FadeSettingsData FadeSettings;

		public DynamicFoVSettings DynamicFOVSettings;

		public StartUpAnimation CamAnimations;

		private float NewFov;

		private float m_VehicleSpeed;

		private float FovDif;

		private float ActualFov;

		private float currentHighSpeedShake;

		private bool m_showBackView;

		private bool m_showBackViewFromMethod;

		private bool m_showLeftView;

		private bool m_showRightView;

		private float tempShake;

		private bool m_CamWillChange;

		private float m_InitialCamAngle;

		private bool m_IsFadeDone;

		private Color m_FadeTextureColor;

		private float m_internalFadeTimer;

		private float m_OrigRotation;

		private bool m_IsSleeping;

		private Transform tempPos;

		public Transform TargetVehicle
		{
			set
			{
				target = value;
			}
		}

		public List<Transform> TargetObjects
		{
			get
			{
				return m_TargetObjects;
			}
			set
			{
				m_TargetObjects = value;
			}
		}

		public bool IsStartupAnimationEnabled
		{
			set
			{
				isStartupAnimationEnabled = value;
				if (isStartupAnimationEnabled)
				{
					isStartUpAnimationComplete = false;
				}
			}
		}

		public int CurrentCount
		{
			set
			{
				currentCountDown = value;
			}
		}

		public string ControlBindingCameraChange
		{
			set
			{
			}
		}

		public string ControlBindingCameraBack
		{
			set
			{
			}
		}

		public string ControlBindingCameraLeft
		{
			set
			{
			}
		}

		public string ControlBindingCameraRight
		{
			set
			{
			}
		}

		public Transform CachedTarget => m_CachedTarget;

		public bool Sleep
		{
			get
			{
				return m_IsSleeping;
			}
			set
			{
				m_IsSleeping = value;
			}
		}

		private void Start()
		{
			if (LockFPSTo > 0)
			{
				Application.targetFrameRate = LockFPSTo;
			}
			m_Camera = base.transform.GetComponent<Camera>();
			if (target != null)
			{
				m_CachedTarget = target;
				SetCameraPoints();
			}
			m_InitialCamAngle = GameCameraSettings.CameraAngle;
			m_internalFadeTimer = FadeSettings.FadeTime;
			m_OrigRotation = GameCameraSettings.RotationDamping;
		}

		private void Update()
		{
			if (m_IsSleeping)
			{
				return;
			}
			if (!m_IsFadeDone && FadeSettings.EnableFadeOnStart)
			{
				m_internalFadeTimer -= Time.deltaTime;
				if (m_internalFadeTimer <= 0f)
				{
					m_IsFadeDone = true;
				}
			}
			if (m_CachedTarget != target && target != null)
			{
				m_CachedTarget = target;
				SetCameraPoints();
			}
			if (m_CachedTarget != null)
			{
				ProcessFoV();
			}
			if (!isStartupAnimationEnabled)
			{
				isStartUpAnimationComplete = true;
			}
			if (m_CachedTarget != null)
			{
				ProcessCameraAction();
			}
		}

		private void OnGUI()
		{
			if (!m_IsFadeDone && FadeSettings.EnableFadeOnStart && FadeSettings.MaskTexture != null)
			{
				GUI.depth = -1;
				m_FadeTextureColor = Color.black;
				if (!FadeSettings.UseCurveForFade)
				{
					m_FadeTextureColor.a = Mathf.Lerp(0f, 1f, m_internalFadeTimer / FadeSettings.FadeTime);
				}
				else
				{
					m_FadeTextureColor.a = FadeSettings.FadeCurve.Evaluate(m_internalFadeTimer / FadeSettings.FadeTime);
				}
				GUI.color = m_FadeTextureColor;
				GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), FadeSettings.MaskTexture);
			}
		}

		private void ProcessFoV()
		{
			try
			{
				if (DynamicFOVSettings.Enabled)
				{
					if (DynamicFOVSettings.StartSpeed < 1f)
					{
						DynamicFOVSettings.StartSpeed = 1f;
					}
					m_VehicleSpeed = m_CachedTarget.parent.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
					FovDif = DynamicFOVSettings.MaxFov - DynamicFOVSettings.MinFov;
					if (m_CurrentCameraPosition == -1 && m_VehicleSpeed >= DynamicFOVSettings.StartSpeed)
					{
						NewFov = DynamicFOVSettings.ActionFovEffectCurve.Evaluate((m_VehicleSpeed - DynamicFOVSettings.StartSpeed) / DynamicFOVSettings.MaxSpeed);
						ActualFov = DynamicFOVSettings.MinFov + FovDif * NewFov;
						m_Camera.fieldOfView = ActualFov;
					}
					else
					{
						NewFov = 0f;
						ActualFov = 0f;
						m_Camera.fieldOfView = DynamicFOVSettings.MinFov;
					}
				}
			}
			catch
			{
				DynamicFOVSettings.Enabled = false;
			}
		}

		private void SetCameraPoints()
		{
			if (m_CachedTarget != null)
			{
				Transform transform = m_CachedTarget.parent.Find("_CameraLocations");
				if (!(transform != null))
				{
					return;
				}
				m_CameraPosition = new List<RGK_CameraPositionHelper>();
				Component[] componentsInChildren = transform.GetComponentsInChildren<RGK_CameraPositionHelper>();
				Component[] array = componentsInChildren;
				foreach (Component component in array)
				{
					RGK_CameraPositionHelper rGK_CameraPositionHelper = (RGK_CameraPositionHelper)component;
					if (!rGK_CameraPositionHelper.BackCamera)
					{
						m_CameraPosition.Add(rGK_CameraPositionHelper);
					}
					else
					{
						m_BackCameraPosition = rGK_CameraPositionHelper;
					}
				}
			}
			else
			{
				m_CurrentCameraPosition = -1;
			}
		}

		private void ProcessCameraAction()
		{
			if (target != null)
			{
				Vector3 eulerAngles = target.eulerAngles;
				float b = eulerAngles.y + GameCameraSettings.CameraAngle;
				Vector3 position = target.position;
				float b2 = position.y + GameCameraSettings.Height;
				Vector3 eulerAngles2 = base.transform.eulerAngles;
				float y = eulerAngles2.y;
				Vector3 position2 = base.transform.position;
				float y2 = position2.y;
				y = Mathf.LerpAngle(y, b, GameCameraSettings.RotationDamping * Time.deltaTime);
				y2 = Mathf.Lerp(y2, b2, GameCameraSettings.HeightDamping * Time.deltaTime);
				Quaternion rotation = Quaternion.Euler(0f, y, 0f);
				Vector3 position3 = target.position - rotation * Vector3.forward * GameCameraSettings.Distance;
				position3.y = y2;
				base.transform.position = position3;
				base.transform.LookAt(target);
				if (ShakeSettings.ShakeOnHighspeed)
				{
					float num = m_CachedTarget.parent.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
					if (num > ShakeSettings.ShakeStartSpeed)
					{
						float num2 = Mathf.Clamp01(m_CachedTarget.parent.GetComponent<Rigidbody>().velocity.magnitude / (ShakeSettings.ShakeStartSpeed / 1.5f));
						currentHighSpeedShake -= (currentHighSpeedShake - num2) * 0.005f;
						if (currentHighSpeedShake > ShakeSettings.MaxShake / 10f)
						{
							currentHighSpeedShake = ShakeSettings.MaxShake / 10f;
						}
						base.transform.Rotate(UnityEngine.Random.onUnitSphere * currentHighSpeedShake * 0.5f);
					}
					else
					{
						currentHighSpeedShake = 0f;
					}
				}
				if (ShakeSettings.ShakeOnStart)
				{
					float num3 = m_CachedTarget.parent.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
					if (num3 > 3f)
					{
						if (tempShake >= 0f)
						{
							if (ShakeSettings.ShakeFadoutRes == 0f)
							{
								ShakeSettings.ShakeFadoutRes = 1f;
							}
							tempShake -= Time.deltaTime / ShakeSettings.ShakeFadoutRes;
							if (tempShake > ShakeSettings.ShakeFrom / 10f)
							{
								tempShake = ShakeSettings.ShakeFrom / 10f;
							}
							base.transform.Rotate(UnityEngine.Random.onUnitSphere * tempShake * 0.5f);
						}
					}
					else
					{
						tempShake = ShakeSettings.ShakeFrom / 10f;
					}
				}
			}
			if (m_CurrentCameraPosition == -1 || m_CurrentCameraPosition == 0)
			{
				base.transform.parent = null;
				if (target == null && m_CachedTarget != null)
				{
					if (m_CurrentCameraPosition == -1)
					{
						target = m_CachedTarget;
						GameCameraSettings.Distance = 5f;
						GameCameraSettings.Height = 1f;
					}
					else if (m_CurrentCameraPosition == 0)
					{
						target = m_CachedTarget;
						GameCameraSettings.Distance = 4.1f;
						GameCameraSettings.Height = 0.45f;
					}
				}
				if (!isStartupAnimationEnabled && isStartUpAnimationComplete)
				{
					if (m_showLeftView)
					{
						GameCameraSettings.CameraAngle = -90f;
					}
					if (m_showRightView)
					{
						GameCameraSettings.CameraAngle = 90f;
					}
					if (m_showBackView)
					{
						GameCameraSettings.CameraAngle = 180f;
					}
					if (m_showBackViewFromMethod)
					{
						GameCameraSettings.CameraAngle = 180f;
					}
					if (!m_showBackView && !m_showRightView && !m_showLeftView && !m_showBackViewFromMethod)
					{
						GameCameraSettings.CameraAngle = m_InitialCamAngle;
						GameCameraSettings.RotationDamping = m_OrigRotation;
					}
				}
				else
				{
					switch (currentCountDown)
					{
					case 3:
						GameCameraSettings.CameraAngle = CamAnimations.Second3.CameraAngle;
						GameCameraSettings.Height = CamAnimations.Second3.CameraHeight;
						GameCameraSettings.Distance = CamAnimations.Second3.CameraDistance;
						break;
					case 2:
						GameCameraSettings.CameraAngle = CamAnimations.Second2.CameraAngle;
						GameCameraSettings.Height = CamAnimations.Second2.CameraHeight;
						GameCameraSettings.Distance = CamAnimations.Second2.CameraDistance;
						break;
					case 1:
						GameCameraSettings.CameraAngle = CamAnimations.Second1.CameraAngle;
						GameCameraSettings.Height = CamAnimations.Second1.CameraHeight;
						GameCameraSettings.Distance = CamAnimations.Second1.CameraDistance;
						break;
					case 0:
						GameCameraSettings.CameraAngle = CamAnimations.Second0.CameraAngle;
						GameCameraSettings.Height = CamAnimations.Second0.CameraHeight;
						GameCameraSettings.Distance = CamAnimations.Second0.CameraDistance;
						isStartUpAnimationComplete = true;
						break;
					default:
						GameCameraSettings.CameraAngle = CamAnimations.Second3.CameraAngle;
						GameCameraSettings.Height = CamAnimations.Second3.CameraHeight;
						GameCameraSettings.Distance = CamAnimations.Second3.CameraDistance;
						break;
					}
				}
			}
			else
			{
				if (m_showBackView)
				{
					m_PositionChaning = true;
					if (m_PositionChaning)
					{
						base.transform.parent = m_BackCameraPosition.transform.parent;
						base.transform.position = m_BackCameraPosition.transform.position;
						base.transform.rotation = m_BackCameraPosition.transform.rotation;
						m_Camera.fieldOfView = m_BackCameraPosition.FieldOfValue;
						m_PositionChaning = false;
					}
				}
				if (!m_showBackView)
				{
					m_PositionChaning = true;
					if (m_PositionChaning)
					{
						base.transform.parent = m_CameraPosition[m_CurrentCameraPosition].transform.parent;
						base.transform.position = m_CameraPosition[m_CurrentCameraPosition].transform.position;
						base.transform.rotation = m_CameraPosition[m_CurrentCameraPosition].transform.rotation;
						m_Camera.fieldOfView = m_CameraPosition[m_CurrentCameraPosition].FieldOfValue;
						m_PositionChaning = false;
					}
				}
			}
			if (m_CamWillChange)
			{
				m_CamWillChange = false;
				if (m_CurrentCameraPosition == m_CameraPosition.Count - 1)
				{
					m_CurrentCameraPosition = -1;
				}
				else
				{
					m_CurrentCameraPosition++;
					m_PositionChaning = true;
					if (m_PositionChaning)
					{
						base.transform.parent = m_CameraPosition[m_CurrentCameraPosition].transform.parent;
						base.transform.position = m_CameraPosition[m_CurrentCameraPosition].transform.position;
						base.transform.rotation = m_CameraPosition[m_CurrentCameraPosition].transform.rotation;
						m_Camera.fieldOfView = m_CameraPosition[m_CurrentCameraPosition].FieldOfValue;
						target = null;
						m_PositionChaning = false;
					}
				}
			}
			m_showBackViewFromMethod = false;
			m_showLeftView = false;
			m_showRightView = false;
		}

		public void ChangeCamera()
		{
			m_CamWillChange = true;
			ProcessCameraAction();
		}

		public void ShowBackView()
		{
			GameCameraSettings.RotationDamping = 25f;
			m_showBackViewFromMethod = true;
		}

		public void ShowLeftView()
		{
			GameCameraSettings.RotationDamping = 25f;
			m_showRightView = true;
		}

		public void ShowRightView()
		{
			GameCameraSettings.RotationDamping = 25f;
			m_showLeftView = true;
		}
	}
}
