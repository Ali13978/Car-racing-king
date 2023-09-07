using UnityEngine;

namespace CinemaDirector
{
	[TimelineTrack("Curve Track", TimelineTrackGenre.MultiActorTrack, new CutsceneItemGenre[]
	{
		CutsceneItemGenre.MultiActorCurveClipItem
	})]
	public class MultiCurveTrack : TimelineTrack, IActorTrack
	{
		public override TimelineItem[] TimelineItems => GetComponentsInChildren<CinemaMultiActorCurveClip>();

		public Transform Actor
		{
			get
			{
				ActorTrackGroup component = base.transform.parent.GetComponent<ActorTrackGroup>();
				if (component == null)
				{
					return null;
				}
				return component.Actor;
			}
		}

		public override void Initialize()
		{
			for (int i = 0; i < TimelineItems.Length; i++)
			{
				CinemaMultiActorCurveClip cinemaMultiActorCurveClip = TimelineItems[i] as CinemaMultiActorCurveClip;
				cinemaMultiActorCurveClip.Initialize();
			}
		}

		public override void UpdateTrack(float time, float deltaTime)
		{
			elapsedTime = time;
			for (int i = 0; i < TimelineItems.Length; i++)
			{
				CinemaMultiActorCurveClip cinemaMultiActorCurveClip = TimelineItems[i] as CinemaMultiActorCurveClip;
				cinemaMultiActorCurveClip.SampleTime(time);
			}
		}

		public override void Stop()
		{
			for (int i = 0; i < TimelineItems.Length; i++)
			{
				CinemaMultiActorCurveClip cinemaMultiActorCurveClip = TimelineItems[i] as CinemaMultiActorCurveClip;
				cinemaMultiActorCurveClip.Revert();
			}
		}
	}
}
