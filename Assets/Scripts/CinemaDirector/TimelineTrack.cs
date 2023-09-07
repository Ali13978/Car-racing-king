using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	public abstract class TimelineTrack : MonoBehaviour, IOptimizable
	{
		[SerializeField]
		public bool lockedStatus;

		[SerializeField]
		private int ordinal = -1;

		[SerializeField]
		private bool canOptimize = true;

		public PlaybackMode PlaybackMode = PlaybackMode.RuntimeAndEdit;

		protected float elapsedTime;

		protected TimelineItem[] itemCache;

		protected List<Type> allowedItemTypes;

		private bool hasBeenOptimized;

		public Cutscene Cutscene => (!(TrackGroup == null)) ? TrackGroup.Cutscene : null;

		public TrackGroup TrackGroup
		{
			get
			{
				TrackGroup trackGroup = null;
				if (base.transform.parent != null)
				{
					trackGroup = base.transform.parent.GetComponent<TrackGroup>();
					if (trackGroup == null)
					{
						UnityEngine.Debug.LogError("No TrackGroup found on parent.", this);
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Track has no parent.", this);
				}
				return trackGroup;
			}
		}

		public int Ordinal
		{
			get
			{
				return ordinal;
			}
			set
			{
				ordinal = value;
			}
		}

		public bool CanOptimize
		{
			get
			{
				return canOptimize;
			}
			set
			{
				canOptimize = value;
			}
		}

		public virtual TimelineItem[] TimelineItems => GetComponentsInChildren<TimelineItem>();

		public virtual void Optimize()
		{
			if (canOptimize)
			{
				itemCache = GetTimelineItems();
				hasBeenOptimized = true;
			}
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				if (timelineItems[i] is IOptimizable)
				{
					(timelineItems[i] as IOptimizable).Optimize();
				}
			}
		}

		public virtual void Initialize()
		{
			elapsedTime = 0f;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				timelineItems[i].Initialize();
			}
		}

		public virtual void UpdateTrack(float runningTime, float deltaTime)
		{
			float num = elapsedTime;
			elapsedTime = runningTime;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaGlobalEvent cinemaGlobalEvent = timelineItems[i] as CinemaGlobalEvent;
				if (!(cinemaGlobalEvent == null))
				{
					if ((num < cinemaGlobalEvent.Firetime || num <= 0f) && elapsedTime >= cinemaGlobalEvent.Firetime)
					{
						cinemaGlobalEvent.Trigger();
					}
					else if (num >= cinemaGlobalEvent.Firetime && elapsedTime <= cinemaGlobalEvent.Firetime)
					{
						cinemaGlobalEvent.Reverse();
					}
				}
			}
			for (int j = 0; j < timelineItems.Length; j++)
			{
				CinemaGlobalAction cinemaGlobalAction = timelineItems[j] as CinemaGlobalAction;
				if (!(cinemaGlobalAction == null))
				{
					if ((num < cinemaGlobalAction.Firetime || num <= 0f) && elapsedTime >= cinemaGlobalAction.Firetime && elapsedTime < cinemaGlobalAction.EndTime)
					{
						cinemaGlobalAction.Trigger();
					}
					else if (num <= cinemaGlobalAction.EndTime && elapsedTime >= cinemaGlobalAction.EndTime)
					{
						cinemaGlobalAction.End();
					}
					else if (num > cinemaGlobalAction.Firetime && num <= cinemaGlobalAction.EndTime && elapsedTime <= cinemaGlobalAction.Firetime)
					{
						cinemaGlobalAction.ReverseTrigger();
					}
					else if ((num > cinemaGlobalAction.EndTime || num >= cinemaGlobalAction.Cutscene.Duration) && elapsedTime > cinemaGlobalAction.Firetime && elapsedTime <= cinemaGlobalAction.EndTime)
					{
						cinemaGlobalAction.ReverseEnd();
					}
					else if (elapsedTime > cinemaGlobalAction.Firetime && elapsedTime < cinemaGlobalAction.EndTime)
					{
						float time = runningTime - cinemaGlobalAction.Firetime;
						cinemaGlobalAction.UpdateTime(time, deltaTime);
					}
				}
			}
		}

		public virtual void Pause()
		{
		}

		public virtual void Resume()
		{
		}

		public virtual void SetTime(float time)
		{
			float num = elapsedTime;
			elapsedTime = time;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				CinemaGlobalEvent cinemaGlobalEvent = timelineItems[i] as CinemaGlobalEvent;
				if (cinemaGlobalEvent != null)
				{
					if ((num < cinemaGlobalEvent.Firetime && time >= cinemaGlobalEvent.Firetime) || (cinemaGlobalEvent.Firetime == 0f && num <= cinemaGlobalEvent.Firetime && time > cinemaGlobalEvent.Firetime))
					{
						cinemaGlobalEvent.Trigger();
					}
					else if (num > cinemaGlobalEvent.Firetime && elapsedTime <= cinemaGlobalEvent.Firetime)
					{
						cinemaGlobalEvent.Reverse();
					}
				}
				CinemaGlobalAction cinemaGlobalAction = timelineItems[i] as CinemaGlobalAction;
				if (cinemaGlobalAction != null)
				{
					cinemaGlobalAction.SetTime(time - cinemaGlobalAction.Firetime, time - num);
				}
			}
		}

		public virtual List<float> GetMilestones(float from, float to)
		{
			bool flag = from > to;
			List<float> list = new List<float>();
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				if (((!flag && from < timelineItems[i].Firetime && to >= timelineItems[i].Firetime) || (flag && from > timelineItems[i].Firetime && to <= timelineItems[i].Firetime)) && !list.Contains(timelineItems[i].Firetime))
				{
					list.Add(timelineItems[i].Firetime);
				}
				if (timelineItems[i] is TimelineAction)
				{
					float endTime = (timelineItems[i] as TimelineAction).EndTime;
					if (((!flag && from < endTime && to >= endTime) || (flag && from > endTime && to <= endTime)) && !list.Contains(endTime))
					{
						list.Add(endTime);
					}
				}
			}
			list.Sort();
			return list;
		}

		public virtual void Stop()
		{
			elapsedTime = 0f;
			TimelineItem[] timelineItems = GetTimelineItems();
			for (int i = 0; i < timelineItems.Length; i++)
			{
				timelineItems[i].Stop();
			}
		}

		public List<Type> GetAllowedCutsceneItems()
		{
			if (allowedItemTypes == null)
			{
				allowedItemTypes = DirectorRuntimeHelper.GetAllowedItemTypes(this);
			}
			return allowedItemTypes;
		}

		public TimelineItem[] GetTimelineItems()
		{
			if (hasBeenOptimized)
			{
				return itemCache;
			}
			List<TimelineItem> list = new List<TimelineItem>();
			List<Type> allowedCutsceneItems = GetAllowedCutsceneItems();
			for (int i = 0; i < allowedCutsceneItems.Count; i++)
			{
				Component[] componentsInChildren = GetComponentsInChildren(allowedCutsceneItems[i]);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					list.Add((TimelineItem)componentsInChildren[j]);
				}
			}
			return list.ToArray();
		}
	}
}
