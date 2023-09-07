using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RacingGameKit.TouchDriveFree
{
	[Serializable]
	[AddComponentMenu("Racing Game Kit/Touch Drive/Touch Wheel")]
	public class TouchWheel : TouchItemBase
	{
		public float m_MaxAngle = 150f;

		public float m_CenterSpeed = 5f;

		public bool m_UseSensitivityCurve;

		public AnimationCurve m_SensitivityCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		private RectTransform m_TransformCache;

		private Vector2 m_TransformCenter;

		private float prevWheelAngle;

		protected override void Start()
		{
			base.Start();
			m_TransformCache = (base.transform as RectTransform);
			UpdatePos();
		}

		internal override void Update()
		{
			base.Update();
			if (m_TransformCache == null)
			{
				m_TransformCache = (base.transform as RectTransform);
			}
			if (!m_IsPressed && !Mathf.Approximately(0f, m_CurrentAngle))
			{
				float num = m_CenterSpeed * 100f * Time.deltaTime;
				if (Mathf.Abs(num) > Mathf.Abs(m_CurrentAngle))
				{
					m_CurrentAngle = 0f;
				}
				else if (m_CurrentAngle > 0f)
				{
					m_CurrentAngle -= num;
				}
				else
				{
					m_CurrentAngle += num;
				}
			}
			m_TransformCache.localEulerAngles = Vector3.back * m_CurrentAngle;
			if (!m_UseSensitivityCurve)
			{
				m_CurrentFloat = Mathf.Clamp(1f / m_MaxAngle * m_CurrentAngle, -1f, 1f);
			}
			else
			{
				m_CurrentFloat = m_SensitivityCurve.Evaluate(Mathf.Abs(m_CurrentAngle) / m_MaxAngle);
				if (m_CurrentAngle < 0f)
				{
					m_CurrentFloat *= -1f;
				}
			}
			if (m_CurrentFloat > 0f)
			{
				m_CurrentInt = 1;
			}
			else if (m_CurrentFloat < 0f)
			{
				m_CurrentInt = -1;
			}
			else
			{
				m_CurrentInt = 0;
			}
		}

		private void UpdatePos()
		{
			Vector3[] array = new Vector3[4];
			m_TransformCache.GetWorldCorners(array);
			for (int i = 0; i < 4; i++)
			{
				array[i] = RectTransformUtility.WorldToScreenPoint(null, array[i]);
			}
			Vector3 vector = array[0];
			Vector3 vector2 = array[2];
			float width = vector2.x - vector.x;
			float height = vector2.y - vector.y;
			Rect rect = new Rect(vector.x, vector2.y, width, height);
			m_TransformCenter = new Vector2(rect.x + rect.width * 0.5f, rect.y - rect.height * 0.5f - 50f);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
		}

		public override void OnPointerDown(BaseEventData eventData)
		{
			PointerEventData eventData2 = (PointerEventData)eventData;
			base.OnPointerDown(eventData2);
			UpdatePos();
			Vector2 position = ((PointerEventData)eventData).position;
			m_IsPressed = true;
			prevWheelAngle = Vector2.Angle(Vector2.up, position - m_TransformCenter);
		}

		public override void OnPointerUp(BaseEventData eventData)
		{
			base.OnPointerUp((PointerEventData)eventData);
		}

		public override void OnDrag(BaseEventData eventData)
		{
			Vector2 position = ((PointerEventData)eventData).position;
			float num = Vector2.Angle(Vector2.up, position - m_TransformCenter);
			if (Vector2.Distance(position, m_TransformCenter) > 10f)
			{
				if (position.x >= m_TransformCenter.x)
				{
					m_CurrentAngle += num - prevWheelAngle;
				}
				else
				{
					m_CurrentAngle -= num - prevWheelAngle;
				}
			}
			m_CurrentAngle = Mathf.Clamp(m_CurrentAngle, 0f - m_MaxAngle, m_MaxAngle);
			prevWheelAngle = num;
		}
	}
}
