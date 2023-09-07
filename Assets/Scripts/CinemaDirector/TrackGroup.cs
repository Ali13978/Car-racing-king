using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[TrackGroup("Track Group", new TimelineTrackGenre[]
	{
		TimelineTrackGenre.GlobalTrack
	})]
	public abstract class TrackGroup : MonoBehaviour, IOptimizable
	{
		[SerializeField]
		private int ordinal = -1;

		[SerializeField]
		private bool canOptimize = true;

		protected TimelineTrack[] trackCache;

		protected List<Type> allowedTrackTypes;

		private bool hasBeenOptimized;

		public Cutscene Cutscene
		{
			get
			{
				Cutscene cutscene = null;
				if (base.transform.parent != null)
				{
					cutscene = base.transform.parent.GetComponentInParent<Cutscene>();
					if (cutscene == null)
					{
						UnityEngine.Debug.LogError("No Cutscene found on parent!", this);
					}
				}
				else
				{
					UnityEngine.Debug.LogError("TrackGroup has no parent!", this);
				}
				return cutscene;
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

		public virtual void Optimize()
		{
			if (canOptimize)
			{
				trackCache = GetTracks();
				hasBeenOptimized = true;
			}
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].Optimize();
			}
		}

		public virtual void Initialize()
		{
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].Initialize();
			}
		}

		public virtual void UpdateTrackGroup(float time, float deltaTime)
		{
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].UpdateTrack(time, deltaTime);
			}
		}

		public virtual void Pause()
		{
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].Pause();
			}
		}

		public virtual void Stop()
		{
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].Stop();
			}
		}

		public virtual void Resume()
		{
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].Resume();
			}
		}

		public virtual void SetRunningTime(float time)
		{
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].SetTime(time);
			}
		}

		public virtual List<float> GetMilestones(float from, float to)
		{
			List<float> list = new List<float>();
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				List<float> milestones = tracks[i].GetMilestones(from, to);
				for (int j = 0; j < milestones.Count; j++)
				{
					if (!list.Contains(milestones[j]))
					{
						list.Add(milestones[j]);
					}
				}
			}
			list.Sort();
			return list;
		}

		public virtual TimelineTrack[] GetTracks()
		{
			if (hasBeenOptimized)
			{
				return trackCache;
			}
			List<TimelineTrack> list = new List<TimelineTrack>();
			List<Type> list2 = GetAllowedTrackTypes();
			for (int i = 0; i < list2.Count; i++)
			{
				Component[] componentsInChildren = GetComponentsInChildren(list2[i]);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					list.Add((TimelineTrack)componentsInChildren[j]);
				}
			}
			list.Sort((TimelineTrack track1, TimelineTrack track2) => track1.Ordinal - track2.Ordinal);
			return list.ToArray();
		}

		public List<Type> GetAllowedTrackTypes()
		{
			if (allowedTrackTypes == null)
			{
				allowedTrackTypes = DirectorRuntimeHelper.GetAllowedTrackTypes(this);
			}
			return allowedTrackTypes;
		}
	}
}
