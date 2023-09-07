using UnityEngine;

namespace RacingGameKit.TouchDrive
{
	public interface iRGKTouchItem
	{
		Transform Transform
		{
			get;
		}

		bool IsPressed
		{
			get;
			set;
		}

		bool WasPressed
		{
			get;
			set;
		}

		bool IsToggled
		{
			get;
			set;
		}

		float CurrentAngle
		{
			get;
			set;
		}

		float CurrentFloat
		{
			get;
			set;
		}

		int CurrentInt
		{
			get;
			set;
		}
	}
}
