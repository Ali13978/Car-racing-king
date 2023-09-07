namespace RacingGameKit.Interfaces
{
	public interface IRGKUI
	{
		bool ShowCountdownWindow
		{
			get;
			set;
		}

		bool ShowWrongWayWindow
		{
			get;
			set;
		}

		float CurrentCount
		{
			get;
			set;
		}

		void ShowResultsWindow();

		void RaceFinished(string RaceType);

		void PlayerCheckPointPassed(CheckPointItem PassedCheckpoint);
	}
}
