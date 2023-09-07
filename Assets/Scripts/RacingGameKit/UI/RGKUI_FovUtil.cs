using UnityEngine;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Utils/UI FOV Util")]
	[RequireComponent(typeof(Camera))]
	public class RGKUI_FovUtil : MonoBehaviour
	{
		public bool m_Enable;

		public Race_Camera m_Race_Camera;

		private Camera ThisCamera;

		public float m_MaxFovChange = 2.5f;

		private float m_OrigFov;

		private float m_VehicleSpeed;

		private float m_FovDiff;

		private float m_NewFov;

		private float m_ActualFov;

		private Transform m_TargetCache;

		private void Start()
		{
			ThisCamera = GetComponent<Camera>();
			if (ThisCamera != null)
			{
				ThisCamera.transparencySortMode = TransparencySortMode.Perspective;
			}
			m_OrigFov = ThisCamera.fieldOfView;
			if (m_Race_Camera != null)
			{
				m_TargetCache = m_Race_Camera.CachedTarget;
			}
		}

		private void LateUpdate()
		{
			if (m_Race_Camera != null)
			{
				if (m_TargetCache != m_Race_Camera.CachedTarget)
				{
					m_TargetCache = m_Race_Camera.CachedTarget;
				}
				ProcessFoV();
			}
		}

		private void ProcessFoV()
		{
			if (!m_Enable || !(m_TargetCache != null))
			{
				return;
			}
			if (m_Race_Camera.DynamicFOVSettings.StartSpeed < 1f)
			{
				m_Race_Camera.DynamicFOVSettings.StartSpeed = 1f;
			}
			m_VehicleSpeed = m_TargetCache.parent.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
			m_FovDiff = m_Race_Camera.DynamicFOVSettings.MaxFov - m_Race_Camera.DynamicFOVSettings.MinFov;
			if (m_VehicleSpeed >= m_Race_Camera.DynamicFOVSettings.StartSpeed)
			{
				m_NewFov = m_Race_Camera.DynamicFOVSettings.ActionFovEffectCurve.Evaluate((m_VehicleSpeed - m_Race_Camera.DynamicFOVSettings.StartSpeed) / m_Race_Camera.DynamicFOVSettings.MaxSpeed);
				m_ActualFov = m_Race_Camera.DynamicFOVSettings.MinFov + m_FovDiff * m_NewFov;
				if (m_ActualFov <= m_OrigFov + m_MaxFovChange)
				{
					ThisCamera.fieldOfView = m_ActualFov;
				}
				else
				{
					ThisCamera.fieldOfView = m_OrigFov + m_MaxFovChange;
				}
			}
			else
			{
				m_NewFov = 0f;
				m_ActualFov = 0f;
				ThisCamera.fieldOfView = m_OrigFov;
			}
		}
	}
}
