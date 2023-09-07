using UnityEngine;

namespace CinemaDirector
{
	[TimelineTrack("Curve Track", TimelineTrackGenre.ActorTrack, new CutsceneItemGenre[]
	{
		CutsceneItemGenre.CurveClipItem
	})]
	public class CurveTrack : TimelineTrack, IActorTrack
	{
		public Transform Actor
		{
			get
			{
				ActorTrackGroup actorTrackGroup = base.TrackGroup as ActorTrackGroup;
				if (actorTrackGroup == null)
				{
					UnityEngine.Debug.LogError("No ActorTrackGroup found on parent.", this);
					return null;
				}
				return actorTrackGroup.Actor;
			}
		}

		public override void UpdateTrack(float time, float deltaTime)
		{
			elapsedTime = time;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaActorClipCurve cinemaActorClipCurve = timelineItems[i] as CinemaActorClipCurve;
				if (cinemaActorClipCurve != null)
				{
					cinemaActorClipCurve.SampleTime(time);
				}
			}
		}

		public override void SetTime(float time)
		{
			elapsedTime = time;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaActorClipCurve cinemaActorClipCurve = timelineItems[i] as CinemaActorClipCurve;
				if (cinemaActorClipCurve != null)
				{
					cinemaActorClipCurve.SampleTime(time);
				}
			}
		}

		public override void Stop()
		{
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaActorClipCurve cinemaActorClipCurve = timelineItems[i] as CinemaActorClipCurve;
				if (cinemaActorClipCurve != null)
				{
					cinemaActorClipCurve.Reset();
				}
			}
		}
	}
}
