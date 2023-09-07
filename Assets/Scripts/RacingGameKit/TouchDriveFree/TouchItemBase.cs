using RacingGameKit.TouchDrive;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RacingGameKit.TouchDriveFree
{
	[Serializable]
	[AddComponentMenu("")]
	public class TouchItemBase : Button, iRGKTouchItem
	{
		public enum ButtonTouchState
		{
			Pressed = 1,
			Released,
			Disabled
		}

		public ButtonTouchState ButtonState = ButtonTouchState.Released;

		public bool m_IsToggleButton;

		public Sprite m_OriginalSprite;

		public Sprite m_ToggledSprite;

		public bool m_ToggleDefault;

		public bool m_IsPressed;

		public bool m_WasPressed;

		private bool m_WasPressedStateBlocker;

		public float m_CurrentAngle;

		public float m_CurrentFloat;

		public int m_CurrentInt;

		public bool m_IsDraggable;

		public bool m_IsToggled;

		internal RectTransform m_SelfRectTransform;

		public RectTransform m_TouchAnywhereRect;

		public CanvasGroup m_CanvasGroup;

		internal TouchDriveManager touchDriveManager;

		public Vector3 m_OriginalPosition;

		public bool tdButtonEdVarsTouchSettings = true;

		public bool tdButtonEdVarsGuiSettings = true;

		public bool tdButtonEdVarsDebugSettings;

		public new virtual bool IsPressed
		{
			get
			{
				return m_IsPressed;
			}
			set
			{
				m_IsPressed = value;
			}
		}

		public virtual bool WasPressed
		{
			get
			{
				return m_WasPressed;
			}
			set
			{
				m_WasPressed = value;
			}
		}

		public bool IsToggled
		{
			get
			{
				return m_IsToggled;
			}
			set
			{
				m_IsToggled = value;
			}
		}

		public float CurrentAngle
		{
			get
			{
				return m_CurrentAngle;
			}
			set
			{
				m_CurrentAngle = value;
			}
		}

		public float CurrentFloat
		{
			get
			{
				return m_CurrentFloat;
			}
			set
			{
				m_CurrentFloat = value;
			}
		}

		public int CurrentInt
		{
			get
			{
				return m_CurrentInt;
			}
			set
			{
				m_CurrentInt = value;
			}
		}

		public bool IsDraggable
		{
			get
			{
				return m_IsDraggable;
			}
			set
			{
				m_IsDraggable = value;
			}
		}

		public Transform Transform => base.transform;

		protected override void Start()
		{
			base.Start();
			m_SelfRectTransform = (base.transform as RectTransform);
			m_CanvasGroup = GetComponent<CanvasGroup>();
			touchDriveManager = UnityEngine.Object.FindObjectOfType<TouchDriveManager>();
			if (m_IsToggleButton)
			{
				GetComponent<Image>().sprite = ((!m_IsToggled) ? m_OriginalSprite : m_ToggledSprite);
			}
			m_OriginalPosition = m_SelfRectTransform.localPosition;
		}

		internal virtual void Update()
		{
			if (m_WasPressed && !m_WasPressedStateBlocker)
			{
				m_WasPressed = false;
				m_WasPressedStateBlocker = true;
			}
		}

		public virtual void OnButtonPressed()
		{
			m_IsPressed = true;
			m_WasPressed = true;
			m_CurrentFloat = 1f;
			m_CurrentInt = 1;
			ButtonState = ButtonTouchState.Pressed;
			if (m_IsToggleButton)
			{
				m_IsToggled = ((!m_IsToggled) ? true : false);
				GetComponent<Image>().sprite = ((!m_IsToggled) ? m_OriginalSprite : m_ToggledSprite);
			}
		}

		public virtual void OnButtonReleased()
		{
			m_IsPressed = false;
			m_WasPressedStateBlocker = false;
			m_CurrentFloat = 0f;
			m_CurrentInt = 0;
			ButtonState = ButtonTouchState.Released;
		}

		public virtual void OnPointerDown(BaseEventData eventData)
		{
			OnButtonPressed();
		}

		public virtual void OnPointerUp(BaseEventData eventData)
		{
			OnButtonReleased();
		}

		public virtual void OnDrag(BaseEventData eventData)
		{
		}
	}
}
