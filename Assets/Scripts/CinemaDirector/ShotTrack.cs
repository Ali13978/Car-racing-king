namespace CinemaDirector
{
	[TimelineTrack("Shot Track", TimelineTrackGenre.GlobalTrack, new CutsceneItemGenre[]
	{
		CutsceneItemGenre.CameraShot
	})]
	public class ShotTrack : TimelineTrack
	{
		public event ShotEndsEventHandler ShotEnds;

		public event ShotBeginsEventHandler ShotBegins;

		public override void Initialize()
		{
			elapsedTime = 0f;
			CinemaGlobalAction cinemaGlobalAction = null;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaGlobalAction cinemaGlobalAction2 = timelineItems[i] as CinemaGlobalAction;
				cinemaGlobalAction2.Initialize();
			}
			for (int j = 0; j < timelineItems.Length; j++)
			{
				CinemaGlobalAction cinemaGlobalAction3 = timelineItems[j] as CinemaGlobalAction;
				if (cinemaGlobalAction3.Firetime == 0f)
				{
					cinemaGlobalAction = cinemaGlobalAction3;
				}
				else
				{
					cinemaGlobalAction3.End();
				}
			}
			if (cinemaGlobalAction != null)
			{
				cinemaGlobalAction.Trigger();
				if (this.ShotBegins != null)
				{
					this.ShotBegins(this, new ShotEventArgs(cinemaGlobalAction));
				}
			}
		}

		public override void UpdateTrack(float time, float deltaTime)
		{
			float elapsedTime = base.elapsedTime;
			base.elapsedTime = time;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaGlobalAction cinemaGlobalAction = timelineItems[i] as CinemaGlobalAction;
				float num = cinemaGlobalAction.Firetime + cinemaGlobalAction.Duration;
				if (elapsedTime <= cinemaGlobalAction.Firetime && base.elapsedTime >= cinemaGlobalAction.Firetime && base.elapsedTime < num)
				{
					cinemaGlobalAction.Trigger();
					if (this.ShotBegins != null)
					{
						this.ShotBegins(this, new ShotEventArgs(cinemaGlobalAction));
					}
				}
				else if (elapsedTime >= num && base.elapsedTime < num && base.elapsedTime >= cinemaGlobalAction.Firetime)
				{
					cinemaGlobalAction.Trigger();
					if (this.ShotBegins != null)
					{
						this.ShotBegins(this, new ShotEventArgs(cinemaGlobalAction));
					}
				}
				else if (elapsedTime >= cinemaGlobalAction.Firetime && elapsedTime < num && base.elapsedTime >= num)
				{
					cinemaGlobalAction.End();
					if (this.ShotEnds != null)
					{
						this.ShotEnds(this, new ShotEventArgs(cinemaGlobalAction));
					}
				}
				else if (elapsedTime > cinemaGlobalAction.Firetime && elapsedTime < num && base.elapsedTime < cinemaGlobalAction.Firetime)
				{
					cinemaGlobalAction.End();
					if (this.ShotEnds != null)
					{
						this.ShotEnds(this, new ShotEventArgs(cinemaGlobalAction));
					}
				}
			}
		}

		public override void SetTime(float time)
		{
			CinemaGlobalAction cinemaGlobalAction = null;
			CinemaGlobalAction cinemaGlobalAction2 = null;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaGlobalAction cinemaGlobalAction3 = timelineItems[i] as CinemaGlobalAction;
				float num = cinemaGlobalAction3.Firetime + cinemaGlobalAction3.Duration;
				if (elapsedTime >= cinemaGlobalAction3.Firetime && elapsedTime < num)
				{
					cinemaGlobalAction = cinemaGlobalAction3;
				}
				if (time >= cinemaGlobalAction3.Firetime && time < num)
				{
					cinemaGlobalAction2 = cinemaGlobalAction3;
				}
			}
			if (cinemaGlobalAction2 != cinemaGlobalAction)
			{
				if (cinemaGlobalAction != null)
				{
					cinemaGlobalAction.End();
					if (this.ShotEnds != null)
					{
						this.ShotEnds(this, new ShotEventArgs(cinemaGlobalAction));
					}
				}
				if (cinemaGlobalAction2 != null)
				{
					cinemaGlobalAction2.Trigger();
					if (this.ShotBegins != null)
					{
						this.ShotBegins(this, new ShotEventArgs(cinemaGlobalAction2));
					}
				}
			}
			elapsedTime = time;
		}
	}
}
