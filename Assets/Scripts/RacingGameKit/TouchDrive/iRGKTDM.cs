using System.Collections.Generic;

namespace RacingGameKit.TouchDrive
{
	public interface iRGKTDM
	{
		List<iRGKTouchItem> TouchItems
		{
			get;
			set;
		}

		bool EnableDrivingOptions
		{
			get;
			set;
		}

		bool EnableAutoThrottle
		{
			get;
			set;
		}

		bool EnableAutoBrake
		{
			get;
			set;
		}

		bool EnableAutoGear
		{
			get;
			set;
		}

		bool EnableTiltSteer
		{
			get;
			set;
		}

		bool EnableButtonSteer
		{
			get;
			set;
		}

		bool EnableTouchWheelSteer
		{
			get;
			set;
		}

		bool UseXAxis
		{
			get;
			set;
		}

		bool InvertAxis
		{
			get;
			set;
		}

		float SteeringSensitivity
		{
			get;
			set;
		}

		bool FlipPositions
		{
			get;
			set;
		}

		bool IsPro
		{
			get;
		}

		void SwitchTemplate(int Template);
	}
}
