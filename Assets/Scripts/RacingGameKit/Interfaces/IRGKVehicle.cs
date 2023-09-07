namespace RacingGameKit.Interfaces
{
	public interface IRGKVehicle
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

		float MaxSteer
		{
			get;
		}

		void ApplyDrive(float Throttle, float Brake, bool HandBrake);

		void ApplySteer(float Steer);

		void ShiftGears();
	}
}
