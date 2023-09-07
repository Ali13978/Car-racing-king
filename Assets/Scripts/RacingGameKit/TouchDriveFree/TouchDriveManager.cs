using UnityEngine;

namespace RacingGameKit.TouchDriveFree
{
	[AddComponentMenu("Racing Game Kit/Touch Drive/TouchDrive Manager")]
	[ExecuteInEditMode]
	public class TouchDriveManager : TouchDriveManagerBase
	{
		public TouchItemBase Throttle;

		public TouchItemBase Brake;

		public TouchItemBase SteerLeft;

		public TouchItemBase SteerRight;

		public TouchItemBase ShiftUp;

		public TouchItemBase ShiftDown;

		public TouchItemBase Wheel;

		public TouchItemBase CameraButton;

		public TouchItemBase ResetButton;

		public TouchItemBase PauseButton;

		public TouchItemBase MirrorButton;

		public TouchItemBase Misc1Button;

		public TouchItemBase Misc2Button;

		public bool tdFreeEdVarsTemplates = true;

		public bool tdFreeEdVarsPanelsAndControls = true;

		public bool tdFreeEdVarsTouchControls = true;

		private void Awake()
		{
			Input.multiTouchEnabled = true;
			TouchItems.Clear();
			TouchItems.Add(Wheel);
			TouchItems.Add(Throttle);
			TouchItems.Add(Brake);
			TouchItems.Add(SteerLeft);
			TouchItems.Add(SteerRight);
			TouchItems.Add(ShiftUp);
			TouchItems.Add(ShiftDown);
			TouchItems.Add(CameraButton);
			TouchItems.Add(MirrorButton);
			TouchItems.Add(ResetButton);
			TouchItems.Add(PauseButton);
			TouchItems.Add(Misc1Button);
			TouchItems.Add(Misc2Button);
		}

		private void Start()
		{
			SwitchTemplate(22);
		}

		public override void SwitchTemplate(int TemplateTo)
		{
			switch (TemplateTo)
			{
			case 20:
				EnableDisableButtons(bThrottle: true, bBreak: true, bSteerLeft: false, bSteerRight: false, bSteerWheel: false, bShiftUp: false, bShiftDown: false);
				base.EnableTiltSteer = true;
				base.EnableTouchWheelSteer = false;
				break;
			case 22:
				EnableDisableButtons(bThrottle: true, bBreak: true, bSteerLeft: true, bSteerRight: true, bSteerWheel: false, bShiftUp: false, bShiftDown: false);
				base.EnableTiltSteer = false;
				base.EnableTouchWheelSteer = false;
				break;
			case 23:
				EnableDisableButtons(bThrottle: true, bBreak: true, bSteerLeft: false, bSteerRight: false, bSteerWheel: true, bShiftUp: false, bShiftDown: false);
				base.EnableTiltSteer = false;
				base.EnableTouchWheelSteer = true;
				break;
			case 30:
				EnableDisableButtons(bThrottle: false, bBreak: false, bSteerLeft: false, bSteerRight: false, bSteerWheel: false, bShiftUp: true, bShiftDown: true);
				break;
			}
		}

		private void EnableDisableButtons(bool bThrottle, bool bBreak, bool bSteerLeft, bool bSteerRight, bool bSteerWheel, bool bShiftUp, bool bShiftDown)
		{
			if (Throttle != null)
			{
				TouchItemBase throttle = Throttle;
				throttle.gameObject.SetActive(bThrottle);
			}
			if (Brake != null)
			{
				TouchItemBase brake = Brake;
				brake.gameObject.SetActive(bBreak);
			}
			if (SteerLeft != null)
			{
				TouchItemBase steerLeft = SteerLeft;
				steerLeft.gameObject.SetActive(bSteerLeft);
			}
			if (SteerRight != null)
			{
				TouchItemBase steerRight = SteerRight;
				steerRight.gameObject.SetActive(bSteerRight);
			}
			if (ShiftUp != null)
			{
				TouchItemBase shiftUp = ShiftUp;
				shiftUp.gameObject.SetActive(bShiftUp);
			}
			if (ShiftDown != null)
			{
				TouchItemBase shiftDown = ShiftDown;
				shiftDown.gameObject.SetActive(bShiftDown);
			}
			if (Wheel != null)
			{
				TouchItemBase wheel = Wheel;
				wheel.gameObject.SetActive(bSteerWheel);
			}
		}
	}
}
