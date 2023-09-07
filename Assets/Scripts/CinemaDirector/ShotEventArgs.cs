using System;

namespace CinemaDirector
{
	public class ShotEventArgs : EventArgs
	{
		public CinemaGlobalAction shot;

		public ShotEventArgs(CinemaGlobalAction shot)
		{
			this.shot = shot;
		}
	}
}
