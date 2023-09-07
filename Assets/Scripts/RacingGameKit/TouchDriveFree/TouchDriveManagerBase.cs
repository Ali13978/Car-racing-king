using RacingGameKit.TouchDrive;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.TouchDriveFree
{
	[AddComponentMenu("")]
	public class TouchDriveManagerBase : RGKTDM, iRGKTDM
	{
		public enum TouchItemName
		{
			Wheel = 0,
			Throttle = 1,
			Brake = 2,
			SteerLeft = 3,
			SteerRight = 4,
			ShiftUp = 5,
			ShiftDown = 6,
			CameraButton = 7,
			MirrorButton = 8,
			ResetButton = 9,
			PauseMenu = 10,
			Misc1Button = 11,
			Misc2Button = 13
		}

		private List<iRGKTouchItem> m_TouchItems = new List<iRGKTouchItem>(13);

		private bool m_EnableDrivingOptions = true;

		private bool m_EnableAutoThrottle;

		private bool m_EnableAutoBrake;

		private bool m_EnableAutoGear = true;

		private bool m_EnableTiltSteer;

		public bool m_EnableTouchWheelSteer;

		private bool m_EnableButtonSteer;

		public bool m_UseXAxis;

		public bool m_InvertAxis;

		public float m_SteerSensitivity = 5f;

		private bool m_FlipPositions;

		public override List<iRGKTouchItem> TouchItems
		{
			get
			{
				return m_TouchItems;
			}
			set
			{
				m_TouchItems = value;
			}
		}

		public bool EnableAutoThrottle
		{
			get
			{
				return m_EnableAutoThrottle;
			}
			set
			{
				m_EnableAutoThrottle = value;
			}
		}

		public bool EnableAutoBrake
		{
			get
			{
				return m_EnableAutoBrake;
			}
			set
			{
				m_EnableAutoBrake = value;
			}
		}

		public bool EnableAutoGear
		{
			get
			{
				return m_EnableAutoGear;
			}
			set
			{
				m_EnableAutoGear = value;
			}
		}

		public bool EnableTiltSteer
		{
			get
			{
				return m_EnableTiltSteer;
			}
			set
			{
				m_EnableTiltSteer = value;
			}
		}

		public bool EnableButtonSteer
		{
			get
			{
				return m_EnableButtonSteer;
			}
			set
			{
				m_EnableButtonSteer = value;
			}
		}

		public bool EnableTouchWheelSteer
		{
			get
			{
				return m_EnableTouchWheelSteer;
			}
			set
			{
				m_EnableTouchWheelSteer = value;
			}
		}

		public bool UseXAxis
		{
			get
			{
				return m_UseXAxis;
			}
			set
			{
				m_UseXAxis = value;
			}
		}

		public bool InvertAxis
		{
			get
			{
				return m_InvertAxis;
			}
			set
			{
				m_InvertAxis = value;
			}
		}

		public float SteeringSensitivity
		{
			get
			{
				return m_SteerSensitivity;
			}
			set
			{
				m_SteerSensitivity = value;
			}
		}

		public bool EnableDrivingOptions
		{
			get
			{
				return m_EnableDrivingOptions;
			}
			set
			{
				m_EnableDrivingOptions = value;
			}
		}

		public bool FlipPositions
		{
			get
			{
				return m_FlipPositions;
			}
			set
			{
				m_FlipPositions = value;
			}
		}

		public bool IsPro => false;

		public TouchItemBase GetTouchItem(TouchItemName TouchItem)
		{
			iRGKTouchItem iRGKTouchItem = m_TouchItems[0];
			switch (TouchItem)
			{
			case TouchItemName.Wheel:
				iRGKTouchItem = m_TouchItems[0];
				break;
			case TouchItemName.Throttle:
				iRGKTouchItem = m_TouchItems[1];
				break;
			case TouchItemName.Brake:
				iRGKTouchItem = m_TouchItems[2];
				break;
			case TouchItemName.SteerLeft:
				iRGKTouchItem = m_TouchItems[3];
				break;
			case TouchItemName.SteerRight:
				iRGKTouchItem = m_TouchItems[4];
				break;
			case TouchItemName.ShiftUp:
				iRGKTouchItem = m_TouchItems[5];
				break;
			case TouchItemName.ShiftDown:
				iRGKTouchItem = m_TouchItems[6];
				break;
			case TouchItemName.CameraButton:
				iRGKTouchItem = m_TouchItems[7];
				break;
			case TouchItemName.MirrorButton:
				iRGKTouchItem = m_TouchItems[8];
				break;
			case TouchItemName.ResetButton:
				iRGKTouchItem = m_TouchItems[9];
				break;
			case TouchItemName.PauseMenu:
				iRGKTouchItem = m_TouchItems[10];
				break;
			case TouchItemName.Misc1Button:
				iRGKTouchItem = m_TouchItems[11];
				break;
			case TouchItemName.Misc2Button:
				iRGKTouchItem = m_TouchItems[12];
				break;
			}
			return (TouchItemBase)iRGKTouchItem;
		}

		public virtual void SwitchTemplate(int Template)
		{
			UnityEngine.Debug.LogWarning("Switch Template called but this function available on TouchDrive Pro only!");
		}
	}
}
