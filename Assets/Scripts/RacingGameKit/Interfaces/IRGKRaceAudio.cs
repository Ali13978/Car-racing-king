namespace RacingGameKit.Interfaces
{
	public interface IRGKRaceAudio
	{
		bool PlayBackgroundMusic
		{
			set;
		}

		bool MuteAllSounds
		{
			get;
			set;
		}

		void InitAudio();

		void PlayAudio(eRaceAudioFXName AudioName);
	}
}
