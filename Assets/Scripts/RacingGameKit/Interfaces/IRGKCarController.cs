namespace RacingGameKit.Interfaces
{
	public interface IRGKCarController
	{
		float Speed
		{
			get;
		}

		float Rpm
		{
			get;
		}

		int Gear
		{
			get;
			set;
		}

		bool IsReversing
		{
			set;
		}

		bool IsPreviouslyReversed
		{
			set;
		}

		bool IsBraking
		{
			set;
		}

		float MaxSteer
		{
			get;
		}

		void ApplyDrive(float Throttle, float Brake, bool HandBrake);

		void ApplySteer(float Steer);

		void ShiftGears();
	}
}
